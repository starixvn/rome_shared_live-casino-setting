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
    using Fanex.LiveCasino.CoreSetting.Criteria.Log;
    using Fanex.LiveCasino.CoreSetting.Criteria.WinLimit;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.DTO.CommandResult;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Models.Log;
    using Fanex.LiveCasino.CoreSetting.Repositories;
    using Fanex.LiveCasino.CoreSetting.Repositories.PTCom;
    using Fanex.LiveCasino.CoreSetting.Resources;
    using Fanex.LiveCasino.CoreSetting.Services.PTCom;
    using Fanex.Logging;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Setting service for update position taking setting, Commission, member win limit.
    /// </summary>
    public class SettingService : ISettingService
    {
        private const string CasinoNo = "01";
        private readonly ISettingRepository _settingRepository;
        private readonly IPTComServiceFactory _ptComServiceFactory;

        public SettingService()
            : this(null, null, null, null)
        {
        }

        public SettingService(
            ISettingRepository settingRepository,
            IPTComService ptComService,
            IPTComServiceFactory ptComServiceFactory,
            IPTComRepository ptComRepository)
        {
            _settingRepository = settingRepository ?? new SettingRepository();
            _ptComServiceFactory = ptComServiceFactory ?? new PTComServiceFactory(ptComService, ptComRepository);
        }

        public Customer GetCustomer(LiveCasinoSettingContext settingContext)
        {
            Customer customer = GetCustomerInfo(settingContext.Customer.Id);

            SetCustomerPtAndComSetting(settingContext, customer);
            customer.LiveCasinoSetting.GameSetting = SetGameSetting(customer);

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
                return GetCustomerInfo(settingContextMultiple.Customer.Id);
            }

            settingContextMultiple.Customers = settingContextMultiple.Customers.Where(customer =>
                {
                    return customer.IsSyncLiveCasino;
                }).ToList();

            //// Super/Master multiple member
            if (settingContextMultiple.Customers.Any())
            {
                GetCustomerInfo(settingContextMultiple.Customers.First().Id);
            }

            return null;
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
                throw new Exception(string.Format(LiveCasinoLang.InvalidCriteriaMsg, criteria.GetSettingKey()));
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

            Customer custInfo = _settingRepository.GetCustomerLiveCasinoInfo(criteria);

            UplineCustomerInfoDTO uplineInfo = _settingRepository.GetUplineCustomerInfo(new GetCustomerInfoCriteria { CustId = custId });

            switch (custInfo.Level)
            {
                case CustomerLevel.Member:
                    custInfo.Parent.Id = uplineInfo.AgentId;
                    custInfo.Parent.Parent = new Customer { Id = uplineInfo.MasterId };
                    custInfo.Parent.Parent.Parent = new Customer { Id = uplineInfo.SuperId };
                    break;

                case CustomerLevel.Agent:
                    custInfo.Parent.Id = uplineInfo.MasterId;
                    custInfo.Parent.Parent = new Customer { Id = uplineInfo.SuperId };
                    break;

                case CustomerLevel.Master:
                    custInfo.Parent.Id = uplineInfo.SuperId;
                    break;

                default:
                    custInfo.Parent.Id = 0;
                    break;
            }

            return custInfo;
        }

        public List<GameType> SetGameSetting(Customer customer)
        {
            Check.Argument.IsNotNull(customer, "customer");
            Check.Argument.IsNotNull(customer.Parent, "customer.Parent");
            Check.Argument.IsNotNegativeOrZero(customer.Id, "customer.Id");
            Check.Argument.IsNotNegativeOrZero((int)customer.Level, "customer.Level");
            Check.Argument.IsNotNull(customer.IsSyncLiveCasino, "customer.IsSyncLiveCasino");
            Check.Argument.IsNotEmpty(customer.Username, "customer.Username");
            Check.Argument.IsNotNegativeOrZero(customer.CurrencyId, "customer.CurrencyId");
            Check.Argument.IsNotNegative(customer.Parent.Id, "customer.Parent.Id");

            var gameSettings = new List<GameType>();

            if (customer.Level == CustomerLevel.Super && !customer.IsSyncLiveCasino)
            {
                var site = GetCustomerSite(customer.Username);
                gameSettings = _settingRepository.GetGameSetting(customer.CurrencyId, site, defaultCheckedValue: true);
            }
            else if (customer.Level != CustomerLevel.Super && !customer.IsSyncLiveCasino)
            {
                gameSettings = _settingRepository.GetGameSetting(customer.Parent.Id);
                gameSettings.RemoveAll(x => string.IsNullOrWhiteSpace(x.CheckedStakeLevelRange));

                // Check all by default
                foreach (var gameSettingItem in gameSettings)
                {
                    gameSettingItem.BettingLimit.RemoveAll(x => !x.IsChecked);
                    if (customer.Level == CustomerLevel.Member)
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
                gameSettings = _settingRepository.GetGameSetting(customer.Id);
            }

            return gameSettings;
        }

        public ICommandResult InsertAutoAccount(int custId, int actorId, string actorUsername, int subAccountId, string ip, string language)
        {
            /*var uplineCustomerInfo = GetUplineCustomerInfo(custId);

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

            return result;*/
            throw new Exception();
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
            //            var fromCustInfo = BuildCustomers(settingContext.OldCustId);
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
            var oldGameTypes = new List<GameType>();
            var newGameTypes = CloneGameTypes(gameTypes);

            if (!settingContext.Customer.IsSyncLiveCasino)
            {
                var insertCommandResult = ManualSyncAccount(settingContext);
                if (settingContext.Customer.Level != CustomerLevel.Super && insertCommandResult.IsSuccessful)
                {
                    Customer directUplineInfo = GetCustomerInfo(settingContext.Customer.Parent.Id);
                    if (directUplineInfo.IsSyncLiveCasino)
                    {
                        if (newGameTypes == null)
                        {
                            string site = GetCustomerSite(settingContext.Customer.Username);
                            newGameTypes = _settingRepository.GetGameSetting(directUplineInfo.CurrencyId, site);
                        }
                        else
                        {
                            List<GameType> directUplineGameTypes = _settingRepository.GetGameSetting(settingContext.Customer.Parent.Id);
                            if (newGameTypes.Count < directUplineGameTypes.Count)
                            {
                                var gameTypeUncheckeds = new List<GameType>();
                                foreach (var itemGameType in directUplineGameTypes)
                                {
                                    var foundedGameType = newGameTypes.Where(x => x.Id == itemGameType.Id).FirstOrDefault();
                                    if (foundedGameType == null)
                                    {
                                        gameTypeUncheckeds.Add(itemGameType);
                                    }
                                }

                                newGameTypes.AddRange(gameTypeUncheckeds);
                                newGameTypes = newGameTypes.OrderBy(x => x.Id).ToList();
                            }
                        }
                    }
                }
            }
            else
            {
                oldGameTypes = _settingRepository.GetGameSetting(settingContext.Customer.Id);
            }

            ICommandResultsDTO resultUpdateGameTypes = UpdateGameTypes(settingContext, newGameTypes, oldGameTypes);

            return resultUpdateGameTypes;
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
            //        var uplineCustInfo = BuildCustomers(settingContext.DirectParentId);
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
        /// <param name="settingContext">The setting context.</param>
        /// <param name="newGameTypes">The game types.</param>
        /// <param name="oldGameTypes">The old game types.</param>
        /// <returns>
        /// Command results update list stake level.
        /// </returns>
        private ICommandResultsDTO UpdateGameTypes(LiveCasinoSettingContext settingContext, List<GameType> newGameTypes, List<GameType> oldGameTypes)
        {
            var results = new CommandResultsDTO();

            if (newGameTypes != null && newGameTypes.Any())
            {
                foreach (var itemNewGameType in newGameTypes)
                {
                    var itemOldGameType = oldGameTypes.Where(x => x.Id == itemNewGameType.Id).FirstOrDefault();
                    var result = UpdateStakeLevel(settingContext, itemNewGameType, itemOldGameType);

                    results.Results.Add(result);
                }
            }

            return results;
        }

        /// <summary>
        /// Updates the stake level.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="newGameType">Type of the game.</param>
        /// <param name="oldGameType">Old type of the game.</param>
        /// <returns>
        /// Command result update stake level.
        /// </returns>
        private ICommandResult UpdateStakeLevel(LiveCasinoSettingContext settingContext, GameType newGameType, GameType oldGameType)
        {
            var criteria = new UpdateStakeLevelCriteria
            {
                CustId = settingContext.Customer.Id,
                RoleId = (int)settingContext.Customer.Level,
                GameTypeId = newGameType.Id,
                StakeLevelList = newGameType.CheckedStakeLevelId
            };

            var updateResult = (CommandResultDTO)_settingRepository.Save(criteria);

            var gameSettingResult = new GameSettingCommandResultDTO(updateResult);

            if (gameSettingResult.IsSuccessful)
            {
                gameSettingResult.Id = newGameType.Id;
                gameSettingResult.Message = string.Format("{0} - {1}: {2}", LiveCasinoLang.lblGameSetting, newGameType.Name, LiveCasinoLang.UpdateSuccessfully);

                var log = new UpdateStakeLevelLog
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
            else
            {
                gameSettingResult.Message = string.Format("{0} - {1}: {2}", LiveCasinoLang.lblGameSetting, newGameType.Name, LiveCasinoLang.UpdatedUnsuccessfully);
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

        public static List<Customer> BuildCustomers(string custIds, string usernames, string syncStatus)
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

        private ICommandResult WriteLog(IGameSettingLog logData)
        {
            var criteria = new InsertLogUserRunSPCriteria
            {
                UserId = logData.SettingContext.ActorId,
                CustId = logData.SettingContext.Customer.Id,
                SpName = logData.SpName,
                Doing = string.Empty,
                ActionId = (int)logData.Action,
                TrackingValue = logData.NewValue,
                TrackingValue2 = logData.OldValue,
                Duration = 0
            };

            try
            {
                var updateResult = (CommandResultDTO)_settingRepository.Save(criteria);

                return updateResult;
            }
            catch (Exception ex)
            {
                Logger.Log.Error(string.Format("{0}\n{1}", ex.Message, criteria.ToString()), LiveCasinoSettings.Settings.LogCategory);

                throw ex;
            }
        }

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

            _ptComServiceFactory
                .GetService(settingContext)
                .SetPTComSetting(customer, settingContext);
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
            var ptComService = _ptComServiceFactory.GetService(settingContext);

            ptComService.SetModifiedData(commission, positionTaking, isApplyPositionTakingAllMembers, positionTakingOnMembers);

            return ptComService.Update(settingContext);
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
            var ptComService = _ptComServiceFactory.GetService(settingContextMultiple);

            ptComService.SetModifiedData(commission, positionTaking, isApplyPositionTakingAllMembers, positionTakingOnMembers);

            return ptComService.UpdateMultiple(settingContextMultiple.Customers, settingContextMultiple);
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