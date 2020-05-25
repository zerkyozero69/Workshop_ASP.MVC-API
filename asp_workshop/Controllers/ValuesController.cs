using System.Collections.Generic;
using global::System.Web.Http;

namespace asp_workshop
{
    [System.Web.Http.Authorize]
    public class ValuesController : ApiController
    {

        // GET api/values
        public IEnumerable<string> GetValues()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string GetValue(int id)
        {
            return "value";
        }

        // POST api/values
        public void PostValue([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void PutValue(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void DeleteValue(int id)
        {
        }
    }
}