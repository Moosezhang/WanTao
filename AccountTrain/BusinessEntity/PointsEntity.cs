using BusinessEntity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntitys
{
    /// <summary>
    /// 积分表
    /// </summary>
    [EnitityMappingAttribute(TableName = "Train_Points")]
    public class PointsEntity : Entity
    {
        /// <summary>
        /// 积分ID
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "PointsId")]
        public string PointsId { get; set; }

        /// <summary>
        /// OpenId
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "OpenId")]
        public string OpenId { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Points")]
        public decimal Points { get; set; }
       

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
