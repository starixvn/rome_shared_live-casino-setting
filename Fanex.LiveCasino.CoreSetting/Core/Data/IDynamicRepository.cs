// -----------------------------------------------------------------------
// <copyright file="IDynamicRepository.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.LiveCasino.CoreSetting.Core.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IDynamicRepository
    {
        T Get<T>(int id);

        T Get<T>(ICriteria criteria);

        IEnumerable<T> Fetch<T>();

        IEnumerable<T> Fetch<T>(ICriteria criteria);

        Result Update(ICriteria criteria);
    }
}
