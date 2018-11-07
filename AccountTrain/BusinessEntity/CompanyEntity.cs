using BusinessEntity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntitys
{
    /// <summary>
    /// 公司信息表
    /// </summary>
    [EnitityMappingAttribute(TableName = "Train_CompanyInfo")]
    public class CompanyEntity : Entity
    {
        /// <summary>
        /// 公司ID
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "CompanyId")]
        public string CompanyId { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "CompanyName")]
        public string CompanyName { get; set; }

        /// <summary>
        /// 公司地址
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "CompanyAddress")]
        public string CompanyAddress { get; set; }

        /// <summary>
        /// 公司电话
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "CompanyTelephone")]
        public string CompanyTelephone { get; set; }
        /// <summary>
        /// 公司邮箱
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "CompanyEmail")]
        public string CompanyEmail { get; set; }

        /// <summary>
        /// 联系人名称
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "ContactName")]
        public string ContactName { get; set; }


        /// <summary>
        /// 联系人电话
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "ContactPhone")]
        public string ContactPhone { get; set; }

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
