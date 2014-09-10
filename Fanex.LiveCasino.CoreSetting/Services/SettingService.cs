namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Common;
    using Fanex.LiveCasino.CoreSetting.Constant;
    using Fanex.LiveCasino.CoreSetting.DTO.Command;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Repositories;
    using Fanex.LiveCasino.CoreSetting.Resources;

    /// <summary>
    /// Setting service for update position taking setting, commission, member win limit.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
    public class SettingService : ISettingService
    {
        private readonly ISettingRepository _settingRepository;
        private readonly IGameSettingService _gameSettingService;
        private readonly IPTComSettingService _pTComSettingService;
        private readonly IWinLimitSettingService _winLimitSettingService;


        public SettingService()
            : this(null, null, null, null)
        {
        }

        public SettingService(
            ISettingRepository settingRepository,
            IPTComSettingService pTComSettingService,
            IWinLimitSettingService winLimitSettingService,
            IGameSettingService gameSettingService)
        {
            _settingRepository = settingRepository ?? new SettingRepository();
            _pTComSettingService = pTComSettingService ?? new PTComSettingService();
            _gameSettingService = gameSettingService ?? new GameSettingService();
            _winLimitSettingService = winLimitSettingService ?? new WinLimitSettingService();
        }

        public Customer GetCustomer(SettingContext settingContext)
        {
            Customer customer = _gameSettingService.GetCustomerInfo(settingContext.TargetCustId);
            customer.PtAndComSetting = _pTComSettingService.GetPtComSetting(settingContext);
            customer.GameSetting = _gameSettingService.GetGameSetting(settingContext, customer);

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
        public ICommandResults UpdateSingleCustomerAllSetting(SettingContext settingContext, Customer customer)
        {
            ICommandResults resultList = new CommandResults();

            if (settingContext.IsAgentEditingMember
                && customer.IsSyncLiveCasino
                && !customer.UplineDisableLiveCasino)
            {
                resultList.Results.Add(_gameSettingService.UpdateLiveCasinoStatus(settingContext, customer.DisableLiveCasino));
            }

            var positionTakingAndCommResults = _pTComSettingService.UpdatePtComSetting(settingContext, customer.PtAndComSetting);
            resultList.Results.AddRange(positionTakingAndCommResults.Results);

            if (customer.IsSyncLiveCasino
                || (!customer.IsSyncLiveCasino && positionTakingAndCommResults.IsSuccessful))
            {
                var winLimitResults = _winLimitSettingService.UpdateWinLimit(settingContext, customer.MemberWinLimit).Results;
                resultList.Results.AddRange(winLimitResults);

                var gameSettingResult = _gameSettingService.UpdateManual(settingContext, customer, customer.GameSetting).Results;
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
        /// <param name="settingContext">Setting context for updating.</param>
        /// <param name="customer">Customer setting contains customer setting information.</param>
        /// <param name="updateType">Update type for multiple.</param>
        /// <returns>Multiple result return to user.</returns>
        public IMultipleCommandResults UpdateMultipleCustomerAllSetting(SettingContext settingContext, Customer customer, int updateType)
        {
            var custInfoes = GetCustomerInfo(settingContext.TargetCustIds, settingContext.TargetUsernames, settingContext.TargetSyncStatus);

            IMultipleCommandResults mainResults = new MultipleCommandResults();

            foreach (var custInfo in custInfoes)
            {
                settingContext.TargetCustId = custInfo.CustId;
                settingContext.TargetUsername = custInfo.UserName;
                customer.IsSyncLiveCasino = custInfo.IsSyncLiveCasino;
                var subResult = UpdateEachCustomerInMultipleEdit(settingContext, customer, updateType);
                subResult.Id = custInfo.CustId;
                subResult.Name = custInfo.UserName;
                mainResults.CommandResultsList.Add(subResult);
            }

            return mainResults;
        }

        /// <summary>
        /// Depend on status of current customer, we decide to update just part of setting or all setting
        /// This handle for updating multiple customer.
        /// </summary>
        /// <param name="settingContext">Setting context for updating.</param>
        /// <param name="customer">Customer setting.</param>
        /// <param name="updateType">Update form type.</param>
        /// <returns>Command result after updating.</returns>
        private ICommandResults UpdateEachCustomerInMultipleEdit(SettingContext settingContext, Customer customer, int updateType)
        {
            var isSync = customer.IsSyncLiveCasino;
            switch (updateType)
            {
                case (int)UpdateType.PositionTakingAndCommission:
                    return isSync ?
                            _pTComSettingService.UpdatePtComSetting(settingContext, customer.PtAndComSetting)
                            : UpdateSingleCustomerAllSetting(settingContext, customer);
                case (int)UpdateType.WinLimit:
                    return isSync ?
                            _winLimitSettingService.UpdateWinLimit(settingContext, customer.MemberWinLimit)
                            : UpdateSingleCustomerAllSetting(settingContext, customer);
                case (int)UpdateType.GameSetting:
                    return isSync ?
                            _gameSettingService.UpdateManual(settingContext, customer, customer.GameSetting)
                            : UpdateSingleCustomerAllSetting(settingContext, customer);
                case (int)UpdateType.AllSetting:
                    return UpdateSingleCustomerAllSetting(settingContext, customer);
                default:
                    return new CommandResults();
            }
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
    }
}