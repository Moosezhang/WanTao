using AccountTrain.Web.Common;
using BusinessComponent;
using BusinessEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountTrain.Web.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        //
        // GET: /Admin/Order/
        public ActionResult Index()
        {
            return View();
        }

      
        public ActionResult GetOrderListByCondition(string name,string orderNo,string startDate,string endDate)
        {
            try
            {
                return Json(new OrderBC().GetOrderListByCondition(name, orderNo, startDate, endDate), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<OrderEntity>(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetOrderGoodsListByOrderId(string orderId)
        {
            try
            {
                return Json(new OrderBC().GetOrderGoodsListByOrderId(orderId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<OrderGoodsEntity>(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetOrderGoodsListByOpenId(string OpenId)
        {
            try
            {
                return Json(new OrderBC().GetOrderGoodsListByOpenId(OpenId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<OrderGoodsEntity>(), JsonRequestBehavior.AllowGet);
            }
        }
	}
}