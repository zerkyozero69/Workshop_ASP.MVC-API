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
        GetmemberModel getmamber();
        void UpdateProfile(string email, profileModel model);
        void ChangePassword(string email,ChangePasswordModel model);
    }
}