using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessEntity.Common
{
    public class SystemMenu
    {
        public string MenuId { get; set; }
        public string MenuResKey { get; set; }
        public string MenuResType { get; set; }
        public string Title { get; set; }
        public string MenuUrl { get; set; }
        public int MenuOrder { get; set; }
        public string Description { get; set; }
        public IList<SystemMenu> SubMenus { get; set; }
    }

    public class SystemMenuList : List<SystemMenu>
    {
    }
}
