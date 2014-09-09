namespace Fanex.LiveCasino.CoreSetting.DTO.Command
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Command result.
    /// </summary>
    public class CommandResult : CommandResultBase
    {
        public override bool IsSuccessful
        {
            get { return this.Code == 0; }
        }
    }
}