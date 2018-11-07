using BusinessEntitys;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessComponent
{
    public class RoleBC
    {
        public List<RoleEntity> GetRoles()
        {
            var da = new RoleDA();
            return da.GetRoles();
        }

        public List<RoleMenuEntity> GetMenusByRoleId(string RoleId)
        {
            var da = new RoleDA();
            return da.GetMenusByRoleId(RoleId);
        }

        public int SaveRole(RoleEntity Role, string loginName)
        {
            if (string.IsNullOrEmpty(Role.Role_Id))
            {
                return AddRole(Role, loginName);
            }
            else
            {
                return UpdateRole(Role, loginName);
            }
        }

        public int UpdateRole(RoleEntity role, string loginName)
        {
            RoleDA da = new RoleDA();
            var result = da.UpdateRole(role, loginName);
            return result;
        }

        public int AddRole(RoleEntity role, string loginName)
        {
            RoleDA da = new RoleDA();
            var result = da.AddRole(role, loginName);
            return result;
        }

        public int EnableRole(string roleId, int status)
        {
            return new RoleDA().EnableRole(roleId, status);
        }

        public RoleEntity GetRoleByKey(string roleId)
        {
            RoleDA da = new RoleDA();
            var result = da.GetRoleByKey(roleId);
            return result;
        }

        public int DeleteRoleMenuByRoleId(string roleId)
        {
            RoleDA da = new RoleDA();
            var result = da.DeleteRoleMenuByRoleId(roleId);
            return result;
        }

        public int SaveRoleMenu(string roleId, string menuId, string loginName)
        {
            RoleDA da = new RoleDA();
            var result = da.SaveRoleMenu(roleId, menuId, loginName);
            return result;
        }
    }
}
