namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common;
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using Fanex.LiveCasino.CoreSetting.Criteria.WinLimit;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Models.Command;
    using Fanex.LiveCasino.CoreSetting.Repositories;
    using Fanex.LiveCasino.CoreSetting.Resources;
    using Fanex.LiveCasino.CoreSetting.Utilities;
    using Fanex.NAP.Framework.Command;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Setting service for update position taking setting, commission, member win limit.
    /// </summary>
    public class SettingService : ISettingService
    {
        private readonly ISettingRepository _settingRepository;
        private readonly ICommonUtil _commonUtil;

        public SettingService()
            : this(null, null)
        {
        }

        public SettingService(ISettingRepository settingRepository, ICommonUtil commonUtil)
        {
            _settingRepository = settingRepository ?? new SettingRepository();
            _commonUtil = commonUtil ?? new CommonUtil();
        }

        /// <summary>
        /// Get Representative customer for editing multiple.
        /// </summary>
        /// <param name="settingContext">Setting context for getting customer id list, username list, status list.</param>
        /// <returns>Customer id representative.</returns>
        public int GetRepresentativeCustIdForEditingMultiple(SettingContext settingContext)
        {
            var custInfoList = _commonUtil.GetCustomerInfo(settingContext.TargetCustIds, settingContext.TargetUsernames, settingContext.TargetSyncStatus);

            //// Edit multiple direct downline. Load setting info of the first selected downline in customer list
            if (custInfoList.Any() && settingContext.IsEditDirectDownline)
            {
                return Converter.ToInt(custInfoList[0].CustId);
            }

            //// Super/Master multiple member
            var newCustIdList = string.Empty;
            var newUsernameList = string.Empty;
            var newTargetSyncList = string.Empty;
            var firstSyncCustId = 0;

            foreach (var custInfo in custInfoList)
            {
                if (custInfo.IsSyncLiveCasino)
                {
                    if (firstSyncCustId == 0)
                    {
                        firstSyncCustId = custInfo.CustId;
                    }

                    newCustIdList += custInfo.CustId.ToString() + ",";
                    newUsernameList += custInfo.UserName + ",";
                    newTargetSyncList += (custInfo.IsSyncLiveCasino ? "1" : "0") + ",";
                }
            }

            settingContext.TargetCustIds = newCustIdList.TrimEnd(',');
            settingContext.TargetUsernames = newUsernameList.TrimEnd(',');
            settingContext.TargetSyncStatus = newTargetSyncList.TrimEnd(',');

            return firstSyncCustId;
        }

        #region Pt and Com

        /// <summary>
        /// Get Customer Setting for Editing.
        /// </summary>
        /// <param name="settingContext">Current context for getting customer information.</param>
        /// <returns>Position taking and commission setting.</returns>
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
                        SRecommend = settingContext.UplineId
                    });
                case CustRole.Agent:
                    return GetSettingForEditAgent(new GetSettingForEditAgentCriteria
                    {
                        CustId = settingContext.TargetCustId,
                        MRecommend = settingContext.UplineId
                    });
                case CustRole.Member:
                    switch ((CustRole)settingContext.UplineRoleId)
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
                                Recommend = settingContext.UplineId
                            });
                    }
            }

            return null;
        }

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

        #region Pt and Com Single Update

        /// <summary>
        /// Update position taking and commission setting, depend on setting context,
        /// we will decide which store procedure and function will be called.
        /// </summary>
        /// <param name="settingContext">Setting context of current update setting.</param>
        /// <param name="positionTakingAndComSetting">Position taking and commission setting for updating.</param>
        /// <returns>Update status and information for showing on web.</returns>
        public ICommandResults UpdatePtAndComSetting(SettingContext settingContext, PtAndComSetting positionTakingAndComSetting)
        {
            ICommandResults updateResults = new CommandResults();
            switch ((CustRole)settingContext.TargetRoleId)
            {
                case CustRole.Super:
                    updateResults.Results = UpdateSuperPtAndComSetting(settingContext, positionTakingAndComSetting);
                    break;

                case CustRole.Master:
                    updateResults.Results = UpdateMasterPtAndComSetting(settingContext, positionTakingAndComSetting);
                    break;

                case CustRole.Agent:
                    updateResults.Results = UpdateAgentPtAndComSetting(settingContext, positionTakingAndComSetting);
                    break;

                case CustRole.Member:
                    switch ((CustRole)settingContext.UplineRoleId)
                    {
                        case CustRole.Super:
                            updateResults.Results = SuperUpdateMemberPtAndComSetting(settingContext, positionTakingAndComSetting);
                            break;

                        case CustRole.Master:
                            updateResults.Results = MasterUpdateMemberPtAndComSetting(settingContext, positionTakingAndComSetting);
                            break;

                        default:
                            updateResults.Results = AgentUpdateMemberPtAndComSetting(settingContext, positionTakingAndComSetting);
                            break;
                    }

                    break;
            }

            return updateResults;
        }

        /// <summary>
        /// Update super position taking and commission setting.
        /// </summary>
        /// <param name="settingContext">Setting context of current update setting.</param>
        /// <param name="positionTakingAndComSetting">Position taking and commission setting for updating.</param>
        /// <returns>Update status and information for showing on web.</returns>
        public List<ICommandResult> UpdateSuperPtAndComSetting(SettingContext settingContext, PtAndComSetting positionTakingAndComSetting)
        {
            ICommandResult commandResult = _settingRepository.InsertOrUpdate(new UpdateSuperSettingCriteria
            {
                CustId = settingContext.TargetCustId,
                IP = settingContext.IP,
                Language = settingContext.Language,
                UserId = settingContext.UserIdDoAction,
                V162discount_1101 = positionTakingAndComSetting.Commission.Value,
                V162spositiontaking_1101 = (decimal)positionTakingAndComSetting.PositionTaking.S4,
                V162spositiontaking_1101_min = (decimal)positionTakingAndComSetting.PositionTaking.N4,
            });

            return GetPtAndComResultDetails(commandResult);
        }

        public List<ICommandResult> UpdateMasterPtAndComSetting(SettingContext settingContext, PtAndComSetting positionTakingAndComSetting)
        {
            ICriteria criteria = GetCriteriaForUpdatingMasterPtAndComSetting(settingContext, positionTakingAndComSetting);
            ICommandResult commandResult = _settingRepository.InsertOrUpdate(criteria);

            return GetPtAndComResultDetails(commandResult);
        }

        public List<ICommandResult> UpdateAgentPtAndComSetting(SettingContext settingContext, PtAndComSetting positionTakingAndComSetting)
        {
            ICriteria criteria = GetCriteriaForUpdatingAgentPtAndComSetting(settingContext, positionTakingAndComSetting);
            ICommandResult commandResult = _settingRepository.InsertOrUpdate(criteria);

            return GetPtAndComResultDetails(commandResult);
        }

        public List<ICommandResult> AgentUpdateMemberPtAndComSetting(SettingContext settingContext, PtAndComSetting positionTakingAndComSetting)
        {
            ICriteria criteria = GetCriteriaForAgentUpdateMemberPtAndComSetting(settingContext, positionTakingAndComSetting);
            ICommandResult commandResult = _settingRepository.InsertOrUpdate(criteria);

            return GetPtAndComResultDetails(commandResult);
        }

        public List<ICommandResult> SuperUpdateMemberPtAndComSetting(SettingContext settingContext, PtAndComSetting positionTakingAndComSetting)
        {
            ICriteria criteria = GetCriteriaForSuperUpdateMemberPtAndComSetting(settingContext, positionTakingAndComSetting);
            ICommandResult commandResult = _settingRepository.InsertOrUpdate(criteria);

            return GetPtAndComResultDetails(commandResult);
        }

        public List<ICommandResult> MasterUpdateMemberPtAndComSetting(SettingContext settingContext, PtAndComSetting positionTakingAndComSetting)
        {
            ICriteria criteria = GetCriteriaForMasterUpdateMemberPtAndComSetting(settingContext, positionTakingAndComSetting);
            ICommandResult commandResult = _settingRepository.InsertOrUpdate(criteria);

            return GetPtAndComResultDetails(commandResult);
        }

        #endregion Pt and Com Single Update

        #region Pt and Com Multiple Update

        /// <summary>
        /// Update multiple customer.
        /// </summary>
        /// <param name="settingContext">Setting context of current update setting.</param>
        /// <param name="positionTakingAndComSetting">Position taking and commission setting for updating.</param>
        /// <returns>Update status and information for showing on web.</returns>
        public IMultipleCommandResults UpdatePtAndComSettingMultiple(SettingContext settingContext, PtAndComSetting positionTakingAndComSetting)
        {
            var custInfoes = _commonUtil.GetCustomerInfo(settingContext.TargetCustIds, settingContext.TargetUsernames, settingContext.TargetSyncStatus);

            IMultipleCommandResults mainResults = new MultipleCommandResults();

            foreach (var custInfo in custInfoes)
            {
                settingContext.TargetCustId = custInfo.CustId;
                var subResult = UpdatePtAndComSetting(settingContext, positionTakingAndComSetting);
                subResult.Id = custInfo.CustId;
                subResult.Name = custInfo.UserName;
                mainResults.CommandResultsList.Add(subResult);
            }

            return mainResults;
        }

        #endregion Pt and Com Multiple Update

        #endregion Pt and Com

        #region Win Limit

        /// <summary>
        /// Gets the current win limit.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <returns>Current member win limit.</returns>
        public CurrentMemberWin GetCurrentWinLimit(int custId)
        {
            var criteria = new GetCustomerWinLimitCriteria
            {
                CustId = custId
            };

            var currentWinLimit = new CurrentMemberWin();

            if (criteria.IsValid())
            {
                currentWinLimit = _settingRepository.Get<CurrentMemberWin>(criteria);
            }

            return currentWinLimit;
        }

        /// <summary>
        /// Update member win limit.
        /// </summary>
        /// <param name="settingContext">Setting context information.</param>
        /// <param name="memberWinLimit">Member win limit information.</param>
        /// <returns>Result after updating.</returns>
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
            
            var winLimitResult = new WinLimitCommandResult(result);

            if (winLimitResult.IsSuccessful)
            {
                winLimitResult.Message = LiveCasinoLang.UpdatedWinLimitSuccessfully;
            }

            var commandResults = new CommandResults();
            commandResults.Results.Add(winLimitResult);

            return commandResults;
        }

        /// <summary>
        /// Updates the win limit multiple.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="memberWinLimit">The member win limit.</param>
        /// <returns>Update win limit multiple result.</returns>
        public IMultipleCommandResults UpdateWinLimitMultiple(SettingContext settingContext, MemberWinLimit memberWinLimit)
        {
            var results = new MultipleCommandResults();
            List<CustInfo> custInfos = _commonUtil.GetCustomerInfo(settingContext.TargetCustIds, settingContext.TargetUsernames, settingContext.TargetSyncStatus);

            foreach (var custInfo in custInfos)
            {
                settingContext.TargetCustId = custInfo.CustId;
                var updateResult = UpdateWinLimit(settingContext, memberWinLimit);

                updateResult.Id = custInfo.CustId;
                updateResult.Name = custInfo.UserName;

                results.CommandResultsList.Add(updateResult);
            }

            return results;
        }

        #endregion Win Limit

        #region Private Methods

        /// <summary>
        /// Get information after updated.
        /// </summary>
        /// <param name="commandResult">General information for analyzing to small message.</param>
        /// <returns>List message for showing on web.</returns>
        private List<ICommandResult> GetPtAndComResultDetails(ICommandResult commandResult)
        {
            List<ICommandResult> resultDetails = new List<ICommandResult>();

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

        /// <summary>
        /// Get store procedure parameter value for updating customer.
        /// </summary>
        /// <param name="settingContext">Setting context for updating customer.</param>
        /// <param name="positionTakingAndComSetting">Position taking and commission setting for updating.</param>
        /// <returns>Criteria for update customer.</returns>
        private ICriteria GetCriteriaForUpdatingMasterPtAndComSetting(SettingContext settingContext, PtAndComSetting positionTakingAndComSetting)
        {
            return new UpdateMasterSettingCriteria
            {
                Custid = settingContext.TargetCustId,
                IP = settingContext.IP,
                Language = settingContext.Language,
                SRecommend = settingContext.UplineId,
                UserId = settingContext.UserIdDoAction,
                SubAccID = settingContext.SubAccId,
                MasterCommission = positionTakingAndComSetting.Commission.Value,
                M3 = (decimal)positionTakingAndComSetting.PositionTaking.M3,
                N3 = (decimal)positionTakingAndComSetting.PositionTaking.N3,
                Is3 = (bool)positionTakingAndComSetting.PositionTaking.Is3,
                Us3 = (decimal)positionTakingAndComSetting.PositionTaking.Us3,
                UpdateAllMembers = (bool)positionTakingAndComSetting.PositionTaking.UpdateAllMembers,
                S3 = (decimal)positionTakingAndComSetting.PositionTaking.S3,
                NewSPositionTaking = (decimal)positionTakingAndComSetting.PositionTaking.NewPresetOnMember
            };
        }

        /// <summary>
        /// Get store procedure parameter value for updating customer.
        /// </summary>
        /// <param name="settingContext">Setting context for updating customer.</param>
        /// <param name="positionTakingAndComSetting">Position taking and commission setting for updating.</param>
        /// <returns>Criteria for update customer.</returns>
        private ICriteria GetCriteriaForUpdatingAgentPtAndComSetting(SettingContext settingContext, PtAndComSetting positionTakingAndComSetting)
        {
            ICriteria criteria = new UpdateAgentSettingCriteria
            {
                CustId = settingContext.TargetCustId,
                IP = settingContext.IP,
                Language = settingContext.Language,
                MRecommend = settingContext.UplineId,
                UserId = settingContext.UserIdDoAction,
                SubAccID = settingContext.SubAccId,
                AgentCommission = positionTakingAndComSetting.Commission.Value,
                M2 = (decimal)positionTakingAndComSetting.PositionTaking.M2,
                N2 = (decimal)positionTakingAndComSetting.PositionTaking.N2,
                Im2 = (bool)positionTakingAndComSetting.PositionTaking.Im2,
                Um2 = (decimal)positionTakingAndComSetting.PositionTaking.Um2,
                UpdateAllMembers = (bool)positionTakingAndComSetting.PositionTaking.UpdateAllMembers,
                A2 = (decimal)positionTakingAndComSetting.PositionTaking.A2,
                NewMasterPtOnMember = (decimal)positionTakingAndComSetting.PositionTaking.NewPresetOnMember
            };
            return criteria;
        }

        /// <summary>
        /// Get store procedure parameter value for updating customer.
        /// </summary>
        /// <param name="settingContext">Setting context for updating customer.</param>
        /// <param name="positionTakingAndComSetting">Position taking and commission setting for updating.</param>
        /// <returns>Criteria for update customer.</returns>
        private ICriteria GetCriteriaForAgentUpdateMemberPtAndComSetting(SettingContext settingContext, PtAndComSetting positionTakingAndComSetting)
        {
            ICriteria criteria = new AgentUpdateMemberSettingCriteria
            {
                CustId = settingContext.TargetCustId,
                IP = settingContext.IP,
                Language = settingContext.Language,
                Recommend = settingContext.UplineId,
                UserId = settingContext.UserIdDoAction,
                SubAccID = settingContext.SubAccId,
                MemberCommission = positionTakingAndComSetting.Commission.Value,
                A1 = (decimal)positionTakingAndComSetting.PositionTaking.A1
            };
            return criteria;
        }

        /// <summary>
        /// Get store procedure parameter value for updating customer.
        /// </summary>
        /// <param name="settingContext">Setting context for updating customer.</param>
        /// <param name="positionTakingAndComSetting">Position taking and commission setting for updating.</param>
        /// <returns>Criteria for update customer.</returns>
        private ICriteria GetCriteriaForSuperUpdateMemberPtAndComSetting(SettingContext settingContext, PtAndComSetting positionTakingAndComSetting)
        {
            ICriteria criteria = new SuperUpdateMemberSettingCriteria
            {
                CustId = settingContext.TargetCustId,
                IP = settingContext.IP,
                Language = settingContext.Language,
                UserId = settingContext.UserIdDoAction,
                SubAccID = settingContext.SubAccId,
                S1 = (decimal)positionTakingAndComSetting.PositionTaking.S1,
                Is1 = (bool)positionTakingAndComSetting.PositionTaking.Is1,
                Us1 = (decimal)positionTakingAndComSetting.PositionTaking.Us1
            };
            return criteria;
        }

        /// <summary>
        /// Get store procedure parameter value for updating customer.
        /// </summary>
        /// <param name="settingContext">Setting context for updating customer.</param>
        /// <param name="positionTakingAndComSetting">Position taking and commission setting for updating.</param>
        /// <returns>Criteria for update customer.</returns>
        private ICriteria GetCriteriaForMasterUpdateMemberPtAndComSetting(SettingContext settingContext, PtAndComSetting positionTakingAndComSetting)
        {
            ICriteria criteria = new MasterUpdateMemberSettingCriteria
            {
                CustId = settingContext.TargetCustId,
                IP = settingContext.IP,
                Language = settingContext.Language,
                UserId = settingContext.UserIdDoAction,
                SubAccID = settingContext.SubAccId,
                M1 = (decimal)positionTakingAndComSetting.PositionTaking.M1,
                Im1 = (bool)positionTakingAndComSetting.PositionTaking.Im1,
                Um1 = (decimal)positionTakingAndComSetting.PositionTaking.Um1
            };
            return criteria;
        }

        #endregion Private Methods
    }
}