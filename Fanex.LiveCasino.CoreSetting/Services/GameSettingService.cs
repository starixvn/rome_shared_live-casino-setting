namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Fanex.LiveCasino.CoreSetting.Configuration;
    using Fanex.LiveCasino.CoreSetting.Criteria;
    using Fanex.LiveCasino.CoreSetting.Criteria.GameSetting;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Models.Command;
    using Fanex.LiveCasino.CoreSetting.Repositories;
    using Fanex.LiveCasino.CoreSetting.Resources;
    using Fanex.NAP.Framework.Command;
    using RemotingRef;

    /// <summary>
    /// Game setting service class.
    /// </summary>
    public class GameSettingService : IGameSettingService
    {
        private const string CasinoNo = "01";
        private const int TimeOutReplication = 5;
        private readonly ILogService _logService = new LogService();
        private readonly ISettingRepository _settingRepository;

        public GameSettingService()
            : this(null)
        {
        }

        public GameSettingService(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository ?? new SettingRepository();
        }

        public string GetCustomerSiteByUsername(string username)
        {
            var userInfo = _settingRepository.Get<CustInfo>(new GetCustomerSiteByUsernameCriteria
            {
                Username = username
            });

            return userInfo.Site;
        }

        /// <summary>
        /// Gets the game setting.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="custInfo">The customer information.</param>
        /// <returns>Game settings.</returns>
        public IList<GameType> GetGameSetting(SettingContext settingContext, CustInfo custInfo)
        {
            var gameSettings = new List<GameType>();

            if (settingContext.IsEdittingSuper && !custInfo.IsSyncLiveCasino)
            {
                var site = GetCustomerSiteByUsername(custInfo.UserName);
                gameSettings = GetGameSettingByExchange(custInfo.CurrencyId, site);

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
                gameSettings = GetGameSettingByCustomerId(settingContext.CustId);
            }
            else
            {
                gameSettings = GetGameSettingByCustomerId(settingContext.TargetCustId);
            }

            foreach (var gameSettingItem in gameSettings)
            {
                gameSettingItem.Name = RefData.GetBetTypeName(gameSettingItem.BetTypeId.ToString(), settingContext.Language);
            }

            return gameSettings;
        }

        /// <summary>
        /// Inserts the automatic account.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Command result insert automatic account.</returns>
        public ICommandResult InsertAutoAccount(InsertAutoAccountCriteria criteria)
        {
            var gameTypes = new List<GameType>();
            ICommandResult result = _settingRepository.InsertGameSettingAccount(criteria, ref gameTypes);

            Task.Factory.StartNew(() =>
            {
                if (!gameTypes.Any())
                {
                    gameTypes = GetGameSetting(criteria.CustId, criteria.ExchangeId, criteria.SiteName).ToList();
                }

                _logService.LogAutoInsertAccount(criteria, gameTypes);
            });

            return result;
        }

        /// <summary>
        /// Inserts from existing account.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Command result insert from existing account.</returns>
        public ICommandResult InsertFromExistingAccount(InsertFromExistingAccountCriteria criteria)
        {
            var gameTypes = new List<GameType>();
            ICommandResult result = _settingRepository.InsertGameSettingAccount(criteria, ref gameTypes);

            Task.Factory.StartNew(() =>
            {
                if (!gameTypes.Any())
                {
                    gameTypes = GetGameSetting(criteria.CustId, criteria.ExchangeId, criteria.SiteName).ToList();
                }

                _logService.LogInsertFromExistingAccount(criteria, gameTypes);
            });

            return result;
        }

        /// <summary>
        /// Updates the live casino status.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="custInfo">The customer information.</param>
        /// <returns>Command result update status.</returns>
        public ICommandResult UpdateLiveCasinoStatus(SettingContext settingContext, CustInfo custInfo)
        {
            var criteria = new UpdateLiveCasinoStatusCriteria
            {
                Custid = settingContext.TargetCustId,
                UserID = settingContext.UserIdDoAction,
                DisabledLiveCS = custInfo.DisableLiveCasino,
                IP = settingContext.IP,
                Language = settingContext.Language
            };

            ICommandResult updateResult = _settingRepository.InsertOrUpdate(criteria);

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
        /// <returns>Game setting command result.</returns>
        public ICommandResults UpdateManual(SettingContext settingContext, CustInfo custInfo, List<GameType> gameTypes)
        {
            var isInsert = false;

            if (!custInfo.IsSyncLiveCasino)
            {
                ICommandResult resultInsertAccount = InsertManualAccount(settingContext);

                if (resultInsertAccount.IsSuccessful)
                {
                    _logService.LogManualInsertAccount(settingContext);
                    CommandResult syncResult = (CommandResult)UpdateSyncLiveCasino(settingContext, true);

                    if (syncResult.IsSuccessful)
                    {
                        custInfo.DisableLiveCasino = false;
                        UpdateLiveCasinoStatus(settingContext, custInfo);
                    }

                    isInsert = true;
                }
            }

            List<GameType> gameTypeOlds = GetGameSettingByCustomerId(settingContext.TargetCustId);

            ICommandResults resultUpdateStakeLevels = UpdateListStakeLevel(settingContext.TargetCustId, settingContext.TargetRoleId, gameTypes, settingContext.Language);

            if (resultUpdateStakeLevels.IsSuccessful)
            {
                var successResultUpdateStakeLevel = resultUpdateStakeLevels.Results.Where(x => x.IsSuccessful);

                foreach (var itemResult in successResultUpdateStakeLevel)
                {
                    var gameSettingResult = (GameSettingCommandResult)itemResult;

                    // When update game setting successfully game type id will be set to message property
                    var newGameType = gameTypes.Where(x => x.Id.ToString() == gameSettingResult.Message).FirstOrDefault();

                    if (newGameType != null)
                    {
                        var oldGameType = gameTypeOlds.Where(x => x.Id == newGameType.Id).FirstOrDefault();
                        _logService.LogManualUpdateStakeLevel(settingContext, oldGameType, newGameType, isInsert);

                        var betType = RefData.GetBetTypeName(newGameType.BetTypeId.ToString(), settingContext.Language);
                        gameSettingResult.Message = string.Format(LiveCasinoLang.UpdateLiveCasinoGameSettingSuccess, betType);
                    }
                }
            }

            return resultUpdateStakeLevels;
        }

        /// <summary>
        /// Updates the synchronize live casino.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="isSync">If set to <c>true</c> [is synchronize].</param>
        /// <returns>Command result update sync.</returns>
        public ICommandResult UpdateSyncLiveCasino(SettingContext settingContext, bool isSync)
        {
            var criteria = new UpdateSyncLiveCasinoCriteria
            {
                CustId = settingContext.TargetCustId,
                UserId = settingContext.UserIdDoAction,
                Ip = settingContext.IP,
                Language = settingContext.Language,
                SubAccId = settingContext.SubAccId,
                SyncLiveCS = isSync
            };

            return _settingRepository.InsertOrUpdate(criteria);
        }

        /// <summary>
        /// Gets the game setting with time out for waiting replication.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="exchangeId">The exchange identifier.</param>
        /// <param name="site">The site name.</param>
        /// <param name="timeOut">The time out.</param>
        /// <returns>Game settings.</returns>
        private IList<GameType> GetGameSetting(int custId, int exchangeId, string site, int timeOut = TimeOutReplication)
        {
            var gameTypes = new List<GameType>();

            // Waiting for replication
            for (int i = 0; i < timeOut; i++)
            {
                Thread.Sleep(1000);
                gameTypes = GetGameSettingByCustomerId(custId);

                if (gameTypes.Any())
                {
                    break;
                }
            }

            if (!gameTypes.Any())
            {
                gameTypes = GetGameSettingByExchange(exchangeId, site);
            }

            return gameTypes;
        }

        private List<GameType> GetGameSettingByCustomerId(int custId)
        {
            var criteria = new GetGameSettingByCustomerIdCriteria
            {
                CustId = custId
            };

            return _settingRepository.GetGameSetting(criteria, true);
        }

        private List<GameType> GetGameSettingByExchange(int exchangeId, string site)
        {
            var criteria = new GetGameSettingByExchangeCriteria
            {
                ExchangeId = exchangeId,
                Site = site
            };

            return _settingRepository.GetGameSetting(criteria, false);
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
                SignupFrom = LiveCasinoSettings.Settings.SystemId,
                ModifyUsername = settingContext.UserName,
                Username = settingContext.TargetUsername,
                CustSite = settingContext.SiteName
            };

            return _settingRepository.InsertOrUpdate(criteria);
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

            foreach (var itemGameType in gameTypes)
            {
                var result = UpdateStakeLevel(custId, roleId, itemGameType, language);
                results.Results.Add(result);
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

            var result = (CommandResult)_settingRepository.InsertOrUpdate(criteria);

            var gameSettingResult = new GameSettingCommandResult(result);

            if (gameSettingResult.IsSuccessful)
            {
                gameSettingResult.Message = gameType.Id.ToString();
            }
            else
            {
                var betType = RefData.GetBetTypeName(gameType.BetTypeId.ToString(), language);
                gameSettingResult.Message = string.Format(LiveCasinoLang.UpdateLiveCasinoGameSettingFail, betType, gameSettingResult.Message);
            }

            return gameSettingResult;
        }
    }
}