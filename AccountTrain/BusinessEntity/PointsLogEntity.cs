using BusinessEntity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntitys
{
    /// <summary>
    /// 积分账单表
    /// </summary>
    [EnitityMappingAttribute(TableName = "Train_PointsLog")]
    public class PointsLogEntity : Entity
    {
        /// <summary>
        /// 积分记录ID
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "PointsLogId")]
        public string PointsLogId { get; set; }

        /// <summary>
        ///OpenId
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "OpenId")]
        public string OpenId { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "OrderId")]
        public string OrderId { get; set; }

        /// <summary>
        /// 类型，1.收入，2.支出
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "LogType")]
        public string LogType { get; set; }


        /// <summary>
        /// 单笔积分
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Points")]
        public string Points { get; set; }
       

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
