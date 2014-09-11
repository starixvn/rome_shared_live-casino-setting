using Fanex.LiveCasino.CoreSetting.Enumerations;
namespace Fanex.LiveCasino.CoreSetting.Models.Log
{
    /// <summary>
    /// Log update stake level.
    /// </summary>
    public class LogUpdateStakeLevel : LogDataBase, ILogData
    {
        private const string TrackingTemplate = "<{0} CustID=\"{1}\" {2} GameTypeId=\"{3}\" BetTypeId=\"{4}\" StakeLevel=\"{5}\"" +
                                                  " Ip=\"{6}\" Lang=\"{7}\" RoleID=\"{8}\" ErrCode=\"{9}\" ErrMsg=\"{10}\" />";

        public GameType NewGameType { get; set; }

        public override string NewValue
        {
            get
            {
                return CreateLogData(NewGameType);
            }
        }

        public GameType OldGameType { get; set; }

        public override string OldValue
        {
            get
            {
                return CreateLogData(OldGameType);
            }
        }

        public override string SpName
        {
            get
            {
                return "TP_User_CustGameType_UpdStakeLevel";
            }
        }

        /// <summary>
        /// Creates the log data.
        /// </summary>
        /// <param name="gameType">Type of the game.</param>
        /// <returns>The tracking value formatted xml.</returns>
        protected string CreateLogData(GameType gameType)
        {
            if (gameType == null)
            {
                return string.Empty;
            }

            var trackingValue = string.Format(
                TrackingTemplate,
                 SpName,
                 SettingContext.TargetCustId,
                 TrackingSubAccount,
                 gameType.Id,
                 gameType.BetTypeId,
                 gameType.CheckedStakeLevelRange,
                 SettingContext.IP,
                 SettingContext.Language,
                 SettingContext.TargetRole,
                 ErrCode,
                 ErrMsg);

            return trackingValue;
        }

        public override LogActions Action
        {
            get
            {
                LogActions logAction;

                switch (SettingContext.TargetRole)
                {
                    case CustomerRole.Super:
                        logAction = LogActions.UpdateStakeLevel_Super;
                        break;

                    case CustomerRole.Master:
                        logAction = LogActions.UpdateStakeLevel_Master;
                        break;

                    case CustomerRole.Agent:
                        logAction = LogActions.UpdateStakeLevel_Agent;
                        break;

                    default:
                        logAction = LogActions.UpdateStakeLevel_Member;
                        break;
                }

                return logAction;
            }
        }
    }
}