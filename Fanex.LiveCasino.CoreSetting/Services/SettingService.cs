namespace Fanex.LiveCasino.CoreSetting.Services
{
    using Fanex.LiveCasino.CoreSetting.Criteria;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Repositories;
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SettingService : ISettingService
    {
        private readonly ISettingRepository _settingRepository;

        public SettingService() : this(null) { }

        public SettingService(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository ?? new SettingRepository();
        }

        public ICommandResult UpdateLiveCasinoStatus(UpdateLiveCasinoStatusCriteria criteria)
        {
            return _settingRepository.Update(criteria);
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
    }
}