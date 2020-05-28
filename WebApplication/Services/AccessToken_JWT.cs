using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.interfaces;
using Jose;
using WebApplication.Models;
using System.Text;
using WebApplication.entity;

namespace WebApplication.Services
{
    public class AccessToken_JWT : iAccessTokenService
    {
        private byte[] secretKey = Encoding.UTF8.GetBytes("C# Anawat Promsorn");
            //{ 164, 60, 194, 0, 161, 189, 41, 38, 130, 89, 141, 164, 45, 170, 159, 209, 69, 137, 243, 216, 191, 131, 47, 250, 32, 107, 231, 117, 37, 158, 225, 234 }; /*jwt*/
        public string GenerateAccessToken(string email, int minute = 60)
        {
            JWTPayload payload = new JWTPayload ( );
            payload.email = email;
            payload.exp = DateTime.UtcNow.AddMinutes(minute);

            return JWT.Encode(payload,secretKey, JwsAlgorithm.HS256);
        }

        public member VerifyAccessToken(string accessToken)
        {
            throw new NotImplementedException();
        }
    }
   
}