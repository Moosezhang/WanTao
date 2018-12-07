using AccountTrain.Web.Common;
using BusinessComponent;
using BusinessEntity.Model;
using BusinessEntitys;
using Common;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountTrain.Web.Areas.Admin.Controllers
{
    public class ClassController : BaseController
    {
        #region 课程管理
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(string id)
        {
            return View();
        }

        public ActionResult GetAllClass()
        {
            try
            {
                return Json(new ClassBC().GetAllClass(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<ClassEntity>(), JsonRequestBehavior.AllowGet);
            }
        }


        //[HttpPost]
        //public ActionResult ClassUpload()
        //{
        //    try
        //    {
        //        if (Request.Files == null || Request.Files.Count == 0)
        //        {
        //            ViewBag.ErrorMessage = "Please select a file!!";
        //            return View();
        //        }
        //        HttpPostedFileBase file = Request.Files["file"];
        //        string filePath = string.Empty;
        //        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        //        string url = "/Images/upload/";
        //        var UserInfo = CacheManager.Instance.CurrentUser;
        //        if (UserInfo == null)
        //            return Json("false");
        //        string fileName = timestamp.ToString() + Path.GetFileName(file.FileName);
        //        filePath = Path.Combine(HttpContext.Server.MapPath(url), fileName);
        //        file.SaveAs(filePath);
        //        Session["ClassImgUrl"] = url + fileName;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Json("success");
        //}


        //[HttpPost]
        //public ActionResult IndexRemove()
        //{
        //    Session["ClassImgUrl"] = "";
        //    return Json("success");
        //}
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

        public ActionResult GetClassByKey(string id)
        {
            try
            {
                return Json(new ClassBC().GetClassByKey(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ClassEntity(), JsonRequestBehavior.AllowGet);
            }
        }

        //public ActionResult GetClassImage(string id)
        //{
        //    try
        //    {
        //        return Json(new ClassBC().GetClassImage(id), JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new List<VMImages>(), JsonRequestBehavior.AllowGet);
        //    }
        //}


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveClass(ClassEntity Class)
        {

            try
            {
                //if (Session["ClassImgUrl"] == null )
                //{

                //    int begin = Class.ClassImages.IndexOf("/Images");
                //    if (begin < 0)
                //    {
                //        Class.ClassImages = "";
                //    }
                //    else
                //    {
                //        Class.ClassImages = Class.ClassImages.Substring(begin);
                //    }
                   
                //    //Class.ClassImages = Class.ClassImages.Split();
                //}
                //else
                //{
                //    Class.ClassImages = Session["ClassImgUrl"].ToString();
                //}
                
                var result = new ClassBC().SaveClass(Class, CurrentUserInfo.Account);
                if (result == 0)
                    return Json(string.Empty);
                return Json("保存成功");
            }
            catch (Exception ex)
            {
                return Json(string.Empty);
            }
        }

        public ActionResult EnableClass(string ClassId, int status)
        {
            try
            {
                if (string.IsNullOrEmpty(ClassId))
                    return Json(string.Empty);
                var result = new ClassBC().EnableClass(ClassId, status);
                if (result > 0)
                {
                    return Json("更新成功", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(string.Empty, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult CreateQR(string classid)
        {
            QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
            QrCode qrCode = new QrCode();
            string url = CommonHelper.GetRedirect("WxClass%2fClassDetail?classId=" + classid);
            qrEncoder.TryEncode(url, out qrCode);

            GraphicsRenderer renderer = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Two), Brushes.Black, Brushes.White);
            
            
            string filename = string.Format("{0}.png", classid);
            string saveUrl = "/Images/QR/";
            string filePath = Path.Combine(HttpContext.Server.MapPath(saveUrl), filename);
            using (MemoryStream ms = new MemoryStream())
            {
                renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);
                Image img = Image.FromStream(ms);

                img.Save(filePath);
            }

            string showImg = CommonHelper.LinkImageUrl(saveUrl+filename);

            return Json(showImg, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region 章节管理
        public ActionResult Chapter()
        {
            return View();
        }

        public ActionResult GetChaptersByCondition(string className, string chapterTitle)
        {
            try
            {
                return Json(new ClassBC().GetChaptersByCondition(className, chapterTitle), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<VMClassChapter>(), JsonRequestBehavior.AllowGet);
            }
        }



      
        public ActionResult GetChaptersByClassKey(string key)
        {
            try
            {
                return Json(new ClassBC().GetChaptersByClassKey(key), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<ChapterEntity>(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetChapterByKey(string id)
        {
            try
            {
                return Json(new ClassBC().GetChapterByKey(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ChapterEntity(), JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult SaveChapter(ChapterEntity entity, string loginName)
        {
            try
            {
                var result = new ClassBC().SaveChapter(entity, CurrentUserInfo.Account);
                if (result == 0)
                    return Json(string.Empty);
                return Json("保存成功");
            }
            catch (Exception ex)
            {
                return Json(string.Empty);
            }
        }

        public ActionResult EnableChapter(string id, int status)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(string.Empty);
                var result = new ClassBC().EnableChapter(id, status);
                if (result > 0)
                {
                    return Json("更新成功", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(string.Empty, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

    }
}