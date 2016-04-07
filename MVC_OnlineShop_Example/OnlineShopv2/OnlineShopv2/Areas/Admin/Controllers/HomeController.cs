using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShopv2.Common;

namespace OnlineShopv2.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Admin/Home
        [HasPermission(RoleID = "HOME")]
        public ActionResult Index()
        {
            return View();
        }
    }
}