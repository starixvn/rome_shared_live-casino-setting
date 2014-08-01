namespace Fanex.LiveCasino.CoreSetting.Models.Log
{
    public class LogUpdateStakeLevel : LogDataBase, ILogData
    {
        protected const string TrackingTemplate = "<{0} CustID=\"{1}\" {2} GameTypeId=\"{3}\" BetTypeId=\"{4}\" StakeLevel=\"{5}\" Ip=\"{6}\" Lang=\"{7}\" RoleID=\"{8}\" ErrCode=\"{9}\" ErrMsg=\"{10}\" />";

        public GameType OldGameType { get; set; }
        public GameType NewGameType { get; set; }

        protected string CreateLogData(GameType gameType)
        {
            return gameType != null ? string.Format(TrackingTemplate
                , SpName
                , CustId
                , RoleId == 4 ? string.Empty : string.Format(TrackingSubAcc, SubAccId, SubAccName)
                , gameType.Id
                , gameType.BetTypeId
                , gameType.CheckedStakeLevelRange
                , SignUpIp
                , Language
                , RoleId
                , ErrCode
                , ErrMsg) : string.Empty;
        }

        public override string SpName
        {
            get
            {
                return "TP_User_CustGameType_UpdStakeLevel";
            }
        }

        public override string NewValues
        {
            get
            {
                return CreateLogData(NewGameType);
            }
        }

        public override string OldValues
        {
            get
            {
                return CreateLogData(OldGameType);
            }
        }

        public override bool IsValid
        {
            get
            {
                return base.IsValid;
            }
        }
    }
}
