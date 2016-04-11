using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using BotDetect.Web.UI.Mvc;
using CKFinder.Connector;
using Common;
using Facebook;
using Microsoft.Owin.Security.OAuth;
using Model.Dao;
using Model.EF;
using OnlineShopv2.Common;
using OnlineShopv2.Models;

namespace OnlineShopv2.Controllers
{
    public class UserController : Controller
    {
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        #region REGISTER AND VERIFY
        // GET: User
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [CaptchaValidation("CaptchaCode", "registerCaptcha", "Mã Captcha không đúng!")]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (dao.CheckUserName(model.UserName))
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                }
                else if (dao.CheckEmail(model.Email))
                {
                    ModelState.AddModelError("", "Email đã được sử dụng");
                }
                else
                {
                    var user = new User();
                    user.UserName = model.UserName;
                    user.Name = model.Name;
                    var encryptedMd5Pas = Encryptor.MD5Hash(model.Password);
                    user.Password = encryptedMd5Pas;
                    user.Phone = model.Phone;
                    user.Email = model.Email;
                    user.Address = model.Address;
                    user.CreatedDate = DateTime.Now;
                    user.Status = false;
                    if (!string.IsNullOrEmpty(model.ProvinceID))
                    {
                        user.ProvinceID = int.Parse(model.ProvinceID);
                    }
                    if (!string.IsNullOrEmpty(model.DistrictID))
                    {
                        user.DistrictID = int.Parse(model.DistrictID);
                    }
                    if (!string.IsNullOrEmpty(model.VillageID))
                    {
                        user.VillageID = int.Parse(model.VillageID);
                    }
                    string token = Guid.NewGuid().ToString();
                    user.CreatedBy = token;
                    var result = dao.Insert(user);
                    if (result > 0)
                    {
                        ViewBag.Success = "Đăng ký thành công";
                        string callbackUrl =
                            System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                            "/User/Verify/" + token;
                        string mail =
                            System.IO.File.ReadAllText(Server.MapPath("~/assets/client/template/ConfirmEmail.html"));
                        mail = mail.Replace("{{Link}}", callbackUrl);

                        new MailHelper().SendMail(model.Email, "Xác nhận tài kooản", mail);
                        return RedirectToAction("Confirmation");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng ký thất bại");
                    }
                }
            }
            return View(model);
        }

        public ActionResult ResendEmail()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ResendEmail(string email)
        {
            var user = new UserDao().GetByEmail(email);
            var token = user.CreatedBy;

            string callbackUrl =
                System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                "/User/Verify/" + token;
            string mail =
                System.IO.File.ReadAllText(Server.MapPath("~/assets/client/template/ConfirmEmail.html"));
            mail = mail.Replace("{{Link}}", callbackUrl);

            new MailHelper().SendMail(user.Email, "Email gửi lại xác nhận tài khoản", mail);
            return Json(new
            {
                status = true
            });

        }
        [AllowAnonymous]
        public ActionResult Confirmation()
        {
            return View();
        }

        public ActionResult Verify()
        {
            string verifystring = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
            string tmp = verifystring.Substring(verifystring.LastIndexOf("/"));
            string token = tmp.Split('/')[1];
            var userdao = new UserDao();
            var user = userdao.GetByToken(token);
            userdao.UpdateStatus(user);
            if (user.CreatedBy == token)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                return null;
            }


        }
        #endregion

        public ActionResult ChangePassword()
        {
            var session = Session[CommonConstants.USER_SESSION];
            if (session == null)
            {
                return RedirectToAction("Login", "User");
            }
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var tmp = model.Email;
                var dao = new UserDao();
                var user = dao.GetByEmail(tmp);
                if (user.Password == model.Password)
                {
                    var encryptedMd5Pas = Encryptor.MD5Hash(model.NewPassword);
                    user.Password = encryptedMd5Pas;
                    var result = dao.ChangePass(user);
                    if (result)
                    {
                        ViewBag.Success = "Đổi mật khẩu thành công";
                        model = new ChangePasswordModel();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đổi mật khẩu thất bại");
                    }
                }
                else
                {
                    ModelState.AddModelError("","bạn đã nhập sai mật khâu cũ");
                }
                
            }
            return View(model);
        }

        //test forgot password
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ForgotPassword(string email,string username)
        {
            var dao = new UserDao();
            var user = dao.GetByEmail(email);
            user.Password = Encryptor.MD5Hash("123456");
            var result = dao.ChangePass(user);
            if (result)
            {
                string password = "123456";
                string mail =
                    System.IO.File.ReadAllText(Server.MapPath("~/assets/client/template/GetPassword.html"));
                mail = mail.Replace("{{UserName}}", username);
                mail = mail.Replace("{{Password}}", password);

                new MailHelper().SendMail(user.Email, "Email reset lại mật khẩu", mail);
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
            
        }
        #region LOGIN
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.UserName, Encryptor.MD5Hash(model.Password));
                if (result == 1)
                {
                    var user = dao.GetById(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;

                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    return Redirect("/");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản của bạn chưa tồn tại");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Rất tiếc tài khoản của bạn chưa được kích hoạt.Vui lòng truy cập email để kích hoạt tài kooản");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Password của bạn chưa đúng");
                }
                else
                {
                    ModelState.AddModelError("", "Không thể đăng nhập!!");
                }
            }
            return View(model);
        }
        #endregion

        public ActionResult LoginFaceBook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });
            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code,
            });
            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string userName = me.email;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;

                var user = new User();
                user.Email = email;
                user.UserName = email;
                user.Status = true;
                user.Name = firstname + " " + middlename + " " + lastname;
                user.CreatedDate = DateTime.Now;

                var userdao = new UserDao().InsertForFacebook(user);
                if (userdao > 0)
                {
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                }

            }
            return Redirect("/");
        }
        #region LOGOUT

        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            return Redirect("/");
        }
        #endregion

        public JsonResult LoadProvince()
        {
            var xmlDoc = XDocument.Load(Server.MapPath(@"~/assets/client/data/Provinces_Data.xml"));

            var xElements = xmlDoc.Element("Root").Elements("Item").Where(x => x.Attribute("type").Value == "province");
            var list = new List<ProvinceModel>();
            ProvinceModel province = null;
            foreach (var item in xElements)
            {
                province = new ProvinceModel();
                province.ID = int.Parse(item.Attribute("id").Value);
                province.Name = item.Attribute("value").Value;
                list.Add(province);

            }
            return Json(new
            {
                data = list,
                status = true
            });
        }
        public JsonResult LoadDistrict(int provinceID)
        {
            var xmlDoc = XDocument.Load(Server.MapPath(@"~/assets/client/data/Provinces_Data.xml"));

            var xElement = xmlDoc.Element("Root").Elements("Item").Single(x => x.Attribute("type").Value == "province" && int.Parse(x.Attribute("id").Value) == provinceID);
            var list = new List<DistrictModel>();
            DistrictModel district = null;
            foreach (var item in xElement.Elements("Item").Where(x => x.Attribute("type").Value == "district"))
            {
                district = new DistrictModel();
                district.ID = int.Parse(item.Attribute("id").Value);
                district.Name = item.Attribute("value").Value;
                district.ProvinceID = int.Parse(xElement.Attribute("id").Value);
                list.Add(district);

            }
            return Json(new
            {
                data = list,
                status = true
            });

        }
        public JsonResult LoadVillage(int provinceID, int districtID)
        {
            var xmlDoc = XDocument.Load(Server.MapPath(@"~/assets/client/data/Provinces_Data.xml"));
            var xElement = xmlDoc.Element("Root").Elements("Item").Single(x => x.Attribute("type").Value == "province" && int.Parse(x.Attribute("id").Value) == provinceID);
            var xElement2 = xElement.Elements("Item").Single(x => x.Attribute("type").Value == "district" && int.Parse(x.Attribute("id").Value) == districtID);
            var plist = new List<VillageModel>();
            VillageModel village = null;
            foreach (var item in xElement2.Elements("Item").Where(x => x.Attribute("type").Value == "precinct"))
            {
                village = new VillageModel();
                village.ID = int.Parse(item.Attribute("id").Value);
                village.Name = item.Attribute("value").Value;
                village.DistrictID = int.Parse(xElement.Attribute("id").Value);
                plist.Add(village);

            }
            return Json(new
            {
                data = plist,
                status = true
            });

        }

    }
}