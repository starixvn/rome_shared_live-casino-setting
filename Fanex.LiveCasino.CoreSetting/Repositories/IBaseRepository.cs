namespace Fanex.LiveCasino.CoreSetting.Repositories
{
    using Fanex.NAP.Framework.Command;
    using Fanex.NAP.Framework.Data;

    public interface IBaseRepository : IDynamicRepository
    {
        ICommandResult InsertOrUpdate(ICriteria criteria);
    }
}