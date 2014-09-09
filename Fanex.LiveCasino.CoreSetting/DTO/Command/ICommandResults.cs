namespace Fanex.LiveCasino.CoreSetting.DTO.Command
{
    using System.Collections.Generic;
    using Fanex.NAP.Framework.Data;

    public interface ICommandResults
    {
        List<ICommandResult> Results { get; set; }

        string Name { get; set; }

        int Id { get; set; }

        bool IsSuccessful { get; }
    }
}