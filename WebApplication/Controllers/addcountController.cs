using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication.Models;


namespace WebApplication.Controllers
{
    public class addcountController : ApiController
    {
        //การลงทะเบียน
        [Route("api/account/register")]
        public IHttpActionResult post_register([FromBody] registerModel model)
        {
            if (ModelState.IsValid)
            {
                return Json(model);
            }

            return Json(new
            {     ModelStateError = registerModel(ModelState) });
             //   (registerModel.model(ModelState));
            
        }
    }
}
