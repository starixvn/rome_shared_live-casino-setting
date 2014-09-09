namespace Fanex.LiveCasino.CoreSetting.DTO.Command
{
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