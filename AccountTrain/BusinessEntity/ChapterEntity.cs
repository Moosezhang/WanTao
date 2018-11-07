using BusinessEntity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntitys
{
    /// <summary>
    /// 章节表
    /// </summary>
    [EnitityMappingAttribute(TableName = "Train_Chapter")]
    public class ChapterEntity : Entity
    {
        /// <summary>
        /// 章节ID
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "ChapterId")]
        public string ChapterId { get; set; }

        /// <summary>
        /// 课程ID
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "ClassId")]
        public string ClassId { get; set; }

        /// <summary>
        /// 职位标题
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "ChapterTitle")]
        public string ChapterTitle { get; set; }

        /// <summary>
        /// 章节介绍
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "ChapterContent")]
        public string ChapterContent { get; set; }

        /// <summary>
        /// 音视频Url
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "VideoUrl")]
        public string VideoUrl { get; set; }
        /// <summary>
        /// 音视频标题
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "VideoTitle")]
        public string VideoTitle { get; set; }

        /// <summary>
        /// 章节顺序
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "ChapterNum")]
        public int ChapterNum { get; set; }


       
        /// <summary>
        /// 备注
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Remark")]
        public string Remark { get; set; }


       
        /// <summary>
        /// 状态
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Status")]
        public int Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "CreateUser")]
        public string CreateUser { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "UpdateTime")]
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "UpdateUser")]
        public string UpdateUser { get; set; }

      

    }
}
