using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Model
{
    public class VMHelpConfig
    {
        /// <summary>
        /// 助力规则ID
        /// </summary>

        public string HelpConfigId { get; set; }

        /// <summary>
        /// 课程ID
        /// </summary>
      
        public string ClassId { get; set; }

        public string ClassName { get; set; }

        /// <summary>
        /// 助力成功人数
        /// </summary>
    
        public int HelpCount { get; set; }

        /// <summary>
        /// 助力金额
        /// </summary>
        
        public decimal HelpPrice { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
      
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 截止时间
        /// </summary>
      
        public DateTime EndTime { get; set; }

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
