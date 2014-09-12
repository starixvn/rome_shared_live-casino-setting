namespace Fanex.LiveCasino.CoreSetting.Models.Log
{
    using Fanex.LiveCasino.CoreSetting.Enumerations;

    public interface IGameSettingLog
    {
        LogActions Action { get; }

        string ErrCode { get; set; }

        string ErrMsg { get; set; }

        string NewValue { get; }

        string OldValue { get; }

        SettingContext SettingContext { get; set; }

        string SpName { get; }
    }
}