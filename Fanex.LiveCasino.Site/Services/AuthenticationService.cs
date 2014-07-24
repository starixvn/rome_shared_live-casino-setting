using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fanex.LiveCasino.CoreSetting.Services;

namespace Fanex.LiveCasino.Site.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public bool IsAuthorized(int custId)
        {
            return custId != 15482577;
        }
    }
}