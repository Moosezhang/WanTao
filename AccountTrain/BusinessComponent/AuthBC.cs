using BusinessEntitys;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessComponent
{
    public class AuthBC
    {
        public IList<MenuEntity> GetMenusByLoginName(string loginName)
        {
           var da = new AuthDA();
           return da.GetMenusByLoginName(loginName);
        }

        public MenuEntity GetMenuByMenuId(string id)
        {
            var da = new AuthDA();
            return da.GetMenuByMenuId(id);
        }

        public List<MenuEntity> GetAllMenus()
        {
            var da = new AuthDA();
            return da.GetAllMenus();
        }
    }
}
