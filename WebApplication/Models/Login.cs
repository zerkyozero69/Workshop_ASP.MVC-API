using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class Login
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        public bool remember { get; set; }
       //prop tap tap
    }
    public class AccessTokenModel
    {
        public string AccessToken { get; set; }
    }
}