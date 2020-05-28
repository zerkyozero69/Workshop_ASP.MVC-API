using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication.Controllers
{
    public class MemberController : ApiController
    {
        [Route("api/member/data")]
        public IHttpActionResult GetMemberdata()
        {
            
            return Json(new {
                 islogin = User.Identity.IsAuthenticated,
                 emaillogin = User.Identity.Name
            });
        }
    }
}
