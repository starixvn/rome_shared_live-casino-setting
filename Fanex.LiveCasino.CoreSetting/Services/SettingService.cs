// --------------------------------------------------------------------------
// <copyright file="SettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.8.4">Created</change>
// </history>
namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
    using Fanex.NAP.Framework.Data;

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
        /// <param name="custId">Get criteria information.</param>
        /// <returns>Customer information.</returns>
        public CustInfo GetCustomerInfo(int custId)
        {
            var criteria = new GetCustomerInfoCriteria
            {
                Custid = custId
            };
            return _settingRepository.GetCustomerInfo(criteria);
        }

        /// <summary>
        /// Get Representative customer for editing multiple.
        /// </summary>
        /// <param name="custIds">Customer id list for editing, each customer separate by ','.</param>
        /// <param name="custSyncStatuses">Customer sync status list for getting first sync customer.</param>
        /// <returns>Customer id representative.</returns>
        public int GetRepresentativeCustIdForEditingMultiple(string custIds, string custSyncStatuses)
        {
            var custIdList = custIds.Split(new char[] { ',' });
            var custSyncList = custSyncStatuses.Split(new char[] { ',' });
            var firstCustIdNotSync = 0;

            for (int i = 0; i < custIds.Length; i++)
            {
                if (custSyncList[i] == "1")
                {
                    return Converter.ToInt(custIdList[i]);
                }
                else if (firstCustIdNotSync == 0)
                {
                    firstCustIdNotSync = Converter.ToInt(custIdList[i]);
                }
            }

            return firstCustIdNotSync;
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
                    switch ((CustRole)settingContext.RoleId)
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
            IMultipleCommandResults updateResults = new MultipleCommandResults();

            switch ((CustRole)settingContext.TargetRoleId)
            {
                case CustRole.Master:
                    updateResults = UpdateMasterPtAndComSettingMultiple(settingContext, positionTakingAndComSetting);
                    break;

                case CustRole.Agent:
                    updateResults = UpdateAgentPtAndComSettingMultiple(settingContext, positionTakingAndComSetting);
                    break;

                case CustRole.Member:
                    switch ((CustRole)settingContext.RoleId)
                    {
                        case CustRole.Super:
                            updateResults = SuperUpdateMemberPtAndComSettingMultiple(settingContext, positionTakingAndComSetting);
                            break;

                        case CustRole.Master:
                            updateResults = MasterUpdateMemberPtAndComSettingMultiple(settingContext, positionTakingAndComSetting);
                            break;

                        default:
                            updateResults = AgentUpdateMemberPtAndComSettingMultiple(settingContext, positionTakingAndComSetting);
                            break;
                    }

                    break;
            }

            return updateResults;
        }

        public IMultipleCommandResults UpdateMasterPtAndComSettingMultiple(SettingContext settingContext, PtAndComSetting positionTakingAndComSetting)
        {
            ICriteria criteria = GetCriteriaForUpdatingMasterPtAndComSetting(settingContext, positionTakingAndComSetting);
            List<int> custIds = GetCustIds(settingContext.TargetCustIds);
            Dictionary<int, ICommandResult> commandResults = _settingRepository.UpdatePtAndCommSettingMultiple(criteria, custIds);

            return GetPtAndComResultDetailsMultiple(settingContext.TargetCustIds, settingContext.TargetUsernames, commandResults);
        }

        public IMultipleCommandResults UpdateAgentPtAndComSettingMultiple(SettingContext settingContext, PtAndComSetting positionTakingAndComSetting)
        {
            ICriteria criteria = GetCriteriaForUpdatingAgentPtAndComSetting(settingContext, positionTakingAndComSetting);
            List<int> custIds = GetCustIds(settingContext.TargetCustIds);
            Dictionary<int, ICommandResult> commandResults = _settingRepository.UpdatePtAndCommSettingMultiple(criteria, custIds);

            return GetPtAndComResultDetailsMultiple(settingContext.TargetCustIds, settingContext.TargetUsernames, commandResults);
        }

        public IMultipleCommandResults AgentUpdateMemberPtAndComSettingMultiple(SettingContext settingContext, PtAndComSetting positionTakingAndComSetting)
        {
            ICriteria criteria = GetCriteriaForAgentUpdateMemberPtAndComSetting(settingContext, positionTakingAndComSetting);
            List<int> custIds = GetCustIds(settingContext.TargetCustIds);
            Dictionary<int, ICommandResult> commandResults = _settingRepository.UpdatePtAndCommSettingMultiple(criteria, custIds);

            return GetPtAndComResultDetailsMultiple(settingContext.TargetCustIds, settingContext.TargetUsernames, commandResults);
        }

        public IMultipleCommandResults SuperUpdateMemberPtAndComSettingMultiple(SettingContext settingContext, PtAndComSetting positionTakingAndComSetting)
        {
            ICriteria criteria = GetCriteriaForSuperUpdateMemberPtAndComSetting(settingContext, positionTakingAndComSetting);
            List<int> custIds = GetCustIds(settingContext.TargetCustIds);
            Dictionary<int, ICommandResult> commandResults = _settingRepository.UpdatePtAndCommSettingMultiple(criteria, custIds);

            return GetPtAndComResultDetailsMultiple(settingContext.TargetCustIds, settingContext.TargetUsernames, commandResults);
        }

        public IMultipleCommandResults MasterUpdateMemberPtAndComSettingMultiple(SettingContext settingContext, PtAndComSetting positionTakingAndComSetting)
        {
            ICriteria criteria = GetCriteriaForMasterUpdateMemberPtAndComSetting(settingContext, positionTakingAndComSetting);
            List<int> custIds = GetCustIds(settingContext.TargetCustIds);
            Dictionary<int, ICommandResult> commandResults = _settingRepository.UpdatePtAndCommSettingMultiple(criteria, custIds);

            return GetPtAndComResultDetailsMultiple(settingContext.TargetCustIds, settingContext.TargetUsernames, commandResults);
        }

        #endregion Pt and Com Multiple Update

        #endregion Pt and Com

        #region Win Limit

        public CurrentMemberWin GetCurrentWinLimit(int custId)
        {
            return _settingRepository.Get<CurrentMemberWin>(new GetCustomerWinLimitCriteria
            {
                CustId = custId
            });
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
                SRecommend = settingContext.CustId,
                UserId = settingContext.UserIdDoAction,
                SubAccID = settingContext.SubAccId,
                V162discount_1101 = positionTakingAndComSetting.Commission.Value,
                V162mpositiontaking_1101 = (decimal)positionTakingAndComSetting.PositionTaking.M3,
                V162mpositiontaking_1101_min = (decimal)positionTakingAndComSetting.PositionTaking.N3,
                IsAuto_v162spositiontaking_1101 = (bool)positionTakingAndComSetting.PositionTaking.Is3,
                Auto_v162spositiontaking_1101 = (decimal)positionTakingAndComSetting.PositionTaking.Us3,
                UpdateAllMembers = (bool)positionTakingAndComSetting.PositionTaking.UpdateAllMembers,
                V162spositiontaking_1101 = (decimal)positionTakingAndComSetting.PositionTaking.S3,
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
                MRecommend = settingContext.CustId,
                UserId = settingContext.UserIdDoAction,
                SubAccID = settingContext.SubAccId,
                V162discount_1101 = positionTakingAndComSetting.Commission.Value,
                V162mpositiontaking_1101 = (decimal)positionTakingAndComSetting.PositionTaking.M2,
                Min_v162apositiontaking_1101 = (decimal)positionTakingAndComSetting.PositionTaking.N2,
                IsAuto_v162mpositiontaking_1101 = (bool)positionTakingAndComSetting.PositionTaking.Im2,
                Auto_v162mpositiontaking_1101 = (decimal)positionTakingAndComSetting.PositionTaking.Um2,
                UpdateAllMembers = (bool)positionTakingAndComSetting.PositionTaking.UpdateAllMembers,
                V162apositiontaking_1101 = (decimal)positionTakingAndComSetting.PositionTaking.A2,
                NewMPositionTaking = (decimal)positionTakingAndComSetting.PositionTaking.NewPresetOnMember
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
                Recommend = settingContext.CustId,
                UserId = settingContext.UserIdDoAction,
                SubAccID = settingContext.SubAccId,
                V162playerdiscount_1101 = positionTakingAndComSetting.Commission.Value,
                V162apositiontaking_1101 = (decimal)positionTakingAndComSetting.PositionTaking.A1
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
                V162spositiontaking_1101 = (decimal)positionTakingAndComSetting.PositionTaking.S1,
                IsAuto_v162spositiontaking_1101 = (bool)positionTakingAndComSetting.PositionTaking.Is1,
                Auto_v162spositiontaking_1101 = (decimal)positionTakingAndComSetting.PositionTaking.Us1
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
                V162mpositiontaking_1101 = (decimal)positionTakingAndComSetting.PositionTaking.M1,
                IsAuto_v162mpositiontaking_mem_1101 = (bool)positionTakingAndComSetting.PositionTaking.Im1,
                Auto_v162mpositiontaking_mem_1101 = (decimal)positionTakingAndComSetting.PositionTaking.Um1
            };
            return criteria;
        }

        /// <summary>
        /// Get result details information from general result.
        /// </summary>
        /// <param name="custIds">Customer id list for mapping username.</param>
        /// <param name="userNames">User name for showing message information.</param>
        /// <param name="commandResults">General result for separating.</param>
        /// <returns>Result details for showing on web.</returns>
        private IMultipleCommandResults GetPtAndComResultDetailsMultiple(string custIds, string userNames, Dictionary<int, ICommandResult> commandResults)
        {
            IMultipleCommandResults multipleResults = new MultipleCommandResults();
            Dictionary<int, string> userNamesWithCustIds = GetUsernamesWithCustIds(custIds, userNames); 

            foreach (var result in commandResults)
            {
                multipleResults.CommandResultsList.Add(new CommandResults
                {
                    Results = GetPtAndComResultDetails(result.Value).ToList(),
                    CustId = result.Key,
                    Username = userNamesWithCustIds[result.Key]
                });
            }

            return multipleResults;
        }

        private List<int> GetCustIds(string custIds)
        {
            return custIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select<string, int>(cid =>
            {
                return Converter.ToInt(cid);
            }).ToList();
        }

        /// <summary>
        /// Get dictionary map between Customer id and username.
        /// </summary>
        /// <param name="custIds">Customer id list.</param>
        /// <param name="usernames">Username list.</param>
        /// <returns>Dictionary mapped.</returns>
        private Dictionary<int, string> GetUsernamesWithCustIds(string custIds, string usernames)
        {
            Dictionary<int, string> userNamesWithCustIds = new Dictionary<int, string>();

            string[] custIdsList = custIds.Split(new char[] { ',' });
            string[] userNameList = usernames.Split(new char[] { ',' });
            int userNameCount = userNameList.Length;
            var tempUsername = string.Empty;

            for (int i = 0; i < custIdsList.Length; i++)
            {
                tempUsername = i < userNameCount ? userNameList[i] : custIdsList[i];
                userNamesWithCustIds.Add(Converter.ToInt(custIdsList[i]), tempUsername);
            }

            return userNamesWithCustIds;
        }

        #endregion Private Methods
    }
}