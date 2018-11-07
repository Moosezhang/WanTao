using BusinessEntity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntitys
{
    /// <summary>
    /// 菜单表
    /// </summary>
    [EnitityMappingAttribute(TableName = "Train_Menu")]
    public class MenuEntity : Entity
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Menu_Id")]
        public string Menu_Id { get; set; }

        /// <summary>
        /// 菜单标题
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Menu_Title")]
        public string Menu_Title { get; set; }

        /// <summary>
        /// 菜单URl
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Menu_Url")]
        public string Menu_Url { get; set; }

        /// <summary>
        /// 父菜单Id
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Menu_FatherId")]
        public string Menu_FatherId { get; set; }

        /// <summary>
        /// 菜单顺序
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Menu_Order")]
        public int Menu_Order { get; set; } 

        /// <summary>
        /// 状态
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Status")]
        public int Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Create_Time")]
        public DateTime Create_Time { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Create_User")]
        public string Create_User { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Update_Time")]
        public DateTime Update_Time { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Update_User")]
        public string Update_User { get; set; }

      

    }
}
