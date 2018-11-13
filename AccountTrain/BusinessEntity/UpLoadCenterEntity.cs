using BusinessEntity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntitys
{
    /// <summary>
    /// 职位信息表
    /// </summary>
    [EnitityMappingAttribute(TableName = "Train_UpLoadCenter")]
    public class UpLoadCenterEntity : Entity
    {
        /// <summary>
        /// 职位ID
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "UpLoadId")]
        public string UpLoadId { get; set; }

        /// <summary>
        /// 职位标题
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "UpLoadTitle")]
        public string UpLoadTitle { get; set; }

        /// <summary>
        /// 职位描述
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "UpLoadUrl")]
        public string UpLoadUrl { get; set; }

        /// <summary>
        /// 任职要求
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "UpLoadType")]
        public string UpLoadType { get; set; }       

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
