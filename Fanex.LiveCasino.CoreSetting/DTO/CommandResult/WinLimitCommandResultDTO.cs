namespace Fanex.LiveCasino.CoreSetting.DTO.CommandResult
{
    using System;

    /// <summary>
    /// Win limit command result class.
    /// </summary>
    public class WinLimitCommandResultDTO : CommandResultDTO
    {
        public WinLimitCommandResultDTO()
        {
        }

        public WinLimitCommandResultDTO(CommandResultDTO result)
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