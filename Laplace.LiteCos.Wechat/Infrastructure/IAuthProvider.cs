namespace Laplace.LiteCOS.Wechat.Infrastructure
{
    public interface IAuthProvider
    {
        bool Authenticate(string username, string password);
        /// <summary>
        /// 认证成功
        /// </summary>
        /// <returns></returns>
        bool AuthSuccess();


        /// <summary>
        /// 认证失败
        /// </summary>
        /// <returns></returns>
        bool AuthFailed();


    }
}
