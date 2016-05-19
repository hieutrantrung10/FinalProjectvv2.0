using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;

namespace OnlineShopv2.Areas.Admin.Controllers
{
    public class ProductCategoryController : Controller
    {
        // GET: Admin/ProductCategory
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ProductCategoryDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(ProductCategory model)
        {
            if (ModelState.IsValid)
            {
                var session = (UserLogin)Session[Common.CommonConstants.USER_SESSION];
                model.CreatedBy = session.UserName;
                new ProductCategoryDao().Insert(model);
                return RedirectToAction("Index", "ProductCategory");
            }
            return View();
        }

        public ActionResult Edit(long id)
        {
            var dao = new ProductCategoryDao();
            var cate = dao.ViewDetail(id);

            return View(cate);
        }
        [HttpPost]
        public ActionResult Edit(ProductCategory cate)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductCategoryDao();
                //var result = dao.Update(content);
                var result = dao.Update(cate);
                if (result)
                {
                    return RedirectToAction("Index", "ProductCategory");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật danh mục thất bại !!!");
                }
            }
            return View();
        }
        [HttpDelete]
        public ActionResult Delete(long id)
        {
            var dao = new ProductCategoryDao();
            dao.Delete(id);
            return RedirectToAction("Index");
        }
    }
}