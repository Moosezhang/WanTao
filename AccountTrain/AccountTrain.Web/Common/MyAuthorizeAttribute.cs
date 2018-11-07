
using AccountTrain.Common.Cache;
using AccountTrain.Common.Session;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountTrain.Web.Common
{
    public class MyAuthorizeAttribute : AuthorizeAttribute
    {
        private bool IsLogin = false;
        private bool NeedLogin = true;
        private UserAuthSession userSession = null;
        //private string _appKey = ConfigurationManager.AppSettings["AppKey"];
        private ObjCacheProvider<UserAuthSession> objCacheProvider = new ObjCacheProvider<UserAuthSession>();

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var token = "";
            string action = httpContext.Request.Url.ToString();
            if (action.ToLower().Contains("/public/"))
            {
                NeedLogin = false;
                return true;
            }
            //Token by QueryString
            if (httpContext.Request.Cookies[ConstCommon.Token] != null)  //从Cookie读取Token
            {
                token = httpContext.Request.Cookies[ConstCommon.Token].Value;
            }
            bool Pass = false;
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    httpContext.Response.StatusCode = 401;//无权限状态码
                    Pass = false;
                    IsLogin = false;
                }
                else
                {
                    //UserAuthSession userSession = objCacheProvider.GetCache(token);
                    this.userSession = objCacheProvider.GetCache(token);
                    if (this.userSession == null || string.IsNullOrEmpty(this.userSession.Token))
                    {
                        httpContext.Response.StatusCode = 401;//无权限状态码
                        Pass = false;
                        IsLogin = false;
                    }
                    else
                    {
                        Pass = true;
                        IsLogin = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Pass = false;
            }
            return Pass;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!NeedLogin)
            {
                return;
            }

            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            else
            {
                if (this.userSession != null && !IsLogin)
                {
                    string fromUrl = filterContext.HttpContext.Request.Url.PathAndQuery;
                    string strUrl = "/Admin/Account/Login/?fromurl={0}";
                    filterContext.HttpContext.Response.Redirect(string.Format(strUrl, fromUrl), true);
                }
                else
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.Result = new JsonResult
                        {
                            Data = new
                            {
                                Success = false,
                                Message = "Session Timeout",
                                message = "Session Timeout",////兼容老数据
                                rows = "[]",////兼容老数据
                                total = 0,////兼容老数据
                                Redirect = "~/Admin/Account/NoPremission"
                            }
                        };
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult("~/Admin/Account/NoPremission");
                    }


                }

            }
        }

    }
}