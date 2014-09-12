namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System.Web.Mvc;
    using Fanex.LiveCasino.CoreSetting.Models;

    /// <summary>
    /// Authentication Service.
    /// </summary>
    public interface ILiveCasinoAuthorizationService
    {
        bool IsAuthorizedRead(AuthorizationContext authorizationContext, LiveCasinoSettingContext settingContext);

        bool IsAuthorizedHost(AuthorizationContext authorizationContext, LiveCasinoSettingContext settingContext);
    }
}