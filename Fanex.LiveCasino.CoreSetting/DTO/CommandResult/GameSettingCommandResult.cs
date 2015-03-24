namespace Fanex.LiveCasino.CoreSetting.DTO.CommandResult
{
    using System;
    using LiveCasino.CoreSetting.Resources;

    /// <summary>
    /// Game setting command result class.
    /// </summary>
    public class GameSettingCommandResult : CommandResult
    {
        public const string E0001 = "e0001";
        public const string E0003 = "e0003";
        public const string E0004 = "e0004";
        public const string E0005 = "e0005";
        public const string E0006 = "e0006";
        public const string E0099 = "e0099";

        public GameSettingCommandResult()
        {
        }

        public GameSettingCommandResult(CommandResult result)
        {
            Code = result.Code;
            Message = result.Message;
        }

        public int Id { get; set; }

        public string FriendlyErrorMessage
        {
            get
            {
                var message = LiveCasinoLang.UpdatedUnsuccessfully;
                var code = ((string)Code).ToLowerInvariant();

                if (code != E0099)
                {
                    message = LiveCasinoLang.ResourceManager.GetString(code);
                }

                return message;
            }
        }

        public override bool IsSuccessful
        {
            get
            {
                return string.IsNullOrWhiteSpace((string)Code);
            }
        }
    }
}