using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.entity;
using WebApplication.interfaces;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class accountServices : iaccountService
    {
        private Database1Entities database = new Database1Entities();

        public bool Login(Login model)
        {
            try
            {
                var memberItem = this.database.members.SingleOrDefault(m=>m.email.Equals(model.email));
                if (memberItem != null)
                {
                    return password_hash.Verify(model.password,memberItem.password);
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex.GetError();
            }
        }

        //ลงทะเบียน
        public void Register(registerModel model)
        {
            try
            {
                this.database.members.Add(new member {
                    firstname = model.firstName,
                    lastname = model.lastname,
                    email = model.email,
                    password = model.password,
                    position = "",
                    image = null,
                    role = Roleaccount.Member,
                    created = DateTime.Now,
                    updated = DateTime.Now
                    
                
                }
                );
                this.database.SaveChanges();
                    //this.datablase.sqlqurey(""); /*คำสั่ง sql */
            }
            catch (Exception ex)
            {
                throw ex.GetError();
            }
        }
    }
}