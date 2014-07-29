namespace Fanex.LiveCasino.CoreSetting.Repositories
{
    using System.Collections.Generic;
    using Fanex.NAP.Framework.Command;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IBaseRepository : IDynamicRepository
    {
        ICommandResult InsertOrUpdate(ICriteria criteria);
    }
}
