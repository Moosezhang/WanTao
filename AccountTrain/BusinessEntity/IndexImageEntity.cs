using BusinessEntity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntitys
{
    /// <summary>
    /// 首页banner图片
    /// </summary>
    [EnitityMappingAttribute(TableName = "Train_IndexImage")]
    public class IndexImageEntity : Entity
    {
        /// <summary>
        /// 图片ID
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "ImageId")]
        public string ImageId { get; set; }

        /// <summary>
        /// 图片标题
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "ImageTitle")]
        public string ImageTitle { get; set; }

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


      

    }
}
