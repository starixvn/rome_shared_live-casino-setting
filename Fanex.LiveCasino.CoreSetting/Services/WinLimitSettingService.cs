namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.Criteria.WinLimit;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.DTO.Command;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Repositories;
    using Fanex.LiveCasino.CoreSetting.Resources;
    using Fanex.LiveCasino.CoreSetting.Utilities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WinLimitSettingService : IWinLimitSettingService
    {
        private readonly ISettingRepository _settingRepository;
        private readonly ICommonUtil _commonUtil;

        public WinLimitSettingService()
            : this(null, null)
        {
        }

        public WinLimitSettingService(ISettingRepository settingRepository, ICommonUtil commonUtil)
        {
            _settingRepository = settingRepository ?? new SettingRepository();
            _commonUtil = commonUtil ?? new CommonUtil();
        }

        /// <summary>
        /// Gets the current win limit.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <returns>Current member win limit.</returns>
        public CurrentMemberWin GetCurrentWinLimit(int custId)
        {
            var criteria = new GetCustomerWinLimitCriteria
            {
                CustId = custId
            };

            var currentWinLimit = (CurrentMemberWin)_settingRepository.GetCurrentMemberWin(criteria);

            return currentWinLimit;
        }

        /// <summary>
        /// Update member win limit.
        /// </summary>
        /// <param name="settingContext">Setting context information.</param>
        /// <param name="memberWinLimit">Member win limit information.</param>
        /// <returns>Result after updating.</returns>
        public ICommandResults UpdateWinLimit(SettingContext settingContext, MemberWinLimit memberWinLimit)
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

            var result = (CommandResult)_settingRepository.InsertOrUpdate(criteria);

            var winLimitResult = new WinLimitCommandResult(result);

            if (winLimitResult.IsSuccessful)
            {
                winLimitResult.Message = LiveCasinoLang.UpdatedWinLimitSuccessfully;
            }
            else
            {
                _commonUtil.LogInfo(string.Format("{0}\n{1}", winLimitResult.Message, criteria.ToString()));
            }

            var commandResults = new CommandResults();
            commandResults.Results.Add(winLimitResult);

            return commandResults;
        }

        /// <summary>
        /// Updates the win limit multiple.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="memberWinLimit">The member win limit.</param>
        /// <returns>Update win limit multiple result.</returns>
        public IMultipleCommandResults UpdateWinLimitMultiple(SettingContext settingContext, MemberWinLimit memberWinLimit)
        {
            var results = new MultipleCommandResults();
            List<Customer> custInfos = _commonUtil.GetCustomerInfo(settingContext.TargetCustIds, settingContext.TargetUsernames, settingContext.TargetSyncStatus);

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
    }
}