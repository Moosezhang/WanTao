using BusinessComponent;
using BusinessEntity.Model;
using BusinessEntitys;
using Common;
using Common.WxPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WxPayAPI;

namespace AccountTrain.Web.Controllers
{
    public class WxOrderController : WxBaseController
    {

        /// <summary>
        /// 新增砍价数据
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="classid"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddOrder(OrderParam param)
        {
            try
            {
                if (param==null)
                    return Json(string.Empty);
                string OrderNo = CommonHelper.CreateOrderNo();
                OrderEntity order = new OrderEntity() 
                {
                    OrderNo = OrderNo,
                    Openid = param.openid,
                    PayPrice=param.price,
                    OrderSource=param.source,
                    Nickname = new WxUserBC().GetWxUserByOpenid(param.openid).Nickname
                };

                List<OrderGoodsEntity> goods = new List<OrderGoodsEntity>();

                var paramList = param.classids.ToString().Split('|').ToList();
                if (paramList != null && paramList.Count > 0)
                {
                    foreach (var item in paramList)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            var entity = new ClassBC().GetClassByKey(item);
                            OrderGoodsEntity good = new OrderGoodsEntity() {
                                ClassId = entity.ClassId,
                                ClassName=entity.ClassName,
                                Price=entity.ClassPrice
                            };
                            goods.Add(good);
                        }

                    }
                }
                
                


                var result = new OrderBC().SaveOrder(order, goods, param.openid);
                if (result == "true")
                {
                    return Json(OrderNo, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(string.Empty, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 确认订单页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string openid,string orderNo)
        {
            if (string.IsNullOrEmpty(openid))
            {              

                if (string.IsNullOrEmpty(openid))
                {
                    Response.Redirect(CommonHelper.GetRedirect("My%2fRegistered"));
                }
            }

            ViewBag.Openid = openid;
            ViewBag.OrderNo = orderNo;
            return View();
        }

        public ActionResult GetSureOrder(string openid, string orderNo)
        {
            try
            {
                List<ClassEntity> entitys = new List<ClassEntity>();

                var goods = new OrderBC().GetOrderGoodsListByOpenIdandOrderNo(openid, orderNo);

                try
                {
                    foreach (var item in goods)
                    {
                        var entity = new ClassBC().GetClassByKey(item.ClassId);
                        entitys.Add(entity);  
                    }
                   
                }
                catch (Exception ex)
                {

                    throw;
                }


                
                return Json(entitys, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<ClassEntity>(), JsonRequestBehavior.AllowGet);
            }
        }


        //public ActionResult PayOrder()
        //{
 
        //}


        /// <summary>
        /// 支付（充值）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="openId"></param>
        /// <param name="mark"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult CheckOut(string openId, int price)
        {
            try
            {
                AppSetting setting = new AppSetting();
                WxPayClient client = new WxPayClient();

                string SerialNo = Guid.NewGuid().ToString();

                string Number = DateTime.Now.ToString("yyMMddHHmmss");
                Random example = new Random();
                int Numradom = example.Next(10000, 99999);
                Number = Number + Numradom.ToString();

               

                //插入充值表
                //DeliveryRechargeEntity Entry = new DeliveryRechargeEntity();
                //Entry.DeliveryManId = list[0].Id;
                //Entry.RechargeMoney = price;
                //Entry.PaySerialNo = Number;
                //Entry.WechatSerialNo = "";
                //Entry.State = 0;
                //Entry.RechargeTime = System.DateTime.Now;
                //Entry.ApplyTime = System.DateTime.Now;


                //int d = deliveryrechargeAppService.InsertAndGetId(Entry);


                int d = 0;
                if (d != 0)
                {
                    UnifiedOrderRequest req = new UnifiedOrderRequest();
                    req.Body = "万韬财税课程购买";//商品描述-----------------------
                    req.Attach = d.ToString();//附加信息，会原样返回,充值人员微信Openid

                    req.GoodTag = "Recharge";
                    req.TradeType = "JSAPI";
                    req.OpenId = openId;
                    req.OutTradeNo = Number;//---商户订单号----------------
                    req.TotalFee = 1;//测试总金额
                    //req.TotalFee = price*100;//总金额
                    req.NotifyUrl = setting.NotifyUrl;//异步通知地址-------------------------
                    var resp = client.UnifiedOrder(req);

                    WxPayData jsApiParam = new WxPayData();
                    jsApiParam.SetValue("appId", resp.AppId);
                    jsApiParam.SetValue("timeStamp", WxPayApi.GenerateTimeStamp());
                    jsApiParam.SetValue("nonceStr", WxPayApi.GenerateNonceStr());
                    jsApiParam.SetValue("package", "prepay_id=" + resp.PrepayId);
                    jsApiParam.SetValue("signType", "MD5");
                    jsApiParam.SetValue("paySign", jsApiParam.MakeSign());

                    //--给Viewbag赋值，供前台页面jsapi调用
                    ViewBag.AppId = (string)jsApiParam.GetValue("appId");
                    ViewBag.Package = (string)jsApiParam.GetValue("package");
                    ViewBag.NonceStr = (string)jsApiParam.GetValue("nonceStr");
                    ViewBag.Paysign = (string)jsApiParam.GetValue("paySign");
                    ViewBag.TimeStamp = (string)jsApiParam.GetValue("timeStamp");
                    ViewBag.OpenId = openId;
                    ViewBag.OrderId = Number;
                }


            }
            catch (Exception ex)
            {
               // Log.WriteLog("CheckOut Error:" + ex.Message);
            }
            return View();
        }

        #region 团购
        public ActionResult GroupBuy(string openid, string classId)
        {
            if (string.IsNullOrEmpty(openid))
            {
                Response.Redirect(CommonHelper.GetRedirect("WxOrder%2fGroupBuy"));
            }

            ViewBag.Openid = openid;
            ViewBag.ClassId = classId;
            return View();
        }

        public ActionResult GetGbClass(string classId)
        {
            try
            {
                return Json(new OrderBC().GetGbClass(classId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new VMGBClass(), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 砍价
        /// <summary>
        /// 砍价页面
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <param name="bargainid"></param>
        /// <returns></returns>
        public ActionResult Bargain(string openid, string code, string state,string bargainid)
        {
            if (string.IsNullOrEmpty(openid))
            {
                openid = GetOpenId(code).openid;

                if (string.IsNullOrEmpty(openid))
                {
                    Response.Redirect(CommonHelper.GetRedirect("WxOrder%2fBargain"));
                }
            }

            ViewBag.Openid = openid;
            ViewBag.Bargainid = bargainid;
            return View();
        }

        
        /// <summary>
        /// 新增砍价数据
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="classid"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public ActionResult AddBargain(string openid,string classid,string price)
        {
            try
            {
                if (string.IsNullOrEmpty(openid)||string.IsNullOrEmpty(classid)||string.IsNullOrEmpty(price))
                    return Json(string.Empty);

                string bargainId=Guid.NewGuid().ToString();

                BargainEntity entity = new BargainEntity()
                {
                    BargainId = bargainId,
                    OpenId = openid,
                    ClassId = classid,
                    PrePrice = Convert.ToDecimal(price),
                    NowPrice = Convert.ToDecimal(price),  
                };

                var result = new OrderBC().AddBargain(entity, openid);
                if (result > 0)
                {
                    return Json(bargainId, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(string.Empty, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取砍价页面数据
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        public ActionResult GetBargainClass(string bargainId)
        {
            try
            {
                return Json(new OrderBC().GetBargainClass(bargainId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new VMBargainClass(), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取砍价记录数据
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        public ActionResult GetBargainLogs(string bargainId)
        {
            try
            {
                return Json(new OrderBC().GetBargainLogs(bargainId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<BargainLogEntity>(), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }

    public class OrderParam
    {
        public string classids { get; set; }

        public string openid { get; set; }

        public string source { get; set; }

        public decimal price { get; set; }
    }
}