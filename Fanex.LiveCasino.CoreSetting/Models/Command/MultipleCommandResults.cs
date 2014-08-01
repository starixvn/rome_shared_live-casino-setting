namespace Fanex.LiveCasino.CoreSetting.Models.Command
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MultipleCommandResults : IMultipleCommandResults
    {
        public MultipleCommandResults()
        {
            CommandResultsList = new Dictionary<int, ICommandResults>();
        }

        public Dictionary<int, ICommandResults> CommandResultsList { get; set; }

        public bool IsSuccessful
        {
            get
            {
                return CommandResultsList.Select(x => !x.Value.IsSuccessful) == null;
            }
        }
    }
}
