using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Model.Dao;
using Model.EF;
using OnlineShopv2.Common;
using OnlineShopv2.Models;
using System.Web.Script.Serialization;

namespace OnlineShopv2.Controllers
{
    public class CartController : Controller
    {

        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[Common.CommonConstants.CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }

        public ActionResult WishList()
        {
            var wish = Session[Common.CommonConstants.WishSession];
            var lst = new List<CartItem>();
            if (wish != null)
            {
                lst = (List<CartItem>)wish;
            }
            return View(lst);
        }
        public JsonResult DeleteAll()
        {
            Session[Common.CommonConstants.CartSession] = null;
            return Json(new
            {
                status = true
            });
        }

        public JsonResult Delete(long id)
        {
            var sessioncart = (List<CartItem>)Session[Common.CommonConstants.CartSession];
            sessioncart.RemoveAll(x => x.Product.ID == id);
            Session[Common.CommonConstants.CartSession] = sessioncart;
            return Json(new
            {
                status = true
            });
        }
        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[Common.CommonConstants.CartSession];

            foreach (var item in sessionCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.Product.ID == item.Product.ID);
                if (jsonItem != null)
                {
                    item.Quantity = jsonItem.Quantity;
                }
            }
            Session[Common.CommonConstants.CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }
        public ActionResult AddItem(long productId, int quantity)
        {
            var product = new ProductDao().GetByID(productId);
            var cart = Session[Common.CommonConstants.CartSession];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.Product.ID == productId))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.ID == productId)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    //tao moi 1 doi tuong CartItem
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = quantity;
                    list.Add(item);
                }
                Session[Common.CommonConstants.CartSession] = list;
            }
            else
            {
                //tao moi 1 doi tuong CartItem
                var item = new CartItem();
                item.Product = product;
                item.Quantity = quantity;
                var list = new List<CartItem>();
                list.Add(item);
                //Gan vao session
                Session[Common.CommonConstants.CartSession] = list;
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Payment()
        {
            var cart = Session[Common.CommonConstants.CartSession];
            var user = Session[CommonConstants.USER_SESSION];
            if (user == null)
            {
                return RedirectToAction("Login","User");
            }
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }
        [HttpPost]
        public ActionResult Payment(string shipName,string address,string phoneNumber,string email)
        {
            var order = new Order();
            var user = Session[CommonConstants.USER_SESSION];
            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }
            order.CreatedDate = DateTime.Now;
            order.ShipAddress = address;
            order.ShipMobile = phoneNumber;
            order.ShipName = shipName;
            order.ShipEmail = email;      

            try
            {
                var id = new OrderDao().Insert(order);
                var cart = (List<CartItem>)Session[Common.CommonConstants.CartSession];
                var detailDao = new OrderDetailDao();
                decimal total = 0;
                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail();
                    orderDetail.ProductID = item.Product.ID;
                    orderDetail.OrderID = id;
                    orderDetail.Price = item.Product.Price;
                    orderDetail.Quantity = item.Quantity;
                    //bổ sung vào trong bảng SQL về tên sản phẩm và đơn giá sản phẩm 
                    detailDao.Insert(orderDetail);

                    total += (item.Product.Price.GetValueOrDefault(0)*item.Quantity);
                }
                string content = System.IO.File.ReadAllText((Server.MapPath("~/assets/client/template/neworder.html")));

                content = content.Replace("{{CustomerName}}", shipName);
                content = content.Replace("{{OrderID}}", order.ID.ToString());
                content = content.Replace("{{PhoneNumber}}", phoneNumber);
                content = content.Replace("{{Email}}", email);
                content = content.Replace("{{Address}}", address);
                content = content.Replace("{{Total}}", total.ToString("N0"));

                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

                new MailHelper().SendMail(email, "Đơn hàng mới từ HomeSHOP", content);
                new MailHelper().SendMail(toEmail,"Đơn hàng mới từ HomeSHOP",content);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Redirect("/loi-gui-don-hang");
            }
            return Redirect("/hoan-thanh");
            
        }

        ////test
        
        //public ActionResult PaypalPayment()
        //{
        //    var getData = new GetDataPaypal();
        //    var order = getData.InformationOrder(getData.GetPayPalResponse(Request.QueryString["tx"]));
        //    ViewBag.tx = Request.QueryString["tx"];
        //    return View();
        //}
        ////end
        public ActionResult Success()
        {
            return View();
        }

        public ActionResult AddWishList(long productId, int quantity)
        {
            var product = new ProductDao().GetByID(productId);
            var wish = Session[Common.CommonConstants.WishSession];
            if (wish != null)
            {
                var list = (List<CartItem>)wish;
                if (list.Exists(x => x.Product.ID == productId))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.ID == productId)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    //tao moi 1 doi tuong CartItem
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = quantity;
                    list.Add(item);
                }
                Session[Common.CommonConstants.WishSession] = list;
            }
            else
            {
                //tao moi 1 doi tuong CartItem
                var item = new CartItem();
                item.Product = product;
                item.Quantity = quantity;
                var list = new List<CartItem>();
                list.Add(item);
                //Gan vao session
                Session[Common.CommonConstants.WishSession] = list;
            }
            return RedirectToAction("WishList");
        }
        public JsonResult DeleteWishList(long id)
        {
            var sessionwish = (List<CartItem>)Session[Common.CommonConstants.WishSession];
            sessionwish.RemoveAll(x => x.Product.ID == id);
            Session[Common.CommonConstants.WishSession] = sessionwish;
            return Json(new
            {
                status = true
            });
        }
        public JsonResult DeleteAllWishList()
        {
            Session[Common.CommonConstants.WishSession] = null;
            return Json(new
            {
                status = true
            });
        }
        public JsonResult AddToCart(long id)
        {
            var sessionwish = (List<CartItem>)Session[Common.CommonConstants.WishSession];
            var product = new ProductDao().GetByID(id);
            var cart = Session[Common.CommonConstants.CartSession];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.Product.ID == id))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.ID == id)
                        {
                            item.Quantity += 1;
                        }
                    }
                }
                else
                {
                    //tao moi 1 doi tuong CartItem
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = 1;
                    list.Add(item);
                }
                Session[Common.CommonConstants.CartSession] = list;
            }
            else
            {
                //tao moi 1 doi tuong CartItem
                var item = new CartItem();
                item.Product = product;
                item.Quantity = 1;
                var list = new List<CartItem>();
                list.Add(item);
                //Gan vao session
                Session[Common.CommonConstants.CartSession] = list;
            }
            //return RedirectToAction("Index");
            return Json(new
            {
                status = true
            });
        }
    }
}