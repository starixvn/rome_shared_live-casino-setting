namespace Fanex.LiveCasino.CoreSetting.DTO.Command
{
    using System.Collections.Generic;

    /// <summary>
    /// Multiple Command Results.
    /// </summary>
    public interface IMultipleCommandResults
    {
        List<ICommandResults> CommandResultsList { get; set; }

        bool IsSuccessful { get; }
    }
}