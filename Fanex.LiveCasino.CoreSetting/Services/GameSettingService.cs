namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Fanex.LiveCasino.CoreSetting.Criteria;
    using Fanex.LiveCasino.CoreSetting.Criteria.GameSetting;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Models.Command;
    using Fanex.LiveCasino.CoreSetting.Repositories;
    using Fanex.LiveCasino.CoreSetting.Resources;
    using Fanex.LiveCasino.CoreSetting.Utilities;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Game setting service class.
    /// </summary>
    public class GameSettingService : IGameSettingService
    {
        private const string CasinoNo = "01";
        private const int TimeOutReplication = 5;
        private readonly ILogService _logService;
        private readonly ISettingRepository _settingRepository;
        private readonly ICommonUtil _commonUtil;

        public GameSettingService()
            : this(null, null, null)
        {
        }

        public GameSettingService(ISettingRepository settingRepository, ILogService logService, ICommonUtil commonUtil)
        {
            _settingRepository = settingRepository ?? new SettingRepository();
            _logService = logService ?? new LogService();
            _commonUtil = commonUtil ?? new CommonUtil();
        }

        /// <summary>
        /// Gets the customer site by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>Customer site name.</returns>
        public string GetCustomerSiteByUsername(string username)
        {
            var siteName = string.Empty;
            var criteria = new GetCustomerSiteByUsernameCriteria
            {
                Username = username
            };

            if (criteria.IsValid())
            {
                var userInfo = _settingRepository.Get<CustInfo>(criteria);
                siteName = userInfo.Site;
            }

            return siteName;
        }

        /// <summary>
        /// Get customer information with customer id.
        /// </summary>
        /// <param name="custId">Get criteria information.</param>
        /// <returns>Customer information.</returns>
        public CustInfo GetCustomerInfo(int custId)
        {
            var custInfo = new CustInfo();
            var criteria = new GetCustomerInfoCriteria
            {
                CustId = custId
            };

            if (criteria.IsValid())
            {
                custInfo = _settingRepository.GetCustomerInfo(criteria);
            }

            return custInfo;
        }

        /// <summary>
        /// Gets the game setting by customer identifier.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <returns>Game setting of customer.</returns>
        public List<GameType> GetGameSettingByCustomerId(int custId)
        {
            var gameSetting = new List<GameType>();
            var criteria = new GetGameSettingByCustomerIdCriteria
            {
                CustId = custId
            };

            if (criteria.IsValid())
            {
                gameSetting = _settingRepository.GetGameSetting(criteria, true);
            }

            return gameSetting;
        }

        /// <summary>
        /// Gets the game setting by exchange.
        /// </summary>
        /// <param name="exchangeId">The exchange identifier.</param>
        /// <param name="site">The site name.</param>
        /// <returns>Game setting by exchange and site.</returns>
        public List<GameType> GetGameSettingByExchange(int exchangeId, string site)
        {
            var gameSetting = new List<GameType>();
            var criteria = new GetGameSettingByExchangeCriteria
            {
                ExchangeId = exchangeId,
                Site = site
            };

            if (criteria.IsValid())
            {
                gameSetting = _settingRepository.GetGameSetting(criteria, false);
            }

            return gameSetting;
        }

        /// <summary>
        /// Gets the game setting.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="custInfo">The customer information.</param>
        /// <returns>Game settings.</returns>
        public List<GameType> GetGameSetting(SettingContext settingContext, CustInfo custInfo)
        {
            var gameSettings = new List<GameType>();

            if (settingContext.IsEdittingSuper && !custInfo.IsSyncLiveCasino)
            {
                var site = GetCustomerSiteByUsername(custInfo.UserName);
                gameSettings = GetGameSettingByExchange(custInfo.CurrencyId, site).ToList();

                // Check all by default
                foreach (var gameSettingItem in gameSettings)
                {
                    foreach (var betLimitItem in gameSettingItem.BettingLimit)
                    {
                        betLimitItem.IsChecked = true;
                    }
                }
            }
            else if (!settingContext.IsEdittingSuper && !custInfo.IsSyncLiveCasino)
            {
                gameSettings = GetGameSettingByCustomerId(settingContext.DirectUplineId);

                if (settingContext.IsEdittingMember)
                {
                    // Check all by default
                    foreach (var gameSettingItem in gameSettings)
                    {
                        gameSettingItem.BettingLimit.RemoveAll(x => !x.IsChecked);
                        for (int i = 1; i < gameSettingItem.BettingLimit.Count; i++)
                        {
                            gameSettingItem.BettingLimit[i].IsChecked = false;
                        }
                    }
                }

                gameSettings.RemoveAll(x => string.IsNullOrWhiteSpace(x.CheckedStakeLevelRange));
            }
            else
            {
                gameSettings = GetGameSettingByCustomerId(settingContext.TargetCustId).ToList();
            }

            return gameSettings;
        }

        /// <summary>
        /// Inserts the automatic account.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <returns>
        /// Command result insert automatic account.
        /// </returns>
        public ICommandResult InsertAutoAccount(SettingContext settingContext)
        {
            if (settingContext.IsEdittingSuper)
            {
                return null;
            }

            var criteria = new InsertAutoAccountCriteria
            {
                AgentId = settingContext.AgentId,
                CasinoNo = CasinoNo,
                CustId = settingContext.TargetCustId,
                ExchangeId = settingContext.ExchangeId,
                MasterId = settingContext.MasterId,
                ModifyUsername = settingContext.UplineUsername,
                RoleId = settingContext.TargetRoleId,
                SignupBy = settingContext.UserIdDoAction,
                SignupFrom = _commonUtil.LiveCasinoSettings.SystemId,
                SignupIp = settingContext.IP,
                SuperId = settingContext.SuperId,
                Username = settingContext.TargetUsername
            };

            var gameTypes = new List<GameType>();
            ICommandResult result = _settingRepository.InsertGameSettingAccount(criteria, ref gameTypes);

            if (result.IsSuccessful)
            {
                Task.Factory.StartNew(() =>
                {
                    if (!gameTypes.Any())
                    {
                        gameTypes = GetGameSetting(settingContext.TargetCustId, settingContext.TargetUsername, settingContext.ExchangeId).ToList();
                    }

                    _logService.LogAutoInsertAccount(settingContext, gameTypes);
                });

                ICommandResult updateSyncResult = UpdateSyncLiveCasino(settingContext, isSynced: true);

                if (updateSyncResult.IsSuccessful)
                {
                    var uplineCustInfo = GetCustomerInfo(settingContext.DirectUplineId);
                    UpdateLiveCasinoStatus(settingContext, uplineCustInfo.DisableLiveCasino);
                }
            }

            return result;
        }

        /// <summary>
        /// Inserts from existing account.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <returns>
        /// Command result insert from existing account.
        /// </returns>
        public ICommandResult InsertFromExistingAccount(SettingContext settingContext)
        {
            var criteria = new InsertFromExistingAccountCriteria
            {
                AgentId = settingContext.AgentId,
                CasinoNo = CasinoNo,
                CustId = settingContext.TargetCustId,
                ExchangeId = settingContext.ExchangeId,
                MasterId = settingContext.MasterId,
                ModifyUsername = settingContext.UplineUsername,
                RoleId = settingContext.TargetRoleId,
                SignupBy = settingContext.UserIdDoAction,
                SignupFrom = _commonUtil.LiveCasinoSettings.SystemId,
                SignupIp = settingContext.IP,
                SuperId = settingContext.SuperId,
                Username = settingContext.TargetUsername,
                OldCustId = settingContext.OldCustId,
                ToUserName = settingContext.ToUsername
            };

            var gameTypes = new List<GameType>();
            ICommandResult result = _settingRepository.InsertGameSettingAccount(criteria, ref gameTypes);

            if (result.IsSuccessful)
            {
                Task.Factory.StartNew(() =>
                {
                    if (!gameTypes.Any())
                    {
                        gameTypes = GetGameSetting(settingContext.UplineId, settingContext.TargetUsername, settingContext.ExchangeId).ToList();
                    }

                    _logService.LogInsertFromExistingAccount(settingContext, gameTypes);
                });

                ICommandResult updateSyncResult = UpdateSyncLiveCasino(settingContext, isSynced: true);

                if (updateSyncResult.IsSuccessful)
                {
                    var fromCustInfo = GetCustomerInfo(settingContext.OldCustId);
                    UpdateLiveCasinoStatus(settingContext, fromCustInfo.DisableLiveCasino);
                }
            }

            return result;
        }

        /// <summary>
        /// Updates the live casino status.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="disableLiveCasino">If set to <c>true</c> [disable live casino].</param>
        /// <returns>
        /// Command result update status.
        /// </returns>
        public ICommandResult UpdateLiveCasinoStatus(SettingContext settingContext, bool disableLiveCasino)
        {
            var criteria = new UpdateLiveCasinoStatusCriteria
            {
                CustId = settingContext.TargetCustId,
                UserID = settingContext.UserIdDoAction,
                DisabledLiveCS = disableLiveCasino,
                EnableDownlinesLiveCS = !disableLiveCasino,
                IP = settingContext.IP,
                Language = settingContext.Language
            };

            var updateResult = (CommandResult)_settingRepository.InsertOrUpdate(criteria);

            if (updateResult.IsSuccessful)
            {
                updateResult = new CommandResult
                {
                    Code = 0,
                    Message = LiveCasinoLang.UpdateLiveCasinoStatusSuccess
                };
            }

            return updateResult;
        }

        /// <summary>
        /// Updates the manual.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="custInfo">The customer information.</param>
        /// <param name="gameTypes">The game types.</param>
        /// <returns>
        /// Game setting command result.
        /// </returns>
        public ICommandResults UpdateManual(SettingContext settingContext, CustInfo custInfo, List<GameType> gameTypes)
        {
            var isInsert = false;

            if (!custInfo.IsSyncLiveCasino)
            {
                isInsert = ManualSyncAccount(settingContext);

                // For case upline is synced with unchecked all stake level
                if (gameTypes == null)
                {
                    CustInfo directUplineInfo = GetCustomerInfo(settingContext.DirectUplineId);
                    if (directUplineInfo.IsSyncLiveCasino)
                    {
                        string site = GetCustomerSiteByUsername(custInfo.UserName);
                        gameTypes = GetGameSettingByExchange(directUplineInfo.CurrencyId, site);
                    }
                }
            }

            List<GameType> gameTypeOlds = GetGameSettingByCustomerId(settingContext.TargetCustId);

            ICommandResults resultUpdateStakeLevels = UpdateListStakeLevel(settingContext.TargetCustId, settingContext.TargetRoleId, gameTypes, settingContext.Language);

            WriteLogUpdateStakeLevel(settingContext, gameTypes, isInsert, gameTypeOlds, resultUpdateStakeLevels);

            return resultUpdateStakeLevels;
        }

        /// <summary>
        /// Updates the manual multiple.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="gameTypes">The game types.</param>
        /// <returns>Multiple result command.</returns>
        public IMultipleCommandResults UpdateManualMultiple(SettingContext settingContext, List<GameType> gameTypes)
        {
            var results = new MultipleCommandResults();
            List<CustInfo> custInfos = _commonUtil.GetCustomerInfo(settingContext.TargetCustIds, settingContext.TargetUsernames, settingContext.TargetSyncStatus);

            foreach (var custInfo in custInfos)
            {
                settingContext.TargetCustId = custInfo.CustId;
                var updateResult = (CommandResults)UpdateManual(settingContext, custInfo, gameTypes);

                updateResult.Id = custInfo.CustId;
                updateResult.Name = custInfo.UserName;

                results.CommandResultsList.Add(updateResult);
            }

            return results;
        }

        /// <summary>
        /// Updates the synchronize live casino.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="isSynced">If set to <c>true</c> [is synchronize].</param>
        /// <returns>Command result update sync.</returns>
        public ICommandResult UpdateSyncLiveCasino(SettingContext settingContext, bool isSynced)
        {
            var criteria = new UpdateSyncLiveCasinoCriteria
            {
                CustId = settingContext.TargetCustId,
                UserId = settingContext.UserIdDoAction,
                Ip = settingContext.IP,
                Language = settingContext.Language,
                SubAccId = settingContext.SubAccId,
                SyncLiveCS = isSynced
            };

            var updateResult = (CommandResult)_settingRepository.InsertOrUpdate(criteria);

            return updateResult;
        }

        /// <summary>
        /// Manuals the synchronize account.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <returns>Insert successfully.</returns>
        private bool ManualSyncAccount(SettingContext settingContext)
        {
            ICommandResult resultInsertAccount = InsertManualAccount(settingContext);

            if (resultInsertAccount.IsSuccessful)
            {
                _logService.LogManualInsertAccount(settingContext);
                var syncResult = (CommandResult)UpdateSyncLiveCasino(settingContext, isSynced: true);
                var disableLiveCasino = false;

                if (!settingContext.IsEdittingSuper)
                {
                    var uplineCustInfo = GetCustomerInfo(settingContext.DirectUplineId);
                    disableLiveCasino = uplineCustInfo.DisableLiveCasino;
                }

                if (syncResult.IsSuccessful)
                {
                    UpdateLiveCasinoStatus(settingContext, disableLiveCasino);
                }
            }

            return resultInsertAccount.IsSuccessful;
        }

        /// <summary>
        /// Writes the log update stake level.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="gameTypes">The game types.</param>
        /// <param name="isInsert">If set to <c>true</c> [is insert].</param>
        /// <param name="gameTypeOlds">The game type olds.</param>
        /// <param name="resultUpdateStakeLevels">The result update stake levels.</param>
        private void WriteLogUpdateStakeLevel(
            SettingContext settingContext,
            List<GameType> gameTypes,
            bool isInsert,
            List<GameType> gameTypeOlds,
            ICommandResults resultUpdateStakeLevels)
        {
            if (resultUpdateStakeLevels.IsSuccessful)
            {
                var successResultUpdateStakeLevel = resultUpdateStakeLevels.Results.Where(x => x.IsSuccessful);

                foreach (var itemResult in successResultUpdateStakeLevel)
                {
                    var gameSettingResult = (GameSettingCommandResult)itemResult;

                    var newGameType = gameTypes.Where(x => x.Id == gameSettingResult.Id).FirstOrDefault();

                    if (newGameType != null)
                    {
                        var oldGameType = gameTypeOlds.Where(x => x.Id == newGameType.Id).FirstOrDefault();
                        _logService.LogManualUpdateStakeLevel(settingContext, oldGameType, newGameType, isInsert);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the game setting with time out for waiting replication.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="username">The username.</param>
        /// <param name="exchangeId">The exchange identifier.</param>
        /// <param name="timeOut">The time out.</param>
        /// <returns>
        /// Game settings.
        /// </returns>
        private List<GameType> GetGameSetting(int custId, string username, int exchangeId, int timeOut = TimeOutReplication)
        {
            var gameTypes = new List<GameType>();

            // Waiting for replication
            for (int i = 0; i < timeOut; i++)
            {
                Thread.Sleep(1000);
                gameTypes = GetGameSettingByCustomerId(custId).ToList();

                if (gameTypes.Any())
                {
                    break;
                }
            }

            if (!gameTypes.Any())
            {
                var site = GetCustomerSiteByUsername(username);
                gameTypes = GetGameSettingByExchange(exchangeId, site).ToList();
            }

            return gameTypes;
        }

        /// <summary>
        /// Inserts the manual account.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <returns>Command result insert manual account.</returns>
        private ICommandResult InsertManualAccount(SettingContext settingContext)
        {
            var criteria = new InsertManualAccountCriteria
            {
                CustId = settingContext.TargetCustId,
                CasinoNo = CasinoNo,
                ExchangeId = settingContext.ExchangeId,
                SuperId = settingContext.SuperId,
                MasterId = settingContext.MasterId,
                AgentId = settingContext.AgentId,
                RoleId = settingContext.TargetRoleId,
                SignupIp = settingContext.IP,
                SignupBy = settingContext.UserIdDoAction,
                SignupFrom = _commonUtil.LiveCasinoSettings.SystemId,
                ModifyUsername = settingContext.UplineUsername,
                Username = settingContext.TargetUsername,
                CustSite = settingContext.SiteName
            };

            var updateResult = (CommandResult)_settingRepository.InsertOrUpdate(criteria);

            return updateResult;
        }

        /// <summary>
        /// Updates the list stake level.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <param name="gameTypes">The game types.</param>
        /// <param name="language">The language.</param>
        /// <returns>
        /// Command results update list stake level.
        /// </returns>
        private ICommandResults UpdateListStakeLevel(int custId, int roleId, List<GameType> gameTypes, string language)
        {
            var results = new CommandResults();

            if (gameTypes != null && gameTypes.Any())
            {
                foreach (var itemGameType in gameTypes)
                {
                    var result = UpdateStakeLevel(custId, roleId, itemGameType, language);
                    results.Results.Add(result);
                }
            }

            return results;
        }

        /// <summary>
        /// Updates the stake level.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <param name="gameType">Type of the game.</param>
        /// <param name="language">The language.</param>
        /// <returns>
        /// Command result update stake level.
        /// </returns>
        private ICommandResult UpdateStakeLevel(int custId, int roleId, GameType gameType, string language)
        {
            var criteria = new UpdateStakeLevelCriteria
            {
                CustId = custId,
                RoleId = roleId,
                GameTypeId = gameType.Id,
                StakeLevelList = gameType.CheckedStakeLevelId
            };

            var updateResult = (CommandResult)_settingRepository.InsertOrUpdate(criteria);

            var gameSettingResult = new GameSettingCommandResult(updateResult);

            if (gameSettingResult.IsSuccessful)
            {
                gameSettingResult.Id = gameType.Id;
                gameSettingResult.Message = string.Format(LiveCasinoLang.UpdateLiveCasinoGameSettingSuccess, gameType.Name);
            }
            else
            {
                gameSettingResult.Message = string.Format(LiveCasinoLang.UpdateLiveCasinoGameSettingFail, gameType.Name, gameSettingResult.Message);
            }

            return gameSettingResult;
        }
    }
}