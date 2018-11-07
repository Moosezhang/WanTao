using BusinessEntity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntitys
{
    /// <summary>
    /// 砍价表
    /// </summary>
    [EnitityMappingAttribute(TableName = "Train_BargainLog")]
    public class BargainLogEntity : Entity
    {
        /// <summary>
        /// 砍价规则ID
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "LogId")]
        public string LogId { get; set; }

        /// <summary>
        /// 课程ID
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "BargainId")]
        public string BargainId { get; set; }

        /// <summary>
        /// 底价
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "OpenId")]
        public string OpenId { get; set; }

        /// <summary>
        /// 最多砍多少
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "BargainPrice")]
        public decimal BargainPrice { get; set; }
       


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
