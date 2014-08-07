namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.Criteria.GameSetting;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Models.Log;
    using Fanex.NAP.Framework.Command;

    public interface ILogService
    {
        void LogAutoInsertAccount(SettingContext settingContext, List<GameType> gameTypes);

        void LogInsertFromExistingAccount(SettingContext settingContext, List<GameType> gameTypes);

        void LogManualInsertAccount(SettingContext settingContext);

        void LogManualUpdateStakeLevel(SettingContext settingContext, GameType gameTypeOld, GameType gameTypeNew, bool isInsert = false);

        ICommandResult WriteLog(ILogData logData);
    }
}
