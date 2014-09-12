namespace Fanex.LiveCasino.CoreSetting.Repositories
{
    using Fanex.NAP.Framework.Data;

    public interface IBaseRepository : IDynamicRepository
    {
        ICommandResult Save(ICriteria criteria);
    }
}