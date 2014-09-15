// -----------------------------------------------------------------------
// <copyright file="MasterPTComService.cs" company="Microsoft">
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
    public class MasterPTComService : PTComServiceBase
    {
        public MasterPTComService(IPTComRepository ptComRepository)
        {
            base.PTComRepository = ptComRepository ?? new MasterPTComRepository();
        }

        private bool _isApplyPositionTakingOnMembers { get; set; }

        private float _newPositionTakingOnMember { get; set; }

        public override void SetPTComSetting(Customer customer, LiveCasinoSettingContext settingContext)
        {
            PTComDTO ptComDTO = base.PTComRepository.Get(new GetSettingForEditMasterCriteria
            {
                CustId = settingContext.Customer.Id,
                SRecommend = settingContext.Upline.Id
            });

            base.MapSetting(ptComDTO, customer);
        }

        public override ICommandResultsDTO Update(LiveCasinoSettingContext settingContext)
        {
            var criteria = new UpdateMasterSettingCriteria
            {
                CustId = settingContext.Customer.Id,
                IP = settingContext.IP,
                Language = settingContext.Language,
                SRecommend = settingContext.Upline.Id,
                UserId = settingContext.ActorId,
                SubAccID = settingContext.SubAccount.Id,
                MasterCommission = (decimal)base.Commission,
                M3 = (decimal)base.PositionTaking.M3,
                N3 = (decimal)base.PositionTaking.N3,
                Is3 = (bool)base.PositionTaking.Is3,
                Us3 = (decimal)base.PositionTaking.Us3,
                UpdateAllMembers = this._isApplyPositionTakingOnMembers,
                S3 = (decimal)base.PositionTaking.S3,
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

            ICommandResult commandResult = base.PTComRepository.Save(criteria);

            return GetResultDetails(commandResult);
        }
    }
}