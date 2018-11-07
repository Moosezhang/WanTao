using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Model
{
    public class VMUserInfo
    {
        public string UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
      
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
