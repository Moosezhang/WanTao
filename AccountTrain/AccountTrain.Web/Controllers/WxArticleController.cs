using AccountTrain.Web.Common;
using BusinessComponent;
using BusinessEntity.Model;
using BusinessEntitys;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountTrain.Web.Controllers
{
    public class WxArticleController : WxBaseController
    {
        #region 线下活动
        /// <summary>
        /// 线下活动页面
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult Activists(string code, string state)
        {
            string openid = "";
            if (new AppSetting().IsDebug != null
                && new AppSetting().IsDebug.ToLower() == "true")
            {
                openid = "123";
            }
            else
            {
                if (Request.Cookies[SystemConfig.WXOpenIDCookieKey] != null)
                    openid = Request.Cookies[SystemConfig.WXOpenIDCookieKey].Value;

                if (string.IsNullOrWhiteSpace(openid) && code == null)
                {
                    Response.Redirect(CommonHelper.GetRedirect("WxArticle%2fActivists"));
                }
                try
                {
                    if (string.IsNullOrWhiteSpace(openid))
                    {
                        
                        openid = GetOpenId(code).openid;
                       

                        // 合法用户，允许访问
                        Response.Cookies[SystemConfig.WXOpenIDCookieKey].Value = openid;
                        Response.Cookies[SystemConfig.WXOpenIDCookieKey].Path = "/";
                        Response.Cookies[SystemConfig.WXOpenIDCookieKey].Expires = DateTime.Now.AddDays(1);
                    }
                }
                catch (Exception ex)
                {
                    LogHelp.WriteLog(DateTime.Now + "ActivistsError:" + ex.Message);
                }
            }            

            ViewBag.Openid = openid;

            return View();
        }

        /// <summary>
        /// 法规资讯页面
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult Laws(string code, string state)
        {
            string openid = "";
            if (new AppSetting().IsDebug != null
                && new AppSetting().IsDebug.ToLower() == "true")
            {
                openid = "123";
            }
            else
            {
                if (Request.Cookies[SystemConfig.WXOpenIDCookieKey] != null)
                    openid = Request.Cookies[SystemConfig.WXOpenIDCookieKey].Value;

                if (string.IsNullOrWhiteSpace(openid) && code == null)
                {
                    Response.Redirect(CommonHelper.GetRedirect("WxArticle%2fLaws"));
                }
                try
                {
                    if (string.IsNullOrWhiteSpace(openid))
                    {

                        openid = GetOpenId(code).openid;


                        // 合法用户，允许访问
                        Response.Cookies[SystemConfig.WXOpenIDCookieKey].Value = openid;
                        Response.Cookies[SystemConfig.WXOpenIDCookieKey].Path = "/";
                        Response.Cookies[SystemConfig.WXOpenIDCookieKey].Expires = DateTime.Now.AddDays(1);
                    }
                }
                catch (Exception ex)
                {
                    LogHelp.WriteLog(DateTime.Now + "LawsError:" + ex.Message);
                }
            }    



            ViewBag.Openid = openid;

            return View();
        }

        public ActionResult Lives(string code, string state)
        {
            string openid = "";
            if (new AppSetting().IsDebug != null
                && new AppSetting().IsDebug.ToLower() == "true")
            {
                openid = "123";
            }
            else
            {
                if (Request.Cookies[SystemConfig.WXOpenIDCookieKey] != null)
                    openid = Request.Cookies[SystemConfig.WXOpenIDCookieKey].Value;

                if (string.IsNullOrWhiteSpace(openid) && code == null)
                {
                    Response.Redirect(CommonHelper.GetRedirect("WxArticle%2fLives"));
                }
                try
                {
                    if (string.IsNullOrWhiteSpace(openid))
                    {

                        openid = GetOpenId(code).openid;


                        // 合法用户，允许访问
                        Response.Cookies[SystemConfig.WXOpenIDCookieKey].Value = openid;
                        Response.Cookies[SystemConfig.WXOpenIDCookieKey].Path = "/";
                        Response.Cookies[SystemConfig.WXOpenIDCookieKey].Expires = DateTime.Now.AddDays(1);
                    }
                }
                catch (Exception ex)
                {
                    LogHelp.WriteLog(DateTime.Now + "LivesError:" + ex.Message);
                }
            }



            ViewBag.Openid = openid;

            return View();
        }

        public ActionResult GetAllArticlesByType(string type)
        {
            try
            {
                return Json(new BaseSetBC().GetAllArticlesByType(type), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<VMWxArticle>(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetArticlesByCondition(string KeyName)
        {
            try
            {
                return Json(new BaseSetBC().GetArticlesByCondition(KeyName), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<ArticleEntity>(), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

       

      

       

	}
}