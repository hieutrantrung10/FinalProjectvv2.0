using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShopv2.Common;
using OnlineShopv2.Models;

namespace OnlineShopv2.Controllers
{
    public class PaypalController : Controller
    {
        // GET: Paypal
        public ActionResult Index()
        {
            if (Session[CommonConstants.CartSession] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var mc = Session[CommonConstants.CartSession] as List<CartItem>;
            return View(mc);
        }

        public ActionResult GetDataPaypal()
        {
            return View();
        }
    }
}