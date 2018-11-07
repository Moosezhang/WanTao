using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Model
{
    public class VMWxUserPoints
    {
        /// <summary>
        /// 用户Id  
        /// </summary>
      
        public string WxUserId { get; set; }

        /// <summary>
        /// 用户是否订阅该公众号标识
        /// </summary>
        
        public int Subscribe { get; set; }

        /// <summary>
        /// 用户的标识
        /// </summary>
      
        public string Openid { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
      
        public string Nickname { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
    
        public int Sex { get; set; }

        /// <summary>
        /// 所在城市
        /// </summary>
      
        public string City { get; set; }




        /// <summary>
        /// 所在国家
        /// </summary>
       
        public string Country { get; set; }

        /// <summary>
        /// 所在省份
        /// </summary>
       
        public string Province { get; set; }

        /// <summary>
        /// 用户的语言
        /// </summary>
       
        public string UserLanguage { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
       
        public string Headimgurl { get; set; }
        /// <summary>
        /// 用户关注时间
        /// </summary>
      
        public DateTime Subscribetime { get; set; }

        /// <summary>
        /// 公众号运营者对粉丝的备注
        /// </summary>
       
        public string Remark { get; set; }



        /// <summary>
        ///用户所在的分组ID
        /// </summary>
      
        public string Groupid { get; set; }

        /// <summary>
        /// 用户被打上的标签ID列表
        /// </summary>
        
        public string Tagidlist { get; set; }

        /// <summary>
        /// 返回用户关注的渠道来源
        /// </summary>
      
        public string Subscribescene { get; set; }

        /// <summary>
        /// 二维码扫码场景
        /// </summary>
      
        public string Qrscene { get; set; }
        /// <summary>
        /// 二维码扫码场景描述
        /// </summary>
     
        public string Qrscenestr { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
     
        public string Phone { get; set; }



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
        /// 积分
        /// </summary>

        public decimal Points { get; set; }
    }
}
