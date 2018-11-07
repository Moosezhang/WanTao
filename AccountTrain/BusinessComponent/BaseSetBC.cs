using BusinessEntity.Model;
using BusinessEntitys;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Common;

namespace BusinessComponent
{
    public class BaseSetBC
    {
        #region 公司信息
        public CompanyEntity GetCompanyInfo()
        {
            BaseSetDA da = new BaseSetDA();
            return da.GetCompanyInfo();
        }

        public int SaveCompanyInfo(CompanyEntity company, string loginName)
        {
            BaseSetDA da = new BaseSetDA();
            return da.SaveCompanyInfo(company, loginName);
        }
        #endregion
      
        #region 招聘信息
        public List<JobEntity> GetJobListByCondition(string title)
        {
            BaseSetDA da = new BaseSetDA();
            return da.GetJobListByCondition(title);
        }

        public JobEntity GetJobInfoByKey(string id)
        {
            BaseSetDA da = new BaseSetDA();
            return da.GetJobInfoByKey(id);
        }


        public int SaveJobInfo(JobEntity job, string loginName)
        {
            BaseSetDA da = new BaseSetDA();
            return da.SaveJobInfo(job, loginName);
        }

        public int EnableJob(string jobId, int status)
        {
            BaseSetDA da = new BaseSetDA();
            return da.EnableJob(jobId, status);
        }
        #endregion

        #region 字典信息
        //public List<VMDicItems> GetDicItemsListByCondition(string KeyName)
        //{
        //    BaseSetDA da = new BaseSetDA();
        //    return da.GetDicItemsListByCondition(KeyName);
        //}


        /// <summary>
        /// 获取所有字典键值
        /// </summary>
        /// <returns></returns>
        public List<DictionaryItemEntity> GetAllDicKey()
        {
            BaseSetDA da = new BaseSetDA();
            return da.GetAllDicKey();
        }

        /// <summary>
        /// 根据键值key获取items
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<DictionaryItemEntity> GetDicItemsByDicKey(string key)
        {
            BaseSetDA da = new BaseSetDA();
            return da.GetDicItemsByDicKey(key);
        }

        public DictionaryItemEntity GetDicItemByKey(string itemId)
        {
            BaseSetDA da = new BaseSetDA();
            return da.GetDicItemByKey(itemId);
        }

        public DictionaryItemEntity GetDicItemValueByKey(string ItemKey, string DicKey)
        {
            BaseSetDA da = new BaseSetDA();
            return da.GetDicItemValueByKey(ItemKey, DicKey);
        }

        public int SaveDicItem(DictionaryItemEntity dicItem, string loginName)
        {
            BaseSetDA da = new BaseSetDA();
            return da.SaveDicItem(dicItem, loginName);
        }

        public int EnableItems(string ItemId, int status)
        {
            BaseSetDA da = new BaseSetDA();
            return da.EnableItems(ItemId, status);
        }
        #endregion


        #region 砍价
        public List<VMBargainConfig> GetBargainConfigsByCondition(string className)
        {
            BaseSetDA da = new BaseSetDA();
            return da.GetBargainConfigsByCondition(className);
        }

        public BargainConfigEntity GetBargainConfigByKey(string id)
        {
            BaseSetDA da = new BaseSetDA();
            return da.GetBargainConfigByKey(id);
        }


        public int SaveBargainConfig(BargainConfigEntity entity, string loginName)
        {
            BaseSetDA da = new BaseSetDA();
            return da.SaveBargainConfig(entity, loginName);
        }

        public int EnableBargainConfig(string Id, int status)
        {
            BaseSetDA da = new BaseSetDA();
            return da.EnableBargainConfig(Id, status);
        }
        #endregion

        #region 团购
        public List<VMGroupBuyConfig> GetGroupBuyConfigsByCondition(string className)
        {
            BaseSetDA da = new BaseSetDA();
            return da.GetGroupBuyConfigsByCondition(className);
        }

        public GroupBuyConfigEntity GetGroupBuyConfigByKey(string id)
        {
            BaseSetDA da = new BaseSetDA();
            return da.GetGroupBuyConfigByKey(id);
        }


        public int SaveGroupBuyConfig(GroupBuyConfigEntity entity, string loginName)
        {
            BaseSetDA da = new BaseSetDA();
            return da.SaveGroupBuyConfig(entity, loginName);
        }

        public int EnableGroupBuyConfig(string Id, int status)
        {
            BaseSetDA da = new BaseSetDA();
            return da.EnableGroupBuyConfig(Id, status);
        }
        #endregion

        #region 助力
        public List<VMHelpConfig> GetHelpConfigsByCondition(string className)
        {
            BaseSetDA da = new BaseSetDA();
            return da.GetHelpConfigsByCondition(className);
        }

        public HelpConfigEntity GetHelpConfigByKey(string id)
        {
            BaseSetDA da = new BaseSetDA();

            var result = da.GetHelpConfigByKey(id);

            result.ImageUrl = CommonHelper.LinkImageUrl(result.ImageUrl);

            return result;
        }


        public int SaveHelpConfig(HelpConfigEntity entity, string loginName)
        {
            BaseSetDA da = new BaseSetDA();
            return da.SaveHelpConfig(entity, loginName);
        }

        public int EnableHelpConfig(string Id, int status)
        {
            BaseSetDA da = new BaseSetDA();
            return da.EnableHelpConfig(Id, status);
        }

        public List<VMImages> GetHelpImage(string id)
        {
            BaseSetDA da = new BaseSetDA();

            List<VMImages> imgs = new List<VMImages>();

            var result = da.GetHelpConfigByKey(id);
            result.ImageUrl = CommonHelper.LinkImageUrl(result.ImageUrl);


            int begin = result.ImageUrl.IndexOf("/Images");
            if (begin > 0)
            {
                VMImages img = new VMImages()
                {
                    imageId = result.HelpConfigId,
                    name = result.Remark,
                    url = result.ImageUrl
                };
                imgs.Add(img);
            }



            return imgs;
        }
        #endregion

        #region 公益金
        public List<VMPublicFunds> GetPublicFundsByCondition(string className)
        {
            BaseSetDA da = new BaseSetDA();
            return da.GetPublicFundsByCondition(className);
        }

        public PublicFundsEntity GetPublicFundsByKey(string id)
        {
            BaseSetDA da = new BaseSetDA();
            return da.GetPublicFundsByKey(id);
        }


        public int SavePublicFunds(PublicFundsEntity entity, string loginName)
        {
            BaseSetDA da = new BaseSetDA();
            return da.SavePublicFunds(entity, loginName);
        }

        public int EnablePublicFunds(string Id, int status)
        {
            BaseSetDA da = new BaseSetDA();
            return da.EnablePublicFunds(Id,status);
        }
        #endregion

        #region 首页管理
        public List<IndexImageEntity> GetIndexImagesByCondition(string KeyName)
        {
            BaseSetDA da = new BaseSetDA();
            var result=da.GetIndexImagesByCondition(KeyName);
            if(result!=null && result.Count>0)
            {
                foreach (var item in result)
                {
                    item.ImageUrl = CommonHelper.LinkImageUrl(item.ImageUrl);
                }
            }
            return result;
        }


        /// <summary>
        /// 获取所有首页图片
        /// </summary>
        /// <returns></returns>
        public List<IndexImageEntity> GetAllIndexImages()
        {
            BaseSetDA da = new BaseSetDA();

            var result = da.GetAllIndexImages();
            if (result != null && result.Count > 0)
            {
                foreach (var item in result)
                {
                    item.ImageUrl = CommonHelper.LinkImageUrl(item.ImageUrl);
                }
            }

            return result;
        }

        public IndexImageEntity GetIndexImageByKey(string id)
        {
            BaseSetDA da = new BaseSetDA();

            var result=da.GetIndexImageByKey(id);

            result.ImageUrl = CommonHelper.LinkImageUrl(result.ImageUrl);

            return result;
        }

        public List<VMImages> GetIndexImages(string id)
        {
            BaseSetDA da = new BaseSetDA();

            List<VMImages> imgs = new List<VMImages>();

            var result=da.GetIndexImageByKey(id);
            result.ImageUrl = CommonHelper.LinkImageUrl(result.ImageUrl);




      
            int begin = result.ImageUrl.IndexOf("/Images");
            if (begin > 0)
            {
                VMImages img = new VMImages()
                {
                    imageId = result.ImageId,
                    name = result.ImageTitle,
                    url = result.ImageUrl
                };
                imgs.Add(img);
            }  
        

            return imgs;
        }


        public int SaveIndexImage(IndexImageEntity indexImage, string loginName)
        {
            BaseSetDA da = new BaseSetDA();
            return da.SaveIndexImage(indexImage, loginName);
        }

        public int EnableImage(string ImageId, int status)
        {
            BaseSetDA da = new BaseSetDA();
            return da.EnableImage(ImageId, status);
        }

       
        #endregion

        #region 文章管理
        public List<ArticleEntity> GetArticlesByCondition(string KeyName)
        {
            BaseSetDA da = new BaseSetDA();
            var result = da.GetArticlesByCondition(KeyName);
            if (result != null && result.Count > 0)
            {
                foreach (var item in result)
                {
                    item.ImageUrl = CommonHelper.LinkImageUrl(item.ImageUrl);
                    item.ArticleType = new BaseSetBC().GetDicItemValueByKey(item.ArticleType, "ArticleKey").ItemValue;
                }
            }
            return result;
        }


       
        public List<ArticleEntity> GetAllArticles()
        {
            BaseSetDA da = new BaseSetDA();
            var result = da.GetAllArticles();
            if (result != null && result.Count > 0)
            {
                foreach (var item in result)
                {
                    item.ImageUrl = CommonHelper.LinkImageUrl(item.ImageUrl);
                    item.ArticleType = new BaseSetBC().GetDicItemValueByKey(item.ArticleType, "ArticleKey").ItemValue;
                }
            }
            return result;
        }

        public List<VMWxArticle> GetAllArticlesByType(string type)
        {
            BaseSetDA da = new BaseSetDA();
            var result = da.GetAllArticlesByType(type);
            if (result != null && result.Count > 0)
            {
                foreach (var item in result)
                {
                    item.ImageUrl = CommonHelper.LinkImageUrl(item.ImageUrl);
                    item.ArticleType = new BaseSetBC().GetDicItemValueByKey(item.ArticleType, "ArticleKey").ItemValue;
                }
            }
            return result;
        }

        public ArticleEntity GetArticleByKey(string id)
        {
            BaseSetDA da = new BaseSetDA();

            var result = da.GetArticleByKey(id);

            result.ImageUrl = CommonHelper.LinkImageUrl(result.ImageUrl);

            return result;
        }

        public List<VMImages> GetArticleImage(string id)
        {
            BaseSetDA da = new BaseSetDA();

            List<VMImages> imgs = new List<VMImages>();

            var result = da.GetArticleByKey(id);
            result.ImageUrl = CommonHelper.LinkImageUrl(result.ImageUrl);


            int begin = result.ImageUrl.IndexOf("/Images");
            if (begin > 0)
            {
                VMImages img = new VMImages()
                {
                    imageId = result.ArticleId,
                    name = result.ArticleTitle,
                    url = result.ImageUrl
                };
                imgs.Add(img);
            }        
                     


            return imgs;
        }


        public int SaveArticle(ArticleEntity Article, string loginName)
        {
            BaseSetDA da = new BaseSetDA();
            return da.SaveArticle(Article, loginName);
        }

        public int EnableArticle(string ArticleId, int status)
        {
            BaseSetDA da = new BaseSetDA();
            return da.EnableArticle(ArticleId, status);
        }       
        #endregion

        #region 点击量
        public int SaveClickCount(string type, string id, string openid)
        {
            BaseSetDA da = new BaseSetDA();
            return da.SaveClickCount(type, id, openid);
        }
        #endregion

    }
}
