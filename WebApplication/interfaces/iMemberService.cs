using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.entity;
using WebApplication.Models;

namespace WebApplication.interfaces
{
    
   interface iMemberService
    {
        IEnumerable<member> MembersItem { get; }
        void UpdateProfile(string email,profileModel model);
    }
}