namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.DTO.Command;
    using Fanex.LiveCasino.CoreSetting.Models;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
    public interface ISettingService
    {
        int GetRepresentativeCustIdForEditingMultiple(SettingContext settingContext);

        #region PT and Com

        PtAndComSetting GetPtAndComSetting(SettingContext settingContext);

        ICommandResults UpdatePtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        IMultipleCommandResults UpdatePtAndComSettingMultiple(SettingContext settingContext, PtAndComSetting ptAndComSetting);

        #endregion PT and Com

        #region Win Limit

        CurrentMemberWin GetCurrentWinLimit(int custId);

        ICommandResults UpdateWinLimit(SettingContext settingContext, MemberWinLimit memberWinLimit);

        IMultipleCommandResults UpdateWinLimitMultiple(SettingContext settingContext, MemberWinLimit memberWinLimit);

        #endregion Win Limit
    }
}