namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Common;
    using Fanex.LiveCasino.CoreSetting.Configuration;
    using Fanex.LiveCasino.CoreSetting.Constant;
    using Fanex.LiveCasino.CoreSetting.Criteria;
    using Fanex.LiveCasino.CoreSetting.Criteria.GameSetting;
    using Fanex.LiveCasino.CoreSetting.Criteria.Log;
    using Fanex.LiveCasino.CoreSetting.Criteria.WinLimit;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.DTO.CommandResult;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Models.Log;
    using Fanex.LiveCasino.CoreSetting.Repositories;
    using Fanex.LiveCasino.CoreSetting.Resources;
    using Fanex.Logging;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Setting service for update position taking setting, Commission, member win limit.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
    public class SettingService : ISettingService
    {
        private const string CasinoNo = "01";
        private readonly ISettingRepository _settingRepository;
        private readonly IPTComSettingService _pTComSettingService;

        public SettingService()
            : this(null, null)
        {
        }

        public SettingService(
            ISettingRepository settingRepository,
            IPTComSettingService pTComSettingService)
        {
            _settingRepository = settingRepository ?? new SettingRepository();
            _pTComSettingService = pTComSettingService ?? new PTComSettingService();
        }

        public Customer GetCustomer(SettingContext settingContext)
        {
            Customer customer = GetCustomerInfo(settingContext.TargetCustId);
            _pTComSettingService.SetCustomerPtAndComSetting(
                customer,
                settingContext.TargetRole,
                settingContext.TargetCustId,
                settingContext.UplineRole,
                settingContext.UplineId);
            customer.GameSetting = GetGameSetting(settingContext);

            return customer;
        }

        /// <summary>
        /// Get Representative customer for editing multiple.
        /// </summary>
        /// <param name="settingContext">Setting context for getting customer id list, username list, status list.</param>
        /// <returns>Customer id representative.</returns>
        public int GetRepresentativeCustIdForEditingMultiple(SettingContext settingContext)
        {
            var custInfoList = GetCustomerInfo(settingContext.TargetCustIds, settingContext.TargetUsernames, settingContext.TargetSyncStatus);

            if (!custInfoList.Any())
            {
                return 0;
            }

            //// Edit multiple direct downline. Load setting info of the first selected downline in customer list
            if (settingContext.IsEditDirectDownline)
            {
                return Converter.ToInt(custInfoList[0].CustId);
            }

            //// Super/Master multiple member
            var newCustIdList = string.Empty;
            var newUsernameList = string.Empty;
            var newTargetSyncList = string.Empty;
            var firstSyncCustId = 0;

            foreach (var custInfo in custInfoList)
            {
                if (custInfo.IsSyncLiveCasino)
                {
                    if (firstSyncCustId == 0)
                    {
                        firstSyncCustId = custInfo.CustId;
                    }

                    newCustIdList += custInfo.CustId.ToString() + ",";
                    newUsernameList += custInfo.UserName + ",";
                    newTargetSyncList += (custInfo.IsSyncLiveCasino ? "1" : "0") + ",";
                }
            }

            settingContext.TargetCustIds = newCustIdList.TrimEnd(',');
            settingContext.TargetUsernames = newUsernameList.TrimEnd(',');
            settingContext.TargetSyncStatus = newTargetSyncList.TrimEnd(',');

            return firstSyncCustId;
        }

        /// <summary>
        /// Update single customer all setting.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="customer">Customer setting information for updating.</param>
        /// <returns>Update result.</returns>
        public ICommandResultsDTO UpdateSingleCustomerAllSetting(SettingContext settingContext,
            Customer customer,
            UplinePositionTakingOnMember uplinePositionTakingOnMember)
        {
            ICommandResultsDTO resultList = new CommandResultsDTO();

            if (settingContext.IsAgentEditingMember
                && customer.IsSyncLiveCasino
                && !customer.UplineDisableLiveCasino)
            {
                resultList.Results.Add(UpdateLiveCasinoStatus(settingContext, customer.DisableLiveCasino));
            }

            var positionTakingAndCommResults = _pTComSettingService.UpdatePtComSetting(settingContext, customer.Commission, customer.PositionTaking, uplinePositionTakingOnMember);
            resultList.Results.AddRange(positionTakingAndCommResults.Results);

            if (customer.IsSyncLiveCasino
                || (!customer.IsSyncLiveCasino && positionTakingAndCommResults.IsSuccessful))
            {
                var winLimitResults = UpdateWinLimit(settingContext, customer.MemberWinLimit).Results;
                resultList.Results.AddRange(winLimitResults);

                var gameSettingResult = UpdateManual(settingContext, customer.GameSetting).Results;
                resultList.Results.AddRange(gameSettingResult);
            }
            else
            {
                resultList.Results.Add(new CommandResultDTO
                {
                    Code = ErrCode.UpdateGameSettingFail,
                    Message = string.Format("{0}: {1}", LiveCasinoLang.lblGameSetting, LiveCasinoLang.UpdatedUnsuccessfully)
                });
            }

            return resultList;
        }

        /// <summary>
        /// Update multiple customer all setting.
        /// </summary>
        /// <param name="settingContext">Setting context for updating.</param>
        /// <param name="customer">Customer setting contains customer setting information.</param>
        /// <param name="updateType">Update type for multiple.</param>
        /// <returns>Multiple result return to user.</returns>
        public IMultipleCommandResultsDTO UpdateMultipleCustomerAllSetting(
            SettingContext settingContext,
            Customer customer,
            UplinePositionTakingOnMember uplinePositionTakingOnMember,
            int updateType)
        {
            var custInfoes = GetCustomerInfo(settingContext.TargetCustIds, settingContext.TargetUsernames, settingContext.TargetSyncStatus);

            IMultipleCommandResultsDTO mainResults = new MultipleCommandResultsDTO();

            foreach (var custInfo in custInfoes)
            {
                settingContext.TargetCustId = custInfo.CustId;
                settingContext.TargetUsername = custInfo.UserName;
                customer.IsSyncLiveCasino = custInfo.IsSyncLiveCasino;
                var subResult = UpdateEachCustomerInMultipleEdit(settingContext, customer, updateType, uplinePositionTakingOnMember);
                subResult.Id = custInfo.CustId;
                subResult.Name = custInfo.UserName;
                mainResults.CommandResultsList.Add(subResult);
            }

            return mainResults;
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
                var userInfo = _settingRepository.Get<Customer>(criteria);
                siteName = userInfo.Site;
            }

            return siteName;
        }

        /// <summary>
        /// Get customer information with customer id.
        /// </summary>
        /// <param name="custId">Get criteria information.</param>
        /// <returns>Customer information.</returns>
        public Customer GetCustomerInfo(int custId)
        {
            var criteria = new GetCustomerInfoCriteria
            {
                CustId = custId
            };

            Customer custInfo = _settingRepository.GetCustomerInfo(criteria);

            return custInfo;
        }

        /// <summary>
        /// Gets the game setting by customer identifier.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <returns>Game setting of customer.</returns>
        public List<GameType> GetGameSettingByCustomerId(int custId)
        {
            var criteria = new GetGameSettingByCustomerIdCriteria
            {
                CustId = custId
            };

            List<GameType> gameSetting = _settingRepository.GetGameSetting(criteria, hasCheckedColumn: true);

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
            var criteria = new GetGameSettingByExchangeCriteria
            {
                ExchangeId = exchangeId,
                Site = site
            };

            List<GameType> gameSetting = _settingRepository.GetGameSetting(criteria, hasCheckedColumn: false);

            return gameSetting;
        }

        /// <summary>
        /// Gets the game setting.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="customer">The customer information.</param>
        /// <returns>Game settings.</returns>
        public List<GameType> GetGameSetting(SettingContext settingContext)
        {
            var gameSettings = new List<GameType>();

            if (settingContext.IsEdittingSuper && !settingContext.IsTargetSyncedLiveCS)
            {
                var site = GetCustomerSiteByUsername(settingContext.TargetUsername);
                gameSettings = GetGameSettingByExchange(settingContext.ExchangeId, site).ToList();

                // Check all by default
                foreach (var gameSettingItem in gameSettings)
                {
                    foreach (var betLimitItem in gameSettingItem.BettingLimit)
                    {
                        betLimitItem.IsChecked = true;
                    }
                }
            }
            else if (!settingContext.IsEdittingSuper && !settingContext.IsTargetSyncedLiveCS)
            {
                gameSettings = GetGameSettingByCustomerId(settingContext.DirectUplineId);

                gameSettings.RemoveAll(x => string.IsNullOrWhiteSpace(x.CheckedStakeLevelRange));

                // Check all by default
                foreach (var gameSettingItem in gameSettings)
                {
                    gameSettingItem.BettingLimit.RemoveAll(x => !x.IsChecked);
                    if (settingContext.IsEdittingMember)
                    {
                        for (int i = 1; i < gameSettingItem.BettingLimit.Count; i++)
                        {
                            gameSettingItem.BettingLimit[i].IsChecked = false;
                        }
                    }
                }
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
                return new CommandResultDTO { Code = ErrCode.InvalidCustomer, Message = LiveCasinoLang.InvalidCustomer };
            }

            var criteria = new InsertAutoAccountCriteria
            {
                AgentId = settingContext.AgentId,
                CasinoNo = CasinoNo,
                CustId = settingContext.TargetCustId,
                ExchangeId = settingContext.ExchangeId,
                MasterId = settingContext.MasterId,
                ModifyUsername = settingContext.UplineUsername,
                RoleId = (int)settingContext.TargetRole,
                SignupBy = settingContext.UserIdDoAction,
                SignupFrom = LiveCasinoSettings.Settings.SystemId,
                SignupIp = settingContext.IP,
                SuperId = settingContext.SuperId,
                Username = settingContext.TargetUsername
            };

            var gameTypes = new List<GameType>();
            ICommandResult result = _settingRepository.InsertGameSettingAccount(criteria, ref gameTypes);

            if (result.IsSuccessful)
            {
                ICommandResult updateSyncResult = UpdateSyncLiveCasino(settingContext, isSynced: true);

                if (updateSyncResult.IsSuccessful)
                {
                    var uplineCustInfo = GetCustomerInfo(settingContext.DirectUplineId);
                    UpdateLiveCasinoStatus(settingContext, uplineCustInfo.DisableLiveCasino);
                }

                if (gameTypes == null || !gameTypes.Any())
                {
                    gameTypes = GetGameSettingByCustomerId(settingContext.DirectUplineId);

                    Logger.Log.Info(
                        string.Format(LiveCasinoLang.AutoInsertAccountLogMsg, settingContext.TargetCustId, settingContext.DirectUplineId),
                        LiveCasinoSettings.Settings.LogCategory);
                }

                var log = new LogAutoInsertAccount
                {
                    SignUpForm = LiveCasinoSettings.Settings.SystemId,
                    GameSetting = gameTypes,
                    SettingContext = settingContext
                };

                WriteLog(log);
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
                RoleId = (int)settingContext.TargetRole,
                SignupBy = settingContext.UserIdDoAction,
                SignupFrom = LiveCasinoSettings.Settings.SystemId,
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
                ICommandResult updateSyncResult = UpdateSyncLiveCasino(settingContext, isSynced: true);

                if (updateSyncResult.IsSuccessful)
                {
                    var fromCustInfo = GetCustomerInfo(settingContext.OldCustId);
                    UpdateLiveCasinoStatus(settingContext, fromCustInfo.DisableLiveCasino);
                }

                if (gameTypes == null || !gameTypes.Any())
                {
                    gameTypes = GetGameSettingByCustomerId(settingContext.OldCustId);
                    Logger.Log.Info(
                        string.Format(LiveCasinoLang.InsertFromExistingAccountLogMsg, settingContext.TargetCustId, settingContext.OldCustId),
                        LiveCasinoSettings.Settings.LogCategory);
                }

                var log = new LogInsertFromExistingAccount
                {
                    SignUpForm = LiveCasinoSettings.Settings.SystemId,
                    GameSetting = gameTypes,
                    SettingContext = settingContext
                };

                WriteLog(log);
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

            var updateResult = (CommandResultDTO)_settingRepository.InsertOrUpdate(criteria);

            if (updateResult.IsSuccessful)
            {
                updateResult = new CommandResultDTO
                {
                    Code = 0,
                    Message = LiveCasinoLang.UpdateLiveCasinoStatusSuccess
                };
            }
            else
            {
                Logger.Log.Info(string.Format("{0}\n{1}", updateResult.Message, criteria.ToString()), LiveCasinoSettings.Settings.LogCategory);
            }

            return updateResult;
        }

        /// <summary>
        /// Updates the manual.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="customer">The customer information.</param>
        /// <param name="gameTypes">The game types.</param>
        /// <returns>
        /// Game setting command result.
        /// </returns>
        public ICommandResultsDTO UpdateManual(SettingContext settingContext, List<GameType> gameTypes)
        {
            var gameTypeOlds = new List<GameType>();
            var gameTypeNews = CloneGameTypes(gameTypes);

            if (!settingContext.IsTargetSyncedLiveCS)
            {
                var insertCommandResult = ManualSyncAccount(settingContext);
                if (!settingContext.IsEdittingSuper && insertCommandResult.IsSuccessful)
                {
                    Customer directUplineInfo = GetCustomerInfo(settingContext.DirectUplineId);
                    if (directUplineInfo.IsSyncLiveCasino)
                    {
                        if (gameTypeNews == null)
                        {
                            string site = GetCustomerSiteByUsername(settingContext.TargetUsername);
                            gameTypeNews = GetGameSettingByExchange(directUplineInfo.CurrencyId, site);
                        }
                        else
                        {
                            List<GameType> directUplineGameTypes = GetGameSettingByCustomerId(settingContext.DirectUplineId);
                            if (gameTypeNews.Count < directUplineGameTypes.Count)
                            {
                                var gameTypeUncheckeds = new List<GameType>();
                                foreach (var itemGameType in directUplineGameTypes)
                                {
                                    var foundedGameType = gameTypeNews.Where(x => x.Id == itemGameType.Id).FirstOrDefault();
                                    if (foundedGameType == null)
                                    {
                                        gameTypeUncheckeds.Add(itemGameType);
                                    }
                                }

                                gameTypeNews.AddRange(gameTypeUncheckeds);
                                gameTypeNews = gameTypeNews.OrderBy(x => x.Id).ToList();
                            }
                        }
                    }
                }
            }
            else
            {
                gameTypeOlds = GetGameSettingByCustomerId(settingContext.TargetCustId);
            }

            ICommandResultsDTO resultUpdateStakeLevels = UpdateListStakeLevel(
                settingContext.TargetCustId,
                settingContext.TargetRole,
                gameTypeNews,
                settingContext.Language);
            WriteLogUpdateStakeLevel(settingContext, gameTypeNews, gameTypeOlds, resultUpdateStakeLevels);

            return resultUpdateStakeLevels;
        }

        /// <summary>
        /// Updates the manual multiple.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="gameTypes">The game types.</param>
        /// <returns>Multiple result command.</returns>
        public IMultipleCommandResultsDTO UpdateManualMultiple(SettingContext settingContext, List<GameType> gameTypes)
        {
            var results = new MultipleCommandResultsDTO();
            List<Customer> custInfos = GetCustomerInfo(settingContext.TargetCustIds, settingContext.TargetUsernames, settingContext.TargetSyncStatus);

            foreach (var custInfo in custInfos)
            {
                settingContext.TargetCustId = custInfo.CustId;
                settingContext.TargetUsername = custInfo.UserName;
                settingContext.IsTargetSyncedLiveCS = custInfo.IsSyncLiveCasino;

                var updateResult = (CommandResultsDTO)UpdateManual(settingContext, gameTypes);

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

            var updateResult = (CommandResultDTO)_settingRepository.InsertOrUpdate(criteria);

            if (!updateResult.IsSuccessful)
            {
                Logger.Log.Info(string.Format("{0}\n{1}", updateResult.Message, criteria.ToString()), LiveCasinoSettings.Settings.LogCategory);
            }

            return updateResult;
        }

        /// <summary>
        /// Gets the current win limit.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <returns>Current member win limit.</returns>
        public CurrentMemberWinDTO GetCurrentWinLimit(int custId)
        {
            var criteria = new GetCustomerWinLimitCriteria
            {
                CustId = custId
            };

            var currentWinLimit = (CurrentMemberWinDTO)_settingRepository.GetCurrentMemberWin(criteria);

            return currentWinLimit;
        }

        /// <summary>
        /// Update member win limit.
        /// </summary>
        /// <param name="settingContext">Setting context information.</param>
        /// <param name="memberWinLimit">Member win limit information.</param>
        /// <returns>Result after updating.</returns>
        public ICommandResultsDTO UpdateWinLimit(SettingContext settingContext, WinLimit memberWinLimit)
        {
            var criteria = new UpdateCustomerWinLimitCriteria
            {
                CustId = settingContext.TargetCustId,
                MaxWinLimit = (decimal)memberWinLimit.Max,
                IsAutoDailyReset = memberWinLimit.IsAutoReset,
                UserId = settingContext.UserIdDoAction,
                Ip = settingContext.IP,
                Language = settingContext.Language
            };

            var result = (CommandResultDTO)_settingRepository.InsertOrUpdate(criteria);

            var winLimitResult = new WinLimitCommandResultDTO(result);

            if (winLimitResult.IsSuccessful)
            {
                winLimitResult.Message = LiveCasinoLang.UpdatedWinLimitSuccessfully;
            }
            else
            {
                Logger.Log.Info(string.Format("{0}\n{1}", winLimitResult.Message, criteria.ToString()), LiveCasinoSettings.Settings.LogCategory);
            }

            var commandResults = new CommandResultsDTO();
            commandResults.Results.Add(winLimitResult);

            return commandResults;
        }

        /// <summary>
        /// Updates the win limit multiple.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="memberWinLimit">The member win limit.</param>
        /// <returns>Update win limit multiple result.</returns>
        public IMultipleCommandResultsDTO UpdateWinLimitMultiple(SettingContext settingContext, WinLimit memberWinLimit)
        {
            var results = new MultipleCommandResultsDTO();
            List<Customer> custInfos = GetCustomerInfo(settingContext.TargetCustIds, settingContext.TargetUsernames, settingContext.TargetSyncStatus);

            foreach (var custInfo in custInfos)
            {
                settingContext.TargetCustId = custInfo.CustId;
                var updateResult = UpdateWinLimit(settingContext, memberWinLimit);

                updateResult.Id = custInfo.CustId;
                updateResult.Name = custInfo.UserName;

                results.CommandResultsList.Add(updateResult);
            }

            return results;
        }

        /// <summary>
        /// Manuals the synchronize account.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <returns>Insert successfully.</returns>
        private ICommandResult ManualSyncAccount(SettingContext settingContext)
        {
            ICommandResult resultInsertAccount = InsertManualAccount(settingContext);

            if (resultInsertAccount.IsSuccessful)
            {
                var log = new LogManualInsertAccount
                {
                    SignUpForm = LiveCasinoSettings.Settings.SystemId,
                    SettingContext = settingContext
                };

                WriteLog(log);

                var syncResult = (CommandResultDTO)UpdateSyncLiveCasino(settingContext, isSynced: true);
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

            return resultInsertAccount;
        }

        /// <summary>
        /// Writes the log update stake level.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="gameTypes">The game types.</param>
        /// <param name="gameTypeOlds">The game type olds.</param>
        /// <param name="resultUpdateStakeLevels">The result update stake levels.</param>
        private void WriteLogUpdateStakeLevel(
            SettingContext settingContext,
            List<GameType> gameTypes,
            List<GameType> gameTypeOlds,
            ICommandResultsDTO resultUpdateStakeLevels)
        {
            if (resultUpdateStakeLevels.IsSuccessful)
            {
                var successResultUpdateStakeLevel = resultUpdateStakeLevels.Results.Where(x => x.IsSuccessful);

                foreach (var itemResult in successResultUpdateStakeLevel)
                {
                    var gameSettingResult = (GameSettingCommandResultDTO)itemResult;

                    var newGameType = gameTypes.Where(x => x.Id == gameSettingResult.Id).FirstOrDefault();

                    if (newGameType != null)
                    {
                        var oldGameType = gameTypeOlds.Where(x => x.Id == newGameType.Id).FirstOrDefault();

                        var log = new LogUpdateStakeLevel
                        {
                            SettingContext = settingContext,
                            NewGameType = newGameType,
                            OldGameType = oldGameType
                        };

                        if (oldGameType == null || newGameType.CheckedStakeLevelId != oldGameType.CheckedStakeLevelId)
                        {
                            WriteLog(log);
                        }
                    }
                }
            }
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
                RoleId = (int)settingContext.TargetRole,
                SignupIp = settingContext.IP,
                SignupBy = settingContext.UserIdDoAction,
                SignupFrom = LiveCasinoSettings.Settings.SystemId,
                ModifyUsername = settingContext.UplineUsername,
                Username = settingContext.TargetUsername,
                CustSite = settingContext.SiteName
            };

            var updateResult = (CommandResultDTO)_settingRepository.InsertOrUpdate(criteria);

            if (!updateResult.IsSuccessful)
            {
                Logger.Log.Info(string.Format("{0}\n{1}", updateResult.Message, criteria.ToString()), LiveCasinoSettings.Settings.LogCategory);
            }

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
        private ICommandResultsDTO UpdateListStakeLevel(int custId, CustomerRole role, List<GameType> gameTypes, string language)
        {
            var results = new CommandResultsDTO();

            if (gameTypes != null && gameTypes.Any())
            {
                foreach (var itemGameType in gameTypes)
                {
                    var result = UpdateStakeLevel(custId, role, itemGameType, language);
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
        private ICommandResult UpdateStakeLevel(int custId, CustomerRole role, GameType gameType, string language)
        {
            var criteria = new UpdateStakeLevelCriteria
            {
                CustId = custId,
                RoleId = (int)role,
                GameTypeId = gameType.Id,
                StakeLevelList = gameType.CheckedStakeLevelId
            };

            var updateResult = (CommandResultDTO)_settingRepository.InsertOrUpdate(criteria);

            var gameSettingResult = new GameSettingCommandResultDTO(updateResult);

            if (gameSettingResult.IsSuccessful)
            {
                gameSettingResult.Id = gameType.Id;
                gameSettingResult.Message = string.Format("{0} - {1}: {2}", LiveCasinoLang.lblGameSetting, gameType.Name, LiveCasinoLang.UpdateSuccessfully);
            }
            else
            {
                gameSettingResult.Message = string.Format("{0} - {1}: {2}", LiveCasinoLang.lblGameSetting, gameType.Name, LiveCasinoLang.UpdatedUnsuccessfully);
                Logger.Log.Info(string.Format("{0}\n{1}", updateResult.Message, criteria.ToString()), LiveCasinoSettings.Settings.LogCategory);
            }

            return gameSettingResult;
        }

        /// <summary>
        /// Clones the game types.
        /// </summary>
        /// <param name="gameType">Type of the game.</param>
        /// <returns>New instance of game type collection.</returns>
        private List<GameType> CloneGameTypes(List<GameType> gameType)
        {
            var list = new List<GameType>();

            foreach (var item in gameType)
            {
                list.Add(new GameType
                {
                    Id = item.Id,
                    BetTypeId = item.BetTypeId,
                    BettingLimit = item.BettingLimit
                });
            }

            return list;
        }

        private List<Customer> GetCustomerInfo(string custIds, string usernames, string syncStatus)
        {
            var list = new List<Customer>();

            if (string.IsNullOrWhiteSpace(custIds)
                || string.IsNullOrWhiteSpace(usernames)
                || string.IsNullOrWhiteSpace(syncStatus))
            {
                return list;
            }

            string[] arrayCustId = custIds.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] arrayUsername = usernames.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] arraySyncStatus = syncStatus.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (arrayCustId.Length == arrayUsername.Length
                && arrayUsername.Length == arraySyncStatus.Length
                && arrayCustId.Any())
            {
                for (int i = 0; i < arrayCustId.Length; i++)
                {
                    list.Add(new Customer
                    {
                        CustId = Converter.ToInt(arrayCustId[i]),
                        UserName = arrayUsername[i],
                        IsSyncLiveCasino = arraySyncStatus[i] == "1"
                    });
                }
            }

            return list;
        }

        /// <summary>
        /// Depend on status of current customer, we decide to update just part of setting or all setting
        /// This handle for updating multiple customer.
        /// </summary>
        /// <param name="settingContext">Setting context for updating.</param>
        /// <param name="customer">Customer setting.</param>
        /// <param name="updateType">Update form type.</param>
        /// <returns>Command result after updating.</returns>
        private ICommandResultsDTO UpdateEachCustomerInMultipleEdit(
            SettingContext settingContext,
            Customer customer,
            int updateType,
            UplinePositionTakingOnMember uplinePositionTakingOnMember)
        {
            var isSync = customer.IsSyncLiveCasino;
            switch (updateType)
            {
                case (int)UpdateType.PositionTakingAndCommission:
                    return isSync ?
                            _pTComSettingService.UpdatePtComSetting(settingContext, customer.Commission, customer.PositionTaking, uplinePositionTakingOnMember)
                            : UpdateSingleCustomerAllSetting(settingContext, customer, uplinePositionTakingOnMember);
                case (int)UpdateType.WinLimit:
                    return isSync ?
                            UpdateWinLimit(settingContext, customer.MemberWinLimit)
                            : UpdateSingleCustomerAllSetting(settingContext, customer, uplinePositionTakingOnMember);
                case (int)UpdateType.GameSetting:
                    return isSync ?
                            UpdateManual(settingContext, customer.GameSetting)
                            : UpdateSingleCustomerAllSetting(settingContext, customer, uplinePositionTakingOnMember);
                case (int)UpdateType.AllSetting:
                    return UpdateSingleCustomerAllSetting(settingContext, customer, uplinePositionTakingOnMember);
                default:
                    return new CommandResultsDTO();
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

            try
            {
                var updateResult = (CommandResultDTO)_settingRepository.InsertOrUpdate(criteria);

                return updateResult;
            }
            catch (Exception ex)
            {
                Logger.Log.Error(string.Format("{0}\n{1}", ex.Message, criteria.ToString()), LiveCasinoSettings.Settings.LogCategory);

                throw ex;
            }
        }
    }
}