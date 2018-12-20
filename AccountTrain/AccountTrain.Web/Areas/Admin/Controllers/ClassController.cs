using AccountTrain.Web.Common;
using BusinessComponent;
using BusinessEntity.Model;
using BusinessEntitys;
using Common;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            classQr cq = new classQr();
            string url = ConfigurationManager.AppSettings["imgMapPath"] + "WxClass/ClassDetail?classId=" + classid;
            //string url = CommonHelper.GetRedirect("WxClass%2fClassDetail?classId=" + classid);
            //string url = "";
            //var aResult = ShortUrl(Logurl);
            //for (int i = 0; i < aResult.Length; i++) 
            //{
            //    url = url+aResult[i];
            //}

            qrEncoder.TryEncode(url, out qrCode);
            


            cq.url = url;
            GraphicsRenderer renderer = new GraphicsRenderer(new FixedModuleSize(2, QuietZoneModules.Two), Brushes.Black, Brushes.White);
            
            
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
            cq.showImg = showImg;
            return Json(cq, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 短网址应用 ，例如QQ微博的url.cn   http://url.cn/2hytQx
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        //public static string[] ShortUrl(string url)
        //{
        //    //可以自定义生成MD5加密字符传前的混合KEY
        //    string key = "fooo_2016";
        //    //要使用生成URL的字符
        //    string[] chars = new string[]{
        //"a","b","c","d","e","f","g","h" ,"i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
        //"0","1","2","3","4","5","6","7","8","9",
        //"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T" ,"U","V","W","X","Y","Z"
        //};

        //    //对传入网址进行MD5加密
        //    string hex = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(key + url, "md5");
        //    string[] resUrl = new string[4];
        //    for (int i = 0; i < 4; i++)
        //    {
        //        //把加密字符按照8位一组16进制与0x3FFFFFFF进行位与运算
        //        int hexint = 0x3FFFFFFF & Convert.ToInt32("0x" + hex.Substring(i * 8, 8), 16);
        //        string outChars = string.Empty;
        //        for (int j = 0; j < 6; j++)
        //        {
        //            //把得到的值与0x0000003D进行位与运算，取得字符数组chars索引
        //            int index = 0x0000003D & hexint;
        //            //把取得的字符相加
        //            outChars += chars[index];
        //            //每次循环按位右移5位
        //            hexint = hexint >> 5;
        //        }
        //        //把字符串存入对应索引的输出数组
        //        resUrl[i] = outChars;
        //    }
        //    return resUrl;
        //}

        public class classQr 
        {
            public string url { get; set; }

            public string showImg { get; set; }
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