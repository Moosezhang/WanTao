
using AccountTrain.Common.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountTrain.Web.Common
{
    public class CacheManager
    {

        private static readonly CacheManager instance = new CacheManager();

        public static CacheManager Instance { get { return instance; } }

        public UserAuthSession CurrentUser
        {
            get
            {
                return HttpContext.Current.Session["CurrentUser"] as UserAuthSession;
            }
            set
            {
                HttpContext.Current.Session["CurrentUser"] = value;
            }
        }
    }
}