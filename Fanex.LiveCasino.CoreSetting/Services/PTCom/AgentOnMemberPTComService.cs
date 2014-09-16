namespace Fanex.LiveCasino.CoreSetting.Services.PTCom
{
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.DTO.CommandResult;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Repositories.PTCom;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Agent Edit member service.
    /// </summary>
    public class AgentOnMemberPTComService : PTComServiceBase
    {
        public AgentOnMemberPTComService(IPTComRepository ptComRepository)
        {
            PTComRepository = ptComRepository ?? new AgentOnMemberPTComRepository();
        }

        public override void SetPTComSetting(Customer customer, LiveCasinoSettingContext settingContext)
        {
            PTComDTO ptComDTO = PTComRepository.Get(new GetSettingForEditMemberCriteria
            {
                CustId = settingContext.Customer.Id,
                Recommend = settingContext.Upline.Id
            });

            MapSetting(ptComDTO, customer);
        }

        /// <summary>
        /// Update PT and Com Setting.
        /// </summary>
        /// <param name="settingContext">Customer information for updating.</param>
        /// <returns>Update result status.</returns>
        public override ICommandResults Update(LiveCasinoSettingContext settingContext)
        {
            ICriteria criteria = new AgentUpdateMemberSettingCriteria
            {
                CustId = settingContext.Customer.Id,
                IP = settingContext.IP,
                Language = settingContext.Language,
                Recommend = settingContext.Upline.Id,
                UserId = settingContext.Actor.Id,
                SubAccID = settingContext.SubAccount.Id,
                MemberCommission = (decimal)Commission,
                A1 = (decimal)PositionTaking.A1
            };

            ICommandResult commandResult = PTComRepository.Save(criteria);

            return GetResultDetails(commandResult);
        }
    }
}