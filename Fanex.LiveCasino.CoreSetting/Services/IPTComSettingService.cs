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
        void SetPtAndComSetting(SettingContext settingContext, Customer customer);

        ICommandResults UpdatePtComSetting(SettingContext settingContext, Commission commission, PositionTaking positionTaking);

        IMultipleCommandResults UpdatePtComSettingMultiple(SettingContext settingContext, Commission commission, PositionTaking positionTaking);
    }
}