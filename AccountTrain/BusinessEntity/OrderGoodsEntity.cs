using BusinessEntity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntitys
{
    /// <summary>
    /// 订单商品表
    /// </summary>
    [EnitityMappingAttribute(TableName = "Train_OrderGoods")]
    public class OrderGoodsEntity : Entity
    {
        /// <summary>
        /// 订单商品ID
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "GoodsId")]
        public string GoodsId { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "OrderId")]
        public string OrderId { get; set; }

        /// <summary>
        /// 课程ID
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "ClassId")]
        public string ClassId { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "ClassName")]
        public string ClassName { get; set; }

        /// <summary>
        /// 实际价格
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Price")]
        public decimal Price { get; set; }


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
