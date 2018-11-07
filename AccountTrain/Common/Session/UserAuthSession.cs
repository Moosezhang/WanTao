using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountTrain.Common.Session
{
    [Serializable]
    public class UserAuthSession
    {
        public string Token { get; set; }

        public string AppKey { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// IP 地址
        /// </summary>
        public string IpAddress { get; set; }
        /// <summary>
        /// 医院ID
        /// </summary>
        public string HospitalId { get; set; }
        /// <summary>
        /// 医院名称
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 科室
        /// </summary>
        public string Dept { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否管理员
        /// </summary>
        public decimal? IsAdmin { get; set; }

        /// <summary>
        /// 一般用户是否审核通过
        /// </summary>
        public decimal? IsAudit { get; set; }

        /// <summary>
        /// 职务
        /// </summary>
        public string Job { get; set; }
    }
}
