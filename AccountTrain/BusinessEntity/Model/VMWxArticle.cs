using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Model
{
    public class VMWxArticle
    {
        /// <summary>
        /// 文章ID
        /// </summary>
       
        public string ArticleId { get; set; }

        /// <summary>
        /// 文章类型
        /// </summary>
       
        public string ArticleType { get; set; }

        public string ArticleGroup { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
      
        public string ArticleTitle { get; set; }


        /// <summary>
        /// 图片存储Url
        /// </summary>
      
        public string ImageUrl { get; set; }

        /// <summary>
        /// 图片超链接
        /// </summary>
      
        public string ImageLink { get; set; }



        public int ClickCount { get; set; }


        /// <summary>
        /// 状态
        /// </summary>
    
        public int Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
     
        public DateTime CreateTime { get; set; }
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
   
        public string CreateUser { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
      
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
      
        public string UpdateUser { get; set; }

      
    }
}
