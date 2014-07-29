namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System;
    using System.Collections.Generic;
    using System.Resources;
    using Fanex.Common;
    using Fanex.LiveCasino.CoreSetting.Criteria;
    using Fanex.LiveCasino.CoreSetting.Criteria.GameSetting;
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using Fanex.LiveCasino.CoreSetting.Criteria.WinLimit;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Repositories;
    using Fanex.LiveCasino.CoreSetting.ViewModels;
    using Fanex.NAP.Framework.Command;
    using Newtonsoft.Json;
    using RemotingRef;
    using Fanex.LiveCasino.CoreSetting.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SettingService : ISettingService
    {
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

        public ICommandResult UpdateLiveCasinoStatus(UpdateLiveCasinoStatusCriteria criteria)
        {
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

        public IList<CommandResult> UpdateSuperPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting)
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

        public IList<CommandResult> UpdateMasterPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting)
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

        public IList<CommandResult> UpdateAgentPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting)
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

        public IList<CommandResult> AgentUpdateMemberPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting)
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

        public IList<CommandResult> SuperUpdateMemberPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting)
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

        public IList<CommandResult> MasterUpdateMemberPtAndComSetting(SettingContext settingContext, PtAndComSetting ptAndComSetting)
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

        #endregion Game Setting

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

        private IList<CommandResult> GetResultDetails(ICommandResult commandResult)
        {
            IList<CommandResult> resultDetails = new List<CommandResult>();

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

        #endregion Private Methods
    }
}