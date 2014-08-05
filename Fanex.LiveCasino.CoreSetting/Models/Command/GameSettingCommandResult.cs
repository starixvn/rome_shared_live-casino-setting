namespace Fanex.LiveCasino.CoreSetting.Models.Command
{
    using System;
    using Fanex.NAP.Framework.Command;

    public class GameSettingCommandResult : CommandResult
    {
        public GameSettingCommandResult()
        {
        }

        public GameSettingCommandResult(CommandResult result)
        {
            Code = result.Code;
            Message = result.Message;
        }

        public override bool IsSuccessful
        {
            get
            {
                return string.IsNullOrWhiteSpace(Convert.ToString(Code));
            }
        }
    }
}
