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
    public class UserBC
    {
        public IList<UserEntity> GetUsers(string username,string psw)
        {
           var da = new UserDA();
           return da.GetUsers(username, psw);
        }

        public List<VMUserInfo> GetUserByCondition(string userName)
        {
            UserDA da = new UserDA();
            var result = da.GetUserByCondition(userName);
            return result;
        }

        public int SaveUser(UserEntity user, string loginName)
        {
            if (string.IsNullOrEmpty(user.User_Id))
            {
                return AddUser(user, loginName);
            }
            else
            {
                return UpdateUser(user, loginName);
            }
        }

        public int UpdateUser(UserEntity user,string loginName)
        {
            UserDA da = new UserDA();
            var result = da.UpdateUser(user, loginName);
            return result;
        }

        public int AddUser(UserEntity user,string loginName)
        {
            UserDA da = new UserDA();
            var result = da.AddUser(user, loginName);
            return result;
        }

        public int EnableUser(string userId, int status)
        {
            return new UserDA().EnableUser(userId, status);
        }

        public UserEntity GetUserByKey(string userId)
        {
            UserDA da = new UserDA();
            var result = da.GetUserByKey(userId);
            return result;
        }

        public int SavePassword(UserEntity user, string loginName)
        {
            UserDA da = new UserDA();
            var result = da.SavePassword(user, loginName);
            return result;
        }



    }
}
