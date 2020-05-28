using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.entity;

namespace WebApplication.interfaces
{
    interface iAccessTokenService
    {
        string GenerateAccessToken(string email ,int minute=60);
        member VerifyAccessToken(string accessToken);
    }
}