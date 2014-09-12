namespace Fanex.LiveCasino.CoreSetting.Models.GameSettingLog
{
    using Fanex.LiveCasino.CoreSetting.Enumerations;

    /// <summary>
    /// Log update stake level.
    /// </summary>
    public class LogUpdateStakeLevel : GameSettingLog, IGameSettingLog
    {
        private const string TrackingTemplate = "<{0} CustID=\"{1}\" {2} GameTypeId=\"{3}\" BetTypeId=\"{4}\" StakeLevel=\"{5}\"" +
                                                  " Ip=\"{6}\" Lang=\"{7}\" RoleID=\"{8}\" ErrCode=\"{9}\" ErrMsg=\"{10}\" />";

        public override LogActions Action
        {
            get
            {
                LogActions logAction;

                switch (SettingContext.CustomerLevel)
                {
                    case CustomerLevel.Super:
                        logAction = LogActions.UpdateStakeLevel_Super;
                        break;

                    case CustomerLevel.Master:
                        logAction = LogActions.UpdateStakeLevel_Master;
                        break;

                    case CustomerLevel.Agent:
                        logAction = LogActions.UpdateStakeLevel_Agent;
                        break;

                    default:
                        logAction = LogActions.UpdateStakeLevel_Member;
                        break;
                }

                return logAction;
            }
        }

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
                     SettingContext.CustomerId,
                     TrackingSubAccount,
                     gameType.Id,
                     gameType.BetTypeId,
                     gameType.CheckedStakeLevelRange,
                     SettingContext.IP,
                     SettingContext.Language,
                     SettingContext.CustomerLevel,
                     ErrCode,
                     ErrMsg);

            return trackingValue;
        }
    }
}