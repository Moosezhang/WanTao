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

namespace DataAccess
{
    public class AuthDA
    {
        /// <summary>
        /// 根据登录账号，获取菜单权限
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public IList<MenuEntity> GetMenusByLoginName(string loginName)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select Distinct t.*
                                                from Train_Menu t
                                                inner join Train_RoleMenu t1 on t.Menu_Id=t1.Menu_Id
                                                inner join Train_User t3 on t1.Role_Id=t3.Role_Id
                                                where t3.status=1  and t3.login_name=@loginName");
                return conn.Query<MenuEntity>(query, new { loginName = loginName }).ToList();
            }
        }

        /// <summary>
        /// 根据id获取menu信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MenuEntity GetMenuByMenuId(string id)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select t.*
                                                from Train_Menu t where t.menu_id=@id");
                return conn.Query<MenuEntity>(query, new { id = id }).FirstOrDefault();
            }
        }

        public List<MenuEntity> GetAllMenus()
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select t.*
                                                from Train_Menu t");
                return conn.Query<MenuEntity>(query).ToList();
            }
        }
    }
}
