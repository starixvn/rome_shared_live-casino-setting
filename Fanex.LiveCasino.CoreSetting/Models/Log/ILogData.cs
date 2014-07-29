namespace Fanex.LiveCasino.CoreSetting.Models.Log
{
    using System.Collections.Generic;
    using Fanex.LiveCasino.CoreSetting.Enumerations;

    public interface ILogData
    {
        int UserId { get; set; }

        int RoleId { get; set; }

        string ErrCode { get; set; }

        string ErrMsg { get; set; }

        string SignUpIp { get; set; }

        string Language { get; set; }

        int CustId { get; set; }

        string SpName { get; }

        string SubAccName { get; set; }

        int SubAccId { get; set; }

        LogActions Action { get; set; }

        string NewValues { get; }

        string OldValues { get; }

        bool IsValid { get; }
    }
}
