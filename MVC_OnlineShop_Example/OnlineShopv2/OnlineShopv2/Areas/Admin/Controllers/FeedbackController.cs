using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;

namespace OnlineShopv2.Areas.Admin.Controllers
{
    public class FeedbackController : BaseController
    {
        // GET: Admin/Feedback
        public ActionResult Index(int page = 1,int pageSize = 10)
        {
            var dao = new FeedbackDao();
            var model = dao.ListAllPaging(page, pageSize);
            return View(model);
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new FeedbackDao().Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult Detail(long id)
        {
            var model = new FeedbackDao().GetById(id);
            return View(model);
        }
    }
}