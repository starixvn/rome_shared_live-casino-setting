namespace Fanex.LiveCasino.CoreSetting.Services.PTCom
{
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.DTO.CommandResult;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Repositories.PTCom;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Super Update Member PT And COM service.
    /// </summary>
    public class SuperOnMemberPTComService : PTComServiceBase
    {
        public SuperOnMemberPTComService(IPTComRepository ptComRepository)
        {
            base.PTComRepository = ptComRepository ?? new SuperOnMemberPTComRepository();
        }

        public override void SetPTComSetting(Customer customer, LiveCasinoSettingContext settingContext)
        {
            PTComDTO ptComDTO = base.PTComRepository.Get(new GetSettingForSuperEditMemberCriteria
            {
                CustId = settingContext.Customer.Id
            });

            base.MapSetting(ptComDTO, customer);
        }

        /// <summary>
        /// Update PT and Com Setting.
        /// </summary>
        /// <param name="settingContext">Customer information for updating.</param>
        /// <returns>Update result status.</returns>
        public override ICommandResults Update(LiveCasinoSettingContext settingContext)
        {
            var criteria = new SuperUpdateMemberSettingCriteria
            {
                CustId = settingContext.Customer.Id,
                IP = settingContext.IP,
                Language = settingContext.Language,
                UserId = settingContext.Actor.Id,
                SubAccID = settingContext.SubAccount.Id,
                S1 = (decimal)base.PositionTaking.S1,
                Is1 = (bool)base.PositionTaking.Is1,
                Us1 = (decimal)base.PositionTaking.Us1,
                IsUpdateSuperPt = !(bool)base.PositionTaking.Is3
            };

            if (criteria.Is1)
            {
                criteria.S1 = 0;
            }
            else
            {
                criteria.Us1 = 0;
            }

            ICommandResult commandResult = base.PTComRepository.Save(criteria);

            return GetResultDetails(commandResult);
        }
    }
}