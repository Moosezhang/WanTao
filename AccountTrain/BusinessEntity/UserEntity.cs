using BusinessEntity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntitys
{
    /// <summary>
    /// 用户表
    /// </summary>
    [EnitityMappingAttribute(TableName = "Train_User")]
    public class UserEntity : Entity
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "User_Id")]
        public string User_Id { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "User_Name")]
        public string User_Name { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Login_Name")]
        public string Login_Name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "PassWord")]
        public string PassWord { get; set; }

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


        /// <summary>
        /// 角色Id
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Role_Id")]
        public string Role_Id { get; set; }
    }
}
