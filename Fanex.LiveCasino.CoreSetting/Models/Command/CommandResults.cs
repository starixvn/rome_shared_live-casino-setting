namespace Fanex.LiveCasino.CoreSetting.Models.Command
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.NAP.Framework.Command;
    using System;

    public class CommandResults : ICommandResults
    {
        public CommandResults()
        {
            Results = new List<ICommandResult>();
        }

        public List<ICommandResult> Results { get; set; }

        public string Username { get; set; }

        public int CustId { get; set; }

        public virtual bool IsSuccessful
        {
            get
            {
                return Results.Where(x => !x.IsSuccessful).FirstOrDefault() == null;
            }
        }
    }
}
