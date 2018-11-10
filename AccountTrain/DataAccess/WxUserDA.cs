using Common;
using DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using BusinessEntitys;
using BusinessEntity.Model;

namespace DataAccess
{
    public class WxUserDA
    {

        public List<WxUserEntity> GetWxUserListByCondition(string name, string phone, string startDate, string endDate)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select * from Train_WxUser
                                where 1=1");
                if (!string.IsNullOrEmpty(name))
                {
                    string sql = string.Format(" and Nickname='{0}'", name);
                    query = query + sql;
                }
                if (!string.IsNullOrEmpty(phone))
                {
                    string sql = string.Format(" and phone='{0}'", phone);
                    query = query + sql;
                }
                if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                {
                    string sql = string.Format(" and CreateTime between '{0}' and '{1}'", startDate, endDate);
                    query = query + sql;
                }

                return conn.Query<WxUserEntity>(query).ToList();
                
            }
        }

        public VMWxUserPoints GetWxUserByOpenid(string Openid)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select t.*,isnull(t1.Points,0) as Points from Train_WxUser t
                                               left join Train_Points t1 on t.Openid=t1.Openid
                                               where t.Openid='{0}'", Openid);

                return conn.Query<VMWxUserPoints>(query).FirstOrDefault();

            }
        }

        public int SaveWxUser(WxUserEntity wxUser, string loginName)
        {
            if (string.IsNullOrEmpty(wxUser.WxUserId))
            {
                using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
                {
                    string query = string.Format(@"INSERT INTO Train_WxUser
                                                   (WxUserId
                                                   ,Subscribe
                                                   ,Openid
                                                   ,Nickname
                                                   ,Sex
                                                   ,City
                                                   ,Country
                                                   ,Province
                                                   ,UserLanguage
                                                   ,Headimgurl
                                                   ,Status
                                                   ,Phone
                                                   ,CreateTime
                                                   ,CreateUser
                                                   ,UpdateTime
                                                   ,UpdateUser
                                                   ,Name)
                                             VALUES
                                                   ('{0}'
                                                   ,'{1}'
                                                   ,'{2}'
                                                   ,'{3}'
                                                   ,'{4}'
                                                   ,'{5}'
                                                   ,'{6}'
                                                   ,'{7}'
                                                   ,'{8}'
                                                   ,'{9}'   
                                                   ,'{10}'
                                                   ,'{11}'
                                                   ,GETDATE()
                                                   ,'{12}'
                                                   ,GETDATE()
                                                   ,'{13}'
                                                   ,'{14}')",
                        Guid.NewGuid().ToString(), wxUser.Subscribe, wxUser.Openid, wxUser.Nickname, wxUser.Sex, wxUser.City, wxUser.Country,
                        wxUser.Province, wxUser.UserLanguage, wxUser.Headimgurl, 1, wxUser.Phone, loginName, loginName, wxUser.Name);
                    return conn.Execute(query);
                }
            }
            else
            {
                using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
                {
                    string query = string.Format(@"UPDATE Train_WxUser
                                                   SET Subscribe = '{0}'
                                                      ,Nickname = '{1}'
                                                      ,Sex = '{2}'
                                                      ,City ='{3}'
                                                      ,Country = '{4}'
                                                      ,Province = '{5}'
                                                      ,UserLanguage = '{6}'
                                                      ,Headimgurl = '{7}'
                                                      ,Phone = '{8}'
                                                      ,Name = '{9}'
                                                      ,UpdateTime =getdate()
                                                      ,UpdateUser = '{10}'
                                                 WHERE WxUserId='{11}'",
                                                wxUser.Subscribe, wxUser.Nickname, wxUser.Sex, wxUser.City, wxUser.Country,
                                                wxUser.Province, wxUser.UserLanguage, wxUser.Headimgurl, wxUser.Phone, wxUser.Name, loginName, wxUser.WxUserId);
                    return conn.Execute(query);
                }
            }
        }

    }
}
