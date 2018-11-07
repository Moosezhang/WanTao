using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Model
{
    public class VMBargainConfig
    {
        /// <summary>
        /// 砍价规则ID
        /// </summary>
       
        public string BargainConfigId { get; set; }

        /// <summary>
        /// 课程ID
        /// </summary>
 
        public string ClassId { get; set; }

        public string ClassName {get;set;}

        /// <summary>
        /// 底价
        /// </summary>
     
        public decimal FloorPrice { get; set; }

        /// <summary>
        /// 最多砍多少
        /// </summary>
       
        public decimal BargainTop { get; set; }
        /// <summary>
        /// 最少砍多少
        /// </summary>
   
        public decimal BargainFloor { get; set; }

        /// <summary>
        /// 备注	
        /// </summary>
      
        public string Remark { get; set; }


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

      
    }
}
