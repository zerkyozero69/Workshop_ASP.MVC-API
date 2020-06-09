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
        public memberData GetMemberdata()
        {
            var userLogin = this.memberService.MembersItem
                .SingleOrDefault(item => item.email.Equals(User.Identity.Name));
            if (userLogin == null) return null;

            return new memberData
            {
                Id = userLogin.Id,
                created = userLogin.created,
                email = userLogin.email,
                firstname = userLogin.firstname,
                lastname = userLogin.lastname,
                imagebyte = userLogin.image,
                image_type=userLogin.imageType,
                position = userLogin.position,
                role = userLogin.role,
                updated = userLogin.updated
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
                try
                {
                    this.memberService.UpdateProfile(User.Identity.Name, model);
                    return Ok(this.GetMemberdata());
                }
                catch(Exception ex )
                {
                    ModelState.AddModelError("Exception", ex.Message);
                }
            }
            return BadRequest(ModelState.GerErrorModelState());

        }

    }
}
