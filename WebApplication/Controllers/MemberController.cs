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
                image_type = userLogin.imageType,
                position = userLogin.position,
                role = userLogin.role,
                updated = userLogin.updated
            };


            //return Json(new {
            //     islogin = User.Identity.IsAuthenticated,
            //     emaillogin = User.Identity.AuthenticationType
            //});
        }
        [Route("api/member/profile")]
        //บันทึกข้อมูล profile
        public IHttpActionResult PostUpdateProfile([FromBody] profileModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    this.memberService.UpdateProfile(User.Identity.Name, model);
                    return Ok(this.GetMemberdata());
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Exception", ex.Message);
                }
            }
            return BadRequest(ModelState.GerErrorModelState());

        }
        //เปลี่ยนรหัสผ่าน
        /// <summary>
        /// เปบี่นพาสเวริค์
        /// </summary>
        /// <returns></returns>
        [Route("api/member/change-password")]
        public IHttpActionResult PostChangePassword([FromBody] ChangePasswordModel model)
        {
            if (ModelState.IsValid)

            {
                try
                {
                    this.memberService.ChangePassword(User.Identity.Name, model);
                    return Ok(new { Message = "Password have change" });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Exception", ex.Message);
                }
            }
            return BadRequest(ModelState.GerErrorModelState());

        }
        //แสดงรายการ สมาชิกทั้งหมด
        //แสดงบางหน้า
        public GetmemberModel GetMember([FromUri]MemberFilterOption filterOption)
        {
            if (ModelState.IsValid)
            {
                 
                return this.memberService.getmamber(filterOption);
            }
            throw new HttpResponseException(Request.CreateResponse(
                HttpStatusCode.BadRequest,
                new { Message = ModelState.GerErrorModelState() } 
                ));
         //   return options;
        }
        //เพิ่มข้อมูลสมาชิก
        [Route("api/member/generate")]
        public IHttpActionResult postGenerateMember()
        {
            try
            {
                var memberitem = new List<member>();
                var password = password_hash.hash("123456");
                var posittion = new string[] { "Frontend Developer", "Backend Developer" };
                var role = new Roleaccount[] { Roleaccount.Member, Roleaccount.Employee, Roleaccount.Admin };
                var random = new Random();
                for (var index = 1; index <= 97; index++)
                {
                    memberitem.Add(new member
                    {
                        email = $"mail-{index}@mail.com",
                        password = password,
                        firstname = $"firstname {index}",
                        lastname = $"lastname {index}",
                        position = posittion[random.Next(0, 2)],
                        role = role[random.Next(0, 2)],
                        created = DateTime.UtcNow,
                        updated = DateTime.UtcNow

                    });
                }
               var db=     new Database1Entities();
                    db.members.AddRange(memberitem);
                    db.SaveChanges();
                return Json(memberitem);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Exception", ex);
                return BadRequest(ModelState.GerErrorModelState());
            }
        }

    }
}
