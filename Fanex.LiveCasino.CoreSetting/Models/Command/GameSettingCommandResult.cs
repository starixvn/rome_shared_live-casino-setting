namespace Fanex.LiveCasino.CoreSetting.Models.Command
{
    using System;
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// Game setting command result class.
    /// </summary>
    public class GameSettingCommandResult : WinLimitCommandResult
    {
        public GameSettingCommandResult()
        {
        }

        public GameSettingCommandResult(CommandResult result)
            : base(result)
        {
        }

        public int Id { get; set; }
    }
}