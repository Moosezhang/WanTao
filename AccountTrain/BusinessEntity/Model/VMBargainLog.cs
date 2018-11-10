
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Model
{
    public class VMBargainLog
    {
        /// <summary>
        /// 砍价规则ID
        /// </summary>
      
        public string LogId { get; set; }

        /// <summary>
        /// 课程ID
        /// </summary>
       
        public string BargainId { get; set; }

        /// <summary>
        /// 底价
        /// </summary>
     
        public string OpenId { get; set; }

        /// <summary>
        /// 最多砍多少
        /// </summary>
      
        public decimal BargainPrice { get; set; }



        /// <summary>
        /// 状态
        /// </summary>
       
        public int Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
     
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
      
        public string CreateUser { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
   
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
      
        public string UpdateUser { get; set; }

        public string Nickname { get; set; }

        public string Headimgurl { get; set; }

      
    }
}
