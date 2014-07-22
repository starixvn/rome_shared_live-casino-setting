namespace Fanex.LiveCasino.CoreSetting.Repositories
{
    using System;
    using System.Data;
    using Fanex.Data;
    using Fanex.LiveCasino.CoreSetting.Criteria;
    using Fanex.LiveCasino.CoreSetting.ViewModels;
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

        protected CustSettingViewModel GetCommonSettingForEdit(
            ICriteria criteria,
            Action<IDataReader, CustSettingViewModel> mapSetting)
        {
            CustSettingViewModel custSetting = new CustSettingViewModel();
            using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (IDataReader reader = objectDb.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        mapSetting(reader, custSetting);
                    }
                }
            }

            return custSetting;
        }
    }
}