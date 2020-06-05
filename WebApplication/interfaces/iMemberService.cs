using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.Models;

namespace WebApplication.interfaces
{
   interface iMemberService
    {
        void UpdateProfile(string email,profileModel model);
    }
}