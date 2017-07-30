using System.Web.Security;

namespace Laplace.LiteCOS.Wechat.Infrastructure
{
    public class FormsAuthProvider: IAuthProvider
    {
        #region IAuthProvider 成员

        public bool Authenticate(string username, string password)
        {
            bool result = FormsAuthentication.Authenticate(username, password);
            if (result)
            {
                FormsAuthentication.SetAuthCookie(username, false);
            }
            return result; 
        }

        public bool AuthSuccess()
        {
            return Authenticate("admin", "admin");
        }

        public bool AuthFailed()
        {
            return Authenticate("admin", "admin11");
        }

        #endregion
    }
}