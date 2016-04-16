using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using Model.Dao;
using Model.EF;
using OnlineShopv2.Common;
using PagedList;

namespace OnlineShopv2.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Admin/Product
        [HasPermission(RoleID = "VIEW_PRODUCT")]
        public ActionResult Index(string searchString,int page = 1, int pageSize=10)
        {
            var dao = new ProductDao();
            var product = dao.ListAllPaging(searchString,page, pageSize);
            ViewBag.SearchString = searchString;
            return View(product as IPagedList<Product>);
        }
        [HttpGet]
        [HasPermission(RoleID = "ADD_PRODUCT")]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [HasPermission(RoleID = "ADD_PRODUCT")]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductDao();
                long id = dao.Insert(product);
                if (id > 0)
                {
                    SetAlert("Thêm mới sản phẩm thành công!", "success");
                    return RedirectToAction("Create", "Product");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm sản phẩm không thành công!!");
                }
            }
            SetViewBag();
            return View();
        }

        [HttpGet]
        [HasPermission(RoleID = "EDIT_PRODUCT")]
        public ActionResult Edit(long id)
        {
            var dao = new ProductDao();
            var product = dao.GetByID(id);
            SetViewBag(product.CategoryID);

            return View(product);
        }

        [HttpPost]
        [HasPermission(RoleID = "EDIT_PRODUCT")]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductDao();
                var result = dao.Update(product);
                if (result)
                {
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ModelState.AddModelError("","Cập nhật sản phẩm thất bại !!!");
                }
            }
            SetViewBag(product.CategoryID);
            return View();
        }
        [HttpDelete]
        [HasPermission(RoleID = "DELETE_PRODUCT")]
        public ActionResult Delete(long id)
        {
            var dao = new ProductDao();
            dao.Delete(id);
            return RedirectToAction("Index");
        }
        public void SetViewBag(long ? selectedId = null)
        {
            var dao = new ProductCategoryDao();
            ViewBag.CategoryID = new SelectList(dao.ListAll(),"ID","Name",selectedId);
        }

        public JsonResult SaveImages(long id,string images)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            var listImages = jss.Deserialize<List<string>>(images);

            XElement xElement = new XElement("Images");
            foreach (var item in listImages)
            {
                int n = item.LastIndexOf('/');
                var subitem = item.Substring(n+1);
                subitem = "/Data/images/" + subitem;
                xElement.Add(new XElement("Image", subitem));
            }
            ProductDao dao = new ProductDao();
            try
            {
                dao.UpdateImages(id, xElement.ToString());
                return Json(new
                {
                    status = true
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    status = false
                });
            }
            
        }

        public JsonResult LoadImages(long id)
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
            }, JsonRequestBehavior.AllowGet);
        }
    }
}