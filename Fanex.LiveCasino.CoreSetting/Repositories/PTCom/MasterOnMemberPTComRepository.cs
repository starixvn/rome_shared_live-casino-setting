namespace Fanex.LiveCasino.CoreSetting.Repositories.PTCom
{
    using System.Data;
    using Fanex.Common.Extensions;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.Models;

    /// <summary>
    /// Master setting on member repository.
    /// </summary>
    public class MasterOnMemberPTComRepository : PTComRepositoryBase
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
                    IsDisableAuto = reader.To<bool>("DisableAutoMPT"),
                    M1 = reader.To<float>("v162mpositiontaking_1101"),
                    A1 = reader.To<float>("V162apositiontaking_1101"),
                    Um1 = reader.To<float>("auto_v162mpositiontaking_1101_mem"),
                    Im1 = reader.To<bool>("isauto_v162mpositiontaking_1101_mem"),
                    M1Max = reader.To<float>("v162mpositiontaking_1101_max"),
                    M1Min = reader.To<float>("v162mpositiontaking_1101_min"),
                    Im2 = reader.To<bool>("isauto_v162mpositiontaking_1101"),
                    M3 = reader.To<float>("auto_v162mpositiontaking_1101_max")
                }
            };

            return positionTakingCommission;
        }
    }
}