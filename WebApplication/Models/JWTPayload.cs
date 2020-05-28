using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class JWTPayload
    {
        public string email { get; set; }
        public DateTime exp { get; set; }
    }
}