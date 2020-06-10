using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.entity;

namespace WebApplication.Models
{
    public class GetmemberModel
    {
        public getmamber[] items { get; set; }
        public int TotalItems { get; set; }
    }
    public class getmamber
    {
        public int Id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string position { get; set; }
        public Roleaccount role { get; set; }
        public System.DateTime updated { get; set; }
    }
}