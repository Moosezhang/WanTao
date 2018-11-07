using AccountTrain.Web.Common;
using BusinessComponent;
using BusinessEntity.Model;
using BusinessEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountTrain.Web.Areas.Admin.Controllers
{
    public class WxUserController : BaseController
    {
        //
        // GET: /Admin/WxUser/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetWxUserListByCondition(string name, string phone, string startDate, string endDate)
        {
            try
            {
                return Json(new WxUserBC().GetWxUserListByCondition(name, phone, startDate, endDate), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<WxUserEntity>(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetWxUserByOpenid(string Openid)
        {
            try
            {
                return Json(new WxUserBC().GetWxUserByOpenid(Openid), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new VMWxUserPoints(), JsonRequestBehavior.AllowGet);
            }
        }
	}
}