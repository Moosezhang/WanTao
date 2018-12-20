using BusinessEntity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntitys
{
    /// <summary>
    /// 文章
    /// </summary>
    [EnitityMappingAttribute(TableName = "Train_Article")]
    public class ArticleEntity : Entity
    {
        /// <summary>
        /// 文章ID
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "ArticleId")]
        public string ArticleId { get; set; }

        /// <summary>
        /// 文章类型
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "ArticleType")]
        public string ArticleType { get; set; }


        [EnitityMappingAttribute(ColumnName = "ArticleGroup")]
        public string ArticleGroup { get; set; }
        

        /// <summary>
        /// 文章标题
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "ArticleTitle")]
        public string ArticleTitle { get; set; }


        /// <summary>
        /// 图片存储Url
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "ImageUrl")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// 图片超链接
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "ImageLink")]
        public string ImageLink { get; set; }
        
        /// <summary>
        /// 备注	
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Remark")]
        public string Remark { get; set; }


        /// <summary>
        /// 状态
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Status")]
        public int Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "CreateUser")]
        public string CreateUser { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "UpdateTime")]
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "UpdateUser")]
        public string UpdateUser { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "StartTime")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "EndTime")]
        public DateTime EndTime { get; set; }
      

    }
}
