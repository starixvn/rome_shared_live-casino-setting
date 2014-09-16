namespace Fanex.LiveCasino.CoreSetting.DTO.CommandResult
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