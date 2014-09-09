namespace Fanex.LiveCasino.CoreSetting.DTO.Command
{
    using Fanex.Common;

    /// <summary>
    /// Position taking And Commission CommandResult.
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