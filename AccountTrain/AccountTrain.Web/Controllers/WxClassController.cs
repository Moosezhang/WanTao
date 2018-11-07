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
    public class WxClassController : WxBaseController
    {
        /// <summary>
        /// 精品课程和专家访谈页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 课程列表
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult ClassList(string openid, string code, string state)
        {
            if (string.IsNullOrEmpty(openid))
            {
                openid = GetOpenId(code).openid;

                if (string.IsNullOrEmpty(openid))
                {
                    Response.Redirect(CommonHelper.GetRedirect("WxClass%2fClassList"));
                }
            }

            ViewBag.Openid = openid;

            return View();
        }

        /// <summary>
        /// 课程详情
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult ClassDetail(string openid, string classId)
        {
            if (string.IsNullOrEmpty(openid))
            {
                Response.Redirect(CommonHelper.GetRedirect("WxClass%ClassDetail"));
            }

            ViewBag.Openid = openid;
            ViewBag.ClassId = classId;
            return View();
        }


        public ActionResult GetClassByCondition(string name, string classType, string startDate, string endDate)
        {
            try
            {
                return Json(new ClassBC().GetClassByCondition(name, classType, startDate, endDate), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<ClassEntity>(), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 根据classId获取课程详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetClassByKey(string classid)
        {
            try
            {
                return Json(new ClassBC().GetClassByKey(classid), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ClassEntity(), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 根据classid获取章节
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ActionResult GetChaptersByClassKey(string classid)
        {
            try
            {
                return Json(new ClassBC().GetChaptersByClassKey(classid), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<ChapterEntity>(), JsonRequestBehavior.AllowGet);
            }
        }



        /// <summary>
        /// 专家访谈列表
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult ExpertViewList(string openid, string code, string state)
        {
            if (string.IsNullOrEmpty(openid))
            {
                openid = GetOpenId(code).openid;

                if (string.IsNullOrEmpty(openid))
                {
                    Response.Redirect(CommonHelper.GetRedirect("WxClass%2fExpertViewList"));
                }
            }

            ViewBag.Openid = openid;

            return View();
        }

        /// <summary>
        /// 专家访谈详情
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult ExpertViewDetail(string openid, string classId)
        {
            if (string.IsNullOrEmpty(openid))
            {
                Response.Redirect(CommonHelper.GetRedirect("WxClass%2fExpertViewDetail"));
            }

            ViewBag.Openid = openid;
            ViewBag.ClassId = classId;
            return View();
        }

	}
}