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
    //jwt token 
    public class AccessToken_JWT : iAccessTokenService
    {
        //    private byte[] secretKey = Encoding.UTF8.GetBytes("C# Anawat Promsorn"); 
        //    //{ 164, 60, 194, 0, 161, 189, 41, 38, 130, 89, 141, 164, 45, 170, 159, 209, 69, 137, 243, 216, 191, 131, 47, 250, 32, 107, 231, 117, 37, 158, 225, 234 }; /*jwt*/
        //    private Database1Entities db = new Database1Entities();

        //    public string GenerateAccessToken(string email, int minute = 60)
        //    {
        //        JWTPayload payload = new JWTPayload();
        //        {
        //            payload.email = email;
        //            payload.exp = DateTime.UtcNow.AddMinutes(minute);
        //        };

        //        return JWT.Encode(payload,this.secretKey, JwsAlgorithm.HS256);
        //    }

        //    public member VerifyAccessToken(string accessToken)
        //    {
        //        try
        //        {
        //            JWTPayload payload = JWT.Decode<JWTPayload>(accessToken, this.secretKey);
        //            if(payload == null) return null;
        //            if (payload.exp < DateTime.UtcNow) return null;
        //      return      this.db.members.SingleOrDefault(item => item.email.Equals(payload.email));
        //        }
        //        catch 
        //        {
        //            return null;
        //        }

        //    }
        //}
        private byte[] secretKey = Encoding.UTF8.GetBytes("C# ASP.NET MEMBER WORKSHOP");
        private Database1Entities db = new Database1Entities();

        public string GenerateAccessToken(string email, int minute = 60)
        {
            JWTPayload payload = new JWTPayload
            {
                email = email,
                exp = DateTime.UtcNow.AddMinutes(minute)
            };
            return JWT.Encode(payload, this.secretKey, JwsAlgorithm.HS256);
        }

        public member VerifyAccessToken(string accessToken)
        {
            try
            {
                JWTPayload payload = JWT.Decode<JWTPayload>(accessToken, this.secretKey);
                if (payload == null) return null;
                if (payload.exp < DateTime.UtcNow) return null;
                return this.db.members.SingleOrDefault(item => item.email.Equals(payload.email));
            }
            catch
            {
                return null;
            }
        }
    }

    public class JWTPayload
    {
        public string email { get; set; }
        public DateTime exp { get; set; }
    }
}