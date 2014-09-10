namespace Fanex.LiveCasino.CoreSetting.DTO.CommandResult
{
    using System.Collections.Generic;
    using Fanex.NAP.Framework.Data;

    public interface ICommandResultsDTO
    {
        List<ICommandResult> Results { get; set; }

        string Name { get; set; }

        int Id { get; set; }

        bool IsSuccessful { get; }
    }
}