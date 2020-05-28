using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.entity;
using WebApplication.interfaces;
using WebApplication.Models;
namespace WebApplication.Services
{
    public class AccessToken_DB : iAccessTokenService
    {
        private Database1Entities db = new Database1Entities();
        public string GenerateAccessToken(string email, int minute = 60)
        {
            try
            {
                var memberItem = this.db.members.SingleOrDefault(m=> m.email.Equals(email));
                if (memberItem == null) throw new Exception("Not Found member");
                var AccessTokenCreate = new AccessToken
                {
                    Token = Guid.NewGuid().ToString(),
                    exprise = DateTime.Now.AddMinutes(minute),
                    memberID =memberItem.Id
                };
                this.db.AccessTokens.Add(AccessTokenCreate);
                this.db.SaveChanges();
                return AccessTokenCreate.Token;
                    }
            catch (Exception ex)
            {
                throw ex.GetError();
            }
        }

        public member VerifyAccessToken(string accessToken)
        {
            try
            {
                var AccessTokenItem = this.db.AccessTokens.SingleOrDefault(item => item.Token.Equals(accessToken));
                if (AccessTokenItem == null) return null;
                if (AccessTokenItem.exprise == DateTime.UtcNow) return null;
                return AccessTokenItem.member;
            }
            catch
            {
                return null;
            }
        }
    }
}