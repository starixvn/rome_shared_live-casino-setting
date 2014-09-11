namespace Fanex.LiveCasino.CoreSetting.Models.GameSettingLog
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.Enumerations;

    /// <summary>
    /// Log insert account when up line is synced Live Casino setting.
    /// </summary>
    public class AutoInsertAccountLog : ManualInsertAccountLog
    {
        private const string GameTypeTemplate = "<GameType GameTypeId=\"{0}\" BetTypeId=\"{1}\" StakeLevel=\"{2}\" />";

        private const string TrackingTemplate = "<{0} CustID=\"{1}\" UserName=\"{2}\" CasinoNo=\"{3}\" ExchangeID=\"{4}\"" +
                                                " SCustID=\"{5}\" MCustID=\"{6}\" ACustID=\"{7}\" RoleID=\"{8}\" SignupIP=\"{9}\"" +
                                                " SignupBy=\"{10}\" AddFrom=\"{11}\" Lang=\"{12}\" ErrCode=\"{13}\" ErrMsg=\"{14}\" {15} >{16}</{17}>";

        public List<GameType> GameSetting { get; set; }

        public override string NewValue
        {
            get
            {
                return GetTrackingValue();
            }
        }

        public override string OldValue
        {
            get
            {
                return string.Empty;
            }
        }

        public override string SpName
        {
            get
            {
                return "TP_User_Customer_AutoInsertAccount";
            }
        }

        /// <summary>
        /// Gets the game type log data.
        /// </summary>
        /// <returns>Game type log data formatted xml.</returns>
        private string GetGameTypeLogData()
        {
            var gameType = string.Empty;

            if (GameSetting != null)
            {
                foreach (var itemGameType in GameSetting)
                {
                    gameType += string.Format(GameTypeTemplate, itemGameType.Id, itemGameType.BetTypeId, itemGameType.CheckedStakeLevelRange);
                }
            }

            return gameType;
        }

        /// <summary>
        /// Gets the tracking value.
        /// </summary>
        /// <returns>Tracking value string formatted xml.</returns>
        private string GetTrackingValue()
        {
            string gameTypeLog = GetGameTypeLogData();

            string trackingValue = string.Format(
                                            TrackingTemplate,
                                            SpName,
                                            SettingContext.TargetCustId,
                                            SettingContext.TargetUsername,
                                            CasinoNo,
                                            SettingContext.ExchangeId,
                                            SettingContext.SuperId,
                                            SettingContext.MasterId,
                                            SettingContext.AgentId,
                                            SettingContext.TargetRole,
                                            SettingContext.IP,
                                            SettingContext.UserIdDoAction,
                                            SignUpForm,
                                            SettingContext.Language,
                                            ErrCode,
                                            ErrMsg,
                                            TrackingSubAccount,
                                            gameTypeLog,
                                            SpName);

            return trackingValue;
        }

        public override LogActions Action
        {
            get
            {
                LogActions logAction;

                switch (SettingContext.TargetRole)
                {
                    case CustomerRole.Master:
                        logAction = LogActions.AutoInsert_Master;
                        break;

                    case CustomerRole.Agent:
                        logAction = LogActions.AutoInsert_Agent;
                        break;

                    default:
                        logAction = LogActions.AutoInsert_Member;
                        break;
                }

                return logAction;
            }
        }
    }
}