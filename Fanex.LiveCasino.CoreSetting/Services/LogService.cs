namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.Criteria.GameSetting;
    using Fanex.LiveCasino.CoreSetting.Criteria.Log;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Models.Log;
    using Fanex.LiveCasino.CoreSetting.Repositories;
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// Log service class.
    /// </summary>
    internal class LogService : ILogService
    {
        private readonly ISettingRepository _settingRepository;

        public LogService()
            : this(null)
        {
        }

        public LogService(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository ?? new SettingRepository();
        }

        /// <summary>
        /// Logs the automatic insert account.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="gameTypes">The game types.</param>
        public void LogAutoInsertAccount(InsertAutoAccountCriteria criteria, List<GameType> gameTypes)
        {
            var log = new LogAutoInsertAccount
            {
                Action = GetInsertManualLogAction(criteria.RoleId),
                AgentId = criteria.AgentId,
                CustId = criteria.CustId,
                CustUserName = criteria.Username,
                ExchangeId = criteria.ExchangeId,
                SignUpIp = criteria.IP,
                SignUpForm = criteria.SignupFrom,
                SubAccId = criteria.SubAccId,
                SubAccName = criteria.SubAccName,
                SuperId = criteria.SuperId,
                UserId = criteria.SignupBy,
                UserName = criteria.ModifyUsername,
                CustSite = criteria.SiteName,
                GameSetting = gameTypes
            };

            WriteLog(log);
        }

        /// <summary>
        /// Logs the insert from existing account.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="gameTypes">The game types.</param>
        public void LogInsertFromExistingAccount(InsertFromExistingAccountCriteria criteria, List<GameType> gameTypes)
        {
            var log = new LogInsertFromExistingAccount
            {
                Action = GetInsertFromExistingLogAction(criteria.RoleId),
                AgentId = criteria.AgentId,
                CustId = criteria.CustId,
                CustUserName = criteria.Username,
                ExchangeId = criteria.ExchangeId,
                SignUpIp = criteria.IP,
                SignUpForm = criteria.SignupFrom,
                SubAccId = criteria.SubAccId,
                SubAccName = criteria.SubAccName,
                SuperId = criteria.SuperId,
                UserId = criteria.SignupBy,
                UserName = criteria.ModifyUsername,
                CustSite = criteria.SiteName,
                GameSetting = gameTypes
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
                AgentId = settingContext.AgentId,
                CustId = settingContext.TargetCustId,
                CustUserName = settingContext.TargetUsername,
                ExchangeId = settingContext.ExchangeId,
                SignUpIp = settingContext.IP,
                SignUpForm = settingContext.SignupFrom,
                SubAccId = settingContext.SubAccId,
                SubAccName = settingContext.SubAccName,
                SuperId = settingContext.SuperId,
                UserId = settingContext.UserIdDoAction,
                UserName = settingContext.UserName,
                CustSite = settingContext.SiteName
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
                CustId = settingContext.TargetCustId,
                SignUpIp = settingContext.IP,
                Language = settingContext.Language,
                UserId = settingContext.UserIdDoAction,
                RoleId = settingContext.TargetRoleId,
                NewGameType = gameTypeNew,
                OldGameType = isInsert ? null : gameTypeOld
            };

            if (gameTypeNew.CheckedStakeLevelId != gameTypeOld.CheckedStakeLevelId)
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
                UserId = logData.UserId,
                CustId = logData.CustId,
                SpName = logData.SpName,
                Doing = string.Empty,
                ActionId = (int)logData.Action,
                TrackingValue = logData.NewValue,
                TrackingValue2 = logData.OldValue,
                Duration = 0
            };

            return _settingRepository.InsertOrUpdate(criteria);
        }

        /// <summary>
        /// Gets the insert automatic log action.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>Log action for insert automatic account.</returns>
        private LogActions GetInsertAutoLogAction(int roleId)
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
        private LogActions GetInsertFromExistingLogAction(int roleId)
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
        private LogActions GetInsertManualLogAction(int roleId)
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
        private LogActions GetUpdateStakeLevelLogAction(int roleId)
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