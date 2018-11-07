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
        /// <summary>
        /// 线下活动和法规快递页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        #region 线下活动
        /// <summary>
        /// 线下活动页面
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult Activists(string openid, string code, string state)
        {
            if (string.IsNullOrEmpty(openid))
            {
                openid = GetOpenId(code).openid;

                if (string.IsNullOrEmpty(openid))
                {
                    Response.Redirect(CommonHelper.GetRedirect("WxArticle%2fActivists"));
                }
            }

            ViewBag.Openid = openid;

            return View();
        }

        /// <summary>
        /// 法规快递页面
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult Laws(string openid, string code, string state)
        {
            if (string.IsNullOrEmpty(openid))
            {
                openid = GetOpenId(code).openid;

                if (string.IsNullOrEmpty(openid))
                {
                    Response.Redirect(CommonHelper.GetRedirect("WxArticle%2fLaws"));
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
        #endregion

       

      

       

	}
}