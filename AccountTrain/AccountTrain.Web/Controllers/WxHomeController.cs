using AccountTrain.Web.Common;
using BusinessComponent;
using BusinessEntitys;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountTrain.Web.Controllers
{
    public class WxHomeController : WxBaseController
    {
        /// <summary>
        /// 首页页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string code, string state)
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
                    Response.Redirect(CommonHelper.GetRedirect("WxHome%2fIndex"));
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
                    LogHelp.WriteLog(DateTime.Now + "IndexError:" + ex.Message);
                }
            }    

            ViewBag.Openid = openid;

            return View();
        }

        /// <summary>
        /// 获取所有首页图片
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllIndexImages()
        {
            try
            {
                var result=new BaseSetBC().GetAllIndexImages();
                result = result.Take(5).ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<IndexImageEntity>(), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 首页全局查询界面
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ActionResult IndexSearch(string openid,string condition)
        {
            if (string.IsNullOrEmpty(openid))
            {
                Response.Redirect(CommonHelper.GetRedirect("WxHome%2fIndex"));
            }


            ViewBag.Openid = openid;
            ViewBag.Condition = condition;

            return View();
        }

        #region 联系我们
        public ActionResult CompanyInfo(string code, string state)
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
                    Response.Redirect(CommonHelper.GetRedirect("WxHome%2fCompanyInfo"));
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
                    LogHelp.WriteLog(DateTime.Now + "CompanyInfoError:" + ex.Message);
                }
            }             

            ViewBag.Openid = openid;

            return View();
        }

        public ActionResult GetCompanyInfo()
        {
            try
            {
                return Json(new BaseSetBC().GetCompanyInfo(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new CompanyEntity(), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 职位自荐
        public ActionResult JobInfo(string code, string state)
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
                    Response.Redirect(CommonHelper.GetRedirect("WxHome%2fJobInfo"));
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
                    LogHelp.WriteLog(DateTime.Now + "JobInfoError:" + ex.Message);
                }
            }              

            ViewBag.Openid = openid;

            return View();
        }

        public ActionResult GetJobInfos(string title)
        {
            try
            {
                return Json(new BaseSetBC().GetJobListByCondition(title), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<JobEntity>(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult JobInfoDetail(string openid, string jobId)
        {
            ViewBag.Openid = openid;
            JobEntity entity = new JobEntity();
            try
            {
                entity = new BaseSetBC().GetJobInfoByKey(jobId);
            }
            catch (Exception ex)
            {
                
            }
            return View(entity);
        }
        #endregion

    }
}