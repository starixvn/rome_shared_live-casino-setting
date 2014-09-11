namespace Fanex.LiveCasino.CoreSetting.Models.Log
{
    using Fanex.LiveCasino.CoreSetting.Enumerations;

    public interface ILogData
    {
        LogActions Action { get; }

        SettingContext SettingContext { get; set; }

        string ErrCode { get; set; }

        string ErrMsg { get; set; }

        string NewValue { get; }

        string OldValue { get; }

        string SpName { get; }
    }
}