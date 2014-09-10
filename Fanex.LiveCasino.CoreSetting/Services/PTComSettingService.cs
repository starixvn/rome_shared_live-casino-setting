namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.LiveCasino.CoreSetting.Constant;
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.DTO.Command;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Repositories;
    using Fanex.LiveCasino.CoreSetting.Resources;
    using Fanex.NAP.Framework.Data;
    using System.Linq;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PTComSettingService : IPTComSettingService
    {
        private readonly ISettingRepository _settingRepository;

        public PTComSettingService()
            : this(null)
        {
        }

        public PTComSettingService(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository ?? new SettingRepository();
        }

        #region Pt and Com

        /// <summary>
        /// Get Customer Setting for Editing.
        /// </summary>
        /// <param name="settingContext">Current context for getting customer information.</param>
        /// <returns>Position taking and commission setting.</returns>
        public PTComSetting GetPtAndComSetting(SettingContext settingContext)
        {
            switch ((CustomerRole)settingContext.TargetRoleId)
            {
                case CustomerRole.Super:
                    return GetSettingForEditSuper(new GetSettingForEditSuperCriteria
                    {
                        CustId = settingContext.TargetCustId
                    });
                case CustomerRole.Master:
                    return GetSettingForEditMaster(new GetSettingForEditMasterCriteria
                    {
                        CustId = settingContext.TargetCustId,
                        SRecommend = settingContext.UplineId
                    });
                case CustomerRole.Agent:
                    return GetSettingForEditAgent(new GetSettingForEditAgentCriteria
                    {
                        CustId = settingContext.TargetCustId,
                        MRecommend = settingContext.UplineId
                    });
                case CustomerRole.Member:
                    switch ((CustomerRole)settingContext.UplineRoleId)
                    {
                        case CustomerRole.Super:
                            return GetSettingForSuperEditMember(new GetSettingForSuperEditMemberCriteria
                            {
                                CustId = settingContext.TargetCustId
                            });
                        case CustomerRole.Master:
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

        #region Pt and Com Single Update

        /// <summary>
        /// Update position taking and commission setting, depend on setting context,
        /// we will decide which store procedure and function will be called.
        /// </summary>
        /// <param name="settingContext">Setting context of current update setting.</param>
        /// <param name="positionTakingAndComSetting">Position taking and commission setting for updating.</param>
        /// <returns>Update status and information for showing on web.</returns>
        public ICommandResults UpdatePtAndComSetting(SettingContext settingContext, PTComSetting positionTakingAndComSetting)
        {
            ICommandResults updateResults = new CommandResults();
            switch ((CustomerRole)settingContext.TargetRoleId)
            {
                case CustomerRole.Super:
                    updateResults.Results = UpdateSuperPtAndComSetting(settingContext, positionTakingAndComSetting);
                    break;

                case CustomerRole.Master:
                    updateResults.Results = UpdateMasterPtAndComSetting(settingContext, positionTakingAndComSetting);
                    break;

                case CustomerRole.Agent:
                    updateResults.Results = UpdateAgentPtAndComSetting(settingContext, positionTakingAndComSetting);
                    break;

                case CustomerRole.Member:
                    switch ((CustomerRole)settingContext.UplineRoleId)
                    {
                        case CustomerRole.Super:
                            updateResults.Results = SuperUpdateMemberPtAndComSetting(settingContext, positionTakingAndComSetting);
                            break;

                        case CustomerRole.Master:
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

        #endregion Pt and Com Single Update

        #region Pt and Com Multiple Update

        /// <summary>
        /// Update multiple customer.
        /// </summary>
        /// <param name="settingContext">Setting context of current update setting.</param>
        /// <param name="positionTakingAndComSetting">Position taking and commission setting for updating.</param>
        /// <returns>Update status and information for showing on web.</returns>
        public IMultipleCommandResults UpdatePtAndComSettingMultiple(SettingContext settingContext, PTComSetting positionTakingAndComSetting)
        {
            var custInfoes = GetCustomerInfo(settingContext.TargetCustIds, settingContext.TargetUsernames, settingContext.TargetSyncStatus);

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

        private PTComSetting GetSettingForEditSuper(GetSettingForEditSuperCriteria criteria)
        {
            return _settingRepository.GetSettingForEditSuper(criteria);
        }

        private PTComSetting GetSettingForEditMaster(GetSettingForEditMasterCriteria criteria)
        {
            return _settingRepository.GetSettingForEditMaster(criteria);
        }

        private PTComSetting GetSettingForEditAgent(GetSettingForEditAgentCriteria criteria)
        {
            return _settingRepository.GetSettingForEditAgent(criteria);
        }

        private PTComSetting GetSettingForEditMember(GetSettingForEditMemberCriteria criteria)
        {
            return _settingRepository.GetSettingForEditMember(criteria);
        }

        private PTComSetting GetSettingForSuperEditMember(GetSettingForSuperEditMemberCriteria criteria)
        {
            return _settingRepository.GetSettingForSuperEditMember(criteria);
        }

        private PTComSetting GetSettingForMasterEditMember(GetSettingForMasterEditMemberCriteria criteria)
        {
            return _settingRepository.GetSettingForMasterEditMember(criteria);
        }

        /// <summary>
        /// Update super position taking and commission setting.
        /// </summary>
        /// <param name="settingContext">Setting context of current update setting.</param>
        /// <param name="positionTakingAndComSetting">Position taking and commission setting for updating.</param>
        /// <returns>Update status and information for showing on web.</returns>
        private List<ICommandResult> UpdateSuperPtAndComSetting(SettingContext settingContext, PTComSetting positionTakingAndComSetting)
        {
            ICommandResult commandResult = _settingRepository.InsertOrUpdate(new UpdateSuperSettingCriteria
            {
                CustId = settingContext.TargetCustId,
                IP = settingContext.IP,
                Language = settingContext.Language,
                UserId = settingContext.UserIdDoAction,
                SuperCommission = (decimal)positionTakingAndComSetting.Commission.Value,
                S4 = (decimal)positionTakingAndComSetting.PositionTaking.S4,
                N4 = (decimal)positionTakingAndComSetting.PositionTaking.N4,
            });

            return GetPtAndComResultDetails(commandResult);
        }

        private List<ICommandResult> UpdateMasterPtAndComSetting(SettingContext settingContext, PTComSetting positionTakingAndComSetting)
        {
            ICriteria criteria = GetCriteriaForUpdatingMasterPtAndComSetting(settingContext, positionTakingAndComSetting);
            ICommandResult commandResult = _settingRepository.InsertOrUpdate(criteria);

            return GetPtAndComResultDetails(commandResult);
        }

        private List<ICommandResult> UpdateAgentPtAndComSetting(SettingContext settingContext, PTComSetting positionTakingAndComSetting)
        {
            ICriteria criteria = GetCriteriaForUpdatingAgentPtAndComSetting(settingContext, positionTakingAndComSetting);
            ICommandResult commandResult = _settingRepository.InsertOrUpdate(criteria);

            return GetPtAndComResultDetails(commandResult);
        }

        private List<ICommandResult> AgentUpdateMemberPtAndComSetting(SettingContext settingContext, PTComSetting positionTakingAndComSetting)
        {
            ICriteria criteria = GetCriteriaForAgentUpdateMemberPtAndComSetting(settingContext, positionTakingAndComSetting);
            ICommandResult commandResult = _settingRepository.InsertOrUpdate(criteria);

            return GetPtAndComResultDetails(commandResult);
        }

        private List<ICommandResult> SuperUpdateMemberPtAndComSetting(SettingContext settingContext, PTComSetting positionTakingAndComSetting)
        {
            ICriteria criteria = GetCriteriaForSuperUpdateMemberPtAndComSetting(settingContext, positionTakingAndComSetting);
            ICommandResult commandResult = _settingRepository.InsertOrUpdate(criteria);

            return GetPtAndComResultDetails(commandResult);
        }

        private List<ICommandResult> MasterUpdateMemberPtAndComSetting(SettingContext settingContext, PTComSetting positionTakingAndComSetting)
        {
            ICriteria criteria = GetCriteriaForMasterUpdateMemberPtAndComSetting(settingContext, positionTakingAndComSetting);
            ICommandResult commandResult = _settingRepository.InsertOrUpdate(criteria);

            return GetPtAndComResultDetails(commandResult);
        }

        /// <summary>
        /// Get information after updated.
        /// </summary>
        /// <param name="commandResult">General information for analyzing to small message.</param>
        /// <returns>List message for showing on web.</returns>
        private List<ICommandResult> GetPtAndComResultDetails(ICommandResult commandResult)
        {
            List<ICommandResult> resultDetails = new List<ICommandResult>();

            if (commandResult.Code != null
                && Converter.ToString2(commandResult.Code) == ErrCode.InvalidCriteria.ToString())
            {
                resultDetails.Add(commandResult);
                return resultDetails;
            }

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
        private ICriteria GetCriteriaForUpdatingMasterPtAndComSetting(SettingContext settingContext, PTComSetting positionTakingAndComSetting)
        {
            var criteria = new UpdateMasterSettingCriteria
            {
                CustId = settingContext.TargetCustId,
                IP = settingContext.IP,
                Language = settingContext.Language,
                SRecommend = settingContext.UplineId,
                UserId = settingContext.UserIdDoAction,
                SubAccID = settingContext.SubAccId,
                MasterCommission = (decimal)positionTakingAndComSetting.Commission.Value,
                M3 = (decimal)positionTakingAndComSetting.PositionTaking.M3,
                N3 = (decimal)positionTakingAndComSetting.PositionTaking.N3,
                Is3 = (bool)positionTakingAndComSetting.PositionTaking.Is3,
                Us3 = (decimal)positionTakingAndComSetting.PositionTaking.Us3,
                UpdateAllMembers = (bool)positionTakingAndComSetting.PositionTaking.UpdateAllMembers,
                S3 = (decimal)positionTakingAndComSetting.PositionTaking.S3,
                NewSuperPositionTakingOnMember = (decimal)positionTakingAndComSetting.PositionTaking.NewPresetOnMember
            };

            if (criteria.Is3)
            {
                criteria.S3 = 0;
            }
            else
            {
                criteria.Us3 = 0;
            }

            return criteria;
        }

        /// <summary>
        /// Get store procedure parameter value for updating customer.
        /// </summary>
        /// <param name="settingContext">Setting context for updating customer.</param>
        /// <param name="positionTakingAndComSetting">Position taking and commission setting for updating.</param>
        /// <returns>Criteria for update customer.</returns>
        private ICriteria GetCriteriaForUpdatingAgentPtAndComSetting(SettingContext settingContext, PTComSetting positionTakingAndComSetting)
        {
            var criteria = new UpdateAgentSettingCriteria
            {
                CustId = settingContext.TargetCustId,
                IP = settingContext.IP,
                Language = settingContext.Language,
                MRecommend = settingContext.UplineId,
                UserId = settingContext.UserIdDoAction,
                SubAccID = settingContext.SubAccId,
                AgentCommission = (decimal)positionTakingAndComSetting.Commission.Value,
                M2 = (decimal)positionTakingAndComSetting.PositionTaking.M2,
                N2 = (decimal)positionTakingAndComSetting.PositionTaking.N2,
                Im2 = (bool)positionTakingAndComSetting.PositionTaking.Im2,
                Um2 = (decimal)positionTakingAndComSetting.PositionTaking.Um2,
                UpdateAllMembers = (bool)positionTakingAndComSetting.PositionTaking.UpdateAllMembers,
                A2 = (decimal)positionTakingAndComSetting.PositionTaking.A2,
                NewMasterPtOnMember = (decimal)positionTakingAndComSetting.PositionTaking.NewPresetOnMember
            };

            if (criteria.Im2)
            {
                criteria.M2 = 0;
            }
            else
            {
                criteria.Um2 = 0;
            }

            return criteria;
        }

        /// <summary>
        /// Get store procedure parameter value for updating customer.
        /// </summary>
        /// <param name="settingContext">Setting context for updating customer.</param>
        /// <param name="positionTakingAndComSetting">Position taking and commission setting for updating.</param>
        /// <returns>Criteria for update customer.</returns>
        private ICriteria GetCriteriaForAgentUpdateMemberPtAndComSetting(SettingContext settingContext, PTComSetting positionTakingAndComSetting)
        {
            ICriteria criteria = new AgentUpdateMemberSettingCriteria
            {
                CustId = settingContext.TargetCustId,
                IP = settingContext.IP,
                Language = settingContext.Language,
                Recommend = settingContext.UplineId,
                UserId = settingContext.UserIdDoAction,
                SubAccID = settingContext.SubAccId,
                MemberCommission = (decimal)positionTakingAndComSetting.Commission.Value,
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
        private ICriteria GetCriteriaForSuperUpdateMemberPtAndComSetting(SettingContext settingContext, PTComSetting positionTakingAndComSetting)
        {
            var criteria = new SuperUpdateMemberSettingCriteria
            {
                CustId = settingContext.TargetCustId,
                IP = settingContext.IP,
                Language = settingContext.Language,
                UserId = settingContext.UserIdDoAction,
                SubAccID = settingContext.SubAccId,
                S1 = (decimal)positionTakingAndComSetting.PositionTaking.S1,
                Is1 = (bool)positionTakingAndComSetting.PositionTaking.Is1,
                Us1 = (decimal)positionTakingAndComSetting.PositionTaking.Us1,
                IsUpdateSuperPt = !(bool)positionTakingAndComSetting.PositionTaking.Is3
            };

            if (criteria.Is1)
            {
                criteria.S1 = 0;
            }
            else
            {
                criteria.Us1 = 0;
            }

            return criteria;
        }

        /// <summary>
        /// Get store procedure parameter value for updating customer.
        /// </summary>
        /// <param name="settingContext">Setting context for updating customer.</param>
        /// <param name="positionTakingAndComSetting">Position taking and commission setting for updating.</param>
        /// <returns>Criteria for update customer.</returns>
        private ICriteria GetCriteriaForMasterUpdateMemberPtAndComSetting(SettingContext settingContext, PTComSetting positionTakingAndComSetting)
        {
            var criteria = new MasterUpdateMemberSettingCriteria
            {
                CustId = settingContext.TargetCustId,
                IP = settingContext.IP,
                Language = settingContext.Language,
                UserId = settingContext.UserIdDoAction,
                SubAccID = settingContext.SubAccId,
                M1 = (decimal)positionTakingAndComSetting.PositionTaking.M1,
                Im1 = (bool)positionTakingAndComSetting.PositionTaking.Im1,
                Um1 = (decimal)positionTakingAndComSetting.PositionTaking.Um1,
                Im2 = (bool)positionTakingAndComSetting.PositionTaking.Im2
            };

            if (criteria.Im1)
            {
                criteria.M1 = 0;
            }
            else
            {
                criteria.Um1 = 0;
            }

            return criteria;
        }

        private List<Customer> GetCustomerInfo(string custIds, string usernames, string syncStatus)
        {
            var list = new List<Customer>();

            if (string.IsNullOrWhiteSpace(custIds)
                || string.IsNullOrWhiteSpace(usernames)
                || string.IsNullOrWhiteSpace(syncStatus))
            {
                return list;
            }

            string[] arrayCustId = custIds.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] arrayUsername = usernames.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] arraySyncStatus = syncStatus.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (arrayCustId.Length == arrayUsername.Length
                && arrayUsername.Length == arraySyncStatus.Length
                && arrayCustId.Any())
            {
                for (int i = 0; i < arrayCustId.Length; i++)
                {
                    list.Add(new Customer
                    {
                        CustId = Converter.ToInt(arrayCustId[i]),
                        UserName = arrayUsername[i],
                        IsSyncLiveCasino = arraySyncStatus[i] == "1"
                    });
                }
            }

            return list;
        }
    }
}