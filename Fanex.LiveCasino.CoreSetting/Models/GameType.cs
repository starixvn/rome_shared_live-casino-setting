namespace Fanex.LiveCasino.CoreSetting.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using LiveCasino.CoreSetting.Utilities;
    using RemotingRef;

    /// <summary>
    /// Gaming type.
    /// </summary>
    public class GameType
    {
        private readonly ICommonUtil _commonUtil;

        public GameType()
            : this(null)
        {
        }

        public GameType(ICommonUtil commonUtil)
        {
            _commonUtil = commonUtil ?? new CommonUtil();
        }

        public int Id { get; set; }

        public string Name
        {
            get
            {
                return RefData.GetBetTypeName(BetTypeId.ToString(), _commonUtil.CurrentLanguage);
            }
        }

        public int BetTypeId { get; set; }

        public List<StakeLevel> BettingLimit { get; set; }

        public string CheckedStakeLevelId
        {
            get
            {
                return BettingLimit != null ? string.Join(",", BettingLimit.Where(x => x.IsChecked).Select(x => x.Id.ToString()).ToArray()) : string.Empty;
            }
        }

        public string CheckedStakeLevelRange
        {
            get
            {
                return BettingLimit != null ? string.Join(",", BettingLimit.Where(x => x.IsChecked).Select(x => x.Range.ToString()).ToArray()) : string.Empty;
            }
        }
    }
}