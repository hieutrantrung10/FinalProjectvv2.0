using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;

namespace OnlineShopv2.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        // GET: Admin/Order
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new OrderDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        public ActionResult Detail(long id)
        {
            var dao = new OrderDetailDao();
            var model = dao.GetByID(id);
            return View(model);
        }

        public JsonResult ChangeStatus(long id)
        {
            var result = new OrderDao().ChangeStatus(id);
            return Json(new
            {
                data = result
            });
        }

        [HttpDelete]
        public ActionResult Delete(long id)
        {
            var dao = new OrderDao();
            dao.Delete(id);
            return RedirectToAction("Index");
        }

    }
}