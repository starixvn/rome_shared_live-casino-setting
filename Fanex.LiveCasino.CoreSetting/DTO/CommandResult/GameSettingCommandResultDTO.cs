namespace Fanex.LiveCasino.CoreSetting.DTO.CommandResult
{
    /// <summary>
    /// Game setting command result class.
    /// </summary>
    public class GameSettingCommandResultDTO : WinLimitCommandResultDTO
    {
        public GameSettingCommandResultDTO()
        {
        }

        public GameSettingCommandResultDTO(CommandResultDTO result)
            : base(result)
        {
        }

        public int Id { get; set; }
    }
}