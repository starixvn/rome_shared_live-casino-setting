namespace Fanex.LiveCasino.CoreSetting.Services.PTCom
{
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.DTO.CommandResult;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Repositories.PTCom;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Super Edit Master PT COM setting.
    /// </summary>
    public class MasterPTComService : PTComServiceBase
    {
        public MasterPTComService(IPTComRepository ptComRepository)
        {
            PTComRepository = ptComRepository ?? new MasterPTComRepository();
        }

        private bool _isApplyPositionTakingOnMembers { get; set; }

        private float _newPositionTakingOnMember { get; set; }

        public override void SetPTComSetting(Customer customer, LiveCasinoSettingContext settingContext)
        {
            PTComDTO ptComDTO = PTComRepository.Get(new GetSettingForEditMasterCriteria
            {
                CustId = settingContext.Customer.Id,
                SRecommend = settingContext.Upline.Id
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
            var criteria = new UpdateMasterSettingCriteria
            {
                CustId = settingContext.Customer.Id,
                IP = settingContext.IP,
                Language = settingContext.Language,
                SRecommend = settingContext.Upline.Id,
                UserId = settingContext.Actor.Id,
                SubAccID = settingContext.SubAccount.Id,
                MasterCommission = (decimal)Commission,
                M3 = (decimal)PositionTaking.M3,
                N3 = (decimal)PositionTaking.N3,
                Is3 = (bool)PositionTaking.Is3,
                Us3 = (decimal)PositionTaking.Us3,
                UpdateAllMembers = this._isApplyPositionTakingOnMembers,
                S3 = (decimal)PositionTaking.S3,
                NewSuperPositionTakingOnMember = (decimal)this._newPositionTakingOnMember
            };

            if (criteria.Is3)
            {
                criteria.S3 = 0;
            }
            else
            {
                criteria.Us3 = 0;
            }

            ICommandResult commandResult = PTComRepository.Save(criteria);

            return GetResultDetails(commandResult);
        }
    }
}