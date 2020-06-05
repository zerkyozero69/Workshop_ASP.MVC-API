using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication.entity;

namespace WebApplication.Models
{
    public class memberData
    {
        public int Id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }

        public string position { get; set; }
        public byte[] image { get; set; }
        public Roleaccount role { get; set; }
        public System.DateTime created { get; set; }
        public System.DateTime updated { get; set; }
    }
    public class profileModel
    {
        [Required]
        public string firstname { get; set; }
        [Required]
        public string lastname { get; set; }
        [Required]
        public string position { get; set; }
        public string image { get; set; }
    }
}