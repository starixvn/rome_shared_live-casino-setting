namespace Fanex.LiveCasino.CoreSetting.Models.Command
{
    using System.Collections.Generic;
    using Fanex.NAP.Framework.Command;

    public interface ICommandResults
    {
        List<ICommandResult> Results { get; set; }

        string Username { get; set; }

        int CustId { get; set; }

        bool IsSuccessful { get; }
    }
}