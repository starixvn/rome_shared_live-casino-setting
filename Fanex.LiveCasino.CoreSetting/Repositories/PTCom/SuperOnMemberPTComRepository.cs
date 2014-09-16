namespace Fanex.LiveCasino.CoreSetting.Repositories.PTCom
{
    using System.Data;
    using Fanex.Common.Extensions;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.Models;

    /// <summary>
    /// Super edit Member setting repository.
    /// </summary>
    public class SuperOnMemberPTComRepository : PTComRepositoryBase
    {
        /// <summary>
        /// Map PT and COM setting from database.
        /// </summary>
        /// <param name="reader">Reader for reading data.</param>
        /// <returns>PT and COM DTO for storaging data.</returns>
        protected override PTComDTO MapData(IDataReader reader)
        {
            var positionTakingCommission = new PTComDTO
            {
                PositionTaking = new PositionTaking
                {
                    S1 = reader.To<float>("v162spositiontaking_1101"),
                    S1Max = reader.To<float>("v162spositiontaking_1101_max"),
                    S1Min = reader.To<float>("v162spositiontaking_1101_min"),
                    M1 = reader.To<float>("v162mpositiontaking_1101"),
                    A1 = reader.To<float>("v162apositiontaking_1101"),
                    Us1 = reader.To<float>("auto_v162spositiontaking_1101_mem"),
                    Is1 = reader.To<bool>("isauto_v162spositiontaking_1101_mem"),
                    Is3 = reader.To<bool>("isauto_v162spositiontaking_1101"),
                    S4 = reader.To<float>("auto_v162spositiontaking_1101_max")
                }
            };

            return positionTakingCommission;
        }
    }
}