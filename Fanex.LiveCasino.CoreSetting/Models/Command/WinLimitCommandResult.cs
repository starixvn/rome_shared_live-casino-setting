namespace Fanex.LiveCasino.CoreSetting.Models.Command
{
    using System;
    using Fanex.NAP.Framework.Command;

    public class WinLimitCommandResult : CommandResult
    {
        public WinLimitCommandResult(CommandResult result)
        {
            Code = result.Code;
            Message = result.Message;
        }

        public override bool IsSuccessful
        {
            get
            {
                var code = Convert.ToString(Code);

                return string.IsNullOrWhiteSpace(code) || code == "0";
            }
        }
    }
}
