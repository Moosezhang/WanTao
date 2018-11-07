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
    public class UserDA
    {
        public IList<UserEntity> GetUsers(string username, string psw)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format("SELECT * FROM Train_User WHERE status=1 and Login_name=@username and password=@psw");
                return conn.Query<UserEntity>(query, new { username = username, psw = psw }).ToList();
            }
        }

        public List<VMUserInfo> GetUserByCondition(string userName)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select t.user_id as UserId,t.user_name as UserName,t.login_name as LoginName,t.Password,t.Status,
                                t.create_time as CreateTime,t.create_user as CreateUser,t.update_time as UpdateTime,t.update_user as UpdateUser,
                                t2.role_id as RolelId,t2.role_name as RoleName
                                from train_user t
                                inner join train_role t2 on t.role_id=t2.role_id
                                where t.status=1");
                if (!string.IsNullOrEmpty(userName))
                {
                    string sql = string.Format(" and t.user_name='{0}'",userName);
                    query = query + sql;
                    return conn.Query<VMUserInfo>(query ).ToList();
                }
                else
                {
                    return conn.Query<VMUserInfo>(query).ToList();
                }
                
            }
        }

        public int UpdateUser(UserEntity user, string loginName)
        {


            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"update train_user set user_name='{0}' , login_name='{1}',Password='{2}',update_time=getdate(),update_user='{3}',role_id='{4}' where user_id='{5}'",
                                            user.User_Name, user.Login_Name, user.PassWord, loginName, user.Role_Id, user.User_Id);
                return conn.Execute(query);
            }
        }

        public int SavePassword(UserEntity user, string loginName)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"update train_user set Password='{0}',update_time=getdate(),update_user='{1}' where user_id='{2}'",
                                            user.PassWord, loginName, user.User_Id);
                return conn.Execute(query);
            }
        }
        

        public int AddUser(UserEntity user, string loginName)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"INSERT INTO Train_User
                                               (User_Id
                                               ,User_Name
                                               ,Login_Name
                                               ,PassWord
                                               ,Status
                                               ,Create_Time
                                               ,Create_User
                                               ,Update_Time
                                               ,Update_User
                                               ,Role_Id)
                                         VALUES
                                               ('{0}'
                                               ,'{1}'
                                               ,'{2}'
                                               ,'{3}'
                                               ,'{4}'
                                               ,getdate()
                                               ,'{5}'
                                               ,getdate()
                                               ,'{6}'
                                               ,'{7}')",
                    Guid.NewGuid().ToString(), user.User_Name, user.Login_Name, user.PassWord, 1, loginName, loginName, user.Role_Id);
                return conn.Execute(query);
            }
        }

        public int EnableUser(string userId, int status)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"update train_user set status={0}  where user_id='{1}'",status, userId);
                return conn.Execute(query);
            }
        }

        public UserEntity GetUserByKey(string userId)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format("SELECT * FROM Train_User WHERE status=1 and user_id=@userId");
                var result = conn.Query<UserEntity>(query, new { userId = userId });
                if (result == null)
                    return new UserEntity();
                return result.FirstOrDefault();
            }
        }
    }
}
