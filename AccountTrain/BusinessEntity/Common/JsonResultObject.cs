using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Common
{
    public class JsonResultObject
    {
        /// <summary>
        /// 执行是否成功
        /// </summary>
        public bool IsSucceed { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 执行结果
        /// </summary>
        public object Result { get; set; }

    }
}
