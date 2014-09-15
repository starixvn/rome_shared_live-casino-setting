// -----------------------------------------------------------------------
// <copyright file="MasterPTComRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.LiveCasino.CoreSetting.Repositories.PTCom
{
    using System.Data;
    using Fanex.Common.Extensions;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MasterPTComRepository : PTComRepositoryBase
    {
        protected override PTComDTO MapData(IDataReader reader)
        {
            var positionTakingCommission = new PTComDTO
            {
                PositionTaking = new PositionTaking
                {
                    S4 = reader.To<float>("v162spositiontaking_1101_max"),
                    N3 = reader.To<float>("v162mpositiontaking_1101_min"),
                    Is3 = reader.To<bool>("isauto_v162spositiontaking_1101"),
                    Us3 = reader.To<float>("auto_v162spostiontaking_1101"),
                    S3 = reader.To<float>("v162spositiontaking_1101"),
                    M3 = reader.To<float>("v162mpositiontaking_1101")
                },
                Commission = reader.ToFloat("v162discount_1101"),
                ParentCommission = reader.ToFloat("v162discount_1101_max")
            };

            return positionTakingCommission;
        }
    }
}