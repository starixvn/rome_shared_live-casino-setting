namespace Fanex.LiveCasino.CoreSetting.Repositories
{
    using Fanex.Data;
    using Fanex.LiveCasino.CoreSetting.Criteria;
    using Fanex.NAP.Framework.Command;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BaseRepository : DynamicRepository, IBaseRepository
    {
        public ICommandResult Update(ICriteria criteria)
        {
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var parameters = criteria.AddSpParams((new ResultCriteria()).GetSpParams());
                objDb.ExecuteNonQuery(parameters);

                ICommandResult result = new CommandResultBase(parameters["errcode"].ToString(), parameters["errmsg"].ToString());

                return result;
            }
        }
    }
}
