using AccountTrain.Web.Common;
using BusinessComponent;
using BusinessEntity.Model;
using BusinessEntitys;
using Common;
using Common.WxPay;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WxPayAPI;
using Common;
using System.Security.Cryptography;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Configuration;

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

                //判断是否注册
                var userInfo = new WxUserBC().GetWxUserByOpenid(param.openid);

                if (string.IsNullOrEmpty(userInfo.Phone))
                {
                    string url = CommonHelper.GetRedirect("WxMy%2fRegistered");
                    //Response.Redirect(url);
                    return Json(url, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string OrderNo = CommonHelper.CreateOrderNo();
                    OrderEntity order = new OrderEntity()
                    {
                        OrderNo = OrderNo,
                        Openid = param.openid,
                        PayPrice = param.price,
                        OrderSource = param.source,
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
                                OrderGoodsEntity good = new OrderGoodsEntity()
                                {
                                    ClassId = entity.ClassId,
                                    ClassName = entity.ClassName,
                                    Price = param.source != "1" ? param.price : entity.ClassPrice
                                };
                                goods.Add(good);
                            }

                        }
                    }

                    var result = new OrderBC().SaveOrder(order, goods, param.openid);
                    if (result == "true")
                    {
                        string okUrl = "/WxOrder/Index?orderNo=" + OrderNo;
                        return Json(okUrl, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(string.Empty, JsonRequestBehavior.AllowGet);
                    }
                }

                //var orderResult = new OrderBC().GetOrderByOpenIdandClassId(param.openid,);
                
                
                


                
            }
            catch (Exception ex)
            {
                LogHelp.WriteLog(DateTime.Now + ":::AddOrderError:::" + ex.Message);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 确认订单页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string code,string orderNo)
        {

            string openid = "";
            if (new AppSetting().IsDebug != null
                && new AppSetting().IsDebug.ToLower() == "true")
            {
                openid = "123";
            }
            else
            {
                if (Request.Cookies[SystemConfig.WXOpenIDCookieKey] != null)
                    openid = Request.Cookies[SystemConfig.WXOpenIDCookieKey].Value;

                if (string.IsNullOrWhiteSpace(openid) && code == null)
                {
                    Response.Redirect(CommonHelper.GetRedirect("WxMy%2fMyClass"));
                }
                try
                {
                    if (string.IsNullOrWhiteSpace(openid))
                    {

                        openid = GetOpenId(code).openid;


                        // 合法用户，允许访问
                        Response.Cookies[SystemConfig.WXOpenIDCookieKey].Value = openid;
                        Response.Cookies[SystemConfig.WXOpenIDCookieKey].Path = "/";
                        Response.Cookies[SystemConfig.WXOpenIDCookieKey].Expires = DateTime.Now.AddDays(1);
                    }
                }
                catch (Exception ex)
                {
                   
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

        #region 支付
        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="id"></param>
        /// <param name="openId"></param>
        /// <param name="mark"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult CheckOut(string code,string orderNo)
        {
            string openid = "";
            if (new AppSetting().IsDebug != null
                && new AppSetting().IsDebug.ToLower() == "true")
            {
                openid = "123";
            }
            else
            {
                if (Request.Cookies[SystemConfig.WXOpenIDCookieKey] != null)
                    openid = Request.Cookies[SystemConfig.WXOpenIDCookieKey].Value;

                if (string.IsNullOrWhiteSpace(openid) && code == null)
                {
                    Response.Redirect(CommonHelper.GetRedirect("WxClass%2fClassList"));
                }
                try
                {
                    if (string.IsNullOrWhiteSpace(openid))
                    {

                        openid = GetOpenId(code).openid;


                        // 合法用户，允许访问
                        Response.Cookies[SystemConfig.WXOpenIDCookieKey].Value = openid;
                        Response.Cookies[SystemConfig.WXOpenIDCookieKey].Path = "/";
                        Response.Cookies[SystemConfig.WXOpenIDCookieKey].Expires = DateTime.Now.AddDays(1);
                    }
                }
                catch (Exception ex)
                {
                    
                }
            }    

            try
            {
                AppSetting setting = new AppSetting();
                WxPayClient client = new WxPayClient();

                OrderBC bc=new OrderBC();

                var order =bc.GetOrderByOrderNo(orderNo);

                string outTradeNumber = string.Format("{0}{1}", orderNo.ToString(), DateTime.Now.ToString("fff"));

                    

                UnifiedOrderRequest req = new UnifiedOrderRequest();
                req.Body = "万韬财税课程购买";//商品描述-----------------------
                req.Attach = openid.ToString();//附加信息，会原样返回,充值人员微信Openid

                req.GoodTag = "Pay";
                req.TradeType = "JSAPI";
                req.OpenId = openid;
                req.OutTradeNo = outTradeNumber;//---商户订单号----------------
                req.TotalFee = 1;//测试总金额
                //req.TotalFee = Convert.ToInt32(order.PayPrice * 100);//总金额
                req.NotifyUrl = setting.NotifyUrl;//异步通知地址-------------------------
                var resp = client.UnifiedOrder(req);

                WxPayData jsApiParam = new WxPayData();
                jsApiParam.SetValue("appId", resp.AppId);
                jsApiParam.SetValue("timeStamp", WxPayApi.GenerateTimeStamp());
                jsApiParam.SetValue("nonceStr", WxPayApi.GenerateNonceStr());
                jsApiParam.SetValue("package", "prepay_id=" + resp.PrepayId);
                jsApiParam.SetValue("signType", "MD5");
                jsApiParam.SetValue("paySign", jsApiParam.MakeSign());

                bc.UpdatePayInfo(outTradeNumber, jsApiParam.ToJson(), orderNo);

                //--给Viewbag赋值，供前台页面jsapi调用
                ViewBag.AppId = (string)jsApiParam.GetValue("appId");
                ViewBag.Package = (string)jsApiParam.GetValue("package");
                ViewBag.NonceStr = (string)jsApiParam.GetValue("nonceStr");
                ViewBag.Paysign = (string)jsApiParam.GetValue("paySign");
                ViewBag.TimeStamp = (string)jsApiParam.GetValue("timeStamp");
                ViewBag.OpenId = openid;
                ViewBag.OrderNo = orderNo;
               

                ViewBag.OpenId = openid;
                ViewBag.OrderNo = orderNo;
            }
            catch (Exception ex)
            {
                
            }
            return View();
        }

        /// <summary>
        /// 支付完成
        /// </summary>
        /// <returns></returns>
        public ActionResult rechargesucc(string openId, string orderNo)
        {
            OrderBC bc = new OrderBC();
            var result = bc.GetOrderByOrderNo(orderNo);

            //支付成功，1.更新订单状态；2.更新课程热度；3.增加用户积分;4.删除购物车
            UpdateOrderStatus(orderNo, 2);//1

            var goods = bc.GetOrderGoodsListByOrderId(result.OrderId);
            if (goods != null && goods.Count > 0)
            {
                foreach (var item in goods)
                {
                    new ClassBC().UpdateClassHot(item.ClassId);//2
                    new ShopCarBC().EnableShopCar(openId,item.ClassId,2);//4
                }
            }

            var point = bc.GetPointsByOpenid(openId);//3
            if (point == null)
            {
                PointsEntity points = new PointsEntity()
                {
                    OpenId = openId,
                    Points = result.PayPrice,
                };
                bc.AddPoint(points, openId);
                PointsLogEntity log=new PointsLogEntity()
                {
                    OrderId=result.OrderId,
                    LogType="1",
                    Points=result.PayPrice,
                    OpenId=openId,

                };                
                bc.AddPointLog(log, openId);
            }
            else
            {
                bc.UpdatePonits(openId, result.PayPrice);
                PointsLogEntity log = new PointsLogEntity()
                {
                    OrderId = result.OrderId,
                    LogType = "1",
                    Points = result.PayPrice,
                    OpenId = openId,

                };
                bc.AddPointLog(log, openId);
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
                    UpdateOrderStatus(orderNo, 3);//1

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
                                ClassId = item.ClassId,
                                NowCount = 1,
                            };
                            bc.AddGroupBuy(buy, openId);
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

                        if (nowCount == needCount)//团购人数已满，变更团购状态为已完成（2）
                        {
                            bc.UpdateGroupBuyStatus(nowCountEntity.GroupBuyId, 2);
                            //更新成员表中所有成员的订单状态
                            var members = bc.GetGroupBuyMember(nowCountEntity.GroupBuyId);
                            foreach (var i in members)
                            {
                                var order = bc.GetOrderByOpenIdandClassId(i.openId,item.ClassId);
                                UpdateOrderStatus(order.OrderNo, 2);//1
                            }
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
        #endregion

        #region 退款
        public ActionResult Refund(string code,string orderNo)
        {

            string openid = "";
            if (new AppSetting().IsDebug != null
                && new AppSetting().IsDebug.ToLower() == "true")
            {
                openid = "123";
            }
            else
            {
                if (Request.Cookies[SystemConfig.WXOpenIDCookieKey] != null)
                    openid = Request.Cookies[SystemConfig.WXOpenIDCookieKey].Value;

                if (string.IsNullOrWhiteSpace(openid) && code == null)
                {
                    Response.Redirect(CommonHelper.GetRedirect("WxClass%2fClassList"));
                }
                try
                {
                    if (string.IsNullOrWhiteSpace(openid))
                    {

                        openid = GetOpenId(code).openid;


                        // 合法用户，允许访问
                        Response.Cookies[SystemConfig.WXOpenIDCookieKey].Value = openid;
                        Response.Cookies[SystemConfig.WXOpenIDCookieKey].Path = "/";
                        Response.Cookies[SystemConfig.WXOpenIDCookieKey].Expires = DateTime.Now.AddDays(1);
                    }
                }
                catch (Exception ex)
                {

                }
            }   

            AppSetting setting = new AppSetting();
            WxPayClient client = new WxPayClient();
            WxPayData data = new WxPayData();

            OrderBC bc = new OrderBC();

            var order = bc.GetOrderByOrderNo(orderNo);


            string outTradeNumber = string.Format("{0}{1}", orderNo.ToString(), DateTime.Now.ToString("fff"));



            RefundOrderRequest req = new RefundOrderRequest();

            data.SetValue("out_trade_no", order.WXPayOutTradeNumber);
            data.SetValue("total_fee", 1);//订单总金额
            data.SetValue("refund_fee", 1);//退款金额
            data.SetValue("out_refund_no", WxPayApi.GenerateOutTradeNo());//随机生成商户退款单号
            //data.SetValue("total_fee", Convert.ToInt32(order.PayPrice * 100));//订单总金额
            //data.SetValue("refund_fee",  Convert.ToInt32(order.PayPrice * 100));//退款金额

            var resp = client.Refund(data);

            //WxPayData jsApiParam = new WxPayData();
            //jsApiParam.SetValue("appId", resp.AppId);
            //jsApiParam.SetValue("timeStamp", WxPayApi.GenerateTimeStamp());
            //jsApiParam.SetValue("nonceStr", WxPayApi.GenerateNonceStr());
            //jsApiParam.SetValue("package", "prepay_id=" + resp.PrepayId);
            //jsApiParam.SetValue("signType", "MD5");
            //jsApiParam.SetValue("paySign", jsApiParam.MakeSign());

            return View();
        }
        #endregion


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
        public ActionResult GroupBuy(string code,string classId)
        {
            string openid = "";
            if (new AppSetting().IsDebug != null
                && new AppSetting().IsDebug.ToLower() == "true")
            {
                openid = "123";
            }
            else
            {
                if (Request.Cookies[SystemConfig.WXOpenIDCookieKey] != null)
                    openid = Request.Cookies[SystemConfig.WXOpenIDCookieKey].Value;

                if (string.IsNullOrWhiteSpace(openid) && code == null)
                {
                    Response.Redirect(CommonHelper.GetRedirect("WxMy%2fGroupBuy"));
                }
                try
                {
                    if (string.IsNullOrWhiteSpace(openid))
                    {

                        openid = GetOpenId(code).openid;


                        // 合法用户，允许访问
                        Response.Cookies[SystemConfig.WXOpenIDCookieKey].Value = openid;
                        Response.Cookies[SystemConfig.WXOpenIDCookieKey].Path = "/";
                        Response.Cookies[SystemConfig.WXOpenIDCookieKey].Expires = DateTime.Now.AddDays(1);
                    }
                }
                catch (Exception ex)
                {
                   
                }
            }
            

            //判断是否注册
            var userInfo = new WxUserBC().GetWxUserByOpenid(openid);
            if (string.IsNullOrEmpty(userInfo.Phone))
            {
                Response.Redirect(CommonHelper.GetRedirect("WxMy%2fRegistered"));
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

        public ActionResult GetGroupBuyConfigByClassId(string classid)
        {
            try
            {
                var result = new OrderBC().GetGroupBuyConfigByClassId(classid);
                if (result == null)
                {
                    result = new GroupBuyConfigEntity();
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new GroupBuyConfigEntity(), JsonRequestBehavior.AllowGet);
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
        public ActionResult Bargain(string code, string state,string bargainid)
        {
            string openid = "";
            if (new AppSetting().IsDebug != null
                && new AppSetting().IsDebug.ToLower() == "true")
            {
                openid = "123";
            }
            else
            {
                if (Request.Cookies[SystemConfig.WXOpenIDCookieKey] != null)
                    openid = Request.Cookies[SystemConfig.WXOpenIDCookieKey].Value;
                if (string.IsNullOrEmpty(openid) && string.IsNullOrEmpty(code))
                {
                    Response.Redirect(CommonHelper.GetRedirect("WxOrder%2fBargain?bargainid" + bargainid));
                }
                try
                {
                    if (string.IsNullOrWhiteSpace(openid))
                    {

                        openid = GetOpenId(code).openid;


                        // 合法用户，允许访问
                        Response.Cookies[SystemConfig.WXOpenIDCookieKey].Value = openid;
                        Response.Cookies[SystemConfig.WXOpenIDCookieKey].Path = "/";
                        Response.Cookies[SystemConfig.WXOpenIDCookieKey].Expires = DateTime.Now.AddDays(1);
                    }
                }
                catch (Exception ex)
                {
                    
                }
            }


            string jsapiTicket = Util.GetJSAPI_Ticket(WxPayConfig.APPID, WxPayConfig.APPSECRET); 
            string link= Request.Url.ToString();


            //--给Viewbag赋值，供前台页面jsapi调用
            ViewBag.AppId = WxPayConfig.APPID;
            ViewBag.TimeStamp = WxPayApi.GenerateTimeStamp();
            ViewBag.NonceStr = WxPayApi.GenerateNonceStr();
            string rawstring = "jsapi_ticket=" + jsapiTicket + "&noncestr=" + ViewBag.NonceStr + "&timestamp=" + ViewBag.TimeStamp + "&url=" + link + "";
            ViewBag.Signature = SHA1_Hash(rawstring);
            ViewBag.Openid = openid;
            ViewBag.Bargainid = bargainid;
            ViewBag.Link = link;
            return View();
        }

        public string SHA1_Hash(string str_sha1_in)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytes_sha1_in = System.Text.UTF8Encoding.Default.GetBytes(str_sha1_in);
            byte[] bytes_sha1_out = sha1.ComputeHash(bytes_sha1_in);
            string str_sha1_out = BitConverter.ToString(bytes_sha1_out);
            str_sha1_out = str_sha1_out.Replace("-", "").ToLower();
            return str_sha1_out;
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


                //判断是否注册
                var userInfo = new WxUserBC().GetWxUserByOpenid(openid);
                if (string.IsNullOrEmpty(userInfo.Phone))
                {
                    string url = CommonHelper.GetRedirect("WxMy%2fRegistered");
                    //Response.Redirect(url);
                    return Json(url, JsonRequestBehavior.AllowGet);
                }

                var result = new OrderBC().GetBargainByOpenIdAndClassId(classid, openid);
                if (result != null)
                {
                    return Json("/WxOrder/Bargain?bargainid="+result.BargainId, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string bargainId = Guid.NewGuid().ToString();

                    BargainEntity entity = new BargainEntity()
                    {
                        BargainId = bargainId,
                        OpenId = openid,
                        ClassId = classid,
                        PrePrice = Convert.ToDecimal(price),
                        NowPrice = Convert.ToDecimal(price),
                    };

                    var addResult = new OrderBC().AddBargain(entity, openid);
                     if (addResult > 0)
                    {
                        string okUrl = "/WxOrder/Bargain?bargainid=" + bargainId;
                        return Json(okUrl, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(string.Empty, JsonRequestBehavior.AllowGet);
                    }
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
                var result = new OrderBC().GetBargainClass(bargainId);
                if (result == null)
                {
                   return Json(new VMBargainClass(), JsonRequestBehavior.AllowGet);
                }
                LogHelp.WriteLog("ClassPrice:::"+result.ClassPrice.ToString());
                return Json(result, JsonRequestBehavior.AllowGet);
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
                var result = new OrderBC().GetBargainLogs(bargainId);
               
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<VMBargainLog>(), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 帮他砍一刀
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="classid"></param>
        /// <returns></returns>
        public ActionResult BargainClass(string ownOpenid,string openid, string classid)
        {
            try
            {
                OrderBC bc = new OrderBC();
                var result = "success";
                LogHelp.WriteLog("ownOpenid:::"+ownOpenid +"openid::: "+ openid +"classid::: "+ classid);
                //获取砍价上下限                
                var config = bc.GetBargainConfigByClassId(classid);
                var top = config.BargainTop;
                var floor = config.BargainFloor;
                decimal cutPrce = Convert.ToDecimal(CommonHelper.GetRandNum(floor*100,top*100)*0.01);
                var floorPrice = config.FloorPrice;
                //更新最新价格，插入砍价记录表
                var bargainEntity = bc.GetBargainByOpenIdAndClassId(classid, ownOpenid);
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
                LogHelp.WriteLog(ex.Message);
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetBargainConfigByClassId(string classid)
        {
            try
            {
                var result = new OrderBC().GetBargainConfigByClassId(classid);
                if (result == null)
                {
                    result = new BargainConfigEntity();
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new BargainConfigEntity(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetBargainByOpenIdAndClassId(string ClassId,string openId)
        {
            try
            {
                var result = new OrderBC().GetBargainByOpenIdAndClassId(ClassId, openId);
                if (result == null)
                {
                    result = new BargainEntity();
                }
                
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new BargainEntity(), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


        #region 助力
        /// <summary>
        /// 1.根据classid找到课程助力配置
        /// 2.根据openid判断是否助力过
        /// 3.生成该openid该课程助力数据
        /// 4.更新助力人的助力人数
        /// 5.插入助力人员表
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="classid"></param>
        /// <returns></returns>
        public ActionResult HelpClass(string code, string state, string classid,string ownOpenid,string helpId)
        {
            string msg = string.Empty;
            string link = string.Empty;
            string fileName = string.Empty;
            string openid = "";
            try
            {
                if (new AppSetting().IsDebug != null
                    && new AppSetting().IsDebug.ToLower() == "true")
                {
                    openid = "123";
                }
                else
                {
                    if (Request.Cookies[SystemConfig.WXOpenIDCookieKey] != null)
                        openid = Request.Cookies[SystemConfig.WXOpenIDCookieKey].Value;
                    if (string.IsNullOrEmpty(openid) && string.IsNullOrEmpty(code))
                    {
                        Response.Redirect(CommonHelper.GetRedirect("WxOrder%2fHelpClass?helpId=" + helpId));
                    }
                    try
                    {
                        if (string.IsNullOrWhiteSpace(openid))
                        {

                            openid = GetOpenId(code).openid;


                            // 合法用户，允许访问
                            Response.Cookies[SystemConfig.WXOpenIDCookieKey].Value = openid;
                            Response.Cookies[SystemConfig.WXOpenIDCookieKey].Path = "/";
                            Response.Cookies[SystemConfig.WXOpenIDCookieKey].Expires = DateTime.Now.AddDays(1);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }


                

                OrderBC bc = new OrderBC();
               
                HelpInfoEntity entity = new HelpInfoEntity();
                if (!string.IsNullOrEmpty(helpId))
                {
                    entity = bc.GetHelpByHelpInfoId(helpId);
                    ownOpenid = entity.OpenId;
                    classid = entity.ClassId;
                }
                var config = bc.GetHelpConfigByClassId(classid);
                if (!string.IsNullOrEmpty(ownOpenid) && !string.IsNullOrEmpty(classid))
                {
                    entity = bc.GetHelpByOpenIdAndClassId(classid, ownOpenid);              
                }
                
                
                if (ownOpenid != openid)//非发起用户进入
                {
                    
                    var helpinfo = bc.GetHelpMemberByOpenid(openid);
                    if (helpinfo != null)
                    {
                        msg = "该用户已助力";
                        return Json(msg, JsonRequestBehavior.AllowGet);
                    }

                    helpId = Guid.NewGuid().ToString();

                    //生成二维码
                    SaveIamge QR = CreateQR(helpId);
                    //添加文字水印
                    string WxName = new WxUserBC().GetWxUserByOpenid(openid).Nickname;

                    string path = HttpContext.Server.MapPath("/Images/upload/");

                    string[] sArray = Regex.Split(config.ImageUrl, "Images/upload/", RegexOptions.IgnoreCase);

                    string filename = sArray[1].ToString();
                    SaveIamge WordsPic = new WaterImageManager().DrawWordsForSaveIamge(filename, path, WxName, 1, FontFamilys.宋体, FontStyle.Bold, ImagePosition.TopMiddle);
                    //添加二维码水印
                    string QrPic = new WaterImageManager().DrawImage(WordsPic.filename, WordsPic.showImg, QR.filename, QR.showImg, 1, ImagePosition.BottomMiddle);
                    LogHelp.WriteLog("QrPic:::" + QrPic);
                    fileName = QrPic;
                    link = CommonHelper.LinkImageUrl("/Images/upload/" + QrPic);

                    HelpInfoEntity help = new HelpInfoEntity()
                    {
                        HelpInfoId=helpId,
                        ClassId = classid,
                        OpenId = openid,
                        NowCount = 0,
                        imgUrl = link
                    };

                    var addResult = bc.AddHelpInfo(help, openid);
                    //增加助力记录
                    HelpMemberEntity member = new HelpMemberEntity()
                    {
                        HelpInfoId = entity.HelpInfoId,
                        OpenId = openid,
                    };
                    var addMember = bc.AddHelpMember(member, openid);
                    //更新助力信息人数
                    var updateInfo = bc.UpdateHelpNowCount(entity.HelpInfoId, entity.NowCount + 1);
                  
                    entity = bc.GetHelpByHelpInfoId(entity.HelpInfoId);
                    
                    int diff = config.HelpCount - entity.NowCount;
                    var wxUser = new WxUserBC().GetWxUserByOpenid(entity.OpenId);
                    if (diff <= 0)
                    {
                        
                        string OrderNo = CommonHelper.CreateOrderNo();
                        OrderEntity order = new OrderEntity()
                        {
                            OrderNo = OrderNo,
                            Openid = entity.OpenId,
                            PayPrice = 0,
                            OrderSource = "4",
                            Nickname = wxUser.Nickname
                        };
                        List<OrderGoodsEntity> goods = new List<OrderGoodsEntity>();

                        var classEntity = new ClassBC().GetClassByKey(classid);
                        OrderGoodsEntity good = new OrderGoodsEntity()
                        {
                            ClassId = classEntity.ClassId,
                            ClassName = classEntity.ClassName,
                            Price = 0
                        };
                        goods.Add(good);

                        var result = new OrderBC().SaveOrder(order, goods, entity.OpenId);


                        msg = "助力成功";
                        
                    }
                }
                else//发起用户进入
                {
                    LogHelp.WriteLog("HelpClass:::22222");
                    if (entity != null)//如果主力已存在，展示助力情况
                    {
                        link = entity.imgUrl;
                        int diff = config.HelpCount - entity.NowCount;
                        if (diff > 0)
                        {
                            msg = string.Format("还差 {0} 人助力成功", diff);
                            //return Json(msg, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            msg = string.Format("已助力成功");
                            //return Json(msg, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else //如果助力不存在，新增助力
                    {
                        helpId = Guid.NewGuid().ToString();
                        //生成二维码
                        SaveIamge QR = CreateQR(helpId);
                        //添加文字水印
                        string WxName = new WxUserBC().GetWxUserByOpenid(ownOpenid).Nickname;
                        string path = HttpContext.Server.MapPath("/Images/upload/");

                        string[] sArray = Regex.Split(config.ImageUrl, "Images/upload/", RegexOptions.IgnoreCase);
                        string filename = sArray[1].ToString();
                        SaveIamge WordsPic = new WaterImageManager().DrawWordsForSaveIamge(filename, path, WxName, 1, FontFamilys.宋体, FontStyle.Bold, ImagePosition.TopMiddle);
                        //添加二维码水印
                        string QrPic = new WaterImageManager().DrawImage(WordsPic.filename, WordsPic.showImg, QR.filename, QR.showImg, 1, ImagePosition.BottomMiddle);
                        fileName = QrPic;
                        link = CommonHelper.LinkImageUrl("/Images/upload/" + QrPic);

                        HelpInfoEntity help = new HelpInfoEntity()
                        {
                            HelpInfoId = helpId,
                            ClassId = classid,
                            OpenId = ownOpenid,
                            NowCount = 0,
                            imgUrl = link
                        };
                        var addResult = bc.AddHelpInfo(help, ownOpenid);

                        msg = "分享图片，请好友帮忙助力吧";
                    }

                }

                //string media_id = Util.uploadMedia(HttpContext.Server.MapPath("/Images/upload/") + fileName, fileName);
                //Util.SendCustomMessage(openid, media_id);
            }
            catch (Exception ex)
            {

                LogHelp.WriteLog("HelpClass:::" + ex.Message);
            }
            

            ViewBag.Message = msg;
            ViewBag.Link = link;
            
            return View();
            
        }

        public SaveIamge CreateQR(string helpId)
        {
          
          
            QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
            QrCode qrCode = new QrCode();
            string url = ConfigurationManager.AppSettings["imgMapPath"] + "WxOrder/HelpClass?helpId=" + helpId;
            //string url = CommonHelper.GetRedirect("WxOrder%2fHelpClass?helpId=" + helpId);
                  
            qrEncoder.TryEncode(url, out qrCode);

            GraphicsRenderer renderer = new GraphicsRenderer(new FixedModuleSize(2, QuietZoneModules.Two), Brushes.Black, Brushes.White);


            string filename = string.Format("{0}.png", helpId);
            string saveUrl = "/Images/QR/";
            string filePath = Path.Combine(HttpContext.Server.MapPath(saveUrl), filename);
            using (MemoryStream ms = new MemoryStream())
            {
                renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);
                Image img = Image.FromStream(ms);

                img.Save(filePath);
            }

            string showImg = CommonHelper.LinkImageUrl(saveUrl);

            SaveIamge model = new SaveIamge() 
            {
                filename = filename,
                showImg = HttpContext.Server.MapPath(saveUrl)
            };

            return model;
        }


        /// <summary>
        /// 指定图片添加指定文字
        /// </summary> 
        /// <param name="text">添加的文字</param>
        /// <param name="picname">生成文件名</param>
        private void AddTextToImg(string text)
        {

            string filename = string.Format("{0}.png", text);
            string saveUrl = "/Images/QR/";
            string filePath = Path.Combine(HttpContext.Server.MapPath(saveUrl), filename);

            

            if (string.IsNullOrEmpty(text))
                return;

            Image image = Image.FromFile(filePath);        
            Bitmap bitmap = new Bitmap(image, image.Width, image.Height);
            Graphics g = Graphics.FromImage(bitmap);

            float fontSize = 50f;
            float textWidth = text.Length * fontSize;

            System.Drawing.Font font = new System.Drawing.Font("加粗", fontSize, FontStyle.Bold);

            //字体矩形位置 ：
            //x = 图片的长度的中心位置 - 字体长度的一半 - 字行距
            //y = 图片的高度的中心位置 - 字体大小的一半 - 偏移（去掉偏移，是居中位置）
            float rectX = image.Width / 2 - textWidth / 2 - font.Height;
            float rectY = image.Height / 2 - fontSize / 2 - 30;

            float rectWidth = text.Length * fontSize + font.Height * 2;

            //英文字体的1磅，相当于1/72 
            //英寸常用的1024x768或800x600等标准的分辨率计算出来的dpi是一个常数：96
            //因此计算出来的毫米与像素的关系也约等于一个常数： 基本上 1毫米 约等于 3.78像素
            float rectHeight = (fontSize / 72) * 96;

            RectangleF textArea = new RectangleF(rectX, rectY, rectWidth, rectHeight);

            Brush whiteBrush = new SolidBrush(Color.White);

            Brush blackBrush = new SolidBrush(Color.Black);

            g.FillRectangle(blackBrush, rectX, rectY, rectWidth, rectHeight);

            g.DrawString(text, font, whiteBrush, textArea);

            bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);

            g.Dispose();
            bitmap.Dispose();
            image.Dispose();

        }


        /// <summary>
        /// 添加水印
        /// </summary>
        /// <param name="imgPath">原图片地址</param>
        /// <param name="sImgPath">水印图片地址</param>
        /// <returns>resMsg[0] 成功,失败 </returns>
        public static string[] AddWaterMark(string imgPath, string sImgPath)
        {
            string[] resMsg = new[] { "成功", sImgPath };
            using (Image image = Image.FromFile(imgPath))
            {
                try
                {
                    Bitmap bitmap = new Bitmap(image);

                    int width = bitmap.Width, height = bitmap.Height;
                    //水印文字
                    string text = "版权保密";

                    Graphics g = Graphics.FromImage(bitmap);

                    g.DrawImage(bitmap, 0, 0);

                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    g.DrawImage(image, new Rectangle(0, 0, width, height), 0, 0, width, height, GraphicsUnit.Pixel);

                    Font crFont = new Font("微软雅黑", 120, FontStyle.Bold);
                    SizeF crSize = new SizeF();
                    crSize = g.MeasureString(text, crFont);

                    //背景位置(去掉了. 如果想用可以自己调一调 位置.)
                    //graphics.FillRectangle(new SolidBrush(Color.FromArgb(200, 255, 255, 255)), (width - crSize.Width) / 2, (height - crSize.Height) / 2, crSize.Width, crSize.Height);

                    SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(120, 177, 171, 171));

                    //将原点移动 到图片中点
                    g.TranslateTransform(width/ 2, height / 2);
                    //以原点为中心 转 -45度
                    g.RotateTransform(-45);

                    g.DrawString(text, crFont, semiTransBrush, new PointF(0, 0));

                    //保存文件
                    bitmap.Save(sImgPath, System.Drawing.Imaging.ImageFormat.Jpeg);

                }
                catch (Exception e)
                {

                    resMsg[0] = "失败";
                    resMsg[1] = e.Message;
                }
            }

            return resMsg;
        }

        public ActionResult GetHelpConfigByClassId(string classid)
        {
            try
            {
                var result = new OrderBC().GetHelpConfigByClassId(classid);
                if (result == null)
                {
                    result = new HelpConfigEntity();
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new HelpConfigEntity(), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 公益金

        public ActionResult GetFundsConfigByClassId(string classid)
        {
            try
            {
                return Json(new BaseSetBC().GetFundsConfigByClassId(classid), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new VMPublicFunds(), JsonRequestBehavior.AllowGet);
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