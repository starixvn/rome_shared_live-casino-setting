namespace Fanex.LiveCasino.CoreSetting.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.LiveCasino.CoreSetting.Configuration;
    using Fanex.LiveCasino.CoreSetting.Criteria.GameSetting;
    using Fanex.LiveCasino.CoreSetting.DTO;
    using Fanex.LiveCasino.CoreSetting.DTO.CommandResult;
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Resources;
    using Fanex.Logging;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Setting repository class.
    /// </summary>
    public class SettingRepository : BaseRepository, ISettingRepository
    {

        public UplineCustomerInfoDTO GetUplineCustomerInfo(ICriteria criteria)
        {
            if (!criteria.IsValid())
            {
                throw new Exception(LiveCasinoLang.InvalidCriteriaMsg);
            }

            var uplineCustomerInfo = Get<UplineCustomerInfoDTO>(criteria);

            return uplineCustomerInfo;
        }

        /// <summary>
        /// Gets the customer information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Customer information.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public Customer GetCustomerInfo(ICriteria criteria)
        {
            if (criteria.IsValid())
            {
                try
                {
                    var custInfo = new Customer();
                    using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                    {
                        var parameters = criteria.GetSpParams();
                        using (var reader = objectDb.ExecuteReader(parameters))
                        {
                            if (reader.Read())
                            {
                                custInfo.Id = Converter.To<int>(parameters["custid"]);
                                custInfo.Username = reader.To<string>("username");
                                custInfo.CurrencyId = reader.To<byte>("currency");
                                custInfo.DisableLiveCasino = reader.To<bool>("DisableLiveCasino");
                                custInfo.Fax = reader.To<string>("fax");
                                custInfo.FirstName = reader.To<string>("firstname");
                                custInfo.IsSyncLiveCasino = reader.To<bool>("SyncLiveCasino");
                                custInfo.LastName = reader.To<string>("lastname");
                                custInfo.Mobile = reader.To<string>("mobilephone");
                                custInfo.NickName = reader.To<string>("username2");
                                custInfo.Phone = reader.To<string>("phone");

                                var roleId = reader.To<byte>("roleid");

                                switch (roleId)
                                {
                                    case (byte)CustomerLevel.Super:
                                        custInfo.Level = CustomerLevel.Super;
                                        break;

                                    case (byte)CustomerLevel.Master:
                                        custInfo.Level = CustomerLevel.Master;
                                        break;

                                    case (byte)CustomerLevel.Agent:
                                        custInfo.Level = CustomerLevel.Agent;
                                        break;

                                    default:
                                        custInfo.Level = CustomerLevel.Member;
                                        break;
                                }

                                custInfo.Site = reader.To<string>("site");

                                custInfo.LiveCasinoSetting = new LiveCasinoSetting
                                {
                                    WinLimit = new WinLimit
                                    {
                                        IsAutoReset = reader.To<bool>("isAutoResetLiveCasino"),
                                        Max = reader.To<float>("MaxWinLiveCasino")
                                    }
                                };

                                custInfo.Parent = new Customer
                                {
                                    DisableLiveCasino = reader.To<bool>("UplineDisableLiveCasino"),
                                    LiveCasinoSetting = new LiveCasinoSetting
                                    {
                                        WinLimit = new WinLimit
                                        {
                                            IsAutoReset = reader.To<bool>("UplineIsAutoResetLiveCasino"),
                                            Max = reader.To<float>("UplineMaxWinLiveCasino")
                                        }
                                    }
                                };
                            }
                        }
                    }

                    return custInfo;
                }
                catch (Exception ex)
                {
                    Logger.Log.Error(ex.Message, LiveCasinoSettings.Settings.LogCategory);

                    throw ex;
                }
            }

            return new Customer();
        }

        /// <summary>
        /// Gets the current member win.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Current member win limit.</returns>
        public CurrentMemberWinLimitDTO GetCurrentMemberWin(ICriteria criteria)
        {
            if (criteria.IsValid())
            {
                var currentMemberWin = new CurrentMemberWinLimitDTO();

                try
                {
                    using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                    {
                        var parameters = criteria.GetSpParams();
                        using (var reader = objectDb.ExecuteReader(parameters))
                        {
                            if (reader.Read())
                            {
                                currentMemberWin.Balance = reader.To<float>("Balance");
                                currentMemberWin.MaxWinCredit = reader.To<float>("MaxWinCredit");
                                currentMemberWin.MaxWinLiveCasino = reader.To<float>("MaxWinLiveCasino");
                                currentMemberWin.WinPercent = reader.To<float>("LCSWinPercent");
                            }
                        }
                    }

                    return currentMemberWin;
                }
                catch (Exception ex)
                {
                    Logger.Log.Error(ex.Message);

                    throw ex;
                }
            }

            return new CurrentMemberWinLimitDTO();
        }

        
        /// <summary>
        /// Gets the game setting by customer identifier.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <returns>Game setting of customer.</returns>
        public List<GameType> GetGameSetting(int custId)
        {
            var criteria = new GetGameSettingByCustomerIdCriteria
            {
                CustId = custId
            };

            List<GameType> gameSetting = GetGameSetting(criteria, hasCheckedColumn: true);

            return gameSetting;
        }

        /// <summary>
        /// Gets the game setting by exchange.
        /// </summary>
        /// <param name="exchangeId">The exchange identifier.</param>
        /// <param name="site">The site name.</param>
        /// <returns>Game setting by exchange and site.</returns>
        public List<GameType> GetGameSetting(int exchangeId, string site, bool defaultCheckedValue = false)
        {
            var criteria = new GetGameSettingByExchangeCriteria
            {
                ExchangeId = exchangeId,
                Site = site
            };

            List<GameType> gameSetting = GetGameSetting(criteria, hasCheckedColumn: false, defaultCheckedValue: defaultCheckedValue);

            return gameSetting;
        }

        /// <summary>
        /// Inserts the game setting account.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="gameTypes">The game types.</param>
        /// <returns>Game setting command result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public ICommandResult InsertGameSettingAccount(ICriteria criteria, List<GameType> gameTypes)
        {
            if (criteria.IsValid())
            {
                try
                {
                    var result = new GameSettingCommandResultDTO();
                    GameType current = null;
                    using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                    {
                        using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                        {
                            while (reader.Read())
                            {
                                var itemGameType = new GameType
                                {
                                    Id = reader.ToInt("GameTypeID"),
                                    BetTypeId = reader.ToInt("IBCBetTypeID")
                                };
                                var itemStakeLevel = new StakeLevel
                                {
                                    Range = reader.ToString("StakeLevelName"),
                                    IsChecked = true
                                };
                                if (current == null || current.Id != itemGameType.Id)
                                {
                                    gameTypes.Add(itemGameType);
                                    current = itemGameType;
                                    current.BettingLimit = new List<StakeLevel>();
                                }

                                current.BettingLimit.Add(itemStakeLevel);
                            }
                        }

                        var outParameters = objDb.GetOuputParams();
                        result = new GameSettingCommandResultDTO(GetCommandResult(outParameters));
                    }

                    if (!result.IsSuccessful)
                    {
                        Logger.Log.Info(string.Format("{0}\n{1}", result.Message, criteria.ToString()), LiveCasinoSettings.Settings.LogCategory);
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    Logger.Log.Error(ex.Message);
                    throw ex;
                }
            }

            return new GameSettingCommandResultDTO();
        }

        /// <summary>
        /// Gets the game setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="hasCheckedColumn">If set to <c>true</c> [has checked column].</param>
        /// <returns>List of game types.</returns>
        private List<GameType> GetGameSetting(ICriteria criteria, bool hasCheckedColumn, bool defaultCheckedValue = false)
        {
            if (criteria.IsValid())
            {
                try
                {
                    var gameTypes = new List<GameType>();
                    GameType current = null;

                    using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                    {
                        using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                        {
                            while (reader.Read())
                            {
                                var itemGameType = new GameType
                                {
                                    Id = reader.ToInt("GameTypeID"),
                                    BetTypeId = reader.ToInt("IBCBetTypeID")
                                };

                                var itemStakeLevel = new StakeLevel
                                {
                                    Id = reader.ToInt("StakeLevelID"),
                                    Range = reader.ToString("StakeLevelName"),
                                    IsChecked = hasCheckedColumn ? reader.ToBool("IsChecked") : defaultCheckedValue
                                };

                                if (current == null || current.Id != itemGameType.Id)
                                {
                                    gameTypes.Add(itemGameType);
                                    current = itemGameType;
                                    current.BettingLimit = new List<StakeLevel>();
                                }

                                current.BettingLimit.Add(itemStakeLevel);
                            }
                        }
                    }

                    return gameTypes;
                }
                catch (Exception ex)
                {
                    Logger.Log.Error(ex.Message);
                    throw ex;
                }
            }

            return new List<GameType>();
        }
    }
}