using BusinessComponent;
using BusinessEntity.Common;
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

            var result = new OrderBC().GetOrderByOpenIdandClassId(openid, classId);
            if (result != null)
                ViewBag.OrderStatus = result.Status;
            else
                ViewBag.OrderStatus = 1;

            ViewBag.Openid = openid;
            ViewBag.ClassId = classId;
            return View();
        }


        public ActionResult GetClassByCondition(string name, string classType, string startDate, string endDate, string classGroup, string order)
        {
            try
            {
                return Json(new ClassBC().GetClassByCondition(name, classType, startDate, endDate, classGroup, order), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<ClassEntity>(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetClassByType(string type)
        {
            try
            {
                return Json(new ClassBC().GetClassByType(type), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<ClassEntity>(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetNewestClass()
        {
            try
            {
                return Json(new ClassBC().GetNewestClass(), JsonRequestBehavior.AllowGet);
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

            var result = new OrderBC().GetOrderByOpenIdandClassId(openid, classId);
            if (result != null)
                ViewBag.OrderStatus = result.Status;
            else
                ViewBag.OrderStatus = 1;

            ViewBag.Openid = openid;
            ViewBag.ClassId = classId;
            return View();
        }


        public ActionResult GetDicInfoList(string key)
        {
            List<DictionaryInfo> DicList = new List<DictionaryInfo>();
            List<DictionaryItemEntity> result = new BaseSetBC().GetAllDicKey();

            List<DictionaryItemEntity> Level1 = result.Where(p => p.DictionaryLevel == 0).ToList();

            List<Cascader> cascader = new List<Cascader>();

            foreach (var item in Level1)
            {
                DictionaryInfo model = new DictionaryInfo()
                {
                    ItemId = item.ItemId,
                    ItemKey = item.ItemKey,
                    ItemValue = item.ItemValue,
                    DictionaryKey = item.DictionaryKey,
                    DictionaryLevel = item.DictionaryLevel
                };
                Cascader ca = new Cascader() {
                    id = item.ItemId,
                    value = item.ItemKey,
                    label = item.ItemValue,
                };
                List<DictionaryInfo> nodes1 = new List<DictionaryInfo>();
                List<Cascader> cascader1 = new List<Cascader>();
                var result1 = result.Where(p => p.DictionaryKey == item.ItemId).ToList();
                if (result1 != null && result1.Count > 0)
                {                    
                    foreach (var i in result1)
                    {
                        DictionaryInfo model1 = new DictionaryInfo()
                        {
                            ItemId = i.ItemId,
                            ItemKey = i.ItemKey,
                            ItemValue = i.ItemValue,
                            DictionaryKey = i.DictionaryKey,
                            DictionaryLevel = i.DictionaryLevel
                        };
                        Cascader ca1 = new Cascader()
                        {
                            id = i.ItemId,
                            value = i.ItemKey,
                            label = i.ItemValue,
                        };
                        List<DictionaryInfo> nodes2 = new List<DictionaryInfo>();
                        List<Cascader> cascader2 = new List<Cascader>();
                        var result2 = result.Where(p => p.DictionaryKey == i.ItemId).ToList();
                        if (result2 != null && result2.Count > 0)
                        {
                            foreach (var j in result2)
                            {
                                DictionaryInfo model2 = new DictionaryInfo()
                                {
                                    ItemId = j.ItemId,
                                    ItemKey = j.ItemKey,
                                    ItemValue = j.ItemValue,
                                    DictionaryKey = j.DictionaryKey,
                                    DictionaryLevel = j.DictionaryLevel
                                };
                                Cascader ca2 = new Cascader()
                                {
                                    id = j.ItemId,
                                    value = j.ItemKey,
                                    label = j.ItemValue,
                                };

                                nodes2.Add(model2);
                                cascader2.Add(ca2);
                            }
                        }
                        model1.SubDic = nodes2;
                        ca1.children = cascader2;
                        nodes1.Add(model1);
                        cascader1.Add(ca1);
                    }
                }

                model.SubDic = nodes1;
                ca.children = cascader1;
                DicList.Add(model);
                cascader.Add(ca);
            }

            DicList = DicList.Where(p => p.ItemId == key).ToList();
            cascader = cascader.Where(p => p.id == key).ToList();
            return Json(cascader, JsonRequestBehavior.AllowGet);
        }
	}

    public class Cascader 
    {
        public string id { get; set; }
        public string value { get; set; }
        public string label { get; set; }

        public List<Cascader> children { get; set; }
    }
}