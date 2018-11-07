using BusinessEntity.Model;
using BusinessEntitys;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessComponent
{
    public class WxUserBC
    {

        public List<WxUserEntity> GetWxUserListByCondition(string name, string phone, string startDate, string endDate)
        {
            WxUserDA da = new WxUserDA();
            return da.GetWxUserListByCondition(name, phone, startDate, endDate);
        }

        public VMWxUserPoints GetWxUserByOpenid(string Openid)
        {
            WxUserDA da = new WxUserDA();
            return da.GetWxUserByOpenid(Openid);
        }

        public int SaveWxUser(WxUserEntity wxUser, string loginName)
        {
            WxUserDA da = new WxUserDA();
            return da.SaveWxUser(wxUser, loginName);
        }
    }
}
