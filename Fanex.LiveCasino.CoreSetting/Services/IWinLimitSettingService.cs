namespace Fanex.LiveCasino.CoreSetting.Services
{
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.DTO.Command;
    using Fanex.LiveCasino.CoreSetting.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IWinLimitSettingService
    {
        CurrentMemberWin GetCurrentWinLimit(int custId);

        ICommandResults UpdateWinLimit(SettingContext settingContext, MemberWinLimit memberWinLimit);

        IMultipleCommandResults UpdateWinLimitMultiple(SettingContext settingContext, MemberWinLimit memberWinLimit);
    }
}