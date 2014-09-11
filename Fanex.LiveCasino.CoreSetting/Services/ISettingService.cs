namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.DTO.CommandResult;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.ViewModels;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
    public interface ISettingService
    {
        int GetRepresentativeCustIdForEditingMultiple(SettingContext settingContext);

        ICommandResultsDTO UpdateSingleCustomerAllSetting(SettingContext settingContext, 
            Customer customer, 
            UplinePositionTakingOnMember uplinePositionTakingOnMember);

        IMultipleCommandResultsDTO UpdateMultipleCustomerAllSetting(
            SettingContext settingContext, 
            Customer customer, 
            UplinePositionTakingOnMember uplinePositionTakingOnMember,
            int updateType);

        Customer GetCustomer(SettingContext settingContext);
    }
}