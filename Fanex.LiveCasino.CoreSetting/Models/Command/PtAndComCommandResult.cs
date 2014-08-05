namespace Fanex.LiveCasino.CoreSetting.Models.Command
{
    using Fanex.Common;
    using Fanex.NAP.Framework.Command;

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