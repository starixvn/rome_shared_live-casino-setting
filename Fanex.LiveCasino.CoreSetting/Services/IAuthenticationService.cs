namespace Fanex.LiveCasino.CoreSetting.Services
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IAuthenticationService // implement authorization filter
    {
        bool IsAuthorized(int custId);
    }
}