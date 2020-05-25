using System;
using global::Microsoft.AspNet.Identity;
using global::Microsoft.Owin;
using global::Microsoft.Owin.Security.Cookies;
using global::Microsoft.Owin.Security.OAuth;
using global::Owin;

namespace asp_workshop
{
    public partial class Startup
    {
        private static OAuthAuthorizationServerOptions _OAuthOptions;
        private static string _PublicClientId;

        public static OAuthAuthorizationServerOptions OAuthOptions
        {
            get
            {
                return _OAuthOptions;
            }

            private set
            {
                _OAuthOptions = value;
            }
        }

        public static string PublicClientId
        {
            get
            {
                return _PublicClientId;
            }

            private set
            {
                _PublicClientId = value;
            }
        }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Configure the application for OAuth based flow
            // In production mode set AllowInsecureHttp = False
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions()
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = true
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            // Uncomment the following lines to enable logging in with third party login providers
            // app.UseMicrosoftAccountAuthentication(
            // clientId:="",
            // clientSecret:="")

            // app.UseTwitterAuthentication(
            // consumerKey:="",
            // consumerSecret:="")

            // app.UseFacebookAuthentication(
            // appId:="",
            // appSecret:="")

            // app.UseGoogleAuthentication(New GoogleOAuth2AuthenticationOptions() With {
            // .ClientId = "",
            // .ClientSecret = ""})
        }
    }
}