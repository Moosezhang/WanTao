using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Model
{
    public class VMWxHistory
    {
        /// <summary>
        /// 文章ID
        /// </summary>

        public string Name { get; set; }

        /// <summary>
        /// 文章类型
        /// </summary>

        public string images { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>

        public string Abstract { get; set; }


        /// <summary>
        /// 图片存储Url
        /// </summary>

        public string ObjectType { get; set; }

        /// <summary>
        /// 图片超链接
        /// </summary>

        public string ObjectId { get; set; }

        public string OpenId { get;set; }

        public string link { get; set; }

      
    }
}
