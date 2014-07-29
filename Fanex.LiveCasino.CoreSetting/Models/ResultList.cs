namespace Fanex.LiveCasino.CoreSetting.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ResultList
    {
        public ResultList()
        {
            CommandResults = new List<ICommandResult>();
        }

        public List<ICommandResult> CommandResults { get; set; }

        public bool IsSuccessful
        {
            get
            {
                foreach (var result in CommandResults)
                {
                    if (result.Code > 0)
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
