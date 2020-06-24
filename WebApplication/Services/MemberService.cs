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

        //ข้อมูลสมาชิก
        public IEnumerable<member> MembersItem => this.db.members.ToList();
        //เปลี่ยนรหัสผ่าน
        public void ChangePassword(string email, ChangePasswordModel  model )
        {
            try
            {
                var memberitem = this.db.members.SingleOrDefault(item => item.email.Equals(email));
                    if (memberitem == null)  throw new Exception("not found member");
                if (!password_hash.Verify(model.old_pass, memberitem.password) )
                    throw new Exception(" The Oid Password is  invalid");
                this.db.members.Attach(memberitem);
                memberitem.password = password_hash.hash(model.new_pass);
                memberitem.updated = DateTime.Now;
                this.db.Entry(memberitem).State = System.Data.Entity.EntityState.Modified;
                this.db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex.GetError();
            }
        }
        //แสดงรายการสมาชิก
        public GetmemberModel getmamber(MemberFilterOption filterOption)
            {
                try
                {
                    var item = this.MembersItem.Select(m => new getmamber
                    {
                        Id = m.Id,
                        email = m.email,
                        firstname = m.firstname,
                        lastname = m.lastname,
                        position = m.position,
                        role = m.role,
                        updated = m.updated
                    }
                       ).ToArray();
                    var totalItem = item.Length;
                var  memberitems     = new GetmemberModel
                    {
                        items = item.Skip((filterOption.startPage - 1) *filterOption.limitPage)
                        .Take(filterOption.limitPage)
                        .ToArray(),
                        TotalItems =item.Count()
                    };
                return memberitems;
                }
                catch (Exception ex)
                {
                    throw ex.GetError();
                }

            }


        //อัพเดทโปรไฟล์ส่วนตัว
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