using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication.interfaces;
using WebApplication.Models;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    public class addcountController : ApiController
    {
        private iaccountService account;
        private iAccessTokenService accesstoken;

        public addcountController()
        {
            this.account = new accountServices();
            this.accesstoken = new AccessToken_JWT();
        }

        //การลงทะเบียน
        [Route("api/account/register")]
        public IHttpActionResult post_register([FromBody] registerModel model)
        {
            if (ModelState.IsValid)
            {
                //   model.password = password_hash.hash(model.password);

                //return Json( new { model,
                //    passwordlength = model.password.Length,
                //verify = password_hash.Verify("123456",model.password)}
                //); / ใช้เทียบรหัสผ่าน/
                try
                {
                    model.password = password_hash.hash(model.password);
                    this.account.Register(model);
                    return Ok("Succesful");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Exception", ex.Message);
                }
                // return Json(model);

            }

            //return Json(new
            //{     ModelStateError = registerModel.GerErrorModelState(ModelState) });
            //   (registerModel.model(ModelState));
            return BadRequest(ModelState.GerErrorModelState());

        }
        //login เข้าสู่ระบบ
        [Route("api/account/login")]
        [HttpPost]
        public AccessTokenModel PostLogin([FromBody] Login model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (this.account.Login(model))
                    {
                        return new AccessTokenModel


                        { AccessToken =this.accesstoken.GenerateAccessToken(model.email)    };

                        throw new Exception("Email or Password worng");

                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Exception", ex.Message);
                }
                //   return Json(this.account.Login(model));
            }

            //      return Json("Login Page")
            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest,
            new { Message = ModelState.GerErrorModelState() }
                ));
            //  return BadRequest(ModelState.GerErrorModelState());

        }
    }
}
