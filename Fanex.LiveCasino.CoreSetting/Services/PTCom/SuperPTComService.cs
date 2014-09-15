// -----------------------------------------------------------------------
// <copyright file="SuperPTComService.cs" company="Microsoft">
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
    public class SuperPTComService : PTComServiceBase
    {
        public SuperPTComService(IPTComRepository ptComRepository)
        {
            base.PTComRepository = ptComRepository ?? new SuperPTComRepository();
        }

        public override void SetPTComSetting(Customer customer, LiveCasinoSettingContext settingContext)
        {
            PTComDTO ptComDTO = base.PTComRepository.Get(new GetSettingForEditSuperCriteria
            {
                CustId = settingContext.Customer.Id
            });

            base.MapSetting(ptComDTO, customer);
        }

        public override ICommandResultsDTO Update(LiveCasinoSettingContext settingContext)
        {
            var criteria = new UpdateSuperSettingCriteria
            {
                CustId = settingContext.Customer.Id,
                IP = settingContext.IP,
                Language = settingContext.Language,
                UserId = settingContext.Actor.Id,
                SuperCommission = (decimal)base.Commission,
                S4 = (decimal)base.PositionTaking.S4,
                N4 = (decimal)base.PositionTaking.N4,
            };

            ICommandResult commandResult = base.PTComRepository.Save(criteria);

            return GetResultDetails(commandResult);
        }
    }
}