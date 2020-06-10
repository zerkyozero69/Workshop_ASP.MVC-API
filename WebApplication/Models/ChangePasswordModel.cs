using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class ChangePasswordModel
    {
        [Required]
        public string old_pass { get;set;}
        [Required]
        public string new_pass { get; set; }
        [Required]
        [Compare("new_pass")]
        public string cnew_pass { get; set; }

    }
}