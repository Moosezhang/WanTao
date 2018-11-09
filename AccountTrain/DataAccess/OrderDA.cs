﻿using Common;
using DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using BusinessEntitys;
using BusinessEntity.Model;
using System.Data.SqlClient;

namespace DataAccess
{
    public class OrderDA
    {

        public List<OrderEntity> GetOrderListByCondition(string name, string orderNo, string startDate, string endDate)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select * from train_order
                                where 1=1");
                if (!string.IsNullOrEmpty(name))
                {
                    string sql = string.Format(" and Nickname='{0}'", name);
                    query = query + sql;
                }
                if (!string.IsNullOrEmpty(orderNo))
                {
                    string sql = string.Format(" and OrderNo='{0}'", orderNo);
                    query = query + sql;
                }
                if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                {
                    string sql = string.Format(" and CreateTime between '{0}' and '{1}'", startDate, endDate);
                    query = query + sql;
                }

                return conn.Query<OrderEntity>(query).ToList();
                
            }
        }

        public List<OrderGoodsEntity> GetOrderGoodsListByOrderId(string orderId)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select * from Train_OrderGoods
                                               where OrderId='{0}'", orderId);

                return conn.Query<OrderGoodsEntity>(query).ToList();

            }
        }

        public List<OrderGoodsEntity> GetOrderGoodsListByOpenId(string OpenId)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select t.* from Train_OrderGoods t
                                               inner join train_Order t1 on t.OrderId=t1.OrderId
                                               where t1.Openid='{0}'", OpenId);

                return conn.Query<OrderGoodsEntity>(query).ToList();

            }
        }

        public List<OrderGoodsEntity> GetOrderGoodsListByOpenIdandOrderNo(string OpenId,string orderNo)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select t.* from Train_OrderGoods t
                                               inner join train_Order t1 on t.OrderId=t1.OrderId
                                               where t1.Openid='{0}' and t1.orderNo='{1}'", OpenId, orderNo);

                return conn.Query<OrderGoodsEntity>(query).ToList();

            }
        }

        public OrderEntity GetOrderByOpenIdandClassId(string OpenId, string ClassId)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select t.*
                                                from Train_Order t
                                                inner join Train_OrderGoods t1 on t.OrderId=t1.OrderId
                                                where t.Openid='{0}' and t1.ClassId='{1}'
                                                ", OpenId, ClassId);

                return conn.Query<OrderEntity>(query).FirstOrDefault();

            }
        }

        public OrderEntity GetOrderByOrderNo(string orderNo)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select t.*
                                                from Train_Order t
                                                where t.orderNo='{0}'
                                                ", orderNo);

                return conn.Query<OrderEntity>(query).FirstOrDefault();

            }
        }

        public string SaveOrder(OrderEntity order,List<OrderGoodsEntity> orderGoods, string loginName)
        {
            string strReturn = "";
            SqlConnection cnn = new System.Data.SqlClient.SqlConnection(ConfigSettings.ConnectionString);
            SqlCommand cm = new System.Data.SqlClient.SqlCommand();
            cm.Connection = cnn;
            cnn.Open();
            SqlTransaction trans = cnn.BeginTransaction();
            cm.Transaction = trans;
            try
            {
                string OrderId = Guid.NewGuid().ToString();
                int status = 1;
                if (order.PayPrice == 0)
                {
                    status = 2;                    
                }
                //新增订单明细
                foreach (var item in orderGoods)
                {
                    string sql=string.Format(@"INSERT INTO Train_OrderGoods
                                               (GoodsId
                                               ,OrderId
                                               ,ClassId
                                               ,ClassName
                                               ,Price
                                               ,Status
                                               ,CreateTime
                                               ,CreateUser
                                               ,UpdateTime
                                               ,UpdateUser)
                                         VALUES
                                               ('{0}'
                                               ,'{1}'
                                               ,'{2}'
                                               ,'{3}'
                                               ,'{4}'
                                               ,'{5}'
                                               ,GETDATE()
                                               ,'{6}'
                                               ,GETDATE()
                                               ,'{7}')",
                    Guid.NewGuid().ToString(), OrderId, item.ClassId, item.ClassName, item.Price, 1, loginName, loginName);
                    cm.CommandText = sql;
                    cm.ExecuteNonQuery();
                    if (status == 2)//价格为0，直接状态变为已支付，并且更新课程中，热度字段
                    {
                        new ClassDA().UpdateClassHot(item.ClassId);
                    }                        
                }


                
                    

                string OrderSql = string.Format(@"INSERT INTO Train_Order
                                               (OrderId
                                               ,OrderNo
                                               ,Openid
                                               ,PayPrice
                                               ,OrderSource
                                               ,Status
                                               ,CreateTime
                                               ,CreateUser
                                               ,UpdateTime
                                               ,UpdateUser
                                               ,Nickname)
                                         VALUES
                                               ('{0}'
                                               ,'{1}'
                                               ,'{2}'
                                               ,'{3}'
                                               ,'{4}'
                                               ,'{5}'
                                               ,GETDATE()
                                               ,'{6}'
                                               ,GETDATE()
                                               ,'{7}'
                                               ,'{8}')",
                OrderId, order.OrderNo, order.Openid, order.PayPrice, order.OrderSource, status, loginName, loginName, order.Nickname);
                cm.CommandText = OrderSql;
                cm.ExecuteNonQuery();

                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                strReturn = "false";
            }
            finally
            {
                cnn.Close();
                trans.Dispose();
                cnn.Dispose();
                strReturn = "true";
            }

            return strReturn;
        }

        public int UpdateOrderStatus(string orderNo, int status)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"update Train_Order set status={0}  where OrderNo='{1}'", status, orderNo);
                return conn.Execute(query);
            }
        }

     
        #region 团购数据
        public VMGBClass GetGbClass(string classid)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select t.*,t1.GroupPrice,t1.NeedCount,t1.StartTime,t1.EndTime,isnull(t2.NowCount,0) as NowCount
                                            from Train_Class t
                                            inner join Train_GroupBuyConfig t1 on t.ClassId=t1.ClassId
                                            left join Train_GroupBuy t2 on t.ClassId=t2.ClassId and t2.status=1
                                            where t.ClassId='{0}' ", classid);

                return conn.Query<VMGBClass>(query).FirstOrDefault();

            }
        }

        /// <summary>
        /// 根据classId获取团购数据
        /// </summary>
        /// <param name="classid"></param>
        /// <returns></returns>
        public GroupBuyEntity GetGroupBuyByClassId(string classid)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select * from Train_GroupBuy
                                            where status=1 and ClassId='{0}'", classid);

                return conn.Query<GroupBuyEntity>(query).FirstOrDefault();

            }
        }

        public GroupBuyConfigEntity GetGroupBuyConfigByClassId(string classid)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select * from Train_GroupBuyConfig
                                            where status=1 and ClassId='{0}'", classid);

                return conn.Query<GroupBuyConfigEntity>(query).FirstOrDefault();

            }
        }


        public int AddGroupBuy(GroupBuyEntity entity, string loginName)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"INSERT INTO Train_GroupBuy
                                               (GroupBuyId
                                               ,ClassId
                                               ,NowCount
                                               ,Status
                                               ,CreateTime
                                               ,CreateUser
                                               ,UpdateTime
                                               ,UpdateUser)
                                         VALUES
                                               ('{0}'
                                               ,'{1}'
                                               ,'{2}'
                                               ,'{3}'
                                               ,getdate()
                                               ,'{4}'
                                               ,getdate()
                                               ,'{5}')",
                    Guid.NewGuid().ToString(), entity.ClassId, entity.NowCount, 1, loginName, loginName);
                return conn.Execute(query);
            }
        }

        public int AddGroupBuyMember(GroupBuyMemberEntity entity, string loginName)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"INSERT INTO Train_GroupBuyMember
                                               (MemberId
                                               ,GroupBuyId
                                               ,GroupPrice
                                               ,openId
                                               ,Status
                                               ,CreateTime
                                               ,CreateUser
                                               ,UpdateTime
                                               ,UpdateUser)
                                         VALUES
                                               ('{0}'
                                               ,'{1}'
                                               ,'{2}'
                                               ,'{3}'
                                               ,'{4}'
                                               ,getdate()
                                               ,'{5}'
                                               ,getdate()
                                               ,'{6}')",
                    Guid.NewGuid().ToString(), entity.GroupBuyId, entity.GroupPrice, entity.openId, 1, loginName, loginName);
                return conn.Execute(query);
            }
        }

        public int UpdateGroupBuyStatus(string GroupBuyId, int Status)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"update Train_GroupBuy set status={0}  where GroupBuyId='{1}'", Status, GroupBuyId);
                return conn.Execute(query);
            }
        }

        public int UpdateGroupBuyCount(string GroupBuyId)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"update Train_GroupBuy set NowCount=NowCount+1 where GroupBuyId='{0}'", GroupBuyId);
                return conn.Execute(query);
            }
        }
        #endregion


        #region 新增砍价数据
        public BargainEntity GetBargainByOpenIdAndClassId(string openid,string classid)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select * from Train_Bargain
                                            where status=1 and ClassId='{0}' and OpenId='{1}'", classid, openid);

                return conn.Query<BargainEntity>(query).FirstOrDefault();

            }
        }

        public BargainConfigEntity GetBargainConfigByClassId(string classid)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select * from Train_BargainConfig
                                            where status=1 and ClassId='{0}'", classid);

                return conn.Query<BargainConfigEntity>(query).FirstOrDefault();

            }
        }

        public int AddBargain(BargainEntity bargain, string loginName)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"INSERT INTO Train_Bargain
                                               (BargainId
                                               ,OpenId
                                               ,ClassId
                                               ,PrePrice
                                               ,NowPrice
                                               ,Status
                                               ,CreateTime
                                               ,CreateUser
                                               ,UpdateTime
                                               ,UpdateUser)
                                         VALUES
                                               ('{0}'
                                               ,'{1}'
                                               ,'{2}'
                                               ,'{3}'
                                               ,'{4}'
                                               ,'{5}'
                                               ,getdate()
                                               ,'{6}'
                                               ,getdate()
                                               ,'{7}')",
                    bargain.BargainId, bargain.OpenId, bargain.ClassId, bargain.PrePrice, bargain.NowPrice, 1, loginName, loginName);
                return conn.Execute(query);
            }
        }

        public int AddBargainLog(BargainLogEntity bargain, string loginName)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"INSERT INTO Train_BargainLog
                                               (LogId
                                               ,BargainId
                                               ,OpenId
                                               ,BargainPrice
                                               ,Status
                                               ,CreateTime
                                               ,CreateUser
                                               ,UpdateTime
                                               ,UpdateUser)
                                         VALUES
                                               ('{0}'
                                               ,'{1}'
                                               ,'{2}'
                                               ,'{3}'
                                               ,'{4}'
                                               ,getdate()
                                               ,'{5}'
                                               ,getdate()
                                               ,'{6}')",
                    Guid.NewGuid().ToString(), bargain.BargainId, bargain.OpenId, bargain.BargainPrice, 1, loginName, loginName);
                return conn.Execute(query);
            }
        }

        public int UpdateBargainNowPrice(string BargainId, decimal nowPrice)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@" UPDATE Train_Bargain
                                                    SET NowPrice= {0}                                                         
                                                       ,UpdateTime = getdate()
                                                    WHERE ClassId='{1}'",
                                          nowPrice, BargainId);
                return conn.Execute(query);
            }
        }

        public VMBargainClass GetBargainClass(string BargainId)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select t.*,isnull(t2.PrePrice,0) as PrePrice,isnull(t2.NowPrice,0) as NowPrice,isnull(t1.FloorPrice,0) as FloorPrice,t1.StartTime,t1.EndTime,t2.OpenId as ownerOpenid
                                                from Train_Class t
                                                inner join Train_BargainConfig t1 on t.ClassId=t1.ClassId
                                                inner join Train_Bargain t2 on t.ClassId=t2.ClassId
                                                where t2.BargainId='{0}'", BargainId);

                return conn.Query<VMBargainClass>(query).FirstOrDefault();

            }
        }

        public List<BargainLogEntity> GetBargainLogs(string BargainId)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select t.* from Train_BargainLog t
                                                where t.status=1 and t.BargainId='{0}'", BargainId);

                return conn.Query<BargainLogEntity>(query).ToList();

            }
        }

        public int UpdateBargainStatus(string BargainId, int Status)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"update Train_Bargain set status={0}  where BargainId='{1}'", Status, BargainId);
                return conn.Execute(query);
            }
        }
           
        #endregion

    }
}
