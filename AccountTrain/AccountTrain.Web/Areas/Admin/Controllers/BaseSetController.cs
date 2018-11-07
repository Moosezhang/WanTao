using AccountTrain.Web.Common;
using BusinessComponent;
using BusinessEntity.Model;
using BusinessEntitys;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountTrain.Web.Areas.Admin.Controllers
{
    public class BaseSetController : BaseController
    {
        //
        // GET: /Admin/BaseSet/
        

        #region 公司信息
        /// <summary>
        /// 公司介绍设置
        /// </summary>
        /// <returns></returns>
        public ActionResult CompanyInfo()
        {
            return View();
        }


        /// <summary>
        /// 获取公司信息
        /// </summary>
        /// <returns></returns>
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

        public ActionResult SaveCompanyInfo(CompanyEntity company)
        {
            try
            {
                var result = new BaseSetBC().SaveCompanyInfo(company, CurrentUserInfo.Account);
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

        #region 招聘信息
        /// <summary>
        /// 招聘信息设置
        /// </summary>
        /// <returns></returns>
        public ActionResult JobInfo()
        {
            return View();
        }

        /// <summary>
        /// 根据条件查询招聘信息列表
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public ActionResult GetJobListByCondition(string title)
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

        /// <summary>
        /// 根据id获取招聘信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetJobInfoByKey(string jobId)
        {
            try
            {
                return Json(new BaseSetBC().GetJobInfoByKey(jobId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new JobEntity(), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存招聘信息
        /// </summary>
        /// <param name="job"></param>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public ActionResult SaveJobInfo(JobEntity job, string loginName)
        {
            try
            {
                var result = new BaseSetBC().SaveJobInfo(job, CurrentUserInfo.Account);
                if (result == 0)
                    return Json(string.Empty);
                return Json("保存成功");
            }
            catch (Exception ex)
            {
                return Json(string.Empty);
            }
        }

        /// <summary>
        /// 用户启用/禁用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult EnableJob(string jobId, int status)
        {
            try
            {
                if (string.IsNullOrEmpty(jobId))
                    return Json(string.Empty);
                var result = new BaseSetBC().EnableJob(jobId, status);
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

        #region 字典信息
        /// <summary>
        /// 数据字典设置
        /// </summary>
        /// <returns></returns>
        public ActionResult DictionaryInfo()
        {
            return View();
        }

        public ActionResult GetDicListByCondition(string dk, string ik, string iv)
        {
            try
            {
                return Json(new BaseSetBC().GetDicListByCondition(dk, ik, iv), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<DictionaryItemEntity>(), JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// 获取所有字典键值
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllDicKey()
        {
            try
            {
                return Json(new BaseSetBC().GetAllDicKey(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<DictionaryItemEntity>(), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 根据键值key获取items
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ActionResult GetDicItemsByDicKey(string key)
        {
            try
            {
                var result = new BaseSetBC().GetDicItemsByDicKey(key);
                return Json(new BaseSetBC().GetDicItemsByDicKey(key), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<DictionaryItemEntity>(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetDicItemByKey(string itemId)
        {
            try
            {
                return Json(new BaseSetBC().GetDicItemByKey(itemId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new DictionaryItemEntity(), JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult SaveDicItem(DictionaryItemEntity dicItem, string loginName)
        {
            try
            {
                BaseSetBC bc= new BaseSetBC();
                //根据DictionaryKey获取level
               
                var result = bc.SaveDicItem(dicItem, CurrentUserInfo.Account);
                if (result == 0)
                    return Json(string.Empty);
                return Json("保存成功");
            }
            catch (Exception ex)
            {
                return Json(string.Empty);
            }
        }

        public ActionResult EnableItems(string ItemId, int status)
        {
            try
            {
                if (string.IsNullOrEmpty(ItemId))
                    return Json(string.Empty);
                var result = new BaseSetBC().EnableItems(ItemId, status);
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
        
        #region 推广管理
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult SpreadConfig() 
        {
            return View();
        }     
        #endregion

        #region 砍价
        public ActionResult BargainConfig()
        {
            return View();
        }

        public ActionResult GetBargainConfigsByCondition(string className)
        {
            try
            {
                return Json(new BaseSetBC().GetBargainConfigsByCondition(className), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<VMBargainConfig>(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetBargainConfigByKey(string id)
        {
            try
            {
                return Json(new BaseSetBC().GetBargainConfigByKey(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new BargainConfigEntity(), JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult SaveBargainConfig(BargainConfigEntity entity)
        {
            try
            {
                var result = new BaseSetBC().SaveBargainConfig(entity, CurrentUserInfo.Account);
                if (result == 0)
                    return Json(string.Empty);
                return Json("保存成功");
            }
            catch (Exception ex)
            {
                return Json(string.Empty);
            }
        }

        public ActionResult EnableBargainConfig(string Id, int status)
        {
            try
            {
                if (string.IsNullOrEmpty(Id))
                    return Json(string.Empty);
                var result = new BaseSetBC().EnableBargainConfig(Id, status);
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

        #region 团购
        public ActionResult GroupBuyConfig()
        {
            return View();
        }

        public ActionResult GetGroupBuyConfigsByCondition(string className)
        {
            try
            {
                return Json(new BaseSetBC().GetGroupBuyConfigsByCondition(className), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<VMGroupBuyConfig>(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetGroupBuyConfigByKey(string id)
        {
            try
            {
                return Json(new BaseSetBC().GetGroupBuyConfigByKey(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new GroupBuyConfigEntity(), JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult SaveGroupBuyConfig(GroupBuyConfigEntity entity, string loginName)
        {
            try
            {
                var result = new BaseSetBC().SaveGroupBuyConfig(entity, CurrentUserInfo.Account);
                if (result == 0)
                    return Json(string.Empty);
                return Json("保存成功");
            }
            catch (Exception ex)
            {
                return Json(string.Empty);
            }
        }

        public ActionResult EnableGroupBuyConfig(string Id, int status)
        {
            try
            {
                if (string.IsNullOrEmpty(Id))
                    return Json(string.Empty);
                var result = new BaseSetBC().EnableGroupBuyConfig(Id, status);
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

        #region 助力
        public ActionResult HelpConfig()
        {
            return View();
        }

        [HttpPost]
        public ActionResult HelpUpload()
        {
            try
            {
                if (Request.Files == null || Request.Files.Count == 0)
                {
                    ViewBag.ErrorMessage = "Please select a file!!";
                    return View();
                }
                HttpPostedFileBase file = Request.Files["file"];
                string filePath = string.Empty;
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                string url = "/Images/upload/";
                var UserInfo = CacheManager.Instance.CurrentUser;
                if (UserInfo == null)
                    return Json("false");
                string fileName = timestamp.ToString() + Path.GetFileName(file.FileName);
                filePath = Path.Combine(HttpContext.Server.MapPath(url), fileName);
                file.SaveAs(filePath);
                Session["HelpImgUrl"] = url + fileName;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json("success");
        }

        [HttpPost]
        public ActionResult HelpRemove()
        {
            Session["HelpImgUrl"] = "";
            return Json("success");
        }

        public ActionResult GetHelpConfigsByCondition(string className)
        {
            try
            {
                return Json(new BaseSetBC().GetHelpConfigsByCondition(className), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<VMHelpConfig>(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetHelpConfigByKey(string id)
        {
            try
            {
                return Json(new BaseSetBC().GetHelpConfigByKey(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new HelpConfigEntity(), JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult SaveHelpConfig(HelpConfigEntity entity, string loginName)
        {
            try
            {

                if (Session["HelpImgUrl"] == null)
                {
                    int begin = entity.ImageUrl.IndexOf("/Images");
                    if (begin < 0)
                    {
                        entity.ImageUrl = "";
                    }
                    else
                    {
                        entity.ImageUrl = entity.ImageUrl.Substring(begin);
                    }

                }
                else
                {
                    entity.ImageUrl = Session["HelpImgUrl"].ToString();
                }

                var result = new BaseSetBC().SaveHelpConfig(entity, CurrentUserInfo.Account);
                if (result == 0)
                    return Json(string.Empty);
                return Json("保存成功");
            }
            catch (Exception ex)
            {
                return Json(string.Empty);
            }
        }

        public ActionResult EnableHelpConfig(string Id, int status)
        {
            try
            {
                if (string.IsNullOrEmpty(Id))
                    return Json(string.Empty);
                var result = new BaseSetBC().EnableHelpConfig(Id, status);
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


        public ActionResult GetHelpImage(string id)
        {
            try
            {
                return Json(new BaseSetBC().GetHelpImage(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<VMImages>(), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 公益金
        public ActionResult PublicFundsConfig()
        {
            return View();
        }

        public ActionResult GetPublicFundsByCondition(string className)
        {
            try
            {
                return Json(new BaseSetBC().GetPublicFundsByCondition(className), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<VMPublicFunds>(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetPublicFundsByKey(string id)
        {
            try
            {
                return Json(new BaseSetBC().GetPublicFundsByKey(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new PublicFundsEntity(), JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult SavePublicFunds(PublicFundsEntity entity, string loginName)
        {
            try
            {
                var result = new BaseSetBC().SavePublicFunds(entity, CurrentUserInfo.Account);
                if (result == 0)
                    return Json(string.Empty);
                return Json("保存成功");
            }
            catch (Exception ex)
            {
                return Json(string.Empty);
            }
        }

        public ActionResult EnablePublicFunds(string Id, int status)
        {
            try
            {
                if (string.IsNullOrEmpty(Id))
                    return Json(string.Empty);
                var result = new BaseSetBC().EnablePublicFunds(Id, status);
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

        #region 首页信息
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult IndexImgUpload()
        {
            try
            {
                if (Request.Files == null || Request.Files.Count == 0)
                {
                    ViewBag.ErrorMessage = "Please select a file!!";
                    return View();
                }
                HttpPostedFileBase file = Request.Files["file"];
                string filePath = string.Empty;
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                string url = "/Images/upload/";
                var UserInfo = CacheManager.Instance.CurrentUser;
                if (UserInfo == null)
                    return Json("false");
                string fileName = timestamp.ToString() + Path.GetFileName(file.FileName);
                filePath = Path.Combine(HttpContext.Server.MapPath(url), fileName);
                file.SaveAs(filePath);
                Session["IndexImgUrl"] = url + fileName;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json("success");
        }

        [HttpPost]
        public ActionResult IndexRemove()
        {
            Session["IndexImgUrl"] = "";
            return Json("success");
        }

        public ActionResult GetIndexImagesByCondition(string KeyName)
        {
            try
            {
                return Json(new BaseSetBC().GetIndexImagesByCondition(KeyName), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<IndexImageEntity>(), JsonRequestBehavior.AllowGet);
            }
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

        public ActionResult GetIndexImageByKey(string id)
        {
            try
            {
                return Json(new BaseSetBC().GetIndexImageByKey(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new IndexImageEntity(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetIndexImages(string id)
        {
            try
            {
                return Json(new BaseSetBC().GetIndexImages(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<VMImages>(), JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult SaveIndexImage(IndexImageEntity indexImage)
        {

            try
            {
                if (Session["IndexImgUrl"] == null)
                {

                    int begin = indexImage.ImageUrl.IndexOf("/Images");
                    if (begin < 0)
                    {
                        indexImage.ImageUrl = "";
                    }
                    else
                    {
                        indexImage.ImageUrl = indexImage.ImageUrl.Substring(begin);
                    }
                }
                else
                {
                    indexImage.ImageUrl = Session["IndexImgUrl"].ToString();
                }

                
                var result = new BaseSetBC().SaveIndexImage(indexImage, CurrentUserInfo.Account);
                if (result == 0)
                    return Json(string.Empty);
                return Json("保存成功");
            }
            catch (Exception ex)
            {
                return Json(string.Empty);
            }
        }

        public ActionResult EnableImage(string ImageId, int status)
        {
            try
            {
                if (string.IsNullOrEmpty(ImageId))
                    return Json(string.Empty);
                var result = new BaseSetBC().EnableImage(ImageId, status);
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

        #region 文章管理
        public ActionResult ArticleInfo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ArticleUpload()
        {
            try
            {
                if (Request.Files == null || Request.Files.Count == 0)
                {
                    ViewBag.ErrorMessage = "Please select a file!!";
                    return View();
                }
                HttpPostedFileBase file = Request.Files["file"];
                string filePath = string.Empty;
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                string url = "/Images/upload/";
                var UserInfo = CacheManager.Instance.CurrentUser;
                if (UserInfo == null)
                    return Json("false");
                string fileName = timestamp.ToString() + Path.GetFileName(file.FileName);
                filePath = Path.Combine(HttpContext.Server.MapPath(url), fileName);
                file.SaveAs(filePath);
                Session["ArticleImgUrl"] = url + fileName;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json("success");
        }

        [HttpPost]
        public ActionResult ArticleRemove()
        {
            Session["ArticleImgUrl"] = "";
            return Json("success");
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



        public ActionResult GetAllArticles()
        {
            try
            {
                return Json(new BaseSetBC().GetAllArticles(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<ArticleEntity>(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetArticleByKey(string id)
        {
            try
            {
                return Json(new BaseSetBC().GetArticleByKey(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ArticleEntity(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetArticleImage(string id)
        {
            try
            {
                return Json(new BaseSetBC().GetArticleImage(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<VMImages>(), JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult SaveArticle(ArticleEntity Article)
        {

            try
            {
                if (Session["ArticleImgUrl"] == null )
                {
                    int begin = Article.ImageUrl.IndexOf("/Images");
                    if (begin < 0)
                    {
                        Article.ImageUrl = "";
                    }
                    else
                    {
                        Article.ImageUrl = Article.ImageUrl.Substring(begin);
                    }                   
                    
                }
                else
                {
                    Article.ImageUrl = Session["ArticleImgUrl"].ToString();
                }
                
                var result = new BaseSetBC().SaveArticle(Article, CurrentUserInfo.Account);
                if (result == 0)
                    return Json(string.Empty);
                return Json("保存成功");
            }
            catch (Exception ex)
            {
                return Json(string.Empty);
            }
        }

        public ActionResult EnableArticle(string ArticleId, int status)
        {
            try
            {
                if (string.IsNullOrEmpty(ArticleId))
                    return Json(string.Empty);
                var result = new BaseSetBC().EnableArticle(ArticleId, status);
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