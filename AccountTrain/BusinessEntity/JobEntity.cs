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
    [EnitityMappingAttribute(TableName = "Train_Job")]
    public class JobEntity : Entity
    {
        /// <summary>
        /// 职位ID
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "JobId")]
        public string JobId { get; set; }

        /// <summary>
        /// 职位标题
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "JobTitle")]
        public string JobTitle { get; set; }

        /// <summary>
        /// 职位描述
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "JobDesc")]
        public string JobDesc { get; set; }

        /// <summary>
        /// 任职要求
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "JobRequirements")]
        public string JobRequirements { get; set; }
        /// <summary>
        /// 职位薪资
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "JobSalary")]
        public string JobSalary { get; set; }

        /// <summary>
        /// 工龄要求
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "JobYear")]
        public string JobYear { get; set; }


        /// <summary>
        /// 学历要求
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "JobEducation")]
        public string JobEducation { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 所属公司
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "JobCompany")]
        public string JobCompany { get; set; }


        /// <summary>
        /// 联系邮箱
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "JobEmail")]
        public string JobEmail { get; set; }

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
