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
        public ActionResult Index(string openid, string code, string state)
        {
            if (string.IsNullOrEmpty(openid))
            {
                openid = GetOpenId(code).openid;

                if (string.IsNullOrEmpty(openid))
                {                    
                    Response.Redirect(CommonHelper.GetRedirect("WxHome%2fIndex"));
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
                return Json(new BaseSetBC().GetAllIndexImages(), JsonRequestBehavior.AllowGet);
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
        public ActionResult CompanyInfo(string openid, string code, string state)
        {
            if (string.IsNullOrEmpty(openid))
            {
                openid = GetOpenId(code).openid;

                if (string.IsNullOrEmpty(openid))
                {
                    //Response.Redirect(string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxa7f322fb262e5a7b&redirect_uri=http%3a%2f%2f {0}%2fonebox%2fgoparty%2findex&response_type=code&scope=snsapi_userinfo&state=1#wechat_redirect", setting.AppDomainName.Replace("http://", "")));
                    Response.Redirect(CommonHelper.GetRedirect("My%2fRegistered"));
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
        public ActionResult JobInfo(string openid, string code, string state)
        {
            if (string.IsNullOrEmpty(openid))
            {
                openid = GetOpenId(code).openid;

                if (string.IsNullOrEmpty(openid))
                {
                    //Response.Redirect(string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxa7f322fb262e5a7b&redirect_uri=http%3a%2f%2f {0}%2fonebox%2fgoparty%2findex&response_type=code&scope=snsapi_userinfo&state=1#wechat_redirect", setting.AppDomainName.Replace("http://", "")));
                    Response.Redirect(CommonHelper.GetRedirect("My%2fRegistered"));
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