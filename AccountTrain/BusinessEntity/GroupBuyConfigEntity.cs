using BusinessEntity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntitys
{
    /// <summary>
    /// 团购规则表
    /// </summary>
    [EnitityMappingAttribute(TableName = "Train_GroupBuyConfig")]
    public class GroupBuyConfigEntity : Entity
    {
        /// <summary>
        /// 砍价规则ID
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "GroupBuyConfigId")]
        public string GroupBuyConfigId { get; set; }

        /// <summary>
        /// 课程ID
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "ClassId")]
        public string ClassId { get; set; }

        /// <summary>
        /// 团购价
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "GroupPrice")]
        public decimal GroupPrice { get; set; }

        /// <summary>
        /// 成团人数
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "NeedCount")]
        public int NeedCount { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "StartTime")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 截止时间
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "EndTime")]
        public DateTime EndTime { get; set; }


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
