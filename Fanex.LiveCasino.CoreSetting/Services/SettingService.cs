namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common;
    using Fanex.Common.Helper;
    using Fanex.LiveCasino.CoreSetting.Configuration;
    using Fanex.LiveCasino.CoreSetting.Constant;
    using Fanex.LiveCasino.CoreSetting.Criteria;
    using Fanex.LiveCasino.CoreSetting.Criteria.GameSetting;
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using Fanex.LiveCasino.CoreSetting.Criteria.WinLimit;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.DTO.CommandResult;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Repositories;
    using Fanex.LiveCasino.CoreSetting.Resources;
    using Fanex.Logging;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Setting service for update position taking setting, Commission, member win limit.
    /// </summary>
    public class SettingService : ISettingService
    {
        private const string CasinoNo = "01";
        private readonly ISettingRepository _settingRepository;

        public SettingService()
            : this(null)
        {
        }

        public SettingService(
            ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository ?? new SettingRepository();
        }

        public Customer GetCustomer(LiveCasinoSettingContext settingContext)
        {
            Customer customer = GetCustomerInfo(settingContext.Customer.Id);

            settingContext.Customer.CurrencyId = customer.CurrencyId;

            settingContext.Customer.Username = customer.Username;
            SetCustomerPtAndComSetting(settingContext, customer);
            customer.LiveCasinoSetting.GameSetting = GetGameSetting(settingContext);

            return customer;
        }

        /// <summary>
        /// Get Representative customer for editing multiple.
        /// </summary>
        /// <param name="settingContextMultiple">Setting context for getting customer id list, username list, status list.</param>
        /// <returns>Customer id representative.</returns>
        public Customer GetRepresentativeCustIdForEditingMultiple(LiveCasinoSettingContextMultiple settingContextMultiple)
        {
            if (!settingContextMultiple.Customers.Any())
            {
                return null;
            }

            //// Edit multiple direct downline. Load setting info of the first selected downline in customer list
            int levelGap = (int)settingContextMultiple.Upline.Level - (int)settingContextMultiple.Customer.Level;
            if (levelGap == 1)
            {
                return settingContextMultiple.Customers.First();
            }

            settingContextMultiple.Customers = settingContextMultiple.Customers.Where(customer =>
                {
                    return customer.IsSyncLiveCasino;
                }).ToList();

            //// Super/Master multiple member
            return settingContextMultiple.Customers.FirstOrDefault();
        }

        /// <summary>
        /// Update single customer all setting.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="customer">Customer setting information for updating.</param>
        /// <returns>Update result.</returns>
        public ICommandResultsDTO UpdateSingleCustomerAllSetting(
            LiveCasinoSettingContext settingContext,
            Customer customer,
            bool isApplyPositionTakingAllMembers,
            float positionTakingOnMembers)
        {
            ICommandResultsDTO resultList = new CommandResultsDTO();

            if (settingContext.Customer.Level == CustomerLevel.Member
                && settingContext.Upline.Level == CustomerLevel.Agent
                && customer.IsSyncLiveCasino
                && !customer.Parent.IsSyncLiveCasino)
            {
                resultList.Results.Add(UpdateLiveCasinoStatus(settingContext, customer.DisableLiveCasino));
            }

            var positionTakingAndCommResults = UpdatePtComSetting(
                settingContext,
                customer.LiveCasinoSetting.Commission,
                customer.LiveCasinoSetting.PositionTaking,
                isApplyPositionTakingAllMembers,
                positionTakingOnMembers);
            resultList.Results.AddRange(positionTakingAndCommResults.Results);

            if (customer.IsSyncLiveCasino
                || (!customer.IsSyncLiveCasino && positionTakingAndCommResults.IsSuccessful))
            {
                var winLimitResults = UpdateWinLimit(settingContext, customer.LiveCasinoSetting.WinLimit).Results;
                resultList.Results.AddRange(winLimitResults);

                var gameSettingResult = UpdateGameSetting(settingContext, customer.LiveCasinoSetting.GameSetting).Results;
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
        /// <param name="settingContextMultiple">Setting context for updating.</param>
        /// <param name="customer">Customer setting contains customer setting information.</param>
        /// <param name="updateType">Update type for multiple.</param>
        /// <returns>Multiple result return to user.</returns>
        public IMultipleCommandResultsDTO UpdateMultipleCustomerAllSetting(
            LiveCasinoSettingContextMultiple settingContextMultiple,
            Customer customer,
            int updateType,
            bool isApplyPositionTakingAllMembers,
            float positionTakingOnMembers)
        {
            IMultipleCommandResultsDTO mainResults = new MultipleCommandResultsDTO();

            foreach (var custInfo in settingContextMultiple.Customers)
            {
                settingContextMultiple.Customer.Id = custInfo.Id;
                settingContextMultiple.Customer.Username = custInfo.Username;
                customer.IsSyncLiveCasino = custInfo.IsSyncLiveCasino;
                var subResult = UpdateEachCustomerInMultipleEdit(settingContextMultiple, customer, updateType, isApplyPositionTakingAllMembers, positionTakingOnMembers);
                subResult.Id = custInfo.Id;
                subResult.Name = custInfo.Username;
                mainResults.CommandResultsList.Add(subResult);
            }

            return mainResults;
        }

        /// <summary>
        /// Gets the customer site by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>Customer site name.</returns>
        public string GetCustomerSite(string username)
        {
            var criteria = new GetCustomerSiteByUsernameCriteria
            {
                Username = username
            };

            if (!criteria.IsValid())
            {
                throw new Exception(LiveCasinoLang.InvalidCriteriaMsg);
            }

            var userInfo = _settingRepository.Get<Customer>(criteria);

            return userInfo.Site;
        }

        /// <summary>
        /// Get customer information with customer id.
        /// </summary>
        /// <param name="custId">Get criteria information.</param>
        /// <returns>Customer information.</returns>
        public Customer GetCustomerInfo(int custId)
        {
            var criteria = new GetCustomerLiveCasinoInfoCriteria
            {
                CustId = custId
            };

            Customer custInfo = _settingRepository.GetCustomerInfo(criteria);

            return custInfo;
        }

        /// <summary>
        /// Gets the game setting.
        /// </summary>
        /// <param name="settingContext">TargetCustId, CustomerLevel, IsCustomerSyncedLiveCS, TargetUsername, ExchangeId, DirectParentId. </param>
        /// <returns>The game setting.</returns>
        public List<GameType> GetGameSetting(LiveCasinoSettingContext settingContext)
        {
            Check.Argument.IsNotNull(settingContext, "settingContext");
            Check.Argument.IsNotNull(settingContext.Customer, "settingContext.Customer");
            Check.Argument.IsNotNull(settingContext.Customer.Parent, "settingContext.Customer.Parent");
            Check.Argument.IsNotNegativeOrZero(settingContext.Customer.Id, "settingContext.Customer.Id");
            Check.Argument.IsNotNegativeOrZero((int)settingContext.Customer.Level, "settingContext.Customer.Level");
            Check.Argument.IsNotNull(settingContext.Customer.IsSyncLiveCasino, "settingContext.Customer.IsSyncLiveCasino");
            Check.Argument.IsNotEmpty(settingContext.Customer.Username, "settingContext.Customer.Username");
            Check.Argument.IsNotNegativeOrZero(settingContext.Customer.CurrencyId, "settingContext.Customer.CurrencyId");
            Check.Argument.IsNotNegative(settingContext.Customer.Parent.Id, "settingContext.Customer.Parent.Id");

            var gameSettings = new List<GameType>();

            if (settingContext.Customer.Level == CustomerLevel.Super && !settingContext.Customer.IsSyncLiveCasino)
            {
                var site = GetCustomerSite(settingContext.Customer.Username);
                gameSettings = _settingRepository.GetGameSetting(settingContext.Customer.CurrencyId, site, defaultCheckedValue: true);
            }
            else if (settingContext.Customer.Level != CustomerLevel.Super && !settingContext.Customer.IsSyncLiveCasino)
            {
                gameSettings = _settingRepository.GetGameSetting(settingContext.Customer.Parent.Id);
                gameSettings.RemoveAll(x => string.IsNullOrWhiteSpace(x.CheckedStakeLevelRange));

                // Check all by default
                foreach (var gameSettingItem in gameSettings)
                {
                    gameSettingItem.BettingLimit.RemoveAll(x => !x.IsChecked);
                    if (settingContext.Customer.Level == CustomerLevel.Member)
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
                gameSettings = _settingRepository.GetGameSetting(settingContext.Customer.Id);
            }

            return gameSettings;
        }

        public ICommandResult InsertAutoAccount(int custId, int actorId, string actorUsername, int subAccountId, string ip, string language)
        {
            var uplineCustomerInfo = GetUplineCustomerInfo(custId);

            var criteria = new InsertAutoAccountCriteria
            {
                AgentId = uplineCustomerInfo.AgentId,
                CasinoNo = CasinoNo,
                CustId = custId,
                ExchangeId = uplineCustomerInfo.CurrencyId,
                MasterId = uplineCustomerInfo.MasterId,
                ModifyUsername = actorUsername,
                RoleId = uplineCustomerInfo.RoleId,
                SignupBy = actorId,
                SignupFrom = LiveCasinoSettings.Settings.SystemId,
                SignupIp = ip,
                SuperId = uplineCustomerInfo.SuperId,
                Username = uplineCustomerInfo.Username,
                Language = language
            };

            var gameTypes = new List<GameType>();
            ICommandResult result = _settingRepository.InsertGameSettingAccount(criteria, gameTypes);

            if (result.IsSuccessful)
            {
                var settingContext = new LiveCasinoSettingContext
                {
                    Customer = new Customer
                    {
                        Id = custId
                    },
                    ActorId = actorId,
                    SubAccount = new User
                    {
                        Id = subAccountId
                    },
                    IP = ip,
                    Language = language
                };

                ICommandResult updateSyncResult = UpdateSyncLiveCasino(settingContext, isSynced: true);

                if (updateSyncResult.IsSuccessful)
                {
                    var uplineCustInfo = GetCustomerInfo(uplineCustomerInfo.DirectUplineId);
                    UpdateLiveCasinoStatus(settingContext, uplineCustInfo.DisableLiveCasino);
                }

                if (gameTypes == null || !gameTypes.Any())
                {
                    gameTypes = _settingRepository.GetGameSetting(uplineCustomerInfo.DirectUplineId);

                    Logger.Log.Info(
                        string.Format(LiveCasinoLang.AutoInsertAccountLogMsg, settingContext.Customer.Id, uplineCustomerInfo.DirectUplineId),
                        LiveCasinoSettings.Settings.LogCategory);
                }

                //var logModel = new AutoInsertAccountLog
                //{
                //    SignUpForm = LiveCasinoSettings.Settings.SystemId,
                //    GameSetting = gameTypes,
                //    Criteria = criteria
                //};

                //WriteLog(logModel);
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
        public ICommandResult InsertFromExistingAccount(LiveCasinoSettingContext settingContext)
        {
            //    var criteria = new InsertFromExistingAccountCriteria
            //    {
            //        AgentId = settingContext.AgentId,
            //        CasinoNo = CasinoNo,
            //        CustId = settingContext.CustomerId,
            //        ExchangeId = settingContext.ExchangeId,
            //        MasterId = settingContext.MasterId,
            //        ModifyUsername = settingContext.ParentUsername,
            //        RoleId = (int)settingContext.CustomerLevel,
            //        SignupBy = settingContext.ActorId,
            //        SignupFrom = LiveCasinoSettings.Settings.SystemId,
            //        SignupIp = settingContext.IP,
            //        SuperId = settingContext.SuperId,
            //        Username = settingContext.Username,
            //        OldCustId = settingContext.OldCustId,
            //        ToUserName = settingContext.ToUsername
            //    };

            //    var gameTypes = new List<GameType>();
            //    ICommandResult result = _settingRepository.InsertGameSettingAccount(criteria, gameTypes);

            //    if (result.IsSuccessful)
            //    {
            //        ICommandResult updateSyncResult = UpdateSyncLiveCasino(settingContext, isSynced: true);

            //        if (updateSyncResult.IsSuccessful)
            //        {
            //            var fromCustInfo = GetCustomerInfo(settingContext.OldCustId);
            //            UpdateLiveCasinoStatus(settingContext, fromCustInfo.DisableLiveCasino);
            //        }

            //        if (gameTypes == null || !gameTypes.Any())
            //        {
            //            gameTypes = _settingRepository.GetGameSetting(settingContext.OldCustId);
            //            Logger.Log.Info(
            //                string.Format(LiveCasinoLang.InsertFromExistingAccountLogMsg, settingContext.CustomerId, settingContext.OldCustId),
            //                LiveCasinoSettings.Settings.LogCategory);
            //        }

            //        //var log = new InsertFromExistingAccountLog
            //        //{
            //        //    SignUpForm = LiveCasinoSettings.Settings.SystemId,
            //        //    GameSetting = gameTypes,
            //        //    SettingContext = settingContext
            //        //};

            //        //WriteLog(log);
            //    }

            //    return result;
            throw new Exception();
        }

        public ICommandResult UpdateLiveCasinoStatus(LiveCasinoSettingContext settingContext, bool disableLiveCasino)
        {
            Check.Argument.IsNotNull(settingContext, "settingContext");
            Check.Argument.IsNotNull(settingContext.Customer, "settingContext.Customer");
            Check.Argument.IsNotNegativeOrZero(settingContext.Customer.Id, "settingContext.Customer.Id");
            Check.Argument.IsNotNegativeOrZero(settingContext.ActorId, "settingContext.ActorId");
            Check.Argument.IsNotEmpty(settingContext.IP, "settingContext.IP");
            Check.Argument.IsNotEmpty(settingContext.Language, "settingContext.Language");

            var criteria = new UpdateLiveCasinoStatusCriteria
            {
                CustId = settingContext.Customer.Id,
                UserID = settingContext.ActorId,
                DisabledLiveCS = disableLiveCasino,
                EnableDownlinesLiveCS = !disableLiveCasino,
                IP = settingContext.IP,
                Language = settingContext.Language
            };

            var updateResult = (CommandResultDTO)_settingRepository.Save(criteria);

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
        public ICommandResultsDTO UpdateGameSetting(LiveCasinoSettingContext settingContext, List<GameType> gameTypes)
        {
            //var gameTypeOlds = new List<GameType>();
            //var gameTypeNews = CloneGameTypes(gameTypes);

            //if (!settingContext.IsCustomerSyncedLiveCS)
            //{
            //    var insertCommandResult = ManualSyncAccount(settingContext);
            //    if (settingContext.CustomerLevel != CustomerLevel.Super && insertCommandResult.IsSuccessful)
            //    {
            //        Customer directUplineInfo = GetCustomerInfo(settingContext.DirectParentId);
            //        if (directUplineInfo.IsSyncLiveCasino)
            //        {
            //            if (gameTypeNews == null)
            //            {
            //                string site = GetCustomerSite(settingContext.Username);
            //                gameTypeNews = _settingRepository.GetGameSetting(directUplineInfo.CurrencyId, site);
            //            }
            //            else
            //            {
            //                List<GameType> directUplineGameTypes = _settingRepository.GetGameSetting(settingContext.DirectParentId);
            //                if (gameTypeNews.Count < directUplineGameTypes.Count)
            //                {
            //                    var gameTypeUncheckeds = new List<GameType>();
            //                    foreach (var itemGameType in directUplineGameTypes)
            //                    {
            //                        var foundedGameType = gameTypeNews.Where(x => x.Id == itemGameType.Id).FirstOrDefault();
            //                        if (foundedGameType == null)
            //                        {
            //                            gameTypeUncheckeds.Add(itemGameType);
            //                        }
            //                    }

            //                    gameTypeNews.AddRange(gameTypeUncheckeds);
            //                    gameTypeNews = gameTypeNews.OrderBy(x => x.Id).ToList();
            //                }
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    gameTypeOlds = _settingRepository.GetGameSetting(settingContext.CustomerId);
            //}

            //ICommandResultsDTO resultUpdateStakeLevels = UpdateListStakeLevel(
            //    settingContext.CustomerId,
            //    settingContext.CustomerLevel,
            //    gameTypeNews,
            //    settingContext.Language);
            //WriteLogUpdateStakeLevel(settingContext, gameTypeNews, gameTypeOlds, resultUpdateStakeLevels);

            //return resultUpdateStakeLevels;

            throw new Exception();
        }

        /// <summary>
        /// Updates the manual multiple.
        /// </summary>
        /// <param name="settingContextMultiple">The setting context.</param>
        /// <param name="gameTypes">The game types.</param>
        /// <returns>Multiple result command.</returns>
        public IMultipleCommandResultsDTO UpdateGameSettingMultiple(LiveCasinoSettingContextMultiple settingContextMultiple, List<GameType> gameTypes)
        {
            var results = new MultipleCommandResultsDTO();

            foreach (var custInfo in settingContextMultiple.Customers)
            {
                settingContextMultiple.Customer.Id = custInfo.Id;
                settingContextMultiple.Customer.Username = custInfo.Username;
                settingContextMultiple.Customer.IsSyncLiveCasino = custInfo.IsSyncLiveCasino;

                var updateResult = (CommandResultsDTO)UpdateGameSetting(settingContextMultiple, gameTypes);

                updateResult.Id = custInfo.Id;
                updateResult.Name = custInfo.Username;

                results.CommandResultsList.Add(updateResult);
            }

            return results;
        }

        public ICommandResult UpdateSyncLiveCasino(LiveCasinoSettingContext settingContext, bool isSynced)
        {
            Check.Argument.IsNotNull(settingContext, "settingContext");
            Check.Argument.IsNotNull(settingContext.Customer, "settingContext.Customer");
            Check.Argument.IsNotNull(settingContext.SubAccount, "settingContext.SubAccount");
            Check.Argument.IsNotNegativeOrZero(settingContext.Customer.Id, "settingContext.Customer.Id");
            Check.Argument.IsNotNegativeOrZero(settingContext.ActorId, "settingContext.ActorId");
            Check.Argument.IsNotEmpty(settingContext.IP, "settingContext.IP");
            Check.Argument.IsNotEmpty(settingContext.Language, "settingContext.Language");

            var criteria = new UpdateSyncLiveCasinoCriteria
            {
                CustId = settingContext.Customer.Id,
                UserId = settingContext.ActorId,
                Ip = settingContext.IP,
                Language = settingContext.Language,
                SubAccId = settingContext.SubAccount.Id,
                SyncLiveCS = isSynced
            };

            var updateResult = (CommandResultDTO)_settingRepository.Save(criteria);

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
        public CurrentMemberWinLimitDTO GetCurrentWinLimit(int custId)
        {
            var criteria = new GetCustomerWinLimitCriteria
            {
                CustId = custId
            };

            var currentWinLimit = (CurrentMemberWinLimitDTO)_settingRepository.GetCurrentMemberWin(criteria);

            return currentWinLimit;
        }

        /// <summary>
        /// Update member win limit.
        /// </summary>
        /// <param name="settingContext">Setting context information.</param>
        /// <param name="memberWinLimit">Member win limit information.</param>
        /// <returns>Result after updating.</returns>
        public ICommandResultsDTO UpdateWinLimit(LiveCasinoSettingContext settingContext, WinLimit memberWinLimit)
        {
            var criteria = new UpdateCustomerWinLimitCriteria
            {
                CustId = settingContext.Customer.Id,
                MaxWinLimit = (decimal)memberWinLimit.Max,
                IsAutoDailyReset = memberWinLimit.IsAutoReset,
                UserId = settingContext.ActorId,
                Ip = settingContext.IP,
                Language = settingContext.Language
            };

            var result = (CommandResultDTO)_settingRepository.Save(criteria);

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
        /// <param name="settingContextMultiple">The setting context.</param>
        /// <param name="memberWinLimit">The member win limit.</param>
        /// <returns>Update win limit multiple result.</returns>
        public IMultipleCommandResultsDTO UpdateWinLimitMultiple(LiveCasinoSettingContextMultiple settingContextMultiple, WinLimit memberWinLimit)
        {
            var results = new MultipleCommandResultsDTO();

            foreach (var custInfo in settingContextMultiple.Customers)
            {
                settingContextMultiple.Customer.Id = custInfo.Id;
                var updateResult = UpdateWinLimit(settingContextMultiple, memberWinLimit);

                updateResult.Id = custInfo.Id;
                updateResult.Name = custInfo.Username;

                results.CommandResultsList.Add(updateResult);
            }

            return results;
        }

        /// <summary>
        /// Manuals the synchronize account.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <returns>Insert successfully.</returns>
        private ICommandResult ManualSyncAccount(LiveCasinoSettingContext settingContext)
        {
            //ICommandResult resultInsertAccount = InsertManualAccount(settingContext);

            //if (resultInsertAccount.IsSuccessful)
            //{
            //    //var log = new ManualInsertAccountLog
            //    //{
            //    //    SignUpForm = LiveCasinoSettings.Settings.SystemId,
            //    //    SettingContext = settingContext
            //    //};

            //    //WriteLog(log);

            //    var syncResult = (CommandResultDTO)UpdateSyncLiveCasino(settingContext, isSynced: true);
            //    var disableLiveCasino = false;

            //    if (settingContext.CustomerLevel != CustomerLevel.Super)
            //    {
            //        var uplineCustInfo = GetCustomerInfo(settingContext.DirectParentId);
            //        disableLiveCasino = uplineCustInfo.DisableLiveCasino;
            //    }

            //    if (syncResult.IsSuccessful)
            //    {
            //        UpdateLiveCasinoStatus(settingContext, disableLiveCasino);
            //    }
            //}

            //return resultInsertAccount;

            throw new Exception();
        }

        /// <summary>
        /// Writes the log update stake level.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="gameTypes">The game types.</param>
        /// <param name="gameTypeOlds">The game type olds.</param>
        /// <param name="resultUpdateStakeLevels">The result update stake levels.</param>
        private void WriteLogUpdateStakeLevel(
            LiveCasinoSettingContext settingContext,
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

                        //var log = new LogUpdateStakeLevel
                        //{
                        //    SettingContext = settingContext,
                        //    NewGameType = newGameType,
                        //    OldGameType = oldGameType
                        //};

                        //if (oldGameType == null || newGameType.CheckedStakeLevelId != oldGameType.CheckedStakeLevelId)
                        //{
                        //    WriteLog(log);
                        //}
                    }
                }
            }
        }

        /// <summary>
        /// Inserts the manual account.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <returns>Command result insert manual account.</returns>
        private ICommandResult InsertManualAccount(LiveCasinoSettingContext settingContext)
        {
            //var criteria = new InsertManualAccountCriteria
            //{
            //    CustId = settingContext.CustomerId,
            //    CasinoNo = CasinoNo,
            //    ExchangeId = settingContext.ExchangeId,
            //    SuperId = settingContext.SuperId,
            //    MasterId = settingContext.MasterId,
            //    AgentId = settingContext.AgentId,
            //    RoleId = (int)settingContext.CustomerLevel,
            //    SignupIp = settingContext.IP,
            //    SignupBy = settingContext.ActorId,
            //    SignupFrom = LiveCasinoSettings.Settings.SystemId,
            //    ModifyUsername = settingContext.ParentUsername,
            //    Username = settingContext.Username,
            //    CustSite = settingContext.SiteName
            //};

            //var updateResult = (CommandResultDTO)_settingRepository.Save(criteria);

            //if (!updateResult.IsSuccessful)
            //{
            //    Logger.Log.Info(string.Format("{0}\n{1}", updateResult.Message, criteria.ToString()), LiveCasinoSettings.Settings.LogCategory);
            //}

            //return updateResult;

            throw new Exception();
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
        private ICommandResultsDTO UpdateListStakeLevel(int custId, CustomerLevel role, List<GameType> gameTypes, string language)
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
        private ICommandResult UpdateStakeLevel(int custId, CustomerLevel role, GameType gameType, string language)
        {
            var criteria = new UpdateStakeLevelCriteria
            {
                CustId = custId,
                RoleId = (int)role,
                GameTypeId = gameType.Id,
                StakeLevelList = gameType.CheckedStakeLevelId
            };

            var updateResult = (CommandResultDTO)_settingRepository.Save(criteria);

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
                        Id = Converter.ToInt(arrayCustId[i]),
                        Username = arrayUsername[i],
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
            LiveCasinoSettingContext settingContext,
            Customer customer,
            int updateType,
            bool isApplyPositionTakingAllMembers,
            float positionTakingOnMembers)
        {
            var isSync = customer.IsSyncLiveCasino;
            switch (updateType)
            {
                case (int)UpdateType.PositionTakingAndCommission:
                    return isSync ?
                            UpdatePtComSetting(settingContext, customer.LiveCasinoSetting.Commission, customer.LiveCasinoSetting.PositionTaking, isApplyPositionTakingAllMembers, positionTakingOnMembers)
                            : UpdateSingleCustomerAllSetting(settingContext, customer, isApplyPositionTakingAllMembers, positionTakingOnMembers);
                case (int)UpdateType.WinLimit:
                    return isSync ?
                            UpdateWinLimit(settingContext, customer.LiveCasinoSetting.WinLimit)
                            : UpdateSingleCustomerAllSetting(settingContext, customer, isApplyPositionTakingAllMembers, positionTakingOnMembers);
                case (int)UpdateType.GameSetting:
                    return isSync ?
                            UpdateGameSetting(settingContext, customer.LiveCasinoSetting.GameSetting)
                            : UpdateSingleCustomerAllSetting(settingContext, customer, isApplyPositionTakingAllMembers, positionTakingOnMembers);
                case (int)UpdateType.AllSetting:
                    return UpdateSingleCustomerAllSetting(settingContext, customer, isApplyPositionTakingAllMembers, positionTakingOnMembers);
                default:
                    return new CommandResultsDTO();
            }
        }

        //public ICommandResult WriteLog(IGameSettingLog logData)
        //{
        //    var criteria = new InsertLogUserRunSPCriteria
        //    {
        //        UserId = logData.SettingContext.ActorId,
        //        CustId = logData.SettingContext.CustomerId,
        //        SpName = logData.SpName,
        //        Doing = string.Empty,
        //        ActionId = (int)logData.Action,
        //        TrackingValue = logData.NewValue,
        //        TrackingValue2 = logData.OldValue,
        //        Duration = 0
        //    };

        //    try
        //    {
        //        var updateResult = (CommandResultDTO)_settingRepository.Save(criteria);

        //        return updateResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Log.Error(string.Format("{0}\n{1}", ex.Message, criteria.ToString()), LiveCasinoSettings.Settings.LogCategory);

        //        throw ex;
        //    }
        //}

        /// <summary>
        /// Get Customer Setting for Editing.
        /// </summary>
        /// <param name="settingContext">Current context for getting customer information.</param>
        /// <param name="customer">Current customer need be set position taking and commission data.</param>
        /// <returns>Position taking and Commission setting.</returns>
        public void SetCustomerPtAndComSetting(
            LiveCasinoSettingContext settingContext,
            Customer customer)
        {
            Check.Argument.IsNotNull(settingContext, "settingContext");
            Check.Argument.IsNotNull(customer, "customer");

            switch (settingContext.Customer.Level)
            {
                case CustomerLevel.Super:
                    SetSuperPtAndCom(settingContext.Customer.Id, customer);
                    break;

                case CustomerLevel.Master:
                    SetMasterPtAndCom(settingContext.Customer.Id, settingContext.Upline.Id, customer);
                    break;

                case CustomerLevel.Agent:
                    SetAgentPtAndCom(settingContext.Customer.Id, settingContext.Upline.Id, customer);
                    break;

                case CustomerLevel.Member:
                    switch (settingContext.Upline.Level)
                    {
                        case CustomerLevel.Super:
                            SetMemberPtAndComSettingForSuperEditMember(settingContext.Customer.Id, customer);
                            break;

                        case CustomerLevel.Master:
                            SetMemberPtAndComSettingForMasterEditMember(settingContext.Customer.Id, customer);
                            break;

                        default:
                            SetMemberPtAndComSettingForAgentEditMember(settingContext.Customer.Id, settingContext.Upline.Id, customer);
                            break;
                    }

                    break;
            }
        }

        /// <summary>
        /// Update position taking and Commission setting, depend on setting context,
        /// we will decide which store procedure and function will be called.
        /// </summary>
        /// <param name="settingContext">Setting context for update customer position taking and commission.</param>
        /// <param name="positionTakingAndComSetting">Position taking and Commission setting for updating.</param>
        /// <returns>Update status and information for showing on web.</returns>
        public ICommandResultsDTO UpdatePtComSetting(
            LiveCasinoSettingContext settingContext,
            float commission,
            PositionTaking positionTaking,
            bool isApplyPositionTakingAllMembers,
            float positionTakingOnMembers)
        {

            ICommandResultsDTO updateResults = new CommandResultsDTO();
            switch (settingContext.Customer.Level)
            {
                case CustomerLevel.Super:
                    updateResults.Results = UpdateSuperPtAndComSetting(settingContext, commission, positionTaking);
                    break;

                case CustomerLevel.Master:
                    updateResults.Results = UpdateMasterPtAndComSetting(settingContext, commission, positionTaking, isApplyPositionTakingAllMembers, positionTakingOnMembers);
                    break;

                case CustomerLevel.Agent:
                    updateResults.Results = UpdateAgentPtAndComSetting(settingContext, commission, positionTaking, isApplyPositionTakingAllMembers, positionTakingOnMembers);
                    break;

                case CustomerLevel.Member:
                    switch (settingContext.Upline.Level)
                    {
                        case CustomerLevel.Super:
                            updateResults.Results = SuperUpdateMemberPtAndComSetting(settingContext, commission, positionTaking);
                            break;

                        case CustomerLevel.Master:
                            updateResults.Results = MasterUpdateMemberPtAndComSetting(settingContext, commission, positionTaking);
                            break;

                        default:
                            updateResults.Results = AgentUpdateMemberPtAndComSetting(settingContext, commission, positionTaking);
                            break;
                    }

                    break;
            }

            return updateResults;
        }

        /// <summary>
        /// Update multiple Customer Position Taking and Commission.
        /// </summary>
        /// <param name="settingContextMultiple">Setting context of current update setting.</param>
        /// <param name="positionTakingAndComSetting">Position taking and Commission setting for updating.</param>
        /// <param name="uplinePositionTakingOnMember">Up-line position taking on member when set from auto PT to none.</param>
        /// <returns>Update status and information for showing on web.</returns>
        public IMultipleCommandResultsDTO UpdatePtComSettingMultiple(
            LiveCasinoSettingContextMultiple settingContextMultiple,
            float commission,
            PositionTaking positionTaking,
            bool isApplyPositionTakingAllMembers,
            float positionTakingOnMembers)
        {
            IMultipleCommandResultsDTO mainResults = new MultipleCommandResultsDTO();

            foreach (var customer in settingContextMultiple.Customers)
            {
                settingContextMultiple.Customer = customer;
                var subResult = UpdatePtComSetting(settingContextMultiple, commission, positionTaking, isApplyPositionTakingAllMembers, positionTakingOnMembers);
                subResult.Id = customer.Id;
                subResult.Name = customer.Username;
                mainResults.CommandResultsList.Add(subResult);
            }

            return mainResults;
        }

        private void SetSuperPtAndCom(int superId, Customer customer)
        {
            PTComDTO ptAndComDTO = _settingRepository.GetSettingForEditSuper(new GetSettingForEditSuperCriteria
            {
                CustId = superId
            });
            SetDataToCustomerPtComSetting(ptAndComDTO, customer);
        }

        private void SetMasterPtAndCom(int masterId, int superId, Customer customer)
        {
            PTComDTO ptAndComDTO = _settingRepository.GetSettingForEditMaster(new GetSettingForEditMasterCriteria
            {
                CustId = masterId,
                SRecommend = superId
            });

            SetDataToCustomerPtComSetting(ptAndComDTO, customer);
        }

        private void SetAgentPtAndCom(int agentId, int masterId, Customer customer)
        {
            PTComDTO ptAndComDTO = _settingRepository.GetSettingForEditAgent(new GetSettingForEditAgentCriteria
            {
                CustId = agentId,
                MRecommend = masterId
            });
            SetDataToCustomerPtComSetting(ptAndComDTO, customer);
        }

        private void SetMemberPtAndComSettingForAgentEditMember(int memberId, int agentId, Customer customer)
        {
            PTComDTO ptAndComDTO = _settingRepository.GetSettingForEditMember(new GetSettingForEditMemberCriteria
            {
                CustId = memberId,
                Recommend = agentId
            });
            SetDataToCustomerPtComSetting(ptAndComDTO, customer);
        }

        private void SetMemberPtAndComSettingForSuperEditMember(int memberId, Customer customer)
        {
            PTComDTO ptAndComDTO = _settingRepository.GetSettingForSuperEditMember(new GetSettingForSuperEditMemberCriteria
            {
                CustId = memberId
            });
            SetDataToCustomerPtComSetting(ptAndComDTO, customer);
        }

        private void SetMemberPtAndComSettingForMasterEditMember(int memberId, Customer customer)
        {
            PTComDTO ptAndComDTO = _settingRepository.GetSettingForMasterEditMember(
                new GetSettingForMasterEditMemberCriteria
                {
                    CustId = memberId
                });

            SetDataToCustomerPtComSetting(ptAndComDTO, customer);
        }

        /// <summary>
        /// Set position taking and commission setting to customer.
        /// </summary>
        /// <param name="positionTakingAndCommission">Array contains position taking and commission setting.</param>
        /// <param name="customer">Customer need be set position taking and commission data.</param>
        private void SetDataToCustomerPtComSetting(PTComDTO ptAndComDTO, Customer customer)
        {
            customer.LiveCasinoSetting.PositionTaking = ptAndComDTO.PositionTaking;
            customer.LiveCasinoSetting.Commission = ptAndComDTO.Commission;
            customer.Parent.LiveCasinoSetting.Commission = ptAndComDTO.Commission;
        }

        /// <summary>
        /// Update super position taking and Commission setting.
        /// </summary>
        /// <param name="settingContext">Setting context of current update setting.</param>
        /// <param name="positionTakingAndComSetting">Position taking and Commission setting for updating.</param>
        /// <param name="uplinePositionTakingOnMember">Up-line position taking on member when set from auto PT to none.</param>
        /// <returns>Update status and information for showing on web.</returns>
        private List<ICommandResult> UpdateSuperPtAndComSetting(
            LiveCasinoSettingContext settingContext,
            float commission,
            PositionTaking positionTaking)
        {
            ICommandResult commandResult = _settingRepository.Save(new UpdateSuperSettingCriteria
            {
                CustId = settingContext.Customer.Id,
                IP = settingContext.IP,
                Language = settingContext.Language,
                UserId = settingContext.ActorId,
                SuperCommission = (decimal)commission,
                S4 = (decimal)positionTaking.S4,
                N4 = (decimal)positionTaking.N4,
            });

            return GetPtAndComResultDetails(commandResult);
        }

        private List<ICommandResult> UpdateMasterPtAndComSetting(
            LiveCasinoSettingContext settingContext,
            float commission,
            PositionTaking positionTaking,
            bool isApplyPositionTakingAllMembers,
            float positionTakingOnMembers)
        {
            ICriteria criteria = GetCriteriaForUpdatingMasterPtAndComSetting(settingContext, commission, positionTaking, isApplyPositionTakingAllMembers, positionTakingOnMembers);
            ICommandResult commandResult = _settingRepository.Save(criteria);

            return GetPtAndComResultDetails(commandResult);
        }

        private List<ICommandResult> UpdateAgentPtAndComSetting(
            LiveCasinoSettingContext settingContext,
            float commission,
            PositionTaking positionTaking,
            bool isApplyPositionTakingAllMembers,
            float positionTakingOnMembers)
        {
            ICriteria criteria = GetCriteriaForUpdatingAgentPtAndComSetting(settingContext, commission, positionTaking, isApplyPositionTakingAllMembers, positionTakingOnMembers);
            ICommandResult commandResult = _settingRepository.Save(criteria);

            return GetPtAndComResultDetails(commandResult);
        }

        private List<ICommandResult> AgentUpdateMemberPtAndComSetting(
            LiveCasinoSettingContext settingContext,
            float commission,
            PositionTaking positionTaking)
        {
            ICriteria criteria = GetCriteriaForAgentUpdateMemberPtAndComSetting(settingContext, commission, positionTaking);
            ICommandResult commandResult = _settingRepository.Save(criteria);

            return GetPtAndComResultDetails(commandResult);
        }

        private List<ICommandResult> SuperUpdateMemberPtAndComSetting(
            LiveCasinoSettingContext settingContext,
            float commission,
            PositionTaking positionTaking)
        {
            ICriteria criteria = GetCriteriaForSuperUpdateMemberPtAndComSetting(settingContext, commission, positionTaking);
            ICommandResult commandResult = _settingRepository.Save(criteria);

            return GetPtAndComResultDetails(commandResult);
        }

        private List<ICommandResult> MasterUpdateMemberPtAndComSetting(
            LiveCasinoSettingContext settingContext,
            float commission,
            PositionTaking positionTaking)
        {
            ICriteria criteria = GetCriteriaForMasterUpdateMemberPtAndComSetting(settingContext, commission, positionTaking);
            ICommandResult commandResult = _settingRepository.Save(criteria);

            return GetPtAndComResultDetails(commandResult);
        }

        /// <summary>
        /// Get information after updated.
        /// </summary>
        /// <param name="commandResult">General information for analyzing to small message.</param>
        /// <returns>List message for showing on web.</returns>
        private List<ICommandResult> GetPtAndComResultDetails(ICommandResult commandResult)
        {
            List<ICommandResult> resultDetails = new List<ICommandResult>();

            if (commandResult.Code != null
                && Converter.ToString2(commandResult.Code) == ErrCode.InvalidCriteria.ToString())
            {
                resultDetails.Add(commandResult);
                return resultDetails;
            }

            string[] errCodes = commandResult.Code.Split('\a');
            string[] errMsgs = commandResult.Message.Split('\a');

            for (int i = 0; i < errCodes.Length; i++)
            {
                if (!string.IsNullOrEmpty(errCodes[i]))
                {
                    var errCode = Converter.ToInt(errCodes[i]);
                    if (errCode < 0)
                    {
                        resultDetails.Add(new PtAndComCommandResultDTO
                        {
                            Code = errCode,
                            Message = LiveCasinoLang.ResourceManager.GetString(string.Format("_{0}", Math.Abs(errCode)))
                        });
                    }
                    else
                    {
                        resultDetails.Add(new PtAndComCommandResultDTO
                        {
                            Code = errCode,
                            Message = errMsgs[i]
                        });
                    }
                }
            }

            return resultDetails;
        }

        /// <summary>
        /// Get store procedure parameter value for updating customer.
        /// </summary>
        /// <param name="settingContext">Setting context for updating customer.</param>
        /// <param name="positionTakingAndComSetting">Position taking and Commission setting for updating.</param>
        /// <param name="uplinePositionTakingOnMember">Up-line position taking on member when set from auto PT to none.</param>
        /// <returns>Criteria for update customer.</returns>
        private ICriteria GetCriteriaForUpdatingMasterPtAndComSetting(
            LiveCasinoSettingContext settingContext,
            float commission,
            PositionTaking positionTaking,
            bool isApplyPositionTakingAllMembers,
            float positionTakingOnMembers)
        {
            var criteria = new UpdateMasterSettingCriteria
            {
                CustId = settingContext.Customer.Id,
                IP = settingContext.IP,
                Language = settingContext.Language,
                SRecommend = settingContext.Upline.Id,
                UserId = settingContext.ActorId,
                SubAccID = settingContext.SubAccount.Id,
                MasterCommission = (decimal)commission,
                M3 = (decimal)positionTaking.M3,
                N3 = (decimal)positionTaking.N3,
                Is3 = (bool)positionTaking.Is3,
                Us3 = (decimal)positionTaking.Us3,
                UpdateAllMembers = isApplyPositionTakingAllMembers,
                S3 = (decimal)positionTaking.S3,
                NewSuperPositionTakingOnMember = (decimal)positionTakingOnMembers
            };

            if (criteria.Is3)
            {
                criteria.S3 = 0;
            }
            else
            {
                criteria.Us3 = 0;
            }

            return criteria;
        }

        /// <summary>
        /// Get store procedure parameter value for updating customer.
        /// </summary>
        /// <param name="settingContext">Setting context for updating customer.</param>
        /// <param name="commission">Commission setting for updating.</param>
        /// <param name="positionTaking">Position taking setting for updating.</param>
        /// <param name="uplinePositionTakingOnMember">Up-line position taking on member when set from auto PT to none.</param>
        /// <returns>Criteria for update customer.</returns>
        private ICriteria GetCriteriaForUpdatingAgentPtAndComSetting(
            LiveCasinoSettingContext settingContext,
            float commission,
            PositionTaking positionTaking,
            bool isApplyPositionTakingAllMembers,
            float positionTakingOnMembers)
        {
            var criteria = new UpdateAgentSettingCriteria
            {
                CustId = settingContext.Customer.Id,
                IP = settingContext.IP,
                Language = settingContext.Language,
                MRecommend = settingContext.Upline.Id,
                UserId = settingContext.ActorId,
                SubAccID = settingContext.SubAccount.Id,
                AgentCommission = (decimal)commission,
                M2 = (decimal)positionTaking.M2,
                N2 = (decimal)positionTaking.N2,
                Im2 = (bool)positionTaking.Im2,
                Um2 = (decimal)positionTaking.Um2,
                UpdateAllMembers = (bool)isApplyPositionTakingAllMembers,
                A2 = (decimal)positionTaking.A2,
                NewMasterPtOnMember = (decimal)positionTakingOnMembers
            };

            if (criteria.Im2)
            {
                criteria.M2 = 0;
            }
            else
            {
                criteria.Um2 = 0;
            }

            return criteria;
        }

        /// <summary>
        /// Get store procedure parameter value for updating customer.
        /// </summary>
        /// <param name="settingContext">Setting context for updating customer.</param>
        /// <param name="commission">Commission setting for updating.</param>
        /// <param name="positionTaking">Position taking setting for updating.</param>
        /// <returns>Criteria for update customer.</returns>
        private ICriteria GetCriteriaForAgentUpdateMemberPtAndComSetting(
            LiveCasinoSettingContext settingContext,
            float commission,
            PositionTaking positionTaking)
        {
            ICriteria criteria = new AgentUpdateMemberSettingCriteria
            {
                CustId = settingContext.Customer.Id,
                IP = settingContext.IP,
                Language = settingContext.Language,
                Recommend = settingContext.Upline.Id,
                UserId = settingContext.ActorId,
                SubAccID = settingContext.SubAccount.Id,
                MemberCommission = (decimal)commission,
                A1 = (decimal)positionTaking.A1
            };
            return criteria;
        }

        /// <summary>
        /// Get store procedure parameter value for updating customer.
        /// </summary>
        /// <param name="settingContext">Setting context for updating customer.</param>
        /// <param name="commission">Commission setting for updating.</param>
        /// <param name="positionTaking">Position taking setting for updating.</param>
        /// <returns>Criteria for update customer.</returns>
        private ICriteria GetCriteriaForSuperUpdateMemberPtAndComSetting(
            LiveCasinoSettingContext settingContext,
            float commission,
            PositionTaking positionTaking)
        {
            var criteria = new SuperUpdateMemberSettingCriteria
            {
                CustId = settingContext.Customer.Id,
                IP = settingContext.IP,
                Language = settingContext.Language,
                UserId = settingContext.ActorId,
                SubAccID = settingContext.SubAccount.Id,
                S1 = (decimal)positionTaking.S1,
                Is1 = (bool)positionTaking.Is1,
                Us1 = (decimal)positionTaking.Us1,
                IsUpdateSuperPt = !(bool)positionTaking.Is3
            };

            if (criteria.Is1)
            {
                criteria.S1 = 0;
            }
            else
            {
                criteria.Us1 = 0;
            }

            return criteria;
        }

        /// <summary>
        /// Get store procedure parameter value for updating customer.
        /// </summary>
        /// <param name="settingContext">Setting context for updating customer.</param>
        /// <param name="commission">Commission setting for updating.</param>
        /// <param name="positionTaking">Position taking setting for updating.</param>
        /// <returns>Criteria for update customer.</returns>
        private ICriteria GetCriteriaForMasterUpdateMemberPtAndComSetting(
            LiveCasinoSettingContext settingContext,
            float commission,
            PositionTaking positionTaking)
        {
            var criteria = new MasterUpdateMemberSettingCriteria
            {
                CustId = settingContext.Customer.Id,
                IP = settingContext.IP,
                Language = settingContext.Language,
                UserId = settingContext.ActorId,
                SubAccID = settingContext.SubAccount.Id,
                M1 = (decimal)positionTaking.M1,
                Im1 = (bool)positionTaking.Im1,
                Um1 = (decimal)positionTaking.Um1,
                Im2 = (bool)positionTaking.Im2
            };

            if (criteria.Im1)
            {
                criteria.M1 = 0;
            }
            else
            {
                criteria.Um1 = 0;
            }

            return criteria;
        }

        private UplineCustomerInfoDTO GetUplineCustomerInfo(int custId)
        {
            var criteria = new GetCustomerInfoCriteria
            {
                CustId = custId
            };

            return _settingRepository.GetUplineCustomerInfo(criteria);
        }
    }
}