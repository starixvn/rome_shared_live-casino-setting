namespace Fanex.LiveCasino.CoreSetting.Models.Command
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Multiple Command Results.
    /// </summary>
    public class MultipleCommandResults : IMultipleCommandResults
    {
        public MultipleCommandResults()
        {
            CommandResultsList = new List<ICommandResults>();
        }

        public List<ICommandResults> CommandResultsList { get; set; }

        public bool IsSuccessful
        {
            get
            {
                return CommandResultsList.Where(x => !x.IsSuccessful).FirstOrDefault() == null;
            }
        }
    }
}