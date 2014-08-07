namespace Fanex.LiveCasino.CoreSetting.Repositories
{
    using Fanex.NAP.Framework.Command;
    using Fanex.NAP.Framework.Data;
    using System.Collections;

    public interface IBaseRepository : IDynamicRepository
    {
        ICommandResult InsertOrUpdate(ICriteria criteria);

        ICommandResult InsertOrUpdate(ICriteria criteria, Hashtable updatePrameters);
    }
}