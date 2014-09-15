namespace Fanex.LiveCasino.CoreSetting.Repositories.PTCom
{
    using System;
    using System.Data;
    using Fanex.Data;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.Resources;
    using Fanex.Logging;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class PTComRepositoryBase : BaseRepository, IPTComRepository
    {
        public virtual PTComDTO Get(ICriteria criteria)
        {
            if (!criteria.IsValid())
            {
                throw new Exception(LiveCasinoLang.InvalidCriteriaMsg);
            }

            try
            {
                using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    using (IDataReader reader = objectDb.ExecuteReader(criteria.GetSpParams()))
                    {
                        while (reader.Read())
                        {
                            return MapData(reader);
                        }
                    }
                }

                return new PTComDTO();
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message);

                throw ex;
            }
        }

        protected abstract PTComDTO MapData(IDataReader reader);
    }
}