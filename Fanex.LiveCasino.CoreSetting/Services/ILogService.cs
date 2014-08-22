namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Models.Log;
    using Fanex.NAP.Framework.Data;

    public interface ILogService
    {
        void LogAutoInsertAccount(SettingContext settingContext, List<GameType> gameTypes);

        void LogInsertFromExistingAccount(SettingContext settingContext, List<GameType> gameTypes);

        void LogManualInsertAccount(SettingContext settingContext);

        void LogManualUpdateStakeLevel(SettingContext settingContext, GameType gameTypeOld, GameType gameTypeNew);

        ICommandResult WriteLog(ILogData logData);
    }
}