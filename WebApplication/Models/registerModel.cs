using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;

namespace WebApplication.Models
{
    public class registerModel
    {
        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastname { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
     //  [RegularExpression("")] //ใช้เช็คพาสเวริค์
        public string password { get; set; }
        [Required]
        [Compare("password",ErrorMessage ="รหัสผ่านต้องตรงกัน")]
        public string cpassword { get; set; }

        /// <summary>
        /// ปรับแต่ง error
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static object GerErrorModelState(ModelStateDictionary modelState)
        {
            var modelValue = modelState.Values;
        }
      
    }
}