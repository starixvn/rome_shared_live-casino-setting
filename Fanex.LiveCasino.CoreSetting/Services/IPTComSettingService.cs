namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.DTO.Command;
    using Fanex.LiveCasino.CoreSetting.Models;

    /// <summary>
    /// Position Taking And Commission Setting Service Interface.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
    public interface IPTComSettingService
    {
        PTComSetting GetPtComSetting(SettingContext settingContext);

        ICommandResults UpdatePtComSetting(SettingContext settingContext, PTComSetting pTComSetting);

        IMultipleCommandResults UpdatePtComSettingMultiple(SettingContext settingContext, PTComSetting pTComSetting);
    }
}