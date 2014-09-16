namespace Fanex.LiveCasino.CoreSetting.Repositories
{
    using System;
    using System.Collections;
    using System.Data;
    using Fanex.Data;
    using Fanex.LiveCasino.CoreSetting.Constant;
    using Fanex.LiveCasino.CoreSetting.Criteria;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.DTO.CommandResult;
    using Fanex.LiveCasino.CoreSetting.Resources;
    using Fanex.Logging;
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
        public ICommandResult Save(ICriteria criteria)
        {
            if (criteria.IsValid())
            {
                try
                {
                    var result = new CommandResult();

                    using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                    {
                        var parameters = criteria.AddSpParams((new ResultCriteria()).GetSpParams());

                        objDb.ExecuteNonQuery(parameters);

                        var outParameters = objDb.GetOuputParams();
                        result = GetCommandResult(outParameters);
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    Logger.Log.Error(ex.Message);

                    throw ex;
                }
            }

            var message = string.Format(LiveCasinoLang.InvalidCriteriaMsg, criteria.GetSettingKey());
            Logger.Log.Error(string.Format("{0}\n{1}", message,criteria.ToString()));

            throw new Exception(message);
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