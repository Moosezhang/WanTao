using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Model
{
    public class VMPublicFunds
    {
        /// <summary>
        /// 公益金ID
        /// </summary>
     
        public string PublicFundsId { get; set; }

        /// <summary>
        /// 课程ID
        /// </summary>
    
        public string ClassId { get; set; }

        public string ClassName { get; set; }
        /// <summary>
        /// 公益金金额
        /// </summary>
     
        public decimal FundsPrice { get; set; }

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

        public string FundsText { get; set; }

      
    }
}
