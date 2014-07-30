namespace Fanex.LiveCasino.CoreSetting.Repositories
{
    using System;
    using System.Data;
    using Fanex.Data;
    using Fanex.LiveCasino.CoreSetting.Criteria;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.NAP.Framework.Command;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BaseRepository : DynamicRepository, IBaseRepository
    {
        public ICommandResult InsertOrUpdate(ICriteria criteria)
        {
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var parameters = criteria.AddSpParams((new ResultCriteria()).GetSpParams());
                objDb.ExecuteNonQuery(parameters);

                var outParameters = objDb.GetOuputParams();
                if (outParameters != null)
                {
                    return new CommandResultBase
                    {
                        Code = outParameters.Contains("errcode") ? outParameters["errcode"].ToString() : string.Empty,
                        Message = outParameters.Contains("errmsg") ? outParameters["errmsg"].ToString() : string.Empty
                    };
                }

                return new CommandResultBase();
            }
        }

        protected PtAndComSetting GetCommonSettingForEdit(
            ICriteria criteria,
            Action<IDataReader, PtAndComSetting> mapSetting)
        {
            PtAndComSetting ptAndComSetting = new PtAndComSetting();
            using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (IDataReader reader = objectDb.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        mapSetting(reader, ptAndComSetting);
                    }
                }
            }

            return ptAndComSetting;
        }
    }
}