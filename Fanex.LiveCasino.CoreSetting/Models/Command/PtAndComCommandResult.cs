// --------------------------------------------------------------------------
// <copyright file="PtAndComCommandResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.8.4">Created</change>
// </history>
namespace Fanex.LiveCasino.CoreSetting.Models.Command
{
    using Fanex.Common;
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PtAndComCommandResult : CommandResult
    {
        public PtAndComCommandResult()
        {
        }

        public PtAndComCommandResult(CommandResult result)
        {
            Code = result.Code;
            Message = result.Message;
        }

        public override bool IsSuccessful
        {
            get
            {
                return Converter.ToInt(Code) <= 0;
            }
        }
    }
}