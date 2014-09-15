// -----------------------------------------------------------------------
// <copyright file="MemberPTComRepository.cs" company="Microsoft">
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
    public class AgentOnMemberPTComRepository : PTComRepositoryBase
    {
        protected override PTComDTO MapData(IDataReader reader)
        {
            var positionTakingCommission = new PTComDTO
            {
                PositionTaking = new PositionTaking
                {
                    A1 = reader.To<float>("v162apositiontaking_1101"),
                    A2 = reader.To<float>("v162apositiontaking_1101_max"),
                    N2 = reader.To<float>("v162apositiontaking_1101_min"),
                },
                Commission = reader.ToFloat("v162playerdiscount_1101"),
                ParentCommission = reader.ToFloat("v162playerdiscount_1101_max")
            };

            return positionTakingCommission;
        }
    }
}