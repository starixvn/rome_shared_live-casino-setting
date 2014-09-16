namespace Fanex.LiveCasino.CoreSetting.Services.PTCom
{
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.DTO.CommandResult;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Repositories.PTCom;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Master edit member PT COM Setting.
    /// </summary>
    public class MasterOnMemberPTComService : PTComServiceBase
    {
        public MasterOnMemberPTComService(IPTComRepository ptComRepository)
        {
            PTComRepository = ptComRepository ?? new MasterOnMemberPTComRepository();
        }

        public override void SetPTComSetting(Customer customer, LiveCasinoSettingContext settingContext)
        {
            PTComDTO ptComDTO = PTComRepository.Get(new GetSettingForMasterEditMemberCriteria
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
            var criteria = new MasterUpdateMemberSettingCriteria
            {
                CustId = settingContext.Customer.Id,
                IP = settingContext.IP,
                Language = settingContext.Language,
                UserId = settingContext.Actor.Id,
                SubAccID = settingContext.SubAccount.Id,
                M1 = (decimal)PositionTaking.M1,
                Im1 = (bool)PositionTaking.Im1,
                Um1 = (decimal)PositionTaking.Um1,
                Im2 = (bool)PositionTaking.Im2
            };

            if (criteria.Im1)
            {
                criteria.M1 = 0;
            }
            else
            {
                criteria.Um1 = 0;
            }

            ICommandResult commandResult = PTComRepository.Save(criteria);

            return GetResultDetails(commandResult);
        }
    }
}