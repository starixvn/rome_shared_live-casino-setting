// -----------------------------------------------------------------------
// <copyright file="Result.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.LiveCasino.CoreSetting.Core.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Result
    {
        public Result() : this("CS0000", string.Empty) { }

        public Result(string errCode, string errMsg)
        {
            ErrCode = errCode;
            ErrMsg = ErrMsg;
        }

        public string ErrCode { get; set; }

        public string ErrMsg { get; set; }
    }
}
