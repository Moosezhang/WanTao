using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum DataBaseName
    {
        /// <summary>
        /// 数据库
        /// </summary>
        AccountTrianDB
    }

    public enum ReadOrWriteDB
    {
        /// <summary>
        /// 写
        /// </summary>
        Write,
        /// <summary>
        /// 读
        /// </summary>
        Read,
    }

    #region sql作者
    /// <summary>
    /// sql作者
    /// </summary>
    public enum SqlAuthor
    {
        沈志荣
    }
    #endregion

    /// <summary>
    /// 0 否,1 是
    /// </summary>
    public enum YesOrNo
    {
        /// <summary>
        /// 否
        /// </summary>
        No = 0,
        /// <summary>
        /// 是
        /// </summary>
        Yes = 1
    }

    /// <summary>
    /// 1未支付 2已支付(等待抢单) 3已派单（配送员已抢单）4配送中(仓库已发货)
    /// 5配送完毕（配送员点）6已完结（2小时或店家点） 7已取消
    /// </summary>
    public enum OrderState
    {
        [Description("未支付")]
        NotPay = 1,
        [Description("已支付(等待抢单)")]
        HasPay = 2,
        [Description("已派单（配送员已抢单）")]
        HasAssigned = 3,
        [Description("配送中(仓库已发货)")]
        Distributing = 4,
        [Description("配送完毕（配送员点）")]
        Distributed = 5,
        [Description("已完结（2小时或店家点）")]
        Closed = 6,
        [Description("已取消")]
        Cancelled = 7
    }

    public enum MoneyState
    {
        /// <summary>
        /// 待处理
        /// </summary>
        InProcess = 0,
        /// <summary>
        /// 失败
        /// </summary>
        Faild = 1,
        /// <summary>
        /// 成功
        /// </summary>
        Success = 2,
        /// <summary>
        /// 已计算
        /// </summary>
        Calculated=3
    }
}
