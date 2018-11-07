using BusinessEntity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntitys
{
    /// <summary>
    /// 砍价规则表
    /// </summary>
    [EnitityMappingAttribute(TableName = "Train_WxUser")]
    public class WxUserEntity : Entity
    {
        /// <summary>
        /// 用户Id  
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "WxUserId")]
        public string WxUserId { get; set; }

        /// <summary>
        /// 用户是否订阅该公众号标识
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Subscribe")]
        public int Subscribe { get; set; }

        /// <summary>
        /// 用户的标识
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Openid")]
        public string Openid { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Nickname")]
        public string Nickname { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Name")]
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Sex")]
        public int Sex { get; set; }

        /// <summary>
        /// 所在城市
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "City")]
        public string City { get; set; }




        /// <summary>
        /// 所在国家
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Country")]
        public string Country { get; set; }

        /// <summary>
        /// 所在省份
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Province")]
        public string Province { get; set; }

        /// <summary>
        /// 用户的语言
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "UserLanguage")]
        public string UserLanguage { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Headimgurl")]
        public string Headimgurl { get; set; }
        /// <summary>
        /// 用户关注时间
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Subscribetime")]
        public DateTime Subscribetime { get; set; }

        /// <summary>
        /// 公众号运营者对粉丝的备注
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Remark")]
        public string Remark { get; set; }



        /// <summary>
        ///用户所在的分组ID
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Groupid")]
        public string Groupid { get; set; }

        /// <summary>
        /// 用户被打上的标签ID列表
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Tagidlist")]
        public string Tagidlist { get; set; }

        /// <summary>
        /// 返回用户关注的渠道来源
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Subscribescene")]
        public string Subscribescene { get; set; }

        /// <summary>
        /// 二维码扫码场景
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Qrscene")]
        public string Qrscene { get; set; }
        /// <summary>
        /// 二维码扫码场景描述
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Qrscenestr")]
        public string Qrscenestr { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [EnitityMappingAttribute(ColumnName = "Phone")]
        public string Phone { get; set; }



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
