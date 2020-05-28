using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WebApplication.interfaces;
using WebApplication.Services;

namespace WebApplication.App_Start
{
    public class AuthenticationHandler : DelegatingHandler
    {
        private iAccessTokenService accessTokenService;
        public AuthenticationHandler()
        {
            this.accessTokenService = new AccessToken_DB();
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var Authorization = request.Headers.Authorization;
            if (Authorization != null)
            {
                string AccessToken = Authorization.Parameter;
                string AccessTokenType = Authorization.Scheme;
                if (AccessTokenType.Equals("Bearer"))
                {
                    var UserLogin = this.accessTokenService.VerifyAccessToken(AccessToken);
                 
                }
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}