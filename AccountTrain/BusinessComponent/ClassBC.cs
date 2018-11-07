using BusinessEntity.Model;
using BusinessEntitys;
using Common;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessComponent
{
    public class ClassBC
    {
        #region 课程管理
        public List<ClassEntity> GetAllClass()
        {
            ClassDA da = new ClassDA();
            return da.GetAllClass();
        }

        public List<ClassEntity> GetClassByCondition(string name, string classType, string startDate, string endDate)
        {
            ClassDA da = new ClassDA();

            var result = da.GetClassByCondition(name, classType, startDate, endDate);
            if (result != null && result.Count > 0)
            {
                foreach (var item in result)
                {
                    item.ClassType = new BaseSetBC().GetDicItemValueByKey(item.ClassType, "ClassKey").ItemValue;
                }
            }

            return result;
        }

        public ClassEntity GetClassByKey(string id)
        {
            ClassDA da = new ClassDA();

            var result = da.GetClassByKey(id);

            result.ClassImages = CommonHelper.LinkImageUrl(result.ClassImages);

            return result;
        }

        public List<VMImages> GetClassImage(string id)
        {
            ClassDA da = new ClassDA();

            List<VMImages> imgs = new List<VMImages>();

            var result = da.GetClassByKey(id);
            result.ClassImages = CommonHelper.LinkImageUrl(result.ClassImages);

            int begin = result.ClassImages.IndexOf("/Images");
            if (begin > 0)
            {
                VMImages img = new VMImages()
                {
                    imageId = result.ClassId,
                    name = result.ClassName,
                    url = result.ClassImages
                };
                imgs.Add(img);
            }        


            return imgs;
        }


        public int SaveClass(ClassEntity Class, string loginName)
        {
            ClassDA da = new ClassDA();
            return da.SaveClass(Class, loginName);
        }

        public int EnableClass(string ClassId, int status)
        {
            ClassDA da = new ClassDA();
            return da.EnableClass(ClassId, status);
        }


        public List<VMOrderClass> GetMyClassByopenId(string openid, string title)
        {
            ClassDA da = new ClassDA();
            return da.GetMyClassByopenId(openid, title);
        }
      
        #endregion

        #region 章节管理
        public List<VMClassChapter> GetChaptersByCondition(string className, string chapterTitle)
        {
            ClassDA da = new ClassDA();
            return da.GetChaptersByCondition(className, chapterTitle);
        }



        /// <summary>
        /// 根据键值key获取items
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<ChapterEntity> GetChaptersByClassKey(string key)
        {
            ClassDA da = new ClassDA();
            return da.GetChaptersByClassKey(key);
        }

        public ChapterEntity GetChapterByKey(string id)
        {
            ClassDA da = new ClassDA();
            return da.GetChapterByKey(id);
        }


        public int SaveChapter(ChapterEntity entity, string loginName)
        {
            ClassDA da = new ClassDA();
            return da.SaveChapter(entity, loginName);
        }

        public int EnableChapter(string id, int status)
        {
            ClassDA da = new ClassDA();
            return da.EnableChapter(id, status);
        }
        #endregion
    }
}
