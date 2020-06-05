using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WebApplication.entity;
using WebApplication.interfaces;
using WebApplication.Services;

namespace WebApplication
{
    public class AuthenticationHandler : DelegatingHandler
    {
        private iAccessTokenService accessTokenService;
        public AuthenticationHandler()
        {
            this.accessTokenService = new AccessToken_JWT();
        }
        //    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        //    {
        //        var Authorization = request.Headers.Authorization;
        //        if (Authorization != null)
        //        {
        //            string AccessToken = Authorization.Parameter;
        //            string AccessTokenType = Authorization.Scheme;
        //            if (AccessTokenType.Equals("Bearer"))
        //            {
        //                var MemberItem = this.accessTokenService.VerifyAccessToken(AccessToken);
        //                if (MemberItem != null)
        //                {
        //                    var userlogin = new Userlogin(new GenericIdentity(MemberItem.email), MemberItem.role);
        //                    userlogin.Member = MemberItem;
        //                    Thread.CurrentPrincipal = userlogin;
        //                    HttpContext.Current.User = userlogin;
        //                }

        //            }

        //        }
        //        return base.SendAsync(request, cancellationToken);
        //    }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var Authorization = request.Headers.Authorization;
            if (Authorization != null)
            {
                string AccessToken = Authorization.Parameter;
                string AccessTokenType = Authorization.Scheme;
                if (AccessTokenType.Equals("Bearer"))
                {
                    var memberItem = this.accessTokenService.VerifyAccessToken(AccessToken);
                    if (memberItem != null)
                    {
                        var userLogin = new UserLogin(new GenericIdentity(memberItem.email), memberItem.role);
                        userLogin.Member = memberItem;
                        Thread.CurrentPrincipal = userLogin;
                        HttpContext.Current.User = userLogin;
                    }
                }
            }
            return base.SendAsync(request, cancellationToken);
        }
    }


    public class UserLogin : GenericPrincipal
    {
        public member Member { get; set; }
        public UserLogin(IIdentity identity, Roleaccount roles)
            : base(identity, new string[] { roles.ToString() })
        {
        }
    }

}
