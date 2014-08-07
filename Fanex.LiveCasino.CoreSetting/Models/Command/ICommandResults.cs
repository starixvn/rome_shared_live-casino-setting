namespace Fanex.LiveCasino.CoreSetting.Models.Command
{
    using System.Collections.Generic;
    using Fanex.NAP.Framework.Command;

    public interface ICommandResults
    {
        List<ICommandResult> Results { get; set; }

        string Name { get; set; }

        int Id { get; set; }

        bool IsSuccessful { get; }
    }
}