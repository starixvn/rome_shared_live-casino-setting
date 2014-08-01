namespace Fanex.LiveCasino.CoreSetting.Models.Command
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMultipleCommandResults
    {
        List<ICommandResults> CommandResultsList { get; set; }

        bool IsSuccessful { get; }
    }
}