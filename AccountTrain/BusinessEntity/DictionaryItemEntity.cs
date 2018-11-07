using BusinessEntity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntitys
{
    /// <summary>
    /// 字典明细表
    /// </summary>
    [EnitityMappingAttribute(TableName = "Train_DictionaryItems")]
    public class DictionaryItemEntity : Entity
    {
        /// <summary>
        /// 字典ID
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "ItemId")]
        public string ItemId { get; set; }

        /// <summary>
        /// 字典值Key
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "ItemKey")]
        public string ItemKey { get; set; }

        /// <summary>
        /// 字典值Value
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "ItemValue")]
        public string ItemValue { get; set; }

        /// <summary>
        /// 字典键值key
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "DictionaryKey")]
        public string DictionaryKey { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Remark")]
        public string Remark { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "OrderNum")]
        public int OrderNum { get; set; }

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

        [EnitityMappingAttribute(ColumnName = "DictionaryLevel")]
        public int DictionaryLevel { get; set; }


    }
}
