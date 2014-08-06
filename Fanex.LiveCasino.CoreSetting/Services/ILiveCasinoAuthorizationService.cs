namespace Fanex.LiveCasino.CoreSetting.Services
{
    using System.Web;
    using System.Web.Mvc;
    using Fanex.LiveCasino.CoreSetting.Models;
    
    /// <summary>
    /// Authentication Service.
    /// </summary>
    public interface ILiveCasinoAuthorizationService
    {
        string UnAuthorizedUrl { get; set; }

        bool IsAuthorizedRead(AuthorizationContext authorizationContext, SettingContext settingContext);

        bool IsAuthorizedHost(AuthorizationContext authorizationContext, SettingContext settingContext);
    }
}