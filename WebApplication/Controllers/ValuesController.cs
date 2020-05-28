using System;
using System.Collections.Generic;

using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication.entity;

namespace WebApplication.Controllers
{
    public class ValuesController : ApiController
    {
        private Database1Entities _database = new Database1Entities();
        // GET api/values
        public IEnumerable<string> Get()
        {
            var item = new string[] { "hi","hello"};
            //return this._database.members.toList();
            return this._database.members.Select(m =>   m.firstname ).ToList();
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "_database";
        } 
        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
