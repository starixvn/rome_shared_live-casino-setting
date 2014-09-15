namespace Fanex.LiveCasino.CoreSetting.Models.Log
{
    using Fanex.LiveCasino.CoreSetting.Enumerations;

    internal interface IGameSettingLog
    {
        LiveCasinoSettingContext SettingContext { get; set; }

        LogActions Action { get; }

        string NewValue { get; }

        string OldValue { get; }

        string SpName { get; }

        string ErrCode { get; set; }

        string ErrMsg { get; set; }
    }
}