namespace Fanex.LiveCasino.CoreSetting.Services.PTCom
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.DTO.CommandResult;
    using Fanex.LiveCasino.CoreSetting.Models;

    /// <summary>
    /// PT and Com setting interface.
    /// </summary>
    public interface IPTComService
    {
        void SetModifiedData(float commission, PositionTaking positionTaking, bool isApplyPositionTakingOnMembers = false, float newPositionTakingOnMembers = 0);

        void SetPTComSetting(Customer customer, LiveCasinoSettingContext settingContext);

        ICommandResults Update(LiveCasinoSettingContext settingContext);

        IMultipleCommandResults UpdateMultiple(List<Customer> customers, LiveCasinoSettingContext settingContext);
    }
}