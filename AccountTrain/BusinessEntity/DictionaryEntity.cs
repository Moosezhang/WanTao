using BusinessEntity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntitys
{
    /// <summary>
    /// 字典键值表
    /// </summary>
    [EnitityMappingAttribute(TableName = "Train_Dictionary")]
    public class DictionaryEntity : Entity
    {
        /// <summary>
        /// 字典键值ID
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "DictionaryId")]
        public string DictionaryId { get; set; }

        /// <summary>
        /// 字典键值名称
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "DictionaryName")]
        public string DictionaryName { get; set; }

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
