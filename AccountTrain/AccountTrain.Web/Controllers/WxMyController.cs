using BusinessComponent;
using BusinessEntity.Model;
using BusinessEntitys;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountTrain.Web.Controllers
{
    public class WxMyController : WxBaseController
    {
        /// <summary>
        /// 个人中心页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string openid, string code, string state)
        {
            if (string.IsNullOrEmpty(openid))
            {
                openid = GetOpenId(code).openid;

                if (string.IsNullOrEmpty(openid))
                {
                    //Response.Redirect(string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxa7f322fb262e5a7b&redirect_uri=http%3a%2f%2f {0}%2fonebox%2fgoparty%2findex&response_type=code&scope=snsapi_userinfo&state=1#wechat_redirect", setting.AppDomainName.Replace("http://", "")));
                    Response.Redirect(CommonHelper.GetRedirect("My%2fRegistered"));
                }
            }

            ViewBag.Openid = openid;

            return View();
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

        #region 注册页面
        /// <summary>
        /// 注册页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Registered(string openid, string code, string state)
        {
            if (string.IsNullOrEmpty(openid))
            {
                openid = GetOpenId(code).openid;

                if (string.IsNullOrEmpty(openid))
                {
                    //Response.Redirect(string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxa7f322fb262e5a7b&redirect_uri=http%3a%2f%2f {0}%2fonebox%2fgoparty%2findex&response_type=code&scope=snsapi_userinfo&state=1#wechat_redirect", setting.AppDomainName.Replace("http://", "")));
                    Response.Redirect(CommonHelper.GetRedirect("My%2fRegistered"));
                }
            }

            ViewBag.Openid = openid;

            return View();
        }


        /// <summary>
        /// 获取短信验证码
        /// </summary>
        /// <param name="strTel"></param>
        /// <returns></returns>
        public ActionResult getValidSMS(string strTel)
        {
            try
            {
                //List<WxUserEntity> list = null;
                //校验次手机号是否已注册
                var list = new WxUserBC().GetWxUserListByCondition(null, strTel,null,null).ToList();
                if (list.Count > 0)
                {
                    return Json("该手机号已注册", JsonRequestBehavior.AllowGet); 
                }
                else
                {
                    string yzmCode = CommonHelper.CreateNumber(4, true);
                    //Session["RegisterCode"] = new SMSCodes { Code = yzmCode, CreteTime = DateTime.Now, IsUsered = false };
                    Session["RegisterCode"] = "1234";
                    //SendMessageHelper.SmsSend(strTel, SendMessageHelper.YZMMBContent(yzmCode));
                    return Json("Success", JsonRequestBehavior.AllowGet); 
                }



             
            }
            catch (Exception ex)
            {
                Log.WriteLog("getValidSMS:" + ex.Message);
            }
            return Json("Error", JsonRequestBehavior.AllowGet); 
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            string ErrMsg;
            try
            {

                //数据校验
                if (string.IsNullOrEmpty(model.openid))
                {
                    ErrMsg = "OpenId不能为空";
                    return Json(ErrMsg, JsonRequestBehavior.AllowGet); 
                }

                if (string.IsNullOrEmpty(model.phone))
                {
                    ErrMsg = "手机号不能为空";
                    return Json(ErrMsg, JsonRequestBehavior.AllowGet); 
                }

                if (string.IsNullOrEmpty(model.code))
                {
                    ErrMsg = "验证码不能为空";
                    return Json(ErrMsg, JsonRequestBehavior.AllowGet); 
                }
                else
                {
                    //SMSCodes smsCode = Session["RegisterCode"] as SMSCodes;
                    if (Session["RegisterCode"] == null)
                    {
                        ErrMsg = "未获取验证码";
                        return Json(ErrMsg, JsonRequestBehavior.AllowGet); 
                    }
                    else
                    {
                        //if (!smsCode.IsExpired && smsCode.IsUsered == false)
                        //{

                            if (model.code != "1234")
                            {
                                ErrMsg = "验证码不正确";
                                return Json(ErrMsg, JsonRequestBehavior.AllowGet); 
                            }
                            else
                            {
                                string access_token = "";

                                access_token = Util.GetAccessTokenOpen();
                                var userInfo = GetUserInfo(model.openid, access_token);

                                //实体赋值
                                WxUserEntity Entry = new WxUserEntity();

                                //List<WxUserEntity> list = null;
                                //校验次手机号是否已注册
                                var list = new WxUserBC().GetWxUserListByCondition(null, model.phone, null, null).ToList();
                                if (list.Count > 0)
                                {
                                    ErrMsg = "手机号已被注册";
                                    return Json(ErrMsg, JsonRequestBehavior.AllowGet); 
                                }
                                else
                                {


                                    Entry.Subscribe = userInfo.subscribe;
                                    Entry.Openid = model.openid;
                                    Entry.Nickname = userInfo.nickname;
                                    Entry.Sex = userInfo.sex;
                                    Entry.City = userInfo.city;
                                    Entry.Country = userInfo.country;
                                    Entry.Province = userInfo.province;
                                    Entry.UserLanguage = userInfo.language;
                                    Entry.Headimgurl = userInfo.headimgurl;
                                    Entry.Phone = model.phone;
                                    int d = new WxUserBC().SaveWxUser(Entry,"system");

                                    if (d == 0)
                                    {
                                        ErrMsg = "插入数据库失败";
                                        return Json(ErrMsg, JsonRequestBehavior.AllowGet); 
                                    }
                                    else
                                    {
                                        //Session["DELIVERYMANCODE"] = new SMSCodes { Code = smsCode.Code, CreteTime = smsCode.CreteTime, IsUsered = true };
                                        ErrMsg = "Success";
                                        return Json(ErrMsg, JsonRequestBehavior.AllowGet); 
                                    }
                                }
                            }                    

                    }
                }
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message.ToString();
                return Json("Error"); 
            }

        }
        #endregion

        /// <summary>
        /// 我的课程页面
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult MyClass(string openid, string code, string state)
        {
            if (string.IsNullOrEmpty(openid))
            {
                openid = GetOpenId(code).openid;

                if (string.IsNullOrEmpty(openid))
                {
                    Response.Redirect(CommonHelper.GetRedirect("My%2fRegistered"));
                }
            }

            ViewBag.Openid = openid;

            return View();
        }

        /// <summary>
        /// 我的购物车页面
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult MyCar(string openid, string code, string state)
        {
            if (string.IsNullOrEmpty(openid))
            {
                openid = GetOpenId(code).openid;

                if (string.IsNullOrEmpty(openid))
                {
                    Response.Redirect(CommonHelper.GetRedirect("My%2fRegistered"));
                }
            }

            ViewBag.Openid = openid;

            return View();
        }


        public ActionResult GetMyClassByopenId(string openid, string title)
        {
            try
            {
                return Json(new ClassBC().GetMyClassByopenId(openid, title), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<VMOrderClass>(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetMyShopByopenId(string openid)
        {
            try
            {
                return Json(new ShopCarBC().GetMyShopByopenId(openid), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<VMClassCar>(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EnableShopCar(string shopCarId, int status)
        {
            try
            {
                if (string.IsNullOrEmpty(shopCarId))
                    return Json(string.Empty);
                var result = new ShopCarBC().EnableShopCar(shopCarId, status);
                if (result > 0)
                {
                    return Json("更新成功", JsonRequestBehavior.AllowGet);
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

        public ActionResult AddCar(string openid, string classId)
        {
            ShopCarEntity entity=new ShopCarEntity(){
                Openid=openid,
                ClassId=classId
            };
            var result = new ShopCarBC().AddShopCar(entity,openid);
            if (result > 0)
            {
                return Json("更新成功", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }
	}

    public class RegisterModel
    {
        public string openid { get; set; }
        public string phone { get; set; }
        public string code { get; set; }

    }
}

