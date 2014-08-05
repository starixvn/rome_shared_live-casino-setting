namespace Fanex.LiveCasino.CoreSetting.Models
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Gaming type.
    /// </summary>
    public class GameType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int BetTypeId { get; set; }

        public List<StakeLevel> BettingLimit { get; set; }

        public string CheckedStakeLevelId
        {
            get
            {
                return string.Join(",", BettingLimit.Where(x => x.IsChecked).Select(x => x.Id.ToString()).ToArray());
            }
        }

        public string CheckedStakeLevelRange
        {
            get
            {
                return string.Join(",", BettingLimit.Where(x => x.IsChecked).Select(x => x.Range.ToString()).ToArray());
            }
        }
    }
}