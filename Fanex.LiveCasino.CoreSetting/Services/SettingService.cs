namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
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
            Customer customer = GetCustomerInfoAndPTComSetting(settingContext);
            SetGameSetting(customer);

            return customer;
        }

        public Customer GetCustomerInfoAndPTComSetting(LiveCasinoSettingContext settingContext)
        {
            Customer customer = GetCustomerLiveCasinoInfo(settingContext.Customer.Id);

            SetCustomerPtAndComSetting(settingContext, customer);

            return customer;
        }

        /// <summary>
        /// Get customer information with customer id.
        /// </summary>
        /// <param name="custId">Get criteria information.</param>
        /// <returns>Customer information.</returns>
        public Customer GetCustomerLiveCasinoInfo(int custId)
        {
            var criteria = new GetCustomerLiveCasinoInfoCriteria
            {
                CustId = custId
            };

            Customer custInfo = _settingRepository.GetCustomerLiveCasinoInfo(criteria);

            CustomerInfoDTO uplineInfo = GetCustomerInfo(custId);

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

        /// <summary>
        /// Get Representative customer for editing multiple.
        /// </summary>
        /// <param name="settingContextMultiple">The setting context multiple.</param>
        /// <returns>
        /// Customer id representative.
        /// </returns>
        public int GetRepresentativeCustomerId(LiveCasinoSettingContextMultiple settingContextMultiple)
        {
            if (!settingContextMultiple.Customers.Any())
            {
                return 0;
            }

            //// Edit multiple direct downline. Load setting info of the first selected downline in customer list
            int levelGap = (int)settingContextMultiple.Upline.Level - (int)settingContextMultiple.Customer.Level;
            if (levelGap == 1)
            {
                return settingContextMultiple.Customers.First().Id;
            }

            //// Remove UnSync Member
            settingContextMultiple.Customers = settingContextMultiple.Customers.Where(customer =>
                {
                    return customer.IsSyncLiveCasino;
                }).ToList();

            //// Super/Master multiple member
            if (settingContextMultiple.Customers.Any())
            {
                return settingContextMultiple.Customers.First().Id;
            }

            return 0;
        }

        /// <summary>
        /// Update single customer all setting.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="customer">Customer setting information for updating.</param>
        /// <param name="isApplyPositionTakingAllMembers">If set to <c>true</c> [is apply position taking all members].</param>
        /// <param name="positionTakingOnMembers">The position taking on members.</param>
        /// <returns>
        /// Update result.
        /// </returns>
        public ICommandResults UpdateCustomerSetting(
            LiveCasinoSettingContext settingContext,
            Customer customer,
            bool isApplyPositionTakingAllMembers = false,
            float positionTakingOnMembers = 0)
        {
            ICommandResults resultList = new CommandResults();

            if (settingContext.Customer.Level == CustomerLevel.Member
                && settingContext.Upline.Level == CustomerLevel.Agent
                && settingContext.Customer.IsSyncLiveCasino
                && !settingContext.Customer.Parent.DisableLiveCasino)
            {
                resultList.Results.Add(UpdateLiveCasinoStatus(settingContext, customer.DisableLiveCasino));
            }

            var positionTakingAndCommResults = UpdatePTComSetting(
                settingContext,
                customer.LiveCasinoSetting.Commission,
                customer.LiveCasinoSetting.PositionTaking,
                isApplyPositionTakingAllMembers,
                positionTakingOnMembers);
            resultList.Results.AddRange(positionTakingAndCommResults.Results);

            if (settingContext.Customer.IsSyncLiveCasino
                || (!settingContext.Customer.IsSyncLiveCasino && positionTakingAndCommResults.IsSuccessful))
            {
                var winLimitResults = UpdateWinLimit(settingContext, customer.LiveCasinoSetting.WinLimit).Results;
                resultList.Results.AddRange(winLimitResults);

                var gameSettingResult = UpdateGameSetting(settingContext, customer.LiveCasinoSetting.GameSetting, customer.DisableLiveCasino).Results;
                resultList.Results.AddRange(gameSettingResult);
            }
            else
            {
                resultList.Results.Add(new CommandResult
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
        /// <param name="settingContextMultiple">The setting context multiple.</param>
        /// <param name="customer">Customer setting contains customer setting information.</param>
        /// <param name="updateType">Update type for multiple.</param>
        /// <param name="isApplyPositionTakingAllMembers">If set to <c>true</c> [is apply position taking all members].</param>
        /// <param name="positionTakingOnMembers">The position taking on members.</param>
        /// <returns>
        /// Multiple result return to user.
        /// </returns>
        public IMultipleCommandResults UpdateMultipleCustomerSetting(
            LiveCasinoSettingContextMultiple settingContextMultiple,
            Customer customer,
            UpdateType updateType,
            bool isApplyPositionTakingAllMembers = false,
            float positionTakingOnMembers = 0)
        {
            IMultipleCommandResults mainResults = new MultipleCommandResults();

            foreach (var custInfo in settingContextMultiple.Customers)
            {
                settingContextMultiple.Customer.Id = custInfo.Id;
                settingContextMultiple.Customer.Username = custInfo.Username;
                settingContextMultiple.Customer.IsSyncLiveCasino = custInfo.IsSyncLiveCasino;

                var subResult = UpdateEachCustomerInMultipleEdit(settingContextMultiple, customer, updateType, isApplyPositionTakingAllMembers, positionTakingOnMembers);
                subResult.Id = custInfo.Id;
                subResult.Name = custInfo.Username;
                mainResults.CommandResultsList.Add(subResult);
            }

            return mainResults;
        }

        /// <summary>
        /// Get Customer Setting for Editing.
        /// </summary>
        /// <param name="settingContext">Current context for getting customer information.</param>
        /// <param name="customer">Current customer need be set position taking and commission data.</param>
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
        /// <param name="commission">Customer commission.</param>
        /// <param name="positionTaking">Customer position taking.</param>
        /// <param name="isApplyPositionTakingAllMembers">Apply new position taking on member or not for case change from auto to none.</param>
        /// <param name="positionTakingOnMembers">New position taking on member for case change from auto to none.</param>
        /// <returns>Update command results.</returns>
        public ICommandResults UpdatePTComSetting(
            LiveCasinoSettingContext settingContext,
            float commission,
            PositionTaking positionTaking,
            bool isApplyPositionTakingAllMembers = false,
            float positionTakingOnMembers = 0)
        {
            Check.Argument.IsNotNull(settingContext, "settingContext");
            Check.Argument.IsNotNull(positionTaking, "positionTaking");

            var ptComService = _ptComServiceFactory.GetService(settingContext);

            ptComService.SetModifiedData(commission, positionTaking, isApplyPositionTakingAllMembers, positionTakingOnMembers);

            return ptComService.Update(settingContext);
        }

        /// <summary>
        /// Update multiple Customer Position Taking and Commission.
        /// </summary>
        /// <param name="settingContextMultiple">Setting context of current update setting.</param>
        /// <param name="commission">Customer commission.</param>
        /// <param name="positionTaking">Customer position taking.</param>
        /// <param name="isApplyPositionTakingAllMembers">Apply new position taking on member or not for case change from auto to none.</param>
        /// <param name="positionTakingOnMembers">New position taking on member for case change from auto to none.</param>
        /// <returns>Update command results.</returns>
        public IMultipleCommandResults UpdateMultiplePTComSetting(
            LiveCasinoSettingContextMultiple settingContextMultiple,
            float commission,
            PositionTaking positionTaking,
            bool isApplyPositionTakingAllMembers = false,
            float positionTakingOnMembers = 0)
        {
            Check.Argument.IsNotNull(settingContextMultiple, "settingContext");
            Check.Argument.IsNotNull(positionTaking, "positionTaking");

            var ptComService = _ptComServiceFactory.GetService(settingContextMultiple);

            ptComService.SetModifiedData(commission, positionTaking, isApplyPositionTakingAllMembers, positionTakingOnMembers);

            return ptComService.UpdateMultiple(settingContextMultiple.Customers, settingContextMultiple);
        }

        /// <summary>
        /// Automatics insert account to Live Casino.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="actorId">The actor identifier.</param>
        /// <param name="actorUsername">The actor username.</param>
        /// <param name="subAccountId">The sub account identifier.</param>
        /// <param name="subAccountUsername">The sub account username.</param>
        /// <param name="ip">The internet protocol.</param>
        /// <param name="language">The language.</param>
        /// <returns>Update result.</returns>
        public ICommandResult AutoInsertAccount(int custId, int actorId, string actorUsername, int subAccountId, string subAccountUsername, string ip, string language)
        {
            Check.Argument.IsNotNegativeOrZero(custId, "custId");
            Check.Argument.IsNotNegativeOrZero(actorId, "actorId");
            Check.Argument.IsNotEmpty(actorUsername, "actorUsername");
            Check.Argument.IsNotNegative(subAccountId, "subAccountId");
            Check.Argument.IsNotEmpty(ip, "ip");
            Check.Argument.IsNotEmpty(language, "language");

            var gameTypes = new List<GameType>();
            var settingContext = new LiveCasinoSettingContext();

            ICommandResult resultInsert = InsertAccount(custId, actorId, actorUsername, subAccountId, subAccountUsername, ip, language, settingContext, gameTypes);

            if (resultInsert.IsSuccessful)
            {
                ICommandResult updateSyncResult = UpdateSyncLiveCasino(settingContext, isSynced: true);

                if (updateSyncResult.IsSuccessful)
                {
                    var uplineCustInfo = GetCustomerLiveCasinoInfo(settingContext.Customer.Parent.Id);

                    if (!uplineCustInfo.DisableLiveCasino)
                    {
                        UpdateLiveCasinoStatus(settingContext, uplineCustInfo.DisableLiveCasino);
                    }
                }

                gameTypes = _settingRepository.GetGameSetting(settingContext.Customer.Parent.Id);

                IntegrityGameSetting(settingContext, ref gameTypes);

                WriteLog(new AutoInsertAccountLog
                {
                    CasinoNo = CasinoNo,
                    SignUpForm = LiveCasinoSettings.Settings.SystemId,
                    GameSetting = gameTypes,
                    SettingContext = settingContext
                });
            }

            return resultInsert;
        }

        /// <summary>
        /// Automatics insert account to Live Casino from existing account.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="oldCustId">The old customer identifier.</param>
        /// <param name="toUsername">To username.</param>
        /// <param name="actorId">The actor identifier.</param>
        /// <param name="actorUsername">The actor username.</param>
        /// <param name="subAccountId">The sub account identifier.</param>
        /// <param name="subAccountUsername">The sub account username.</param>
        /// <param name="ip">The internet protocol.</param>
        /// <param name="language">The language.</param>
        /// <returns>Update result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public ICommandResults AutoInsertAccountFromExisting(
            int custId,
            int oldCustId,
            string toUsername,
            int actorId,
            string actorUsername,
            int subAccountId,
            string subAccountUsername,
            string ip,
            string language)
        {
            Check.Argument.IsNotNegativeOrZero(custId, "custId");
            Check.Argument.IsNotNegativeOrZero(oldCustId, "oldCustId");
            Check.Argument.IsNotEmpty(toUsername, "toUsername");
            Check.Argument.IsNotNegativeOrZero(actorId, "actorId");
            Check.Argument.IsNotEmpty(actorUsername, "actorUsername");
            Check.Argument.IsNotNegative(subAccountId, "subAccountId");
            Check.Argument.IsNotEmpty(ip, "ip");
            Check.Argument.IsNotEmpty(language, "language");

            var result = new CommandResults();
            var oldSettingContext = new LiveCasinoSettingContext();
            var oldCustomerSyncedStatus = false;

            InitSettingContext(oldCustId, subAccountId, subAccountUsername, actorId, actorUsername, ip, language, oldSettingContext);

            oldCustomerSyncedStatus = oldSettingContext.Customer.IsSyncLiveCasino;

            if (oldSettingContext.Customer.Level == CustomerLevel.Member)
            {
                Customer oldCustomer = GetCustomer(oldSettingContext);

                oldCustomer.Id = custId;
                oldCustomer.IsSyncLiveCasino = false;

                oldSettingContext.Customer = oldCustomer;

                ICommandResults resultPTCom = UpdatePTComSetting(oldSettingContext, oldCustomer.LiveCasinoSetting.Commission, oldCustomer.LiveCasinoSetting.PositionTaking);
                result.Results.AddRange(resultPTCom.Results);

                ICommandResults resultWinLimit = UpdateWinLimit(oldSettingContext, oldCustomer.LiveCasinoSetting.WinLimit);
                result.Results.AddRange(resultWinLimit.Results);
            }

            if (oldCustomerSyncedStatus)
            {
                oldSettingContext.Customer.Id = custId;
                oldSettingContext.Customer.IsSyncLiveCasino = false;

                var gameTypes = new List<GameType>();

                ICommandResult resultInsert = InsertAccount(
                                                custId,
                                                oldCustId,
                                                toUsername,
                                                subAccountId,
                                                subAccountUsername,
                                                actorId,
                                                actorUsername,
                                                ip,
                                                language,
                                                oldSettingContext,
                                                gameTypes);

                result.Results.Add(resultInsert);

                if (resultInsert.IsSuccessful)
                {
                    ICommandResult updateSyncResult = UpdateSyncLiveCasino(oldSettingContext, isSynced: true);

                    gameTypes = _settingRepository.GetGameSetting(oldCustId);

                    IntegrityGameSetting(oldSettingContext, ref gameTypes);

                    WriteLog(new InsertFromExistingAccountLog
                    {
                        ToUsername = toUsername,
                        CasinoNo = CasinoNo,
                        SignUpForm = LiveCasinoSettings.Settings.SystemId,
                        GameSetting = gameTypes,
                        SettingContext = oldSettingContext
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Updates the live casino status.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="disableLiveCasino">If set to <c>true</c> [disable live casino].</param>
        /// <returns>Update result.</returns>
        public ICommandResult UpdateLiveCasinoStatus(LiveCasinoSettingContext settingContext, bool disableLiveCasino)
        {
            Check.Argument.IsNotNull(settingContext, "settingContext");
            Check.Argument.IsNotNull(settingContext.Customer, "settingContext.Customer");
            Check.Argument.IsNotNull(settingContext.Actor, "settingContext.Actor");
            Check.Argument.IsNotNull(settingContext.SubAccount, "settingContext.SubAccount");
            Check.Argument.IsNotNegativeOrZero(settingContext.Customer.Id, "settingContext.Customer.Id");
            Check.Argument.IsNotNegativeOrZero(settingContext.Actor.Id, "settingContext.ActorId");
            Check.Argument.IsNotEmpty(settingContext.IP, "settingContext.IP");
            Check.Argument.IsNotEmpty(settingContext.Language, "settingContext.Language");

            var criteria = new UpdateLiveCasinoStatusCriteria
            {
                CustId = settingContext.Customer.Id,
                UserID = settingContext.Actor.Id,
                DisabledLiveCS = disableLiveCasino,
                EnableDownlinesLiveCS = !disableLiveCasino,
                SubAccID = settingContext.SubAccount.Id,
                IP = settingContext.IP,
                Language = settingContext.Language
            };

            var updateResult = (CommandResult)_settingRepository.Save(criteria);

            if (updateResult.IsSuccessful)
            {
                updateResult = new CommandResult
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
        /// Updates the synchronize live casino.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="isSynced">If set to <c>true</c> [is synced].</param>
        /// <returns>Update result.</returns>
        public ICommandResult UpdateSyncLiveCasino(LiveCasinoSettingContext settingContext, bool isSynced)
        {
            Check.Argument.IsNotNull(settingContext, "settingContext");
            Check.Argument.IsNotNull(settingContext.Customer, "settingContext.Customer");
            Check.Argument.IsNotNull(settingContext.SubAccount, "settingContext.SubAccount");
            Check.Argument.IsNotNull(settingContext.Actor, "settingContext.Actor");
            Check.Argument.IsNotNegativeOrZero(settingContext.Customer.Id, "settingContext.Customer.Id");
            Check.Argument.IsNotNegativeOrZero(settingContext.Actor.Id, "settingContext.Actor.Id");
            Check.Argument.IsNotEmpty(settingContext.IP, "settingContext.IP");
            Check.Argument.IsNotEmpty(settingContext.Language, "settingContext.Language");

            var criteria = new UpdateSyncLiveCasinoCriteria
            {
                CustId = settingContext.Customer.Id,
                UserId = settingContext.Actor.Id,
                Ip = settingContext.IP,
                Language = settingContext.Language,
                SubAccId = settingContext.SubAccount.Id,
                SyncLiveCS = isSynced
            };

            var updateResult = (CommandResult)_settingRepository.Save(criteria);

            if (!updateResult.IsSuccessful)
            {
                Logger.Log.Info(string.Format("{0}\n{1}", updateResult.Message, criteria.ToString()), LiveCasinoSettings.Settings.LogCategory);
            }

            return updateResult;
        }

        /// <summary>
        /// Updates the manual.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="gameTypes">The game types.</param>
        /// <param name="disableLiveCasino">If set to <c>true</c> [disable live casino].</param>
        /// <returns>
        /// Update result.
        /// </returns>
        public ICommandResults UpdateGameSetting(LiveCasinoSettingContext settingContext, List<GameType> gameTypes, bool disableLiveCasino)
        {
            Check.Argument.IsNotNull(settingContext, "settingContext");
            Check.Argument.IsNotNull(settingContext.Customer, "settingContext.Customer");
            Check.Argument.IsNotNull(settingContext.Customer.Parent, "settingContext.Customer.Parent");
            Check.Argument.IsNotNull(settingContext.Actor, "settingContext.Actor");
            Check.Argument.IsNotNegativeOrZero(settingContext.Customer.Id, "settingContext.Customer.Id");

            var oldGameTypes = new List<GameType>();
            var newGameTypes = CloneGameTypes(gameTypes);

            if (!settingContext.Customer.IsSyncLiveCasino)
            {
                var insertCommandResult = InsertAccount(settingContext, disableLiveCasino);

                if (settingContext.Customer.Level != CustomerLevel.Super && insertCommandResult.IsSuccessful)
                {
                    IntegrityGameSetting(settingContext, ref newGameTypes);
                }
            }
            else
            {
                oldGameTypes = _settingRepository.GetGameSetting(settingContext.Customer.Id);
            }

            ICommandResults resultUpdateGameTypes = UpdateGameTypes(settingContext, newGameTypes, oldGameTypes);

            return resultUpdateGameTypes;
        }

        /// <summary>
        /// Updates the multiple game setting.
        /// </summary>
        /// <param name="settingContextMultiple">The setting context multiple.</param>
        /// <param name="gameTypes">The game types.</param>
        /// <param name="disableLiveCasino">If set to <c>true</c> [disable live casino].</param>
        /// <returns>
        /// Update result.
        /// </returns>
        public IMultipleCommandResults UpdateMultipleGameSetting(
            LiveCasinoSettingContextMultiple settingContextMultiple,
            List<GameType> gameTypes,
            bool disableLiveCasino)
        {
            var results = new MultipleCommandResults();

            foreach (var custInfo in settingContextMultiple.Customers)
            {
                settingContextMultiple.Customer.Id = custInfo.Id;
                settingContextMultiple.Customer.Username = custInfo.Username;
                settingContextMultiple.Customer.IsSyncLiveCasino = custInfo.IsSyncLiveCasino;

                var updateResult = (CommandResults)UpdateGameSetting(settingContextMultiple, gameTypes, disableLiveCasino);

                updateResult.Id = custInfo.Id;
                updateResult.Name = custInfo.Username;

                results.CommandResultsList.Add(updateResult);
            }

            return results;
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
        public ICommandResults UpdateWinLimit(LiveCasinoSettingContext settingContext, WinLimit memberWinLimit)
        {
            Check.Argument.IsNotNull(settingContext, "settingContext");
            Check.Argument.IsNotNull(settingContext.Customer, "settingContext.Customer");
            Check.Argument.IsNotNegativeOrZero(settingContext.Customer.Id, "settingContext.Customer.Id");
            Check.Argument.IsNotNull(settingContext.Actor, "settingContext.Actor");
            Check.Argument.IsNotNull(settingContext.SubAccount, "settingContext.SubAccount");
            Check.Argument.IsNotNull(memberWinLimit, "memberWinLimit");
            Check.Argument.IsNotEmpty(settingContext.IP, "settingContext.IP");
            Check.Argument.IsNotEmpty(settingContext.Language, "settingContext.Language");

            var commandResults = new CommandResults();
            var uplineMaximumWinLimit = (int)settingContext.Customer.Parent.LiveCasinoSetting.WinLimit.Max;

            if (uplineMaximumWinLimit != (int)MaximumWinLimit.Unlimited && memberWinLimit.Max > uplineMaximumWinLimit)
            {
                commandResults.Results.Add(new WinLimitCommandResult { Code = ErrCode.InvalidMaximumWinLimit, Message = LiveCasinoLang.UpdatedUnsuccessfully });

                return commandResults;
            }

            var criteria = new UpdateCustomerWinLimitCriteria
            {
                CustId = settingContext.Customer.Id,
                MaxWinLimit = (decimal)memberWinLimit.Max,
                IsAutoDailyReset = memberWinLimit.IsAutoReset,
                UserId = settingContext.Actor.Id,
                SubAccID = settingContext.SubAccount.Id,
                Ip = settingContext.IP,
                Language = settingContext.Language
            };

            var result = (CommandResult)_settingRepository.Save(criteria);

            var winLimitResult = new WinLimitCommandResult(result);

            if (winLimitResult.IsSuccessful)
            {
                winLimitResult.Message = LiveCasinoLang.UpdatedWinLimitSuccessfully;
            }
            else
            {
                Logger.Log.Info(string.Format("{0}\n{1}", winLimitResult.Message, criteria.ToString()), LiveCasinoSettings.Settings.LogCategory);
            }

            commandResults.Results.Add(winLimitResult);

            return commandResults;
        }

        /// <summary>
        /// Updates the win limit multiple.
        /// </summary>
        /// <param name="settingContextMultiple">The setting context multiple.</param>
        /// <param name="memberWinLimit">The member win limit.</param>
        /// <returns>
        /// Update win limit multiple result.
        /// </returns>
        public IMultipleCommandResults UpdateMultipleWinLimit(LiveCasinoSettingContextMultiple settingContextMultiple, WinLimit memberWinLimit)
        {
            var results = new MultipleCommandResults();

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
        /// <param name="disableLiveCasino">If set to <c>true</c> [disable live casino].</param>
        /// <returns>
        /// Insert successfully.
        /// </returns>
        private ICommandResult InsertAccount(LiveCasinoSettingContext settingContext, bool disableLiveCasino)
        {
            int agentId = 0, masterId = 0, superId = 0;
            settingContext.Customer.SetUplineIdentifier(ref agentId, ref masterId, ref superId);

            var criteria = new ManualInsertAccountCriteria
            {
                CustId = settingContext.Customer.Id,
                CasinoNo = CasinoNo,
                ExchangeId = settingContext.Customer.CurrencyId,
                AgentId = agentId,
                MasterId = masterId,
                SuperId = superId,
                RoleId = (int)settingContext.Customer.Level,
                SignupIp = settingContext.IP,
                SignupBy = settingContext.Actor.Id,
                SignupFrom = LiveCasinoSettings.Settings.SystemId,
                ModifyUsername = settingContext.Actor.Username,
                Username = settingContext.Customer.Username,
                CustSite = settingContext.Customer.Site
            };

            var updateResult = (CommandResult)_settingRepository.Save(criteria);
            if (!updateResult.IsSuccessful)
            {
                Logger.Log.Info(string.Format("{0}\n{1}", updateResult.Message, criteria.ToString()), LiveCasinoSettings.Settings.LogCategory);
            }

            if (updateResult.IsSuccessful)
            {
                WriteLog(new ManualInsertAccountLog { SignUpForm = LiveCasinoSettings.Settings.SystemId, CasinoNo = CasinoNo, SettingContext = settingContext });
                var syncResult = (CommandResult)UpdateSyncLiveCasino(settingContext, isSynced: true);

                if (settingContext.Customer.Level != CustomerLevel.Super)
                {
                    var uplineCustInfo = GetCustomerLiveCasinoInfo(settingContext.Customer.Parent.Id);

                    if (settingContext.Customer.Level != CustomerLevel.Member)
                    {
                        disableLiveCasino = uplineCustInfo.DisableLiveCasino;
                    }

                    if (syncResult.IsSuccessful && !uplineCustInfo.DisableLiveCasino)
                    {
                        UpdateLiveCasinoStatus(settingContext, disableLiveCasino);
                    }
                }
            }

            return updateResult;
        }

        /// <summary>
        /// Inserts the account.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="oldCustId">The old customer identifier.</param>
        /// <param name="toUsername">To username.</param>
        /// <param name="subAccountId">The sub account identifier.</param>
        /// <param name="subAccountUsername">The sub account username.</param>
        /// <param name="actorId">The actor identifier.</param>
        /// <param name="actorUsername">The actor username.</param>
        /// <param name="ip">The internet protocol.</param>
        /// <param name="language">The language.</param>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="gameTypes">The game types.</param>
        /// <returns>
        /// Insert result.
        /// </returns>
        private ICommandResult InsertAccount(
            int custId,
            int oldCustId,
            string toUsername,
            int subAccountId,
            string subAccountUsername,
            int actorId,
            string actorUsername,
            string ip,
            string language,
            LiveCasinoSettingContext settingContext,
            List<GameType> gameTypes)
        {
            int superId = 0, masterId = 0, agentId = 0;
            settingContext.Customer.SetUplineIdentifier(ref agentId, ref masterId, ref superId);

            var criteria = new AutoInsertAccountFromExistingCriteria
            {
                CasinoNo = CasinoNo,
                CustId = custId,
                ModifyUsername = actorUsername,
                SignupBy = actorId,
                SignupFrom = LiveCasinoSettings.Settings.SystemId,
                SignupIp = ip,
                OldCustId = oldCustId,
                ToUserName = toUsername,
                Level = settingContext.Customer.Level
            };

            if (settingContext.Customer.Level == CustomerLevel.Super)
            {
                var site = GetCustomerSite(settingContext.Customer.Username);

                criteria = new AutoInsertSuperFromExistingCriteria
                {
                    CasinoNo = CasinoNo,
                    CustId = custId,
                    ModifyUsername = actorUsername,
                    SignupBy = actorId,
                    SignupFrom = LiveCasinoSettings.Settings.SystemId,
                    SignupIp = ip,
                    OldCustId = oldCustId,
                    ToUserName = toUsername,
                    Level = settingContext.Customer.Level,
                    CustSite = site
                };
            }

            ICommandResult result = _settingRepository.InsertGameSettingAccount(criteria, gameTypes);

            return result;
        }

        /// <summary>
        /// Inserts the account.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="actorId">The actor identifier.</param>
        /// <param name="actorUsername">The actor username.</param>
        /// <param name="subAccountId">The sub account identifier.</param>
        /// <param name="subAccountUsername">The sub account username.</param>
        /// <param name="ip">The internet protocol.</param>
        /// <param name="language">The language.</param>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="gameTypes">The game types.</param>
        /// <returns>
        /// Insert result.
        /// </returns>
        private ICommandResult InsertAccount(
            int custId,
            int actorId,
            string actorUsername,
            int subAccountId,
            string subAccountUsername,
            string ip,
            string language,
            LiveCasinoSettingContext settingContext,
            List<GameType> gameTypes)
        {
            int superId = 0, masterId = 0, agentId = 0;
            InitSettingContext(custId, subAccountId, subAccountUsername, actorId, actorUsername, ip, language, settingContext);
            settingContext.Customer.SetUplineIdentifier(ref agentId, ref masterId, ref superId);

            var criteria = new AutoInsertAccountCriteria
            {
                AgentId = agentId,
                CasinoNo = CasinoNo,
                CustId = custId,
                ExchangeId = settingContext.Customer.CurrencyId,
                MasterId = masterId,
                ModifyUsername = actorUsername,
                RoleId = (int)settingContext.Customer.Level,
                SignupBy = actorId,
                SignupFrom = LiveCasinoSettings.Settings.SystemId,
                SignupIp = ip,
                SuperId = superId,
                Username = settingContext.Customer.Username,
                Language = language
            };

            ICommandResult result = _settingRepository.InsertGameSettingAccount(criteria, gameTypes);

            return result;
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
        private ICommandResults UpdateGameTypes(LiveCasinoSettingContext settingContext, List<GameType> newGameTypes, List<GameType> oldGameTypes)
        {
            var results = new CommandResults();

            if (newGameTypes != null && newGameTypes.Any())
            {
                foreach (var itemNewGameType in newGameTypes)
                {
                    var itemOldGameType = oldGameTypes.Where(x => x.Id == itemNewGameType.Id).FirstOrDefault();
                    var result = UpdateStakeLevel(settingContext, itemNewGameType, itemOldGameType);

                    results.Results.Add(result);
                }
            }
            else
            {
                results.Results.Add(new GameSettingCommandResult { Message = LiveCasinoLang.lblBettingLimit + ": " + LiveCasinoLang.UpdateSuccessfully });
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

            var updateResult = (CommandResult)_settingRepository.Save(criteria);

            var gameSettingResult = new GameSettingCommandResult(updateResult);

            if (gameSettingResult.IsSuccessful)
            {
                gameSettingResult.Id = newGameType.Id;
                gameSettingResult.Message = string.Format("{0} - {1}: {2}", LiveCasinoLang.lblGameSetting, newGameType.Name, LiveCasinoLang.UpdateSuccessfully);

                if (oldGameType == null || newGameType.CheckedStakeLevelId != oldGameType.CheckedStakeLevelId)
                {
                    WriteLog(new UpdateStakeLevelLog { SettingContext = settingContext, NewGameType = newGameType, OldGameType = oldGameType });
                }
            }
            else
            {
                gameSettingResult.Message = string.Format("{0} - {1}: {2}", LiveCasinoLang.lblGameSetting, newGameType.Name, gameSettingResult.FriendlyErrorMessage);
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
            if (gameType != null && gameType.Any())
            {
                foreach (var item in gameType)
                {
                    list.Add(new GameType
                    {
                        Id = item.Id,
                        BetTypeId = item.BetTypeId,
                        BettingLimit = item.BettingLimit
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
        /// <param name="isApplyPositionTakingAllMembers">If set to <c>true</c> [is apply position taking all members].</param>
        /// <param name="positionTakingOnMembers">The position taking on members.</param>
        /// <returns>
        /// Command result after updating.
        /// </returns>
        private ICommandResults UpdateEachCustomerInMultipleEdit(
            LiveCasinoSettingContext settingContext,
            Customer customer,
            UpdateType updateType,
            bool isApplyPositionTakingAllMembers,
            float positionTakingOnMembers)
        {
            var isSync = settingContext.Customer.IsSyncLiveCasino;
            switch (updateType)
            {
                case UpdateType.PositionTakingAndCommission:
                    return isSync ?
                            UpdatePTComSetting(
                                settingContext,
                                customer.LiveCasinoSetting.Commission,
                                customer.LiveCasinoSetting.PositionTaking,
                                isApplyPositionTakingAllMembers,
                                positionTakingOnMembers)
                            : UpdateCustomerSetting(settingContext, customer, isApplyPositionTakingAllMembers, positionTakingOnMembers);
                case UpdateType.WinLimit:
                    return isSync ?
                            UpdateWinLimit(settingContext, customer.LiveCasinoSetting.WinLimit)
                            : UpdateCustomerSetting(settingContext, customer, isApplyPositionTakingAllMembers, positionTakingOnMembers);
                case UpdateType.GameSetting:
                    return isSync ?
                            UpdateGameSetting(settingContext, customer.LiveCasinoSetting.GameSetting, customer.DisableLiveCasino)
                            : UpdateCustomerSetting(settingContext, customer, isApplyPositionTakingAllMembers, positionTakingOnMembers);
                case UpdateType.AllSetting:
                    return UpdateCustomerSetting(settingContext, customer, isApplyPositionTakingAllMembers, positionTakingOnMembers);
                default:
                    return new CommandResults();
            }
        }

        /// <summary>
        /// Writes the log.
        /// </summary>
        /// <param name="logData">The log data.</param>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("T4", "1.0.0.0")]
        private void WriteLog(IGameSettingLog logData)
        {
            var criteria = new InsertLogUserRunSPCriteria
            {
                UserId = logData.SettingContext.Actor.Id,
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
                _settingRepository.Save(criteria);
            }
            catch (Exception ex)
            {
                Logger.Log.Error(string.Format("{0}\n{1}", ex.Message, criteria.ToString()), LiveCasinoSettings.Settings.LogCategory);

                throw ex;
            }
        }

        private CustomerInfoDTO GetCustomerInfo(int custId)
        {
            var criteria = new GetCustomerInfoCriteria
            {
                CustId = custId
            };

            return _settingRepository.GetCustomerInfo(criteria);
        }

        /// <summary>
        /// Sets the game setting.
        /// </summary>
        /// <param name="customer">The customer.</param>
        private void SetGameSetting(Customer customer)
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

            customer.LiveCasinoSetting.GameSetting = gameSettings;
        }

        /// <summary>
        /// Gets the customer site by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>Customer site name.</returns>
        private string GetCustomerSite(string username)
        {
            var criteria = new GetCustomerSiteByUsernameCriteria
            {
                Username = username
            };

            if (!criteria.IsValid())
            {
                var message = string.Format(LiveCasinoLang.InvalidCriteriaMsg, criteria.GetSettingKey());
                Logger.Log.Error(string.Format("{0}\n{1}", message, criteria.ToString()));

                throw new Exception(message);
            }

            var userInfo = _settingRepository.Get<Customer>(criteria);

            return userInfo.Site;
        }

        /// <summary>
        /// Integrities the game setting.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="newGameTypes">The new game types.</param>
        private void IntegrityGameSetting(LiveCasinoSettingContext settingContext, ref List<GameType> newGameTypes)
        {
            string site = GetCustomerSite(settingContext.Customer.Username);
            List<GameType> defaultGameSetting = _settingRepository.GetGameSetting(settingContext.Customer.CurrencyId, site);

            newGameTypes = newGameTypes ?? new List<GameType>();
            if (newGameTypes.Count < defaultGameSetting.Count)
            {
                var notFoundedGameTypes = new List<GameType>();

                foreach (var itemGameType in defaultGameSetting)
                {
                    var foundedGameType = newGameTypes.Where(x => x.Id == itemGameType.Id).FirstOrDefault();

                    if (foundedGameType == null)
                    {
                        notFoundedGameTypes.Add(itemGameType);
                    }
                }

                newGameTypes.AddRange(notFoundedGameTypes);
                newGameTypes = newGameTypes.OrderBy(x => x.Id).ToList();
            }
        }

        /// <summary>
        /// Initializes the setting context.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="subAccountId">The sub account identifier.</param>
        /// <param name="subAccountUsername">The sub account username.</param>
        /// <param name="actorId">The actor identifier.</param>
        /// <param name="actorUsername">The actor username.</param>
        /// <param name="ip">The internet protocol.</param>
        /// <param name="language">The language.</param>
        /// <param name="settingContext">The setting context.</param>
        private void InitSettingContext(
            int custId,
            int subAccountId,
            string subAccountUsername,
            int actorId,
            string actorUsername,
            string ip,
            string language,
            LiveCasinoSettingContext settingContext)
        {
            CustomerInfoDTO customerInfo = GetCustomerInfo(custId);

            settingContext.Customer = new Customer
            {
                Id = custId,
                Username = customerInfo.Username,
                Level = CustomerLevel.Member,
                CurrencyId = (byte)customerInfo.CurrencyId,
                IsSyncLiveCasino = customerInfo.SyncLiveCasino,
                Parent = new Customer { Id = customerInfo.AgentId, Parent = new Customer { Id = customerInfo.MasterId, Parent = new Customer { Id = customerInfo.SuperId } } }
            };

            settingContext.Upline = new Customer { Id = customerInfo.AgentId, Level = CustomerLevel.Agent };

            switch (customerInfo.RoleId)
            {
                case (int)CustomerLevel.Master:
                    settingContext.Customer.Level = CustomerLevel.Master;
                    settingContext.Customer.Parent = new Customer { Id = customerInfo.SuperId };
                    settingContext.Upline = new Customer { Id = customerInfo.SuperId, Level = CustomerLevel.Super };
                    break;

                case (int)CustomerLevel.Agent:
                    settingContext.Customer.Level = CustomerLevel.Agent;
                    settingContext.Customer.Parent = new Customer { Id = customerInfo.MasterId, Parent = new Customer { Id = customerInfo.SuperId } };
                    settingContext.Upline = new Customer { Id = customerInfo.MasterId, Level = CustomerLevel.Master };
                    break;

                case (int)CustomerLevel.Super:
                    settingContext.Customer.Level = CustomerLevel.Super;
                    settingContext.Customer.Parent = new Customer();
                    settingContext.Upline = new Customer();
                    break;
            }

            settingContext.Actor = new User { Id = actorId, Username = actorUsername };
            settingContext.SubAccount = new User { Id = subAccountId, Username = subAccountUsername };
            settingContext.IP = ip;
            settingContext.Language = language;
        }
    }
}