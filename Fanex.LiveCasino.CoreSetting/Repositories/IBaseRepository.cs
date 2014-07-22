// -----------------------------------------------------------------------
// <copyright file="IBaseRepository.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.LiveCasino.CoreSetting.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.LiveCasino.CoreSetting.Core.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IBaseRepository : IDynamicRepository
    {
        Result Update(ICriteria criteria);
    }
}
