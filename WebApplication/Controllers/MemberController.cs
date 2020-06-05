using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication;
using WebApplication.entity;
using WebApplication.interfaces;
using WebApplication.Models;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [Authorize]
    public class MemberController : ApiController
    {
        private iMemberService memberService;
        private MemberController()
        {
            this.memberService = new MemberService();
        }
 
        [Route("api/member/data")]
        //แสดงชื่อผู้ใช้ที่ login เข้ามา
        public member GetMemberdata()
        {
            var userLogin = User as UserLogin;
            return new member
            {
                Id = userLogin.Member.Id,
                created = userLogin.Member.created,
                email = userLogin.Member.email,
                firstname = userLogin.Member.firstname,
                lastname =  userLogin.Member.lastname,
                 image = userLogin.Member.image,
                position = userLogin.Member.position,
                role = userLogin.Member.role,
                updated = userLogin.Member.updated
            };

            //return Json(new {
            //     islogin = User.Identity.IsAuthenticated,
            //     emaillogin = User.Identity.AuthenticationType
            //});
        }
        [Route ("api/member/profile")]
        //บันทึกข้อมูล profile
        public IHttpActionResult PostUpdateProfile([FromBody] profileModel model)
        {
            if(ModelState.IsValid)
            {
                return Json(model);
            }
            return BadRequest(ModelState.GerErrorModelState());

        }

    }
}
