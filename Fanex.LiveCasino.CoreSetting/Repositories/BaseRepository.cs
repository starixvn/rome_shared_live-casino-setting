namespace Fanex.LiveCasino.CoreSetting.Repositories
{
    using System;
    using System.Collections;
    using System.Data;
    using Fanex.Data;
    using Fanex.LiveCasino.CoreSetting.Criteria;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.NAP.Framework.Command;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// The base repository class.
    /// </summary>
    public class BaseRepository : DynamicRepository, IBaseRepository
    {
        /// <summary>
        /// Inserts the or update.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Command results class.</returns>
        public ICommandResult InsertOrUpdate(ICriteria criteria)
        {
            return InsertOrUpdate(criteria, null);
        }

        /// <summary>
        /// Insert or update with update parameter.
        /// </summary>
        /// <param name="criteria">Criteria for update.</param>
        /// <param name="updatePrameters">Parameter update criteria.</param>
        /// <returns>Result of updating command.</returns>
        public ICommandResult InsertOrUpdate(ICriteria criteria, Hashtable updatePrameters)
        {
            var result = new CommandResult();

            if (criteria.IsValid())
            {
                using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var parameters = criteria.AddSpParams((new ResultCriteria()).GetSpParams());
                    if (updatePrameters != null)
                    {
                        foreach (DictionaryEntry updatePrameter in updatePrameters)
                        {
                            parameters[updatePrameter.Key] = updatePrameter.Value;
                        }
                    }

                    objDb.ExecuteNonQuery(parameters);

                    var outParameters = objDb.GetOuputParams();
                    result = GetCommandResult(outParameters);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the common setting for edit.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="mapSetting">The map setting.</param>
        /// <returns>Position taking and commission setting models.</returns>
        protected PtAndComSetting GetCommonSettingForEdit(ICriteria criteria, Action<IDataReader, PtAndComSetting> mapSetting)
        {
            var positionTakingAndComSetting = new PtAndComSetting();

            if (criteria.IsValid())
            {
                using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    using (IDataReader reader = objectDb.ExecuteReader(criteria.GetSpParams()))
                    {
                        while (reader.Read())
                        {
                            mapSetting(reader, positionTakingAndComSetting);
                        }
                    }
                }
            }

            return positionTakingAndComSetting;
        }

        /// <summary>
        /// Gets the command result.
        /// </summary>
        /// <param name="outParameters">The out parameters.</param>
        /// <returns>Error code and error message.</returns>
        protected CommandResult GetCommandResult(Hashtable outParameters)
        {
            var result = new CommandResult();

            if (outParameters != null)
            {
                result.Code = outParameters.Contains("errcode") && outParameters["errcode"] != DBNull.Value ? outParameters["errcode"] : 0;
                result.Message = outParameters.Contains("errmsg") ? outParameters["errmsg"].ToString() : string.Empty;
            }

            return result;
        }
    }
}