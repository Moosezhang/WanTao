using BusinessEntity.Model;
using BusinessEntitys;
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
                    item.OrderSource = new BaseSetBC().GetDicItemValueByKey(item.OrderSource,"OSKey").ItemValue;
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


        public VMGBClass GetGbClass(string classid)
        {
            OrderDA da = new OrderDA();
            return da.GetGbClass(classid);
        }


        public string SaveOrder(OrderEntity order, List<OrderGoodsEntity> orderGoods, string loginName) 
        {
            OrderDA da = new OrderDA();
            return da.SaveOrder(order, orderGoods, loginName);
        }
        #region 砍价
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

        public int UpdateBargainNowPrice(string BargainId, string nowPrice)
        {
            OrderDA da = new OrderDA();
            return da.UpdateBargainNowPrice(BargainId,nowPrice);
        }

        public VMBargainClass GetBargainClass(string BargainId)
        {
            OrderDA da = new OrderDA();
            return da.GetBargainClass(BargainId);
        }

        public List<BargainLogEntity> GetBargainLogs(string BargainId)
        {
            OrderDA da = new OrderDA();
            return da.GetBargainLogs(BargainId);
        }

        #endregion


    }
}
