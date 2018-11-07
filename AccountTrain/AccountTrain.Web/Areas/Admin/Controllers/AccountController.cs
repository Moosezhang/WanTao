
using AccountTrain.Common.Cache;
using AccountTrain.Common.Session;
using AccountTrain.Web.Common;
using BusinessComponent;
using BusinessEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountTrain.Web.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult LoginAccess(string username, string password)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(username))
            {
                ViewBag.ValidateInfo = "用户名或密码不能为空请重新登录";
                return View("Login");//new RedirectResult("/Admin/Account/Login");
            }
            UserBC bc = new UserBC();
            var result = bc.GetUsers(username, password);
            UserEntity entity=new UserEntity();
            if (result != null && result.Count > 0)
            {
                entity = result.FirstOrDefault();
            }
            else
            {
                ViewBag.ValidateInfo = "用户名或密码错误请重新登录";
                ////用户名密码错误请重新登录
            }
            
            if (entity != null && !string.IsNullOrEmpty(entity.User_Id))
            {
                var currentSession = new UserAuthSession
                {
                    Id=entity.User_Id,
                    Account = entity.Login_Name,
                    Name=entity.User_Name,
                    Token = Guid.NewGuid().ToString().GetHashCode().ToString("x"),
                    CreateTime = DateTime.Now,
                    IpAddress = HttpContext.Request.UserHostAddress,
                };

               
                CacheManager.Instance.CurrentUser = currentSession;
                //创建Session
                new ObjCacheProvider<UserAuthSession>().Create(currentSession.Token, currentSession, DateTime.Now.AddHours(1));
                var cookie = new HttpCookie("Token", currentSession.Token)
                {
                    Expires = DateTime.Now.AddHours(1)
                };
                HttpContext.Response.Cookies.Add(cookie);
                return new RedirectResult("/Admin/Account/Index");
            }
            else
            {
                ViewBag.ValidateInfo = "用户名或密码错误请重新登录";
                ////用户名密码错误请重新登录
            }

            // return new RedirectResult("/Admin/Account/Login");
            return View("Login");
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            ObjCacheProvider<UserAuthSession> objCacheProvider = new ObjCacheProvider<UserAuthSession>();
            string token = string.Empty;
            //Token by QueryString

            if (HttpContext.Request.Cookies[ConstCommon.Token] != null)  //从Cookie读取Token
            {
                token = HttpContext.Request.Cookies[ConstCommon.Token].Value;
            }

            UserAuthSession userSession = objCacheProvider.GetCache(token);
            if (userSession != null && string.IsNullOrEmpty(userSession.Token))
            {
                objCacheProvider.Remove(userSession.Token);
            }
            return new RedirectResult("/Admin/Account/Login");
        }

        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult NoPremission()
        {
            return View();
        }

        #region 修改密码
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ChangePassword()
        {
            if (CurrentUserInfo == null || string.IsNullOrEmpty(CurrentUserInfo.Account))
            {
                return RedirectToAction("Login");
            }
            ViewBag.UserName = CurrentUserInfo.Account;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult SavePassword(UserEntity user)
        {
            try
            {
                var result = new UserBC().SavePassword(user, CurrentUserInfo.Account);
                if (result == 0)
                    return Json(string.Empty);
                return Json("保存成功");
            }
            catch (Exception ex)
            {
                return Json(string.Empty);
            }
        }
        #endregion
       
     
    }
}
