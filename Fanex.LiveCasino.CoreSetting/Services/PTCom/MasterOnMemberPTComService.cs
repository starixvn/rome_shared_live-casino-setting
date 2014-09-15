// -----------------------------------------------------------------------
// <copyright file="MasterOnMemberPTComService.cs" company="Microsoft">
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
    public class MasterOnMemberPTComService : PTComServiceBase
    {
        public MasterOnMemberPTComService(IPTComRepository ptComRepository)
        {
            base.PTComRepository = ptComRepository ?? new MasterOnMemberPTComRepository();
        }

        public override void SetPTComSetting(Customer customer, LiveCasinoSettingContext settingContext)
        {
            PTComDTO ptComDTO = base.PTComRepository.Get(new GetSettingForMasterEditMemberCriteria
                {
                    CustId = settingContext.Customer.Id
                });

            base.MapSetting(ptComDTO, customer);
        }

        public override ICommandResultsDTO Update(LiveCasinoSettingContext settingContext)
        {
            var criteria = new MasterUpdateMemberSettingCriteria
            {
                CustId = settingContext.Customer.Id,
                IP = settingContext.IP,
                Language = settingContext.Language,
                UserId = settingContext.Actor.Id,
                SubAccID = settingContext.SubAccount.Id,
                M1 = (decimal)base.PositionTaking.M1,
                Im1 = (bool)base.PositionTaking.Im1,
                Um1 = (decimal)base.PositionTaking.Um1,
                Im2 = (bool)base.PositionTaking.Im2
            };

            if (criteria.Im1)
            {
                criteria.M1 = 0;
            }
            else
            {
                criteria.Um1 = 0;
            }

            ICommandResult commandResult = base.PTComRepository.Save(criteria);

            return GetResultDetails(commandResult);
        }
    }
}