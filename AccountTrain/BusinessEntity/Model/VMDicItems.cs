using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Model
{
    public class VMDicItems
    {
        /// <summary>
        /// 字典ID
        /// </summary>
        public string ItemId { get; set; }
        /// <summary>
        /// 字典值Key
        /// </summary>
        public string ItemKey { get; set; }

        /// <summary>
        /// 字典值Value
        /// </summary>
        public string ItemValue { get; set; }
        /// <summary>
        /// 字典键值key
        /// </summary>
        public string DictionaryKey { get; set; }

        /// <summary>
        /// 字典键值 名称
        /// </summary>
        public string DictionaryName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderNum { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 所属角色
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 所属角色ID
        /// </summary>
        public string RolelId { get; set; }

        public DateTime CreateTime { get; set; }

        public string CreateUser { get; set; }

        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }

      
    }
}
