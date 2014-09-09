namespace Fanex.LiveCasino.CoreSetting.DTO.Command
{
    using System;

    /// <summary>
    /// Win limit command result class.
    /// </summary>
    public class WinLimitCommandResult : CommandResult
    {
        public WinLimitCommandResult()
        {
        }

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