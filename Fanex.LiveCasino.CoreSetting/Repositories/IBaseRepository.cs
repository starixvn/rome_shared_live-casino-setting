namespace Fanex.LiveCasino.CoreSetting.Repositories
{
    using System.Collections;
    using Fanex.NAP.Framework.Command;
    using Fanex.NAP.Framework.Data;

    public interface IBaseRepository : IDynamicRepository
    {
        ICommandResult InsertOrUpdate(ICriteria criteria);

        ICommandResult InsertOrUpdate(ICriteria criteria, Hashtable updatePrameters);
    }
}