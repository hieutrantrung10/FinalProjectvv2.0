using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Common;
using Model.Dao;
using Model.EF;

namespace OnlineShopv2.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            var contact = new ContactDao().GetActiveContact();
            return View(contact);
        }

        [HttpPost]
        public JsonResult Send(string name, string email, string address, string mobile, string content)
        {
            var feedback = new Feedback();
            feedback.Name = name;
            feedback.Email = email;
            feedback.CreatedDate = DateTime.Now;
            feedback.Phone = mobile;
            feedback.Content = content;
            feedback.Address = address;

            var id = new ContactDao().InsertFeedBack(feedback);

            string mail = System.IO.File.ReadAllText(Server.MapPath("~/assets/client/template/feedback.html"));

            mail = mail.Replace("{{CustomerName}}", name);
            mail = mail.Replace("{{Email}}", email);
            mail = mail.Replace("{{Address}}", address);
            mail = mail.Replace("{{Mobile}}", mobile);
            mail = mail.Replace("{{Content}}", content);

            var toEmailAddress = ConfigurationManager.AppSettings["ToEmailAddress"];
            new MailHelper().SendMail(toEmailAddress,"Phản hồi từ khách hàng",mail);

            if (id > 0)
            {
                return Json(new
                {
                    status = true
                });
                //send mail
            }

            else
                return Json(new
                {
                    status = false
                });


        }
    }
}
