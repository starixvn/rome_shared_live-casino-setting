// --------------------------------------------------------------------------
// <copyright file="BaseRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.7.22">Created</change>
// </history>

namespace Fanex.LiveCasino.CoreSetting.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.LiveCasino.CoreSetting.Core.Data;
    using Fanex.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BaseRepository : DynamicRepository, IBaseRepository
    {

        public Result Update(ICriteria criteria)
        {
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var parameters = criteria.AddSpParams((new ResultCriteria()).GetSpParams());
                objDb.ExecuteNonQuery(parameters);

                Result result = new Result(parameters["errcode"].ToString(), parameters["errmsg"].ToString());

                return result;
            }
        }

    }
}
