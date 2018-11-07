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
    public class RoleDA
    {
        public List<RoleEntity> GetRoles()
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format("SELECT * FROM Train_Role WHERE status=1 ");
                return conn.Query<RoleEntity>(query).ToList();
            }
        }

        public List<RoleMenuEntity> GetMenusByRoleId(string RoleId)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format("SELECT * FROM Train_RoleMenu WHERE status=1 and Role_Id=@Role_Id");
                return conn.Query<RoleMenuEntity>(query, new { Role_Id = RoleId }).ToList();
            }
        }

        public int UpdateRole(RoleEntity role, string loginName)
        {


            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"update train_role set role_name='{0}' ,update_time=getdate(),update_user='{1}' where role_id='{2}'",
                                            role.Role_Name, loginName, role.Role_Id);
                return conn.Execute(query);
            }

            //string strConn = ConfigSettings.ConnectionString;
            //string strSql = @"update train_user set user_name='{0}' , login_name='{1}',Password='{2}',update_time=getdate(),update_user='{3}' where user_id='{4}'";
            //strSql = string.Format(strSql,);
            //return SqlHelper.ExecuteNonQuery(strConn, CommandType.Text, strSql);
        }

        public int AddRole(RoleEntity role, string loginName)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"INSERT INTO Train_Role
                                               (Role_Id
                                               ,Role_Name
                                               ,Status
                                               ,Create_Time
                                               ,Create_User
                                               ,Update_Time
                                               ,Update_User
                                               )
                                         VALUES
                                               ('{0}'
                                               ,'{1}'
                                               ,'{2}' 
                                               ,getdate()
                                               ,'{3}'
                                               ,getdate()
                                               ,'{4}')",
                    Guid.NewGuid().ToString(), role.Role_Name,  1, loginName, loginName);
                return conn.Execute(query);
            }
        }

        public int EnableRole(string roleId, int status)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"update train_role set status={0}  where role_id='{1}'", status, roleId);
                return conn.Execute(query);
            }
        }

        public RoleEntity GetRoleByKey(string roleId)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format("SELECT * FROM Train_Role WHERE status=1 and role_id=@role_id");
                var result = conn.Query<RoleEntity>(query, new { role_id = roleId });
                if (result == null)
                    return new RoleEntity();
                return result.FirstOrDefault();
            }
        }

        public int DeleteRoleMenuByRoleId(string roleId)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"delete Train_RoleMenu where role_id='{0}'", roleId);
                return conn.Execute(query);
            }
        }

        public int SaveRoleMenu(string roleId, string menuId, string loginName)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"INSERT INTO Train_RoleMenu
                                               (RoleMenu_Id
                                               ,Role_Id
                                               ,Menu_Id
                                               ,Status
                                               ,Create_Time
                                               ,Create_User
                                               ,Update_Time
                                               ,Update_User
                                               )
                                         VALUES
                                               ('{0}'
                                               ,'{1}'
                                               ,'{2}'
                                               ,'{3}' 
                                               ,getdate()
                                               ,'{4}'
                                               ,getdate()
                                               ,'{5}')",
                    Guid.NewGuid().ToString(), roleId, menuId, 1, loginName, loginName);
                return conn.Execute(query);
            }
        }
    }
}
