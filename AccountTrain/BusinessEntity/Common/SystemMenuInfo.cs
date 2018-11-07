using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessEntity.Common
{
    [Serializable]
    public class SystemMenuInfo
    {
        public string MenuId { get; set; }
        public string MenuName { get; set; }
        public string Url { get; set; }

        public string Icon { get; set; }

        public bool Selected = false;
        public IList<SystemMenuInfo> SubMenus { get; set; }
    }
}
