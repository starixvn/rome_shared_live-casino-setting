namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common;
    using Fanex.LiveCasino.CoreSetting.Constant;
    using Fanex.LiveCasino.CoreSetting.Criteria.PtAndCom;
    using Fanex.LiveCasino.CoreSetting.DTO.CommandResult;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Repositories;
    using Fanex.LiveCasino.CoreSetting.Resources;
    using Fanex.NAP.Framework.Data;

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
        /// <param name="customer">Current customer need be set position taking and commission data.</param>
        /// <returns>Position taking and Commission setting.</returns>
        public void SetCustomerPtAndComSetting(
            Customer customer, 
            CustomerRole targetCustRole, 
            int targetCustId, 
            CustomerRole uplineRole, 
            int uplineId)
        {
            switch (targetCustRole)
            {
                case CustomerRole.Super:
                    SetSuperPtAndCom(targetCustId, customer);
                    break;

                case CustomerRole.Master:
                    SetMasterPtAndCom(targetCustId, uplineId, customer);
                    break;

                case CustomerRole.Agent:
                    SetAgentPtAndCom(targetCustId, uplineId, customer);
                    break;

                case CustomerRole.Member:
                    switch (uplineRole)
                    {
                        case CustomerRole.Super:
                            SetMemberPtAndComSettingForSuperEditMember(targetCustId, customer);
                            break;

                        case CustomerRole.Master:
                            SetMemberPtAndComSettingForMasterEditMember(targetCustId, customer);
                            break;

                        default:
                            SetMemberPtAndComSettingForAgentEditMember(targetCustId, uplineId, customer);
                            break;
                    }

                    break;
            }
        }

        #region Pt and Com Single Update

        /// <summary>
        /// Update position taking and Commission setting, depend on setting context,
        /// we will decide which store procedure and function will be called.
        /// </summary>
        /// <param name="settingContext">Setting context of current update setting.</param>
        /// <param name="positionTakingAndComSetting">Position taking and Commission setting for updating.</param>
        /// <returns>Update status and information for showing on web.</returns>
        public ICommandResultsDTO UpdatePtComSetting(SettingContext settingContext, Commission commission, PositionTaking positionTaking)
        {
            ICommandResultsDTO updateResults = new CommandResultsDTO();
            switch ((CustomerRole)settingContext.TargetRole)
            {
                case CustomerRole.Super:
                    updateResults.Results = UpdateSuperPtAndComSetting(settingContext, commission, positionTaking);
                    break;

                case CustomerRole.Master:
                    updateResults.Results = UpdateMasterPtAndComSetting(settingContext, commission, positionTaking);
                    break;

                case CustomerRole.Agent:
                    updateResults.Results = UpdateAgentPtAndComSetting(settingContext, commission, positionTaking);
                    break;

                case CustomerRole.Member:
                    switch ((CustomerRole)settingContext.UplineRole)
                    {
                        case CustomerRole.Super:
                            updateResults.Results = SuperUpdateMemberPtAndComSetting(settingContext, commission, positionTaking);
                            break;

                        case CustomerRole.Master:
                            updateResults.Results = MasterUpdateMemberPtAndComSetting(settingContext, commission, positionTaking);
                            break;

                        default:
                            updateResults.Results = AgentUpdateMemberPtAndComSetting(settingContext, commission, positionTaking);
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
        /// <param name="positionTakingAndComSetting">Position taking and Commission setting for updating.</param>
        /// <returns>Update status and information for showing on web.</returns>
        public IMultipleCommandResultsDTO UpdatePtComSettingMultiple(SettingContext settingContext, Commission commission, PositionTaking positionTaking)
        {
            var custInfoes = GetCustomerInfo(settingContext.TargetCustIds, settingContext.TargetUsernames, settingContext.TargetSyncStatus);

            IMultipleCommandResultsDTO mainResults = new MultipleCommandResultsDTO();

            foreach (var custInfo in custInfoes)
            {
                settingContext.TargetCustId = custInfo.CustId;
                var subResult = UpdatePtComSetting(settingContext, commission, positionTaking);
                subResult.Id = custInfo.CustId;
                subResult.Name = custInfo.UserName;
                mainResults.CommandResultsList.Add(subResult);
            }

            return mainResults;
        }

        #endregion Pt and Com Multiple Update

        #endregion Pt and Com

        private void SetSuperPtAndCom(int superId, Customer customer)
        {
            object[] positionTakingAndCommissionData = _settingRepository.GetSettingForEditSuper(new GetSettingForEditSuperCriteria
                    {
                        CustId = superId
                    });
            SetDataToCustomerPtComSetting(positionTakingAndCommissionData, customer);
        }

        private void SetMasterPtAndCom(int masterId, int superId, Customer customer)
        {
            object[] positionTakingAndCommissionData = _settingRepository.GetSettingForEditMaster(new GetSettingForEditMasterCriteria
                    {
                        CustId = masterId,
                        SRecommend = superId
                    });

            SetDataToCustomerPtComSetting(positionTakingAndCommissionData, customer);
        }

        private void SetAgentPtAndCom(int agentId, int masterId, Customer customer)
        {
            object[] positionTakingAndCommissionData = _settingRepository.GetSettingForEditAgent(new GetSettingForEditAgentCriteria
                    {
                        CustId = agentId,
                        MRecommend = masterId
                    });
            SetDataToCustomerPtComSetting(positionTakingAndCommissionData, customer);
        }

        private void SetMemberPtAndComSettingForAgentEditMember(int memberId, int agentId, Customer customer)
        {
            object[] positionTakingAndCommissionData = _settingRepository.GetSettingForEditMember(new GetSettingForEditMemberCriteria
                            {
                                CustId = memberId,
                                Recommend = agentId
                            });
            SetDataToCustomerPtComSetting(positionTakingAndCommissionData, customer);
        }

        private void SetMemberPtAndComSettingForSuperEditMember(int memberId, Customer customer)
        {
            object[] positionTakingAndCommissionData = _settingRepository.GetSettingForSuperEditMember(new GetSettingForSuperEditMemberCriteria
            {
                CustId = memberId
            });
            SetDataToCustomerPtComSetting(positionTakingAndCommissionData, customer);
        }

        private void SetMemberPtAndComSettingForMasterEditMember(int memberId, Customer customer)
        {
            object[] positionTakingAndCommissionData = _settingRepository.GetSettingForMasterEditMember(
                new GetSettingForMasterEditMemberCriteria
                            {
                                CustId = memberId
                            });

            SetDataToCustomerPtComSetting(positionTakingAndCommissionData, customer);
        }

        /// <summary>
        /// Set position taking and commission setting to customer.
        /// </summary>
        /// <param name="positionTakingAndCommission">Array contains position taking and commission setting.</param>
        /// <param name="customer">Customer need be set position taking and commission data.</param>
        private void SetDataToCustomerPtComSetting(object[] positionTakingAndCommission, Customer customer)
        {
            if (positionTakingAndCommission[0] is PositionTaking)
            {
                customer.PositionTaking = positionTakingAndCommission[0] as PositionTaking;
            }

            if (positionTakingAndCommission[1] is Commission)
            {
                customer.Commission = positionTakingAndCommission[1] as Commission;
            }
        }

        /// <summary>
        /// Update super position taking and Commission setting.
        /// </summary>
        /// <param name="settingContext">Setting context of current update setting.</param>
        /// <param name="positionTakingAndComSetting">Position taking and Commission setting for updating.</param>
        /// <returns>Update status and information for showing on web.</returns>
        private List<ICommandResult> UpdateSuperPtAndComSetting(SettingContext settingContext, Commission commission, PositionTaking positionTaking)
        {
            ICommandResult commandResult = _settingRepository.InsertOrUpdate(new UpdateSuperSettingCriteria
            {
                CustId = settingContext.TargetCustId,
                IP = settingContext.IP,
                Language = settingContext.Language,
                UserId = settingContext.UserIdDoAction,
                SuperCommission = (decimal)commission.Value,
                S4 = (decimal)positionTaking.S4,
                N4 = (decimal)positionTaking.N4,
            });

            return GetPtAndComResultDetails(commandResult);
        }

        private List<ICommandResult> UpdateMasterPtAndComSetting(SettingContext settingContext, Commission commission, PositionTaking positionTaking)
        {
            ICriteria criteria = GetCriteriaForUpdatingMasterPtAndComSetting(settingContext, commission, positionTaking);
            ICommandResult commandResult = _settingRepository.InsertOrUpdate(criteria);

            return GetPtAndComResultDetails(commandResult);
        }

        private List<ICommandResult> UpdateAgentPtAndComSetting(SettingContext settingContext, Commission commission, PositionTaking positionTaking)
        {
            ICriteria criteria = GetCriteriaForUpdatingAgentPtAndComSetting(settingContext, commission, positionTaking);
            ICommandResult commandResult = _settingRepository.InsertOrUpdate(criteria);

            return GetPtAndComResultDetails(commandResult);
        }

        private List<ICommandResult> AgentUpdateMemberPtAndComSetting(SettingContext settingContext, Commission commission, PositionTaking positionTaking)
        {
            ICriteria criteria = GetCriteriaForAgentUpdateMemberPtAndComSetting(settingContext, commission, positionTaking);
            ICommandResult commandResult = _settingRepository.InsertOrUpdate(criteria);

            return GetPtAndComResultDetails(commandResult);
        }

        private List<ICommandResult> SuperUpdateMemberPtAndComSetting(SettingContext settingContext, Commission commission, PositionTaking positionTaking)
        {
            ICriteria criteria = GetCriteriaForSuperUpdateMemberPtAndComSetting(settingContext, commission, positionTaking);
            ICommandResult commandResult = _settingRepository.InsertOrUpdate(criteria);

            return GetPtAndComResultDetails(commandResult);
        }

        private List<ICommandResult> MasterUpdateMemberPtAndComSetting(SettingContext settingContext, Commission commission, PositionTaking positionTaking)
        {
            ICriteria criteria = GetCriteriaForMasterUpdateMemberPtAndComSetting(settingContext, commission, positionTaking);
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
                        resultDetails.Add(new PtAndComCommandResultDTO
                        {
                            Code = errCode,
                            Message = LiveCasinoLang.ResourceManager.GetString(string.Format("_{0}", Math.Abs(errCode)))
                        });
                    }
                    else
                    {
                        resultDetails.Add(new PtAndComCommandResultDTO
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
        /// <param name="positionTakingAndComSetting">Position taking and Commission setting for updating.</param>
        /// <returns>Criteria for update customer.</returns>
        private ICriteria GetCriteriaForUpdatingMasterPtAndComSetting(SettingContext settingContext, Commission commission, PositionTaking positionTaking)
        {
            var criteria = new UpdateMasterSettingCriteria
            {
                CustId = settingContext.TargetCustId,
                IP = settingContext.IP,
                Language = settingContext.Language,
                SRecommend = settingContext.UplineId,
                UserId = settingContext.UserIdDoAction,
                SubAccID = settingContext.SubAccId,
                MasterCommission = (decimal)commission.Value,
                M3 = (decimal)positionTaking.M3,
                N3 = (decimal)positionTaking.N3,
                Is3 = (bool)positionTaking.Is3,
                Us3 = (decimal)positionTaking.Us3,
                UpdateAllMembers = (bool)positionTaking.UpdateAllMembers,
                S3 = (decimal)positionTaking.S3,
                NewSuperPositionTakingOnMember = (decimal)positionTaking.NewPresetOnMember
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
        /// <param name="commission">Commission setting for updating.</param>
        /// <param name="positionTaking">Position taking setting for updating.</param>
        /// <returns>Criteria for update customer.</returns>
        private ICriteria GetCriteriaForUpdatingAgentPtAndComSetting(SettingContext settingContext, Commission commission, PositionTaking positionTaking)
        {
            var criteria = new UpdateAgentSettingCriteria
            {
                CustId = settingContext.TargetCustId,
                IP = settingContext.IP,
                Language = settingContext.Language,
                MRecommend = settingContext.UplineId,
                UserId = settingContext.UserIdDoAction,
                SubAccID = settingContext.SubAccId,
                AgentCommission = (decimal)commission.Value,
                M2 = (decimal)positionTaking.M2,
                N2 = (decimal)positionTaking.N2,
                Im2 = (bool)positionTaking.Im2,
                Um2 = (decimal)positionTaking.Um2,
                UpdateAllMembers = (bool)positionTaking.UpdateAllMembers,
                A2 = (decimal)positionTaking.A2,
                NewMasterPtOnMember = (decimal)positionTaking.NewPresetOnMember
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
        /// <param name="commission">Commission setting for updating.</param>
        /// <param name="positionTaking">Position taking setting for updating.</param>
        /// <returns>Criteria for update customer.</returns>
        private ICriteria GetCriteriaForAgentUpdateMemberPtAndComSetting(SettingContext settingContext, Commission commission, PositionTaking positionTaking)
        {
            ICriteria criteria = new AgentUpdateMemberSettingCriteria
            {
                CustId = settingContext.TargetCustId,
                IP = settingContext.IP,
                Language = settingContext.Language,
                Recommend = settingContext.UplineId,
                UserId = settingContext.UserIdDoAction,
                SubAccID = settingContext.SubAccId,
                MemberCommission = (decimal)commission.Value,
                A1 = (decimal)positionTaking.A1
            };
            return criteria;
        }

        /// <summary>
        /// Get store procedure parameter value for updating customer.
        /// </summary>
        /// <param name="settingContext">Setting context for updating customer.</param>
        /// <param name="commission">Commission setting for updating.</param>
        /// <param name="positionTaking">Position taking setting for updating.</param>
        /// <returns>Criteria for update customer.</returns>
        private ICriteria GetCriteriaForSuperUpdateMemberPtAndComSetting(SettingContext settingContext, Commission commission, PositionTaking positionTaking)
        {
            var criteria = new SuperUpdateMemberSettingCriteria
            {
                CustId = settingContext.TargetCustId,
                IP = settingContext.IP,
                Language = settingContext.Language,
                UserId = settingContext.UserIdDoAction,
                SubAccID = settingContext.SubAccId,
                S1 = (decimal)positionTaking.S1,
                Is1 = (bool)positionTaking.Is1,
                Us1 = (decimal)positionTaking.Us1,
                IsUpdateSuperPt = !(bool)positionTaking.Is3
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
        /// <param name="commission">Commission setting for updating.</param>
        /// <param name="positionTaking">Position taking setting for updating.</param>
        /// <returns>Criteria for update customer.</returns>
        private ICriteria GetCriteriaForMasterUpdateMemberPtAndComSetting(SettingContext settingContext, Commission commission, PositionTaking positionTaking)
        {
            var criteria = new MasterUpdateMemberSettingCriteria
            {
                CustId = settingContext.TargetCustId,
                IP = settingContext.IP,
                Language = settingContext.Language,
                UserId = settingContext.UserIdDoAction,
                SubAccID = settingContext.SubAccId,
                M1 = (decimal)positionTaking.M1,
                Im1 = (bool)positionTaking.Im1,
                Um1 = (decimal)positionTaking.Um1,
                Im2 = (bool)positionTaking.Im2
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