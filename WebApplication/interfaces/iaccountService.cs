using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.interfaces
{
    interface iaccountService
    {
        void Register(registerModel model);
        bool Login(Login model);
    }
}
