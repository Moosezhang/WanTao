
using AccountTrain.Common.Session;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountTrain.Web.Common
{
    [MyAuthorize]
    public abstract class BaseController : Controller
    {
        public UserAuthSession CurrentUserInfo
        {
            get
            {
                if (CacheManager.Instance.CurrentUser != null)
                {
                    return CacheManager.Instance.CurrentUser;
                }
                else
                {
                    return null;
                }
            }
        }

       
    }
}