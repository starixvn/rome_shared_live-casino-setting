namespace Fanex.LiveCasino.CoreSetting.DTO.CommandResult
{
    using System.Collections.Generic;

    /// <summary>
    /// Multiple Command Results.
    /// </summary>
    public interface IMultipleCommandResultsDTO
    {
        List<ICommandResultsDTO> CommandResultsList { get; set; }

        bool IsSuccessful { get; }
    }
}