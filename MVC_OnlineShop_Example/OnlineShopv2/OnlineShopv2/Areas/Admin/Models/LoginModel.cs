using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShopv2.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage="Please insert your account!!!")]
        public string UserName { set; get; }

        [Required(ErrorMessage = "Please insert your password!!!")]
        public string Password { set; get; }

        public bool RememberMe { set; get; }
    }
}