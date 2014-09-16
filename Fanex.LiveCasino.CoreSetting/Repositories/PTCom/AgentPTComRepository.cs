namespace Fanex.LiveCasino.CoreSetting.Repositories.PTCom
{
    using System.Data;
    using Fanex.Common.Extensions;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.Models;

    /// <summary>
    /// Agent repository for settting.
    /// </summary>
    public class AgentPTComRepository : PTComRepositoryBase
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
                    M2 = reader.To<float>("v162mpositiontaking_1101"),
                    Im2 = reader.To<bool>("isauto_v162mpositiontaking_1101"),
                    Um2 = reader.To<float>("auto_v162mpositiontaking_1101"),
                    A2 = reader.To<float>("v162apositiontaking_1101"),
                    M3 = reader.To<float>("v162mpositiontaking_1101_max"),
                    N2 = reader.To<float>("v162apositiontaking_1101_min")
                },
                Commission = reader.ToFloat("v162discount_1101"),
                ParentCommission = reader.ToFloat("v162discount_1101_max")
            };

            return positionTakingCommission;
        }
    }
}