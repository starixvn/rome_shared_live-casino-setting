namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common;
    using Fanex.LiveCasino.CoreSetting.Criteria;
    using Fanex.LiveCasino.CoreSetting.Criteria.GameSetting;
    using Fanex.LiveCasino.CoreSetting.Criteria.Log;
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using Fanex.LiveCasino.CoreSetting.Criteria.WinLimit;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Models.Log;
    using Fanex.LiveCasino.CoreSetting.Repositories;
    using Fanex.LiveCasino.CoreSetting.Resources;
    using Fanex.LiveCasino.CoreSetting.ViewModels;
    using Fanex.NAP.Framework.Command;
    using Newtonsoft.Json;
    using RemotingRef;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SettingService : ISettingService
    {
        private const string CasinoNo = "01";
        private const int InsertFormId = 1;

        private static JsonSerializerSettings jsonSerializeSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented
        };

        private readonly ISettingRepository _settingRepository;

        public SettingService()
            : this(null)
        {
        }

        public SettingService(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository ?? new SettingRepository();
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

        /// <summary>
        /// Get customer information with customer id.
        /// </summary>
        /// <param name="criteria">Get criteria information.</param>
        /// <returns>Customer information.</returns>
        public CustInfo GetCustomerInfo(GetCustomerInfoCriteria criteria)
        {
            return _settingRepository.GetCustomerInfo(criteria);
        }

        public string GetSettingJavascriptObjectForUpdate(SettingContext settingContext, string objectName)
        {
            CustInfo custInfo = GetCustomerInfo(new GetCustomerInfoCriteria
                {
                    Custid = settingContext.TargetCustId
                });

            var custSetting = new CustSettingViewModel
            {
                SettingContext = settingContext,
                CustInfo = custInfo,
                PtAndComSetting = GetPtAndComSetting(settingContext),
                GameSetting = GetGameSetting(settingContext, custInfo)
            };

            return FormatJavascript(custSetting, objectName);
        }

        public string GetCustomerSiteByUsername(string username)
        {
            var userInfo = _settingRepository.Get<CustInfo>(new GetCustomerSiteByUsernameCriteria
            {
                Username = username
            });

            return userInfo.Site;
        }

        #region Pt and Com

        public PtAndComSetting GetSettingForEditSuper(GetSettingForEditSuperCriteria criteria)
        {
            return _settingRepository.GetSettingForEditSuper(criteria);
        }

        public PtAndComSetting GetSettingForEditMaster(GetSettingForEditMasterCriteria criteria)
        {
            return _settingRepository.GetSettingForEditMaster(criteria);
        }

        public PtAndComSetting GetSettingForEditAgent(GetSettingForEditAgentCriteria criteria)
        {
            return _settingRepository.GetSettingForEditAgent(criteria);
        }

        public PtAndComSetting GetSettingForEditMember(GetSettingForEditMemberCriteria criteria)
        {
            return _settingRepository.GetSettingForEditMember(criteria);
        }

        public PtAndComSetting GetSettingForSuperEditMember(GetSettingForSuperEditMemberCriteria criteria)
        {
            return _settingRepository.GetSettingForSuperEditMember(criteria);
        }

        public PtAndComSetting GetSettingForMasterEditMember(GetSettingForMasterEditMemberCriteria criteria)
        {
            return _settingRepository.GetSettingForMasterEditMember(criteria);
        }

        public IList<ICommandResult> UpdatePtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting)
        {
            IList<ICommandResult> updateResults = new List<ICommandResult>();
            switch ((CustRole)settingContext.TargetRoleId)
            {
                case CustRole.Super:
                    updateResults = UpdateSuperPtAndComSetting(settingContext, ptAndComSetting);
                    break;
                case CustRole.Master:
                    updateResults = UpdateMasterPtAndComSetting(settingContext, ptAndComSetting);
                    break;
                case CustRole.Agent:
                    updateResults = UpdateAgentPtAndComSetting(settingContext, ptAndComSetting);
                    break;
                case CustRole.Member:
                    switch ((CustRole)settingContext.RoleId)
                    {
                        case CustRole.Super:
                            updateResults = SuperUpdateMemberPtAndComSetting(settingContext, ptAndComSetting);
                            break;
                        case CustRole.Master:
                            updateResults = MasterUpdateMemberPtAndComSetting(settingContext, ptAndComSetting);
                            break;
                        default:
                            updateResults = AgentUpdateMemberPtAndComSetting(settingContext, ptAndComSetting);
                            break;
                    }
                    break;
            }

            return updateResults;
        }

        public IList<ICommandResult> UpdateSuperPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting)
        {
            ICommandResult commandResult = _settingRepository.InsertOrUpdate(new UpdateSuperSettingCriteria
            {
                Custid = settingContext.TargetCustId,
                IP = settingContext.IP,
                Language = settingContext.Language,
                UserID = settingContext.UserIdDoAction,
                v162discount_1101 = ptAndComSetting.Commission.Value,
                v162spositiontaking_1101 = (decimal)ptAndComSetting.PositionTaking.S4,
                v162spositiontaking_1101_min = (decimal)ptAndComSetting.PositionTaking.N4,
            });

            return GetResultDetails(commandResult);
        }

        public IList<ICommandResult> UpdateMasterPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting)
        {
            ICommandResult commandResult = _settingRepository.InsertOrUpdate(new UpdateMasterSettingCriteria
            {
                Custid = settingContext.TargetCustId,
                IP = settingContext.IP,
                Language = settingContext.Language,
                SRecommend = settingContext.CustId,
                Userid = settingContext.AdminId,
                SubAccID = settingContext.SubAccId,
                v162discount_1101 = ptAndComSetting.Commission.Value,
                v162mpositiontaking_1101 = (decimal)ptAndComSetting.PositionTaking.M3,
                v162mpositiontaking_1101_min = (decimal)ptAndComSetting.PositionTaking.N3,
                isauto_v162spositiontaking_1101 = (bool)ptAndComSetting.PositionTaking.Is3,
                auto_v162spositiontaking_1101 = (decimal)ptAndComSetting.PositionTaking.Us3,
                UpdateAllMembers = (bool)ptAndComSetting.PositionTaking.UpdateAllMembers,
                v162spositiontaking_1101 = (decimal)ptAndComSetting.PositionTaking.S3,
                NewSPositionTaking = (decimal)ptAndComSetting.PositionTaking.NewPresetOnMember
            });

            return GetResultDetails(commandResult);
        }

        public IList<ICommandResult> UpdateAgentPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting)
        {
            ICommandResult commandResult = _settingRepository.InsertOrUpdate(new UpdateAgentSettingCriteria
            {
                CustId = settingContext.TargetCustId,
                IP = settingContext.IP,
                Language = settingContext.Language,
                MRecommend = settingContext.CustId,
                UserId = settingContext.AdminId,
                SubAccID = settingContext.SubAccId,
                v162discount_1101 = ptAndComSetting.Commission.Value,
                v162mpositiontaking_1101 = (decimal)ptAndComSetting.PositionTaking.M2,
                min_v162apositiontaking_1101 = (decimal)ptAndComSetting.PositionTaking.N2,
                isauto_v162mpositiontaking_1101 = (bool)ptAndComSetting.PositionTaking.Im2,
                auto_v162mpositiontaking_1101 = (decimal)ptAndComSetting.PositionTaking.Um2,
                UpdateAllMembers = (bool)ptAndComSetting.PositionTaking.UpdateAllMembers,
                v162apositiontaking_1101 = (decimal)ptAndComSetting.PositionTaking.A2,
                NewMPositionTaking = (decimal)ptAndComSetting.PositionTaking.NewPresetOnMember
            });

            return GetResultDetails(commandResult);
        }

        public IList<ICommandResult> AgentUpdateMemberPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting)
        {
            ICommandResult commandResult = _settingRepository.InsertOrUpdate(new AgentUpdateMemberSettingCriteria
            {
                CustId = settingContext.TargetCustId,
                IP = settingContext.IP,
                Language = settingContext.Language,
                Recommend = settingContext.CustId,
                UserID = settingContext.AdminId,
                SubAccID = settingContext.SubAccId,
                v162playerdiscount_1101 = ptAndComSetting.Commission.Value,
                v162apositiontaking_1101 = (decimal)ptAndComSetting.PositionTaking.A1
            });

            return GetResultDetails(commandResult);
        }

        public IList<ICommandResult> SuperUpdateMemberPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting)
        {
            ICommandResult commandResult = _settingRepository.InsertOrUpdate(new SuperUpdateMemberSettingCriteria
            {
                CustId = settingContext.TargetCustId,
                IP = settingContext.IP,
                Language = settingContext.Language,
                UserID = settingContext.AdminId,
                SubAccID = settingContext.SubAccId,
                v162spositiontaking_1101 = (decimal)ptAndComSetting.PositionTaking.S1,
                isauto_v162spositiontaking_1101 = (bool)ptAndComSetting.PositionTaking.Is1,
                auto_v162spositiontaking_1101 = (decimal)ptAndComSetting.PositionTaking.Us1
            });
            return GetResultDetails(commandResult);
        }

        public IList<ICommandResult> MasterUpdateMemberPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting)
        {
            ICommandResult commandResult = _settingRepository.InsertOrUpdate(new MasterUpdateMemberSettingCriteria
            {
                CustId = settingContext.TargetCustId,
                IP = settingContext.IP,
                Language = settingContext.Language,
                UserId = settingContext.UserIdDoAction,
                SubAccID = settingContext.SubAccId,
                v162mpositiontaking_1101 = (decimal)ptAndComSetting.PositionTaking.M1,
                isauto_v162mpositiontaking_mem_1101 = (bool)ptAndComSetting.PositionTaking.Im1,
                auto_v162mpositiontaking_mem_1101 = (decimal)ptAndComSetting.PositionTaking.Um1
            });

            return GetResultDetails(commandResult);
        }

        #endregion Pt and Com

        #region Win Limit

        public CurrentMemberWin GetCurrentWinLimit(int custId)
        {
            return _settingRepository.Get<CurrentMemberWin>(new GetCustomerWinLimitCriteria
            {
                CustId = custId
            });
        }

        public ICommandResult UpdateWinLimit(SettingContext settingContext, MemberWinLimit memberWinLimit)
        {
            ICommandResult result = _settingRepository.InsertOrUpdate(new UpdateCustomerWinLimitCriteria
            {
                CustId = settingContext.TargetCustId,
                MaxWinLimit = memberWinLimit.Max,
                IsAutoDailyReset = memberWinLimit.IsAutoReset,
                UserId = settingContext.UserIdDoAction,
                Ip = settingContext.IP,
                Language = settingContext.Language
            });

            return result;
        }

        #endregion Win Limit

        #region Game Setting

        public List<GameType> GetGameSettingByExchange(int exchangeId, string site)
        {
            var criteria = new GetGameSettingByExchangeCriteria
            {
                ExchangeId = exchangeId,
                Site = site
            };

            return _settingRepository.GetGameSetting(criteria);
        }

        public List<GameType> GetGameSettingByCustomerId(int custId)
        {
            var criteria = new GetGameSettingByCustomerIdCriteria
            {
                CustId = custId
            };

            return _settingRepository.GetGameSetting(criteria);
        }

        public ICommandResult InsertAutoAccount(SettingContext settingContext)
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
                SignupForm = InsertFormId,
                ModifyUsername = settingContext.UserName,
                Username = settingContext.TargetUsername
            };

            return _settingRepository.InsertOrUpdate(criteria);
        }

        public ICommandResult InsertManualAccount(SettingContext settingContext)
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
               SignupForm = InsertFormId,
               ModifyUsername = settingContext.UserName,
               Username = settingContext.TargetUsername,
               CustSite = settingContext.SiteName
           };

            return _settingRepository.InsertOrUpdate(criteria);
        }

        public ICommandResult UpdateStakeLevel(int custId, int roleId, GameType gameType)
        {
            var criteria = new UpdateStakeLevelCriteria
            {
                CustId = custId,
                RoleId = roleId,
                GameTypeId = gameType.Id,
                StakeLevelList = string.Join(",", gameType.BettingLimit.Select(x => x.Id.ToString()).ToArray())
            };

            return _settingRepository.InsertOrUpdate(criteria);
        }

        public List<ICommandResult> UpdateListStakeLevel(int custId, int roleId, List<GameType> gameTypes)
        {
            var results = new List<ICommandResult>();

            foreach (var itemGameType in gameTypes)
            {
                var result = UpdateStakeLevel(custId, roleId, itemGameType);
                results.Add(result);
            }

            return results;
        }

        public List<ICommandResult> UpdateManualGameSetting(SettingContext settingContext, CustInfo custInfo, List<GameType> gameTypes)
        {
            if (!custInfo.IsSyncLiveCasino)
            {
                ICommandResult resultInsertAccount = InsertManualAccount(settingContext);

                if (resultInsertAccount.IsSuccessful)
                {
                    var logData = new LogManualInsertAccount
                    {
                        Action = GetUpdateStakeLevelLogAction(settingContext.TargetRoleId),
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

                    List<ICommandResult> resultUpdateStakeLevels = UpdateListStakeLevel(settingContext.TargetCustId, settingContext.TargetRoleId, gameTypes);

                    var successResultUpdateStakeLevel = resultUpdateStakeLevels.Select(x => x.IsSuccessful).FirstOrDefault();
                    if (successResultUpdateStakeLevel != null)
                    {
                        if (settingContext.IsEdittingSuper)
                        {
                            UpdateLiveCasinoStatus(new UpdateLiveCasinoStatusCriteria
                            {
                                
                            });
                        }
                    }

                    return resultUpdateStakeLevels;
                }
            }

            return new List<ICommandResult>();
        }

        #endregion Game Setting

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

        #endregion

        #region Private Methods

        private PtAndComSetting GetPtAndComSetting(SettingContext settingContext)
        {
            switch ((CustRole)settingContext.TargetRoleId)
            {
                case CustRole.Super:
                    return GetSettingForEditSuper(new GetSettingForEditSuperCriteria
                    {
                        Custid = settingContext.TargetCustId
                    });
                case CustRole.Master:
                    return GetSettingForEditMaster(new GetSettingForEditMasterCriteria
                        {
                            CustId = settingContext.TargetCustId,
                            SRecommend = settingContext.CustId
                        });
                case CustRole.Agent:
                    return GetSettingForEditAgent(new GetSettingForEditAgentCriteria
                        {
                            CustId = settingContext.TargetCustId,
                            MRecommend = settingContext.CustId
                        });
                case CustRole.Member:
                    switch ((CustRole)settingContext.RoleId)
                    {
                        case CustRole.Super:
                            return GetSettingForSuperEditMember(new GetSettingForSuperEditMemberCriteria
                            {
                                CustId = settingContext.TargetCustId
                            });
                        case CustRole.Master:
                            return GetSettingForMasterEditMember(new GetSettingForMasterEditMemberCriteria
                            {
                                CustId = settingContext.TargetCustId
                            });
                        default:
                            return GetSettingForEditMember(new GetSettingForEditMemberCriteria
                            {
                                CustId = settingContext.TargetCustId,
                                Recommend = settingContext.CustId
                            });
                    }
            }

            return null;
        }

        private IList<GameType> GetGameSetting(SettingContext settingContext, CustInfo custInfo)
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
                    gameSettingItem.Name = RefData.GetBetTypeName(gameSettingItem.BetTypeId.ToString(), "E");
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

            return gameSettings;
        }

        private string FormatJavascript(object obj, string name)
        {
            if (obj != null)
            {
                return string.Format("var {0} = {1}; \n", name, JsonConvert.SerializeObject(obj, jsonSerializeSettings));
            }

            return string.Empty;
        }

        private IList<ICommandResult> GetResultDetails(ICommandResult commandResult)
        {
            IList<ICommandResult> resultDetails = new List<ICommandResult>();

            string[] errCodes = commandResult.Code.Split('\a');
            string[] errMsgs = commandResult.Message.Split('\a');

            for (int i = 0; i < errCodes.Length; i++)
            {
                if (!string.IsNullOrEmpty(errCodes[i]))
                {
                    var errCode = Converter.ToInt(errCodes[i]);
                    if (errCode < 0)
                    {
                        resultDetails.Add(new CommandResult
                        {
                            Code = errCode,
                            Message = LiveCasinoLang.ResourceManager.GetString(string.Format("_{0}", Math.Abs(errCode)))
                        });
                    }
                    else
                    {
                        resultDetails.Add(new CommandResult
                        {
                            Code = errCode,
                            Message = errMsgs[i]
                        });
                    }
                }
            }

            return resultDetails;
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

        #endregion Private Methods
    }
}