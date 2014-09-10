namespace Fanex.LiveCasino.CoreSetting.Services
{
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.DTO.CommandResult;
    using Fanex.LiveCasino.CoreSetting.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IWinLimitSettingService
    {
        CurrentMemberWinDTO GetCurrentWinLimit(int custId);

        ICommandResultsDTO UpdateWinLimit(SettingContext settingContext, MemberWinLimit memberWinLimit);

        IMultipleCommandResultsDTO UpdateWinLimitMultiple(SettingContext settingContext, MemberWinLimit memberWinLimit);
    }
}