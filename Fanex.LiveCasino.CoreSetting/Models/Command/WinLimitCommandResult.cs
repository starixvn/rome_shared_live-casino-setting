namespace Fanex.LiveCasino.CoreSetting.Models.Command
{
    using Fanex.NAP.Framework.Command;

    public class WinLimitCommandResult : CommandResultBase
    {
        public WinLimitCommandResult(CommandResultBase result)
        {
            Code = result.Code;
            Message = result.Message;
        }

        public override bool IsSuccessful
        {
            get
            {
                return string.IsNullOrWhiteSpace((string)Code) || (string)Code == "0";
            }
        }
    }
}
