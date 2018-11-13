using Common;
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

namespace DataAccess
{
    public class ShopCarDA
    {

        public List<VMClassCar> GetMyShopByopenId(string openid)
        {

            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@" select t1.*,t.ShopCarId, 0 as IsChecked
                                                from Train_ShopCar t
                                                inner join Train_Class t1 on t.ClassId=t1.ClassId
                                                where t.Openid='{0}' and t.status=1", openid);

                return conn.Query<VMClassCar>(query).ToList();
            }

        }

        public int AddShopCar(ShopCarEntity car, string loginName)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"INSERT INTO Train_ShopCar
                                               (ShopCarId
                                               ,Openid
                                               ,ClassId
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
                    Guid.NewGuid().ToString(), car.Openid, car.ClassId, 1, loginName, loginName);
                return conn.Execute(query);
            }
        }

        public int EnableShopCar(string shopCarId, int status)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"update Train_ShopCar set status={0}  where shopCarId='{1}'", status, shopCarId);
                return conn.Execute(query);
            }
        }

        public int EnableShopCar(string Openid, string ClassId, int status)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"update Train_ShopCar set status={0}  where Openid='{1}' and ClassId='{2}'", status, Openid, ClassId);
                return conn.Execute(query);
            }
        }

       
    }
}
