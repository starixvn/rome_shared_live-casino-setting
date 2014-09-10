namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.DTO.CommandResult;
    using Fanex.LiveCasino.CoreSetting.Models;

    /// <summary>
    /// Position Taking And Commission Setting Service Interface.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
    public interface IPTComSettingService
    {
        void SetCustomerPtAndComSetting(SettingContext settingContext, Customer customer);

        ICommandResultsDTO UpdatePtComSetting(SettingContext settingContext, Commission commission, PositionTaking positionTaking);

        IMultipleCommandResultsDTO UpdatePtComSettingMultiple(SettingContext settingContext, Commission commission, PositionTaking positionTaking);
    }
}