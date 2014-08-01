namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.LiveCasino.CoreSetting.Criteria;
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using Fanex.LiveCasino.CoreSetting.Criteria.WinLimit;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Models.Command;
    using Fanex.LiveCasino.CoreSetting.Repositories;
    using Fanex.LiveCasino.CoreSetting.Resources;
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SettingService : ISettingService
    {
        private readonly ISettingRepository _settingRepository;

        public SettingService()
            : this(null)
        {
        }

        public SettingService(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository ?? new SettingRepository();
        }

        /// <summary>
        /// Get customer information with customer id.
        /// </summary>
        /// <param name="criteria">Get criteria information.</param>
        /// <returns>Customer information.</returns>
        public CustInfo GetCustomerInfo(int custId)
        {
            var criteria = new GetCustomerInfoCriteria
            {
                Custid = custId
            };
            return _settingRepository.GetCustomerInfo(criteria);
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

            return GetPtAnhComResultDetails(commandResult);
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

            return GetPtAnhComResultDetails(commandResult);
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

            return GetPtAnhComResultDetails(commandResult);
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

            return GetPtAnhComResultDetails(commandResult);
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
            return GetPtAnhComResultDetails(commandResult);
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

            return GetPtAnhComResultDetails(commandResult);
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

        public ICommandResults UpdateWinLimit(SettingContext settingContext, MemberWinLimit memberWinLimit)
        {
            var result = (CommandResult)_settingRepository.InsertOrUpdate(new UpdateCustomerWinLimitCriteria
            {
                CustId = settingContext.TargetCustId,
                MaxWinLimit = memberWinLimit.Max,
                IsAutoDailyReset = memberWinLimit.IsAutoReset,
                UserId = settingContext.UserIdDoAction,
                Ip = settingContext.IP,
                Language = settingContext.Language
            });

            var commandResults = new CommandResults();
            var winLimitResult = new WinLimitCommandResult(result);

            if (winLimitResult.IsSuccessful)
            {
                winLimitResult.Message = LiveCasinoLang.UpdatedWinLimitSuccessfully;
            }
            commandResults.Results.Add(winLimitResult);

            return commandResults;
        }

        #endregion Win Limit

        #region Private Methods

        public PtAndComSetting GetPtAndComSetting(SettingContext settingContext)
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

        private IList<ICommandResult> GetPtAnhComResultDetails(ICommandResult commandResult)
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
                        resultDetails.Add(new PtAndComCommandResult
                        {
                            Code = errCode,
                            Message = LiveCasinoLang.ResourceManager.GetString(string.Format("_{0}", Math.Abs(errCode)))
                        });
                    }
                    else
                    {
                        resultDetails.Add(new PtAndComCommandResult
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