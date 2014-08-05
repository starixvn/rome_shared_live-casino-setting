namespace Fanex.LiveCasino.CoreSetting.Models.Log
{
    using System.Collections.Generic;
    using System.Linq;

    public class LogAutoInsertAccount : LogManualInsertAccount
    {
        private const string TrackingTemplate = "<{0} CustID=\"{1}\" UserName=\"{2}\" CasinoNo=\"{3}\" ExchangeID=\"{4}\" SCustID=\"{5}\" MCustID=\"{6}\" ACustID=\"{7}\" RoleID=\"{8}\" SignupIP=\"{9}\" SignupBy=\"{10}\" AddFrom=\"{11}\" Lang=\"{12}\" ErrCode=\"{13}\" ErrMsg=\"{14}\" {15} >{16}</{17}>";

        private const string GameTypeTemplate = "<GameType GameTypeId=\"{0}\" BetTypeId=\"{1}\" StakeLevel=\"{2}\" />";

        public List<GameType> GameSetting { get; set; }

        public override string SpName
        {
            get
            {
                return "TP_User_Customer_AutoInsertAccount";
            }
        }

        private string GetGameTypeLogData()
        {
            var gameType = string.Empty;

            foreach (var itemGameType in GameSetting)
            {
                gameType += string.Format(GameTypeTemplate, itemGameType.Id, itemGameType.BetTypeId, string.Join(",", itemGameType.BettingLimit.Select(x => x.Range.ToString()).ToArray()));
            }

            return gameType;
        }

        private string GetTrackingValue()
        {
            string gameTypeLog = GetGameTypeLogData();
            string trackingValue = string.Format(TrackingTemplate
                                        , SpName
                                        , CustId
                                        , CustUserName
                                        , CasinoNo
                                        , ExchangeId
                                        , SuperId
                                        , MasterId
                                        , AgentId
                                        , RoleId
                                        , SignUpIp
                                        , UserId
                                        , SignUpForm
                                        , Language
                                        , ErrCode
                                        , ErrMsg
                                        , RoleId == 4 ? string.Empty : string.Format(TrackingSubAcc, SubAccId, SubAccName)
                                        , gameTypeLog
                                        , SpName);

            return trackingValue;
        }


        public override string NewValues
        {
            get
            {
                return GetTrackingValue();
            }
        }

        public override string OldValues
        {
            get
            {
                return string.Empty;
            }
        }
    }
}
