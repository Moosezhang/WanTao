using BusinessEntity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntitys
{
    /// <summary>
    /// 订单表
    /// </summary>
    [EnitityMappingAttribute(TableName = "Train_Order")]
    public class OrderEntity : Entity
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "OrderId")]
        public string OrderId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "OrderNo")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 用户的标识，对当前公众号唯一
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Openid")]
        public string Openid { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Nickname")]
        public string Nickname { get; set; }
        

        /// <summary>
        /// 实际价格
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "PayPrice")]
        public decimal PayPrice { get; set; }


        /// <summary>
        /// 订单来源（1.单独购买，2.砍价，3.团购，4.助力）
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "OrderSource")]
        public string OrderSource { get; set; }

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
