﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Model
{
    public class VMBargainClass
    {
        /// <summary>
        /// 课程ID
        /// </summary>
     
        public string ClassId { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
      
        public string ClassName { get; set; }

        /// <summary>
        /// 课程简介
        /// </summary>
        public string ClassAbstract { get; set; }

        /// <summary>
        /// 课程类型
        /// </summary>
    
        public string ClassType { get; set; }

        /// <summary>
        /// 课程详情
        /// </summary>
        public string ClassContent { get; set; }
        /// <summary>
        /// 课程配图Url
        /// </summary>
     
        public string ClassImages { get; set; }

        /// <summary>
        /// 课程价格
        /// </summary>
       
        public decimal ClassPrice { get; set; }

      

        /// <summary>
        /// 课程讲师
        /// </summary>
      
        public string ClassTeacher { get; set; }

        /// <summary>
        /// 热度（多少人买过，冗余字段）
        /// </summary>
     
        public int HotCount { get; set; }
        /// <summary>
        /// 是否允许团购
        /// </summary>
   
        public string IsGroupBuy { get; set; }


        /// <summary>
        /// 是否允许砍价
        /// </summary>
    
        public string IsBargain { get; set; }

        /// <summary>
        /// 是否允许助力
        /// </summary>
      
        public string IsHelp { get; set; }

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

        /// <summary>
        /// 已砍价
        /// </summary>
        public decimal PrePrice { get; set; }

        /// <summary>
        /// 还可砍价
        /// </summary>
        public decimal NowPrice { get; set; }

        public decimal FloorPrice { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 截止如期
        /// </summary>
        public DateTime EndTime { get; set; }


        public string ownerOpenid { get; set; }

      
    }
}
