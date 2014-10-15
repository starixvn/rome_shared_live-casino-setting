namespace Fanex.LiveCasino.CoreSetting.Services.PTCom
{
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.DTO.CommandResult;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Repositories.PTCom;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Company Edit super TP COM service.
    /// </summary>
    public class SuperPTComService : PTComServiceBase
    {
        public SuperPTComService(IPTComRepository ptComRepository)
        {
            PTComRepository = ptComRepository ?? new SuperPTComRepository();
        }

        public override void SetPTComSetting(Customer customer, LiveCasinoSettingContext settingContext)
        {
            PTComDTO ptComDTO = PTComRepository.Get(new GetSettingForEditSuperCriteria
            {
                CustId = settingContext.Customer.Id
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
            var criteria = new UpdateSuperSettingCriteria
            {
                CustId = settingContext.Customer.Id,
                IP = settingContext.IP,
                Language = settingContext.Language,
                UserId = settingContext.Actor.Id,
                SuperCommission = (decimal)Commission,
                S4 = (decimal)PositionTaking.S4,
                N4 = (decimal)PositionTaking.N4,
            };

            ICommandResult commandResult = PTComRepository.Save(criteria);

            return GetResultDetails(commandResult);
        }
    }
}