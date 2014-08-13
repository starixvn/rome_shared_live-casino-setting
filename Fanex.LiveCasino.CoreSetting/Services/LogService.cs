namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.Configuration;
    using Fanex.LiveCasino.CoreSetting.Criteria.Log;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Models.Log;
    using Fanex.LiveCasino.CoreSetting.Repositories;
    using Fanex.LiveCasino.CoreSetting.Utilities;
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// Log service class.
    /// </summary>
    public class LogService : ILogService
    {
        private readonly ISettingRepository _settingRepository;
        private readonly ICommonUtil _commonUtil;

        public LogService()
            : this(null, null)
        {
        }

        public LogService(ISettingRepository settingRepository, ICommonUtil commonUtil)
        {
            _settingRepository = settingRepository ?? new SettingRepository();
            _commonUtil = commonUtil ?? new CommonUtil();
        }

        /// <summary>
        /// Logs the automatic insert account.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="gameTypes">The game types.</param>
        public void LogAutoInsertAccount(SettingContext settingContext, List<GameType> gameTypes)
        {
            var log = new LogAutoInsertAccount
            {
                Action = GetInsertAutoLogAction(settingContext.TargetRoleId),
                SignUpForm = _commonUtil.LiveCasinoSettings.SystemId,
                GameSetting = gameTypes,
                SettingContext = settingContext
            };

            WriteLog(log);
        }

        /// <summary>
        /// Logs the insert from existing account.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="gameTypes">The game types.</param>
        public void LogInsertFromExistingAccount(SettingContext settingContext, List<GameType> gameTypes)
        {
            var log = new LogInsertFromExistingAccount
            {
                Action = GetInsertFromExistingLogAction(settingContext.TargetRoleId),
                SignUpForm = _commonUtil.LiveCasinoSettings.SystemId,
                GameSetting = gameTypes,
                SettingContext = settingContext
            };

            WriteLog(log);
        }

        /// <summary>
        /// Logs the manual insert account.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        public void LogManualInsertAccount(SettingContext settingContext)
        {
            var log = new LogManualInsertAccount
            {
                Action = GetInsertManualLogAction(settingContext.TargetRoleId),
                SignUpForm = _commonUtil.LiveCasinoSettings.SystemId,
                SettingContext = settingContext
            };

            WriteLog(log);
        }

        /// <summary>
        /// Logs the manual update stake level.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="gameTypeOld">The game type old.</param>
        /// <param name="gameTypeNew">The game type new.</param>
        /// <param name="isInsert">If set to <c>true</c> [is insert].</param>
        public void LogManualUpdateStakeLevel(SettingContext settingContext, GameType gameTypeOld, GameType gameTypeNew, bool isInsert = false)
        {
            var log = new LogUpdateStakeLevel
            {
                Action = GetUpdateStakeLevelLogAction(settingContext.TargetRoleId),
                SettingContext = settingContext,
                NewGameType = gameTypeNew,
                OldGameType = isInsert ? null : gameTypeOld
            };

            if (gameTypeOld == null || gameTypeNew.CheckedStakeLevelId != gameTypeOld.CheckedStakeLevelId)
            {
                WriteLog(log);
            }
        }

        /// <summary>
        /// Writes the log.
        /// </summary>
        /// <param name="logData">The log data.</param>
        /// <returns>Command results.</returns>
        public ICommandResult WriteLog(ILogData logData)
        {
            var criteria = new InsertLogUserRunSPCriteria
            {
                UserId = logData.SettingContext.UserIdDoAction,
                CustId = logData.SettingContext.TargetCustId,
                SpName = logData.SpName,
                Doing = string.Empty,
                ActionId = (int)logData.Action,
                TrackingValue = logData.NewValue,
                TrackingValue2 = logData.OldValue,
                Duration = 0
            };

            var updateResult = (CommandResult)_settingRepository.InsertOrUpdate(criteria);

            return updateResult;
        }

        /// <summary>
        /// Gets the insert automatic log action.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>Log action for insert automatic account.</returns>
        private static LogActions GetInsertAutoLogAction(int roleId)
        {
            LogActions logAction;

            switch (roleId)
            {
                case (int)CustRole.Master:
                    logAction = LogActions.AutoInsert_Master;
                    break;

                case (int)CustRole.Agent:
                    logAction = LogActions.AutoInsert_Agent;
                    break;

                default:
                    logAction = LogActions.AutoInsert_Member;
                    break;
            }

            return logAction;
        }

        /// <summary>
        /// Gets the insert from existing log action.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>Log action for insert from existing account.</returns>
        private static LogActions GetInsertFromExistingLogAction(int roleId)
        {
            LogActions logAction;

            switch (roleId)
            {
                case (int)CustRole.Master:
                    logAction = LogActions.AutoInsertExisting_Master;
                    break;

                case (int)CustRole.Agent:
                    logAction = LogActions.AutoInsertExisting_Agent;
                    break;

                default:
                    logAction = LogActions.AutoInsertExisting_Member;
                    break;
            }

            return logAction;
        }

        /// <summary>
        /// Gets the insert manual log action.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>Log action for manual insert account.</returns>
        private static LogActions GetInsertManualLogAction(int roleId)
        {
            LogActions logAction;

            switch (roleId)
            {
                case (int)CustRole.Super:
                    logAction = LogActions.ManualInsert_Super;
                    break;

                case (int)CustRole.Master:
                    logAction = LogActions.ManualInsert_Master;
                    break;

                case (int)CustRole.Agent:
                    logAction = LogActions.ManualInsert_Agent;
                    break;

                default:
                    logAction = LogActions.ManualInsert_Member;
                    break;
            }

            return logAction;
        }

        /// <summary>
        /// Gets the update stake level log action.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>Log action for update stake level.</returns>
        private static LogActions GetUpdateStakeLevelLogAction(int roleId)
        {
            LogActions logAction;

            switch (roleId)
            {
                case (int)CustRole.Super:
                    logAction = LogActions.UpdateStakeLevel_Super;
                    break;

                case (int)CustRole.Master:
                    logAction = LogActions.UpdateStakeLevel_Master;
                    break;

                case (int)CustRole.Agent:
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