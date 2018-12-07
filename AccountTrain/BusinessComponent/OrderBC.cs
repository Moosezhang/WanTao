using BusinessEntity.Model;
using BusinessEntitys;
using Common;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessComponent
{
    public class OrderBC
    {

        public List<OrderEntity> GetOrderListByCondition(string name, string orderNo, string startDate, string endDate)
        {
            OrderDA da = new OrderDA();
            var result=da.GetOrderListByCondition(name, orderNo, startDate, endDate);
            if (result != null && result.Count > 0)
            {
                foreach (var item in result)
                {
                    item.OrderSource = new BaseSetBC().GetDicItemValueByKey(item.OrderSource,DictionaryConstant.OSKey).ItemValue;
                }
            }
            return result;
        }

        public List<OrderGoodsEntity> GetOrderGoodsListByOrderId(string orderId)
        {
            OrderDA da = new OrderDA();
            return da.GetOrderGoodsListByOrderId(orderId);
        }

        public List<OrderGoodsEntity> GetOrderGoodsListByOpenId(string OpenId)
        {
            OrderDA da = new OrderDA();
            return da.GetOrderGoodsListByOpenId(OpenId);
        }

        public List<OrderGoodsEntity> GetOrderGoodsListByOpenIdandOrderNo(string OpenId,string orderNo)
        {
            OrderDA da = new OrderDA();
            return da.GetOrderGoodsListByOpenIdandOrderNo(OpenId, orderNo);
        }
        public OrderEntity GetOrderByOpenIdandClassId(string OpenId, string ClassId)
        {
            OrderDA da = new OrderDA();
            return da.GetOrderByOpenIdandClassId(OpenId, ClassId);
        }

        public OrderEntity GetOrderByOrderNo(string orderNo)
        {
            OrderDA da = new OrderDA();
            return da.GetOrderByOrderNo(orderNo);
        }

        #region 团购

        public VMGBClass GetGbClass(string classid)
        {
            OrderDA da = new OrderDA();
            return da.GetGbClass(classid);
        }

        public GroupBuyEntity GetGroupBuyByClassId(string classid)
        {
            OrderDA da = new OrderDA();
            return da.GetGroupBuyByClassId(classid);
        }

        public GroupBuyConfigEntity GetGroupBuyConfigByClassId(string classid)
        {
            OrderDA da = new OrderDA();
            return da.GetGroupBuyConfigByClassId(classid);
        }

        public int AddGroupBuy(GroupBuyEntity entity, string loginName)
        {
            OrderDA da = new OrderDA();
            return da.AddGroupBuy(entity, loginName);
        }

        public int AddGroupBuyMember(GroupBuyMemberEntity entity, string loginName)
        {
            OrderDA da = new OrderDA();
            return da.AddGroupBuyMember(entity, loginName);
        }

        public int UpdateGroupBuyStatus(string GroupBuyId, int Status)
        {
            OrderDA da = new OrderDA();
            return da.UpdateGroupBuyStatus(GroupBuyId, Status);
        }

        public int UpdateGroupBuyCount(string GroupBuyId)
        {
            OrderDA da = new OrderDA();
            return da.UpdateGroupBuyCount(GroupBuyId);
        }
        #endregion
        


        public string SaveOrder(OrderEntity order, List<OrderGoodsEntity> orderGoods, string loginName) 
        {
            OrderDA da = new OrderDA();
            return da.SaveOrder(order, orderGoods, loginName);
        }


        public int UpdateOrderStatus(string orderNo, int status)
        {
            OrderDA da = new OrderDA();
            return da.UpdateOrderStatus(orderNo, status);
        }

        public int UpdatePayInfo(string outTradeNumber, string PayInfo, string orderNo)
        {
            OrderDA da = new OrderDA();
            return da.UpdatePayInfo(outTradeNumber, PayInfo, orderNo);
        }

        #region 砍价
        public BargainEntity GetBargainByOpenIdAndClassId(string classid, string openid)
        {
            OrderDA da = new OrderDA();
            return da.GetBargainByOpenIdAndClassId(classid, openid);
        }

        public BargainConfigEntity GetBargainConfigByClassId(string classid)
        {
            OrderDA da = new OrderDA();
            return da.GetBargainConfigByClassId(classid);
        }

        public int AddBargain(BargainEntity bargain, string loginName)
        {
            OrderDA da = new OrderDA();
            return da.AddBargain(bargain,loginName);
        }

        public int AddBargainLog(BargainLogEntity bargain, string loginName)
        {
            OrderDA da = new OrderDA();
            return da.AddBargainLog(bargain,loginName);
        }

        public int UpdateBargainNowPrice(string BargainId, decimal nowPrice)
        {
            OrderDA da = new OrderDA();
            return da.UpdateBargainNowPrice(BargainId,nowPrice);
        }

        public VMBargainClass GetBargainClass(string BargainId)
        {
            OrderDA da = new OrderDA();
            return da.GetBargainClass(BargainId);
        }

        public List<VMBargainLog> GetBargainLogs(string BargainId)
        {
            OrderDA da = new OrderDA();
            return da.GetBargainLogs(BargainId);
        }


        public int UpdateBargainStatus(string BargainId, int Status) 
        {
            OrderDA da = new OrderDA();
            return da.UpdateBargainStatus(BargainId,Status);
        }
        #endregion

        #region 助力
        public HelpInfoEntity GetHelpByOpenIdAndClassId(string classid, string openid)
        {
            OrderDA da = new OrderDA();
            return da.GetHelpByOpenIdAndClassId(classid, openid);
        }

        public HelpInfoEntity GetHelpByHelpInfoId(string helpId)
        {
            OrderDA da = new OrderDA();
            return da.GetHelpByHelpInfoId(helpId);
        }
        public HelpConfigEntity GetHelpConfigByClassId(string classid)
        {
            OrderDA da = new OrderDA();
            return da.GetHelpConfigByClassId(classid);
        }


        public HelpMemberEntity GetHelpMemberByOpenid(string openid)
        {
            OrderDA da = new OrderDA();
            return da.GetHelpMemberByOpenid(openid);
        }


        public int AddHelpInfo(HelpInfoEntity help, string loginName)
        {
            OrderDA da = new OrderDA();
            return da.AddHelpInfo(help,loginName);
        }

        public int UpdateHelpNowCount(string HelpInfoId, int NowCount)
        {
            OrderDA da = new OrderDA();
            return da.UpdateHelpNowCount(HelpInfoId,NowCount);
        }


        public int AddHelpMember(HelpMemberEntity member, string loginName)
        {
            OrderDA da = new OrderDA();
            return da.AddHelpMember(member, loginName);
        }
        #endregion

        #region 积分
        public PointsEntity GetPointsByOpenid(string openid)
        {

            OrderDA da = new OrderDA();
            return da.GetPointsByOpenid(openid);
        }

        public int AddPoint(PointsEntity entity, string loginName)
        {
            OrderDA da = new OrderDA();
            return da.AddPoint(entity, loginName);
        }

        public int AddPointLog(PointsLogEntity entity, string loginName)
        {
            OrderDA da = new OrderDA();
            return da.AddPointLog(entity, loginName);
        }

        public int UpdatePonits(string OpenId, decimal point)
        {
            OrderDA da = new OrderDA();
            return da.UpdatePonits(OpenId, point);
        }
        #endregion
    }
}
