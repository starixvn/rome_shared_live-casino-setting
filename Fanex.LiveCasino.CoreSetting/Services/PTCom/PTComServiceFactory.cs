namespace Fanex.LiveCasino.CoreSetting.Services.PTCom
{
    using Fanex.LiveCasino.CoreSetting.Enumerations;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Repositories.PTCom;

    /// <summary>
    /// TODO: Update summary.
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

        public IPTComService GetService(LiveCasinoSettingContext settingContext)
        {
            if (_ptComService != null)
            {
                return _ptComService;
            }

            switch (settingContext.Customer.Level)
            {
                case CustomerLevel.Super:
                    return new SuperPTComService(_ptComRepository);

                case CustomerLevel.Master:
                    return new MasterPTComService(_ptComRepository);

                case CustomerLevel.Agent:
                    return new AgentPTComService(_ptComRepository);

                case CustomerLevel.Member:
                    switch (settingContext.Upline.Level)
                    {
                        case CustomerLevel.Super:
                            return new SuperOnMemberPTComService(_ptComRepository);

                        case CustomerLevel.Master:
                            return new MasterOnMemberPTComService(_ptComRepository);

                        default:
                            return new AgentOnMemberPTComService(_ptComRepository);
                    }
                default:
                    return _ptComService;
            }
        }
    }
}