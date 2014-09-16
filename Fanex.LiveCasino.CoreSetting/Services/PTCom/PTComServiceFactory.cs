namespace Fanex.LiveCasino.CoreSetting.Services.PTCom
{
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Repositories.PTCom;

    /// <summary>
    /// Factory for get PT COM service.
    /// </summary>
    public class PTComServiceFactory : IPTComServiceFactory
    {
        private IPTComService _ptComService { get; set; }

        private IPTComRepository _ptComRepository { get; set; }

        public PTComServiceFactory(IPTComService ptComService, IPTComRepository ptComRepository)
        {
            _ptComService = ptComService;
            _ptComRepository = ptComRepository;
        }

        /// <summary>
        /// Get Service for handling PT Com setting.
        /// </summary>
        /// <param name="settingContext">Context for getting service.</param>
        /// <returns>PT COM service.</returns>
        public IPTComService GetService(LiveCasinoSettingContext settingContext)
        {
            if (_ptComService != null)
            {
                return _ptComService;
            }

            switch (settingContext.Customer.Level)
            {
                case CustomerLevel.Super:
                    _ptComService = new SuperPTComService(_ptComRepository);
                    break;

                case CustomerLevel.Master:
                    _ptComService = new MasterPTComService(_ptComRepository);
                    break;

                case CustomerLevel.Agent:
                    _ptComService = new AgentPTComService(_ptComRepository);
                    break;

                case CustomerLevel.Member:
                    switch (settingContext.Upline.Level)
                    {
                        case CustomerLevel.Super:
                            _ptComService = new SuperOnMemberPTComService(_ptComRepository);
                            break;

                        case CustomerLevel.Master:
                            _ptComService = new MasterOnMemberPTComService(_ptComRepository);
                            break;

                        default:
                            _ptComService = new AgentOnMemberPTComService(_ptComRepository);
                            break;
                    }
                    break;
            }

            return _ptComService;
        }
    }
}