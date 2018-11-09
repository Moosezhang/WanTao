using BusinessEntity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntitys
{
    /// <summary>
    /// 团购规则表
    /// </summary>
    [EnitityMappingAttribute(TableName = "Train_GroupBuyMember")]
    public class GroupBuyMemberEntity : Entity
    {
        /// <summary>
        /// 砍价规则ID
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "MemberId")]
        public string MemberId { get; set; }

        /// <summary>
        /// 课程ID
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "GroupBuyId")]
        public string GroupBuyId { get; set; }

        /// <summary>
        /// 现有人数
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "GroupPrice")]
        public decimal GroupPrice { get; set; }      
       
         /// <summary>
        /// 课程ID
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "openId")]
        public string openId { get; set; }
        

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
