// -----------------------------------------------------------------------
// <copyright file="AgentPTComService.cs" company="Microsoft">
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
    public class AgentPTComService : PTComServiceBase
    {
        public AgentPTComService(IPTComRepository ptComRepository)
        {
            base.PTComRepository = ptComRepository ?? new AgentPTComRepository();
        }

        private bool _isApplyPositionTakingOnMembers { get; set; }

        private float _newPositionTakingOnMember { get; set; }

        public override void SetModifiedData(float commission, PositionTaking positionTaking, bool isApplyPositionTakingOnMembers = false, float newPositionTakingOnMembers = 0)
        {
            base.SetModifiedData(commission, positionTaking);

            this._isApplyPositionTakingOnMembers = isApplyPositionTakingOnMembers;
            this._newPositionTakingOnMember = newPositionTakingOnMembers;
        }

        public override void SetPTComSetting(Customer customer, LiveCasinoSettingContext settingContext)
        {
            PTComDTO ptComDTO = base.PTComRepository.Get(new GetSettingForEditAgentCriteria
            {
                CustId = settingContext.Customer.Id,
                MRecommend = settingContext.Upline.Id
            });

            base.MapSetting(ptComDTO, customer);
        }

        public override ICommandResultsDTO Update(LiveCasinoSettingContext settingContext)
        {
            var criteria = new UpdateAgentSettingCriteria
            {
                CustId = settingContext.Customer.Id,
                IP = settingContext.IP,
                Language = settingContext.Language,
                MRecommend = settingContext.Upline.Id,
                UserId = settingContext.ActorId,
                SubAccID = settingContext.SubAccount.Id,
                AgentCommission = (decimal)base.Commission,
                M2 = (decimal)base.PositionTaking.M2,
                N2 = (decimal)base.PositionTaking.N2,
                Im2 = (bool)base.PositionTaking.Im2,
                Um2 = (decimal)base.PositionTaking.Um2,
                UpdateAllMembers = (bool)this._isApplyPositionTakingOnMembers,
                A2 = (decimal)base.PositionTaking.A2,
                NewMasterPtOnMember = (decimal)this._newPositionTakingOnMember
            };

            if (criteria.Im2)
            {
                criteria.M2 = 0;
            }
            else
            {
                criteria.Um2 = 0;
            }

            ICommandResult commandResult = base.PTComRepository.Save(criteria);

            return GetResultDetails(commandResult);
        }
    }
}