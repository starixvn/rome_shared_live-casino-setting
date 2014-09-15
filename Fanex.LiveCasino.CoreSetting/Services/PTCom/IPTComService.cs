// -----------------------------------------------------------------------
// <copyright file="IPTComService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.LiveCasino.CoreSetting.Services.PTCom
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.DTO.CommandResult;
    using Fanex.LiveCasino.CoreSetting.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IPTComService
    {
        void SetModifiedData(float commission, PositionTaking positionTaking, bool isApplyPositionTakingOnMembers = false, float newPositionTakingOnMembers = 0);

        void SetPTComSetting(Customer customer, LiveCasinoSettingContext settingContext);

        ICommandResultsDTO Update(LiveCasinoSettingContext settingContext);

        IMultipleCommandResultsDTO UpdateMultiple(List<Customer> customers, LiveCasinoSettingContext settingContext);
    }
}