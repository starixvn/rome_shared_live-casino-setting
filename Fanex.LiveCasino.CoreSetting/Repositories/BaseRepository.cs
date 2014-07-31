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
            var result = new CommandResult();

            if (criteria.IsValid())
            {
                using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var parameters = criteria.AddSpParams((new ResultCriteria()).GetSpParams());
                    objDb.ExecuteNonQuery(parameters);

                    var outParameters = objDb.GetOuputParams();
                    if (outParameters != null)
                    {
                        result.Code = outParameters.Contains("errcode") ? outParameters["errcode"] : 0;
                        result.Message = outParameters.Contains("errmsg") ? outParameters["errmsg"].ToString() : string.Empty;
                    }
                }
            }

            return result;
        }

        protected PtAndComSetting GetCommonSettingForEdit(
            ICriteria criteria,
            Action<IDataReader, PtAndComSetting> mapSetting)
        {
            PtAndComSetting ptAndComSetting = new PtAndComSetting();

            if (criteria.IsValid())
            {
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
            }

            return ptAndComSetting;
        }
    }
}