﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using Model.Dao;

namespace OnlineShopv2.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index(int page = 1, int pageSize = 1)
        {
            //var model = new ProductDao().ListAllPagingClient(page, pageSize);
            //int totalRecord = 0;

            //ViewBag.Total = totalRecord;
            //ViewBag.Page = page;

            //int maxPage = 5;
            //int totalPage = 0;

            //totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
            //ViewBag.TotalPage = totalPage;
            //ViewBag.MaxPage = maxPage;
            //ViewBag.First = 1;
            //ViewBag.Last = totalPage;
            //ViewBag.Next = page + 1;
            //ViewBag.Prev = page - 1;
            var dao = new ProductDao();
            var model = dao.ListAllPagingClient(page, pageSize);
            return View(model);
            //return PartialView(model);
        }

        public PartialViewResult ProductCategory()
        {
            var dao = new ProductCategoryDao().ListAll();
            return PartialView(dao);
        }

        public ActionResult Category(long cateId, int page = 1, int pageSize = 1)
        {
            var category = new CategoryDao().ViewDetail(cateId);
            ViewBag.Category = category;
            int totalRecord = 0;
            var model = new ProductDao().ListByCategoryId(cateId, ref totalRecord, page, pageSize);

            ViewBag.Total = totalRecord;
            ViewBag.Page = page;

            int maxPage = 5;
            int totalPage = 0;

            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;

            return View(model);
        }

        public ActionResult Detail(long id)
        {

            var product = new ProductDao().GetByID(id);
            string xImages = product.MoreImages;
            string newimages = string.Empty;
            if (xImages != null)
            {
                //int n = xImages.LastIndexOf("</Images>");
                //string subImages = xImages.Substring(8, n - 8);
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(xImages);               
                XmlNodeList xList = xDoc.SelectNodes("/Images/Image");
                
                foreach (XmlNode item in xList)
                {
                    newimages += item.InnerText + "#";
                }
            }
            ViewBag.NewImages = newimages;
            ViewBag.Category = new ProductCategoryDao().ViewDetail(product.CategoryID.Value);
            ViewBag.RelatedProducts = new ProductDao().ListRelatedProducts(id);
            return View(product);
        }
        public JsonResult ListName(string q)
        {
            var data = new ProductDao().ListName(q);
            return Json(new
            {
                data = data,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Search(string keyword, int page = 1, int pageSize = 1)
        {
            int totalRecord = 0;
            var model = new ProductDao().Search(keyword, ref totalRecord, page, pageSize);

            ViewBag.Total = totalRecord;
            ViewBag.Page = page;
            ViewBag.Keyword = keyword;
            int maxPage = 5;
            int totalPage = 0;

            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;

            return View(model);
        }

        public JsonResult LoadImage(long id)
        {
            ProductDao dao = new ProductDao();
            var product = dao.GetByID(id);
            var images = product.MoreImages;
            XElement xImages = XElement.Parse(images);
            List<string> listImagesReturn = new List<string>();

            foreach (XElement element in xImages.Elements())
            {
                listImagesReturn.Add(element.Value);
            }
            return Json(new
            {
                data = listImagesReturn
            },JsonRequestBehavior.AllowGet);
        }
    }
}