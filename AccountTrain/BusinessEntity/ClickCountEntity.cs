using BusinessEntity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntitys
{
    /// <summary>
    /// 点击量记录表
    /// </summary>
    [EnitityMappingAttribute(TableName = "Train_ClickCount")]
    public class ClickCountEntity : Entity
    {
        /// <summary>
        /// 点击Id
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "ClickId")]
        public string ClickId { get; set; }

        /// <summary>
        /// 课程或者文章Id
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "ObjectId")]
        public string ObjectId { get; set; }

        /// <summary>
        /// 点击类型：1.课程 2.文章ID
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "ObjectType")]
        public decimal ObjectType { get; set; }
       

        /// <summary>
        /// OpenId
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "OpenId")]
        public string OpenId { get; set; }



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
