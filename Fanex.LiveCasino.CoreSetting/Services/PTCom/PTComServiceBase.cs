namespace Fanex.LiveCasino.CoreSetting.Services.PTCom
{
    using System;
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.LiveCasino.CoreSetting.Constant;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.DTO.CommandResult;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Repositories.PTCom;
    using Fanex.LiveCasino.CoreSetting.Resources;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class PTComServiceBase : IPTComService
    {
        public float Commission { get; set; }

        protected PositionTaking PositionTaking { get; set; }

        protected IPTComRepository PTComRepository { get; set; }

        public virtual void SetModifiedData(float commission, PositionTaking positionTaking, bool isApplyPositionTakingOnMembers = false, float newPositionTakingOnMembers = 0)
        {
            Commission = commission;
            PositionTaking = positionTaking;
        }

        public abstract void SetPTComSetting(Customer customer, LiveCasinoSettingContext settingContext);

        public abstract ICommandResults Update(LiveCasinoSettingContext settingContext);

        public virtual IMultipleCommandResults UpdateMultiple(List<Customer> customers, LiveCasinoSettingContext settingContext)
        {
            IMultipleCommandResults mainResults = new MultipleCommandResults();

            foreach (var customer in customers)
            {
                settingContext.Customer = customer;
                var subResult = Update(settingContext);
                subResult.Id = customer.Id;
                subResult.Name = customer.Username;
                mainResults.CommandResultsList.Add(subResult);
            }

            return mainResults;
        }

        /// <summary>
        /// Get result detail by error code combined.
        /// </summary>
        /// <param name="commandResult">Result contains error code combined.</param>
        /// <returns>Command result list.</returns>
        protected ICommandResults GetResultDetails(ICommandResult commandResult)
        {
            CommandResults resultDetails = new CommandResults();

            if (commandResult.Code != null
                && Converter.ToString2(commandResult.Code) == ErrCode.InvalidCriteria.ToString())
            {
                resultDetails.Results.Add(commandResult);
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
                        resultDetails.Results.Add(new PtAndComCommandResult
                        {
                            Code = errCode,
                            Message = LiveCasinoLang.ResourceManager.GetString(string.Format("_{0}", Math.Abs(errCode)))
                        });
                    }
                    else
                    {
                        resultDetails.Results.Add(new PtAndComCommandResult
                        {
                            Code = errCode,
                            Message = errMsgs[i]
                        });
                    }
                }
            }

            return resultDetails;
        }

        protected virtual void MapSetting(PTComDTO ptComDTO, Customer customer)
        {
            customer.LiveCasinoSetting.PositionTaking = ptComDTO.PositionTaking;
            customer.LiveCasinoSetting.Commission = ptComDTO.Commission;
            customer.Parent.LiveCasinoSetting.Commission = ptComDTO.ParentCommission;
        }
    }
}