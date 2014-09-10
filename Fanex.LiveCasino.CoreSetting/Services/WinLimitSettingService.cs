namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common;
    using Fanex.LiveCasino.CoreSetting.Configuration;
    using Fanex.LiveCasino.CoreSetting.Criteria.WinLimit;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.DTO.CommandResult;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Repositories;
    using Fanex.LiveCasino.CoreSetting.Resources;
    using Fanex.Logging;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WinLimitSettingService : IWinLimitSettingService
    {
        private readonly ISettingRepository _settingRepository;

        public WinLimitSettingService()
            : this(null)
        {
        }

        public WinLimitSettingService(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository ?? new SettingRepository();
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
        public ICommandResultsDTO UpdateWinLimit(SettingContext settingContext, MemberWinLimit memberWinLimit)
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
        public IMultipleCommandResultsDTO UpdateWinLimitMultiple(SettingContext settingContext, MemberWinLimit memberWinLimit)
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