using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApplication.entity;
using WebApplication.interfaces;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class MemberService : iMemberService
    {
        private Database1Entities db = new Database1Entities();

        public IEnumerable<member> MembersItem => this.db.members.ToList();

        public void UpdateProfile(string email, profileModel model)
        {
            try
            {
                var memberItem =  this.db.members.SingleOrDefault(item => item.email.Equals(email));
                if (memberItem == null) throw new Exception("No found Member");
                this.db.members.Attach(memberItem);
                memberItem.firstname = model.firstname;
                memberItem.lastname = model.lastname;
                memberItem.position = model.position;
                memberItem.updated= DateTime.UtcNow;

                //ตรวจสอบว่ามีภาพเข้ามาไหม
                if (!string.IsNullOrEmpty(model.image))
                {
                    string[] images = model.image.Split(',');
                    if (images.Length == 2)
                    {
                        if (images[0].IndexOf("image") >= 0)
                        {
                            memberItem.imageType = images[0];
                            memberItem.image = Convert.FromBase64String(images[1]);   //to ไว้แปลงกลับ -- from แปลงจาก
                        }

                        //   var convert = Convert.ToBase64String(memberItem.image);
                    }
                }
                else if(model.image == null)
                {
                    memberItem.imageType = null;
                    memberItem.image = null;
                }

                    this.db.Entry(memberItem).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                throw ex.GetError();
            }
        }

 
    }
}