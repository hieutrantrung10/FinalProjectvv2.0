﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;
using PagedList;

namespace OnlineShopv2.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        // GET: Admin/Category
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new CategoryDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model as IPagedList<Category>);
        }
        public ActionResult Create()
        {            
            return View();
        }
        [HttpPost]
        public ActionResult Create(Category model)
        {
            if (ModelState.IsValid)
            {
                var session = (UserLogin)Session[Common.CommonConstants.USER_SESSION];
                model.CreatedBy = session.UserName;
                new CategoryDao().Insert(model);
                return RedirectToAction("Index", "Category");
            }            
            return View();
        }

        public ActionResult Edit(long id)
        {
            var dao = new CategoryDao();
            var cate = dao.GetByID(id);
            
            return View(cate);
        }
        [HttpPost]
        public ActionResult Edit(Category cate)
        {
            if (ModelState.IsValid)
            {
                var dao = new CategoryDao();
                //var result = dao.Update(content);
                var result = dao.Update(cate);
                if (result)
                {
                    return RedirectToAction("Index", "Category");
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
            var dao = new CategoryDao();
            dao.Delete(id);
            return RedirectToAction("Index");
        }

    }
}