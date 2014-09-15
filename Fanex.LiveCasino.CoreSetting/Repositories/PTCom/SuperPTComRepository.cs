// -----------------------------------------------------------------------
// <copyright file="SuperPTComRepository.cs" company="Microsoft">
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
    public class SuperPTComRepository : PTComRepositoryBase
    {
        protected override PTComDTO MapData(IDataReader reader)
        {
            var positionTakingCommission = new PTComDTO
            {
                PositionTaking = new PositionTaking
                {
                    S4 = reader.To<float>("v162spositiontaking_1101"),
                    L4 = reader.To<float>("v162spositiontaking_1101_max"),
                    N4 = reader.To<float>("v162spositiontaking_1101_min")
                },
                Commission = reader.ToFloat("v162sdiscount_1101"),
                ParentCommission = reader.ToFloat("v162sdicount_1101_max")
            };

            return positionTakingCommission;
        }
    }
}