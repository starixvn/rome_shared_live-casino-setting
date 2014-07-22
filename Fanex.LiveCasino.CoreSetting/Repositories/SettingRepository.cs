// -----------------------------------------------------------------------
// <copyright file="SettingRepository.cs" company="">
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
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SettingRepository : BaseRepository, ISettingRepository
    {
        public CustInfo GetCustomerInfo(ICriteria criteria)
        {
            using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var custInfo = objectDb.QueryEntity<CustInfo, MemberWinLimit, MemberWinLimit, CustInfo>(
                    (ci, mw, smw) => { ci.MemberWinLimit = mw; mw.Upline = smw; return ci; },
                    "isAutoResetLiveCasino,UplineIsAutoResetLiveCasino",
                    criteria.GetSpParams());

                return custInfo;
            }
        }
    }
}
