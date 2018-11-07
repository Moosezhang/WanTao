using BusinessEntity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntitys
{
    /// <summary>
    /// 砍价规则表
    /// </summary>
    [EnitityMappingAttribute(TableName = "Train_BargainConfig")]
    public class BargainConfigEntity : Entity
    {
        /// <summary>
        /// 砍价规则ID
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "BargainConfigId")]
        public string BargainConfigId { get; set; }

        /// <summary>
        /// 课程ID
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "ClassId")]
        public string ClassId { get; set; }

        /// <summary>
        /// 底价
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "FloorPrice")]
        public decimal FloorPrice { get; set; }

        /// <summary>
        /// 最多砍多少
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "BargainTop")]
        public decimal BargainTop { get; set; }
        /// <summary>
        /// 最少砍多少
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "BargainFloor")]
        public decimal BargainFloor { get; set; }

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
