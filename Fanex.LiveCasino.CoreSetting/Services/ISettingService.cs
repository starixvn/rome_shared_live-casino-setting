namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.DTO.Command;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.ViewModels;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
    public interface ISettingService
    {
        int GetRepresentativeCustIdForEditingMultiple(SettingContext settingContext);

        ICommandResults UpdateSingleCustomerAllSetting(SettingContext settingContext, Customer customer);

        IMultipleCommandResults UpdateMultipleCustomerAllSetting(SettingContext settingContext, Customer customer, int updateType);

        Customer GetCustomer(SettingContext settingContext);
    }
}