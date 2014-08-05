namespace Fanex.LiveCasino.CoreSetting.Models.Command
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