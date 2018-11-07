using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Model
{
    public class VMClassChapter
    {
        /// <summary>
        /// 章节ID
        /// </summary>
  
        public string ChapterId { get; set; }

        /// <summary>
        /// 课程ID
        /// </summary>
     
        public string ClassId { get; set; }

        /// <summary>
        /// 职位标题
        /// </summary>
       
        public string ChapterTitle { get; set; }

        /// <summary>
        /// 章节介绍
        /// </summary>
     
        public string ChapterContent { get; set; }

        /// <summary>
        /// 音视频Url
        /// </summary>
      
        public string VideoUrl { get; set; }
        /// <summary>
        /// 音视频标题
        /// </summary>
     
        public string VideoTitle { get; set; }

        /// <summary>
        /// 章节顺序
        /// </summary>
      
        public int ChapterNum { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }


        /// <summary>
        /// 是否启用
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 所属角色
        /// </summary>
        public string ClassName { get; set; }

        public DateTime CreateTime { get; set; }

        public string CreateUser { get; set; }

        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }

      
    }
}
