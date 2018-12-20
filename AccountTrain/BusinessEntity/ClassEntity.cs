using BusinessEntity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntitys
{
    /// <summary>
    /// 课程表
    /// </summary>
    [EnitityMappingAttribute(TableName = "Train_Class")]
    public class ClassEntity : Entity
    {
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
        /// 课程简介
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "ClassAbstract")]
        public string ClassAbstract { get; set; }

        [EnitityMappingAttribute(ColumnName = "ClassGroup")]
        public string ClassGroup { get; set; }

        /// <summary>
        /// 课程类型
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "ClassType")]
        public string ClassType { get; set; }

        /// <summary>
        /// 课程详情
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "ClassContent")]
        public string ClassContent { get; set; }
        /// <summary>
        /// 课程配图Url
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "ClassImages")]
        public string ClassImages { get; set; }

        /// <summary>
        /// 课程价格
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "ClassPrice")]
        public decimal ClassPrice { get; set; }


        /// <summary>
        /// 虚拟价格
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "VirPrice")]
        public decimal VirPrice { get; set; }
        

        /// <summary>
        /// 课程讲师
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "ClassTeacher")]
        public string ClassTeacher { get; set; }

        /// <summary>
        /// 热度（多少人买过，冗余字段）
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "HotCount")]
        public int HotCount { get; set; }
        /// <summary>
        /// 是否允许团购
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "IsGroupBuy")]
        public string IsGroupBuy { get; set; }


        /// <summary>
        /// 是否允许砍价
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "IsBargain")]
        public string IsBargain { get; set; }

        /// <summary>
        /// 是否允许助力
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "IsHelp")]
        public string IsHelp { get; set; }

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


        /// <summary>
        /// 开始时间
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "StartTime")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "EndTime")]
        public DateTime EndTime { get; set; }

    }
}
