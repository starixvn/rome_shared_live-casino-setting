namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.LiveCasino.CoreSetting.Criteria;
    using Fanex.LiveCasino.CoreSetting.Criteria.GameSetting;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Models.Command;
    using Fanex.LiveCasino.CoreSetting.Models.Log;
    using Fanex.LiveCasino.CoreSetting.Repositories;
    using Fanex.LiveCasino.CoreSetting.Resources;
    using Fanex.NAP.Framework.Command;
    using RemotingRef;
    using Fanex.LiveCasino.CoreSetting.Criteria.Log;

    public class GameSettingService : IGameSettingService
    {
        private const string CasinoNo = "01";
        private const int InsertFormId = 1;

        private readonly ISettingRepository _settingRepository;

        public GameSettingService()
            : this(null)
        {
        }

        public GameSettingService(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository ?? new SettingRepository();
        }

        public string GetCustomerSiteByUsername(string username)
        {
            var userInfo = _settingRepository.Get<CustInfo>(new GetCustomerSiteByUsernameCriteria
            {
                Username = username
            });

            return userInfo.Site;
        }

        public ICommandResult UpdateLiveCasinoStatus(SettingContext settingContext, CustInfo custInfo)
        {
            var criteria = new UpdateLiveCasinoStatusCriteria
            {
                Custid = settingContext.TargetCustId,
                UserID = settingContext.UserIdDoAction,
                DisabledLiveCS = custInfo.DisableLiveCasino,
                IP = settingContext.IP,
                Language = settingContext.Language
            };

            ICommandResult updateResult = _settingRepository.InsertOrUpdate(criteria);

            //UpdateLiveCasinoStatusSuccess
            if (updateResult.Code.ToString() == "0")
            {
                updateResult = new CommandResult
                {
                    Code = 0,
                    Message = LiveCasinoLang.UpdateLiveCasinoStatusSuccess
                };
            }

            return updateResult;
        }

        public ICommandResult UpdateSyncLiveCasino(SettingContext settingContext, bool isSync)
        {
            var criteria = new UpdateSyncLiveCasinoCriteria
            {
                CustId = settingContext.TargetCustId,
                UserId = settingContext.UserIdDoAction,
                Ip = settingContext.IP,
                Language = settingContext.Language,
                SubAccId = settingContext.SubAccId,
                SyncLiveCS = isSync
            };

            return _settingRepository.InsertOrUpdate(criteria);
        }

        public ICommandResults UpdateManualGameSetting(SettingContext settingContext, CustInfo custInfo, List<GameType> gameTypes)
        {
            var isInsert = false;

            if (!custInfo.IsSyncLiveCasino)
            {
                ICommandResult resultInsertAccount = InsertManualAccount(settingContext);

                if (resultInsertAccount.IsSuccessful)
                {
                    var logData = new LogManualInsertAccount
                    {
                        Action = GetInsertManualLogAction(settingContext.TargetRoleId),
                        AgentId = settingContext.AgentId,
                        CustId = settingContext.TargetCustId,
                        CustUserName = settingContext.TargetUsername,
                        ExchangeId = settingContext.ExchangeId,
                        SignUpIp = settingContext.IP,
                        SignUpForm = InsertFormId,
                        SubAccId = settingContext.SubAccId,
                        SubAccName = settingContext.SubAccName,
                        SuperId = settingContext.SuperId,
                        UserId = settingContext.UserIdDoAction,
                        UserName = settingContext.UserName,
                        CustSite = settingContext.SiteName
                    };

                    WriteLog(logData);

                    CommandResult syncResult = (CommandResult)UpdateSyncLiveCasino(settingContext, true);

                    if (syncResult.IsSuccessful)
                    {
                        custInfo.DisableLiveCasino = false;
                        UpdateLiveCasinoStatus(settingContext, custInfo);
                    }

                    isInsert = true;
                }
            }

            List<GameType> gameTypeOlds = GetGameSettingByCustomerId(settingContext.TargetCustId);

            ICommandResults resultUpdateStakeLevels = UpdateListStakeLevel(settingContext.TargetCustId, settingContext.TargetRoleId, gameTypes);

            if (resultUpdateStakeLevels.IsSuccessful)
            {
                var successResultUpdateStakeLevel = resultUpdateStakeLevels.Results.Where(x => x.IsSuccessful);

                foreach (var itemResult in successResultUpdateStakeLevel)
                {
                    var gameSettingResult = (GameSettingCommandResult)itemResult;
                    var newGameType = gameTypes.Where(x => x.Id.ToString() == gameSettingResult.Message).FirstOrDefault();
                    if (newGameType != null)
                    {
                        var oldGameType = gameTypeOlds.Where(x => x.Id == newGameType.Id).FirstOrDefault();
                        WriteLogUpdateStakeLevel(settingContext, oldGameType, newGameType, isInsert);

                        gameSettingResult.Message = string.Format(LiveCasinoLang.UpdateLiveCasinoGameSettingSuccess, RefData.GetBetTypeName(newGameType.BetTypeId.ToString(), "E"));
                    }
                }
            }

            return resultUpdateStakeLevels;
        }

        public ICommandResults UpdateAutoGameSetting(SettingContext settingContext, CustInfo custInfo, List<GameType> gameTypes)
        {
            return null;
        }

        public List<GameType> GetGameSettingByExchange(int exchangeId, string site)
        {
            var criteria = new GetGameSettingByExchangeCriteria
            {
                ExchangeId = exchangeId,
                Site = site
            };

            return _settingRepository.GetGameSetting(criteria, false);
        }

        public List<GameType> GetGameSettingByCustomerId(int custId)
        {
            var criteria = new GetGameSettingByCustomerIdCriteria
            {
                CustId = custId
            };

            return _settingRepository.GetGameSetting(criteria, true);
        }

        #region Log

        public ICommandResult WriteLog(ILogData logData)
        {
            var criteria = new InsertLogUserRunSPCriteria
            {
                UserId = logData.UserId,
                CustId = logData.CustId,
                SpName = logData.SpName,
                Doing = string.Empty,
                ActionId = (int)logData.Action,
                TrackingValue = logData.NewValues,
                TrackingValue2 = logData.OldValues,
                Duration = 0
            };

            return _settingRepository.InsertOrUpdate(criteria);
        }

        private LogActions GetInsertManualLogAction(int roleId)
        {
            LogActions logAction;

            switch (roleId)
            {
                case (int)CustRole.Super:
                    logAction = LogActions.ManualInsert_Super;
                    break;
                case (int)CustRole.Master:
                    logAction = LogActions.ManualInsert_Master;
                    break;
                case (int)CustRole.Agent:
                    logAction = LogActions.ManualInsert_Agent;
                    break;
                default:
                    logAction = LogActions.ManualInsert_Member;
                    break;
            }

            return logAction;
        }

        private LogActions GetUpdateStakeLevelLogAction(int roleId)
        {
            LogActions logAction;

            switch (roleId)
            {
                case (int)CustRole.Super:
                    logAction = LogActions.UpdateStakeLevel_Super;
                    break;
                case (int)CustRole.Master:
                    logAction = LogActions.UpdateStakeLevel_Master;
                    break;
                case (int)CustRole.Agent:
                    logAction = LogActions.UpdateStakeLevel_Agent;
                    break;
                default:
                    logAction = LogActions.UpdateStakeLevel_Member;
                    break;
            }

            return logAction;
        }

        private void WriteLogUpdateStakeLevel(SettingContext settingContext, GameType gameTypeOld, GameType gameTypeNew, bool isInsert = false)
        {
            var log = new LogUpdateStakeLevel
            {
                Action = GetUpdateStakeLevelLogAction(settingContext.TargetRoleId),
                CustId = settingContext.TargetCustId,
                SignUpIp = settingContext.IP,
                Language = "E",
                UserId = settingContext.UserIdDoAction,
                RoleId = settingContext.TargetRoleId,
                NewGameType = gameTypeNew,
                OldGameType = isInsert ? null : gameTypeOld
            };

            if (gameTypeNew.CheckedStakeLevelId != gameTypeOld.CheckedStakeLevelId)
            {
                WriteLog(log);
            }
        }

        public IList<GameType> GetGameSetting(SettingContext settingContext, CustInfo custInfo)
        {
            var gameSettings = new List<GameType>();

            if (settingContext.IsEdittingSuper && !custInfo.IsSyncLiveCasino)
            {
                var site = GetCustomerSiteByUsername(custInfo.UserName);
                gameSettings = GetGameSettingByExchange(custInfo.CurrencyId, site);

                // Check all by default
                foreach (var gameSettingItem in gameSettings)
                {
                    foreach (var betLimitItem in gameSettingItem.BettingLimit)
                    {
                        betLimitItem.IsChecked = true;
                    }
                }
            }
            else if (!settingContext.IsEdittingSuper && !custInfo.IsSyncLiveCasino)
            {
                gameSettings = GetGameSettingByCustomerId(settingContext.CustId);
            }
            else
            {
                gameSettings = GetGameSettingByCustomerId(settingContext.TargetCustId);
            }

            foreach (var gameSettingItem in gameSettings)
            {
                gameSettingItem.Name = RefData.GetBetTypeName(gameSettingItem.BetTypeId.ToString(), "E");
            }

            return gameSettings;
        }

        private ICommandResult InsertAutoAccount(SettingContext settingContext)
        {
            var criteria = new InsertAutoAccountCriteria
            {
                CustId = settingContext.TargetCustId,
                CasinoNo = CasinoNo,
                ExchangeId = settingContext.ExchangeId,
                SuperId = settingContext.SuperId,
                MasterId = settingContext.MasterId,
                AgentId = settingContext.AgentId,
                RoleId = settingContext.TargetRoleId,
                SignupIp = settingContext.IP,
                SignupBy = settingContext.UserIdDoAction,
                SignupFrom = InsertFormId,
                ModifyUsername = settingContext.UserName,
                Username = settingContext.TargetUsername
            };

            return _settingRepository.InsertOrUpdate(criteria);
        }

        private ICommandResult InsertManualAccount(SettingContext settingContext)
        {
            var criteria = new InsertManualAccountCriteria
            {
                CustId = settingContext.TargetCustId,
                CasinoNo = CasinoNo,
                ExchangeId = settingContext.ExchangeId,
                SuperId = settingContext.SuperId,
                MasterId = settingContext.MasterId,
                AgentId = settingContext.AgentId,
                RoleId = settingContext.TargetRoleId,
                SignupIp = settingContext.IP,
                SignupBy = settingContext.UserIdDoAction,
                SignupFrom = InsertFormId,
                ModifyUsername = settingContext.UserName,
                Username = settingContext.TargetUsername,
                CustSite = settingContext.SiteName
            };

            return _settingRepository.InsertOrUpdate(criteria);
        }

        private ICommandResult UpdateStakeLevel(int custId, int roleId, GameType gameType)
        {
            var criteria = new UpdateStakeLevelCriteria
            {
                CustId = custId,
                RoleId = roleId,
                GameTypeId = gameType.Id,
                StakeLevelList = gameType.CheckedStakeLevelId
            };

            var result = (CommandResult)_settingRepository.InsertOrUpdate(criteria);

            var gameSettingResult = new GameSettingCommandResult(result);

            if (gameSettingResult.IsSuccessful)
            {
                gameSettingResult.Message = gameType.Id.ToString();
            }
            else
            {
                var betType = RefData.GetBetTypeName(gameType.BetTypeId.ToString(), "E");
                gameSettingResult.Message = string.Format(LiveCasinoLang.UpdateLiveCasinoGameSettingFail, betType, gameSettingResult.Message);
            }

            return gameSettingResult;
        }

        private ICommandResults UpdateListStakeLevel(int custId, int roleId, List<GameType> gameTypes)
        {
            var results = new CommandResults();

            foreach (var itemGameType in gameTypes)
            {
                var result = UpdateStakeLevel(custId, roleId, itemGameType);
                results.Results.Add(result);
            }

            return results;
        }
        #endregion
    }
}
