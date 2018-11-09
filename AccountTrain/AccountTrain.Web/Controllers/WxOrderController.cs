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
                                Price = param.source != "1" ? param.price : entity.ClassPrice
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
                        entity.ClassPrice = item.Price;
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


        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="id"></param>
        /// <param name="openId"></param>
        /// <param name="mark"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult CheckOut(string openId, int price,string orderNo)
        {
            try
            {
                AppSetting setting = new AppSetting();
                WxPayClient client = new WxPayClient();

               

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
                    req.OutTradeNo = orderNo;//---商户订单号----------------
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
                    ViewBag.OrderNo = orderNo;
                }


            }
            catch (Exception ex)
            {
               // Log.WriteLog("CheckOut Error:" + ex.Message);
            }
            return View();
        }

        /// <summary>
        /// 支付完成
        /// </summary>
        /// <returns></returns>
        public ActionResult rechargesucc(string openId,string orderNo)
        {
            OrderBC bc= new OrderBC();
            var result=bc.GetOrderByOrderNo(orderNo);

            //支付成功，1.更新订单状态；2.更新课程热度
            UpdateOrderStatus(orderNo,2);//1

            var goods = bc.GetOrderGoodsListByOrderId(result.OrderId);
            if (goods != null && goods.Count > 0)
            {
                foreach (var item in goods)
                {
                    new ClassBC().UpdateClassHot(item.ClassId);//2
                }
            }
            //根据订单来源，变更不同推广状态
            switch (result.OrderSource)
            {
                case "1"://单独购买
                    break;
                case "2"://砍价
                    foreach (var item in goods)
                    {
                        var barginEntity = bc.GetBargainByOpenIdAndClassId(item.ClassId, openId);
                        bc.UpdateBargainStatus(barginEntity.BargainId, 2);
                    }
                    break;
                case "3"://团购
                    foreach (var item in goods)
                    {
                        var gbEntity = bc.GetGroupBuyByClassId(item.ClassId);
                        if (gbEntity != null)//该商品已有团购，更新团购人数
                        {
                            bc.UpdateGroupBuyCount(gbEntity.GroupBuyId);
                        }
                        else//该商品没有团购，新增团购数据
                        {
                            GroupBuyEntity buy = new GroupBuyEntity()
                            {
                                ClassId=item.ClassId,
                                NowCount=1,
                            };
                            bc.AddGroupBuy(buy,openId);
                        }
                        gbEntity = bc.GetGroupBuyByClassId(item.ClassId);
                        //记录团购成员表
                        GroupBuyMemberEntity member = new GroupBuyMemberEntity()
                        {
                            GroupBuyId = gbEntity.GroupBuyId,
                            GroupPrice = item.Price,
                            openId = openId
                        };
                        bc.AddGroupBuyMember(member, openId);

                        var nowCountEntity = bc.GetGroupBuyByClassId(item.ClassId);
                        var nowCount = nowCountEntity.NowCount;
                        var needCount = bc.GetGroupBuyConfigByClassId(item.ClassId).NeedCount;

                        if(nowCount==needCount)
                        {
                            bc.UpdateGroupBuyStatus(nowCountEntity.GroupBuyId,2);
                        }
                    }
                    
                    break;
                case "4"://助力

                    break;
            }


            ViewBag.OrderNo = orderNo;
            ViewBag.OpenId = openId;
            ViewBag.Price = result.PayPrice;

            return View();
        }


        public ActionResult UpdateOrderStatus(string orderNo,int status)
        {
            try
            {
                return Json(new OrderBC().UpdateOrderStatus(orderNo, status), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new VMGBClass(), JsonRequestBehavior.AllowGet);
            }
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

        /// <summary>
        /// 帮他砍一刀
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="classid"></param>
        /// <returns></returns>
        public ActionResult BargainClass(string openid, string classid)
        {
            try
            {
                OrderBC bc = new OrderBC();
                var result = "success";
                
                //获取砍价上下限                
                var config = bc.GetBargainConfigByClassId(classid);
                var top = config.BargainTop;
                var floor = config.BargainFloor;
                decimal cutPrce = CommonHelper.GetRandNum(floor*100,top*100)/100;
                var floorPrice = config.FloorPrice;
                //更新最新价格，插入砍价记录表
                var bargainEntity = bc.GetBargainByOpenIdAndClassId(classid,openid);
                var nowPrice = bargainEntity.NowPrice;
                if (nowPrice - floorPrice < cutPrce)
                {
                    cutPrce = nowPrice - floorPrice;
                }
                nowPrice=nowPrice-cutPrce;
                bc.UpdateBargainNowPrice(bargainEntity.BargainId, nowPrice);
                BargainLogEntity log = new BargainLogEntity() 
                {
                    BargainId = bargainEntity.BargainId,
                    OpenId = openid,
                    BargainPrice = cutPrce,
                };
                bc.AddBargainLog(log,openid);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
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