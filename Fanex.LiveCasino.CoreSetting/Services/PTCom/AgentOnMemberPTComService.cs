// -----------------------------------------------------------------------
// <copyright file="AgentOnMemberPTComService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.LiveCasino.CoreSetting.Services.PTCom
{
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.DTO.CommandResult;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Repositories.PTCom;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AgentOnMemberPTComService : PTComServiceBase
    {
        public AgentOnMemberPTComService(IPTComRepository ptComRepository)
        {
            base.PTComRepository = ptComRepository ?? new AgentOnMemberPTComRepository();
        }

        public override void SetPTComSetting(Customer customer, LiveCasinoSettingContext settingContext)
        {
            PTComDTO ptComDTO = base.PTComRepository.Get(new GetSettingForEditMemberCriteria
            {
                CustId = settingContext.Customer.Id,
                Recommend = settingContext.Upline.Id
            });

            base.MapSetting(ptComDTO, customer);
        }

        public override ICommandResultsDTO Update(LiveCasinoSettingContext settingContext)
        {
            ICriteria criteria = new AgentUpdateMemberSettingCriteria
            {
                CustId = settingContext.Customer.Id,
                IP = settingContext.IP,
                Language = settingContext.Language,
                Recommend = settingContext.Upline.Id,
                UserId = settingContext.ActorId,
                SubAccID = settingContext.SubAccount.Id,
                MemberCommission = (decimal)base.Commission,
                A1 = (decimal)base.PositionTaking.A1
            };

            ICommandResult commandResult = base.PTComRepository.Save(criteria);

            return GetResultDetails(commandResult);
        }
    }
}