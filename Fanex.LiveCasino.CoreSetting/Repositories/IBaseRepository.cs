namespace Fanex.LiveCasino.CoreSetting.Repositories
{
    using Fanex.NAP.Framework.Command;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IBaseRepository : IDynamicRepository
    {
        ICommandResult Update(ICriteria criteria);
    }
}
