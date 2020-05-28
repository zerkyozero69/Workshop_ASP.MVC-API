using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;

namespace WebApplication.Models
{
    public static class extensionModel
    {

        /// <summary>
        /// ปรับแต่ง error
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static string GerErrorModelState(this ModelStateDictionary modelState)
        {
            var modelValue = modelState.Values.Select(value => value.Errors).FirstOrDefault();
            if (modelValue == null)
            {
                return null;
            }
            return modelValue[0].ErrorMessage;
        }
        //ปรับแต่ง ค่า error exception
        public static Exception GetError(this Exception exception)
        {
            if (exception.InnerException != null)
             return exception.InnerException.GetError(); 
            return exception;
        }

    }
}