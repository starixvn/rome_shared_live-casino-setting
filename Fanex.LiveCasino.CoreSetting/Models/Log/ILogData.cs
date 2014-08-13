namespace Fanex.LiveCasino.CoreSetting.Models.Log
{
    using Fanex.LiveCasino.CoreSetting.Enumerations;

    public interface ILogData
    {
        LogActions Action { get; set; }

        CustInfo CustInfo { get; set; }

        CustInfo SubAccount { get; set; }

        CustInfo Modifier { get; set; }

        string ErrCode { get; set; }

        string ErrMsg { get; set; }

        string Language { get; set; }

        string NewValue { get; }

        string OldValue { get; }

        string SignUpIp { get; set; }

        string SpName { get; }
    }
}