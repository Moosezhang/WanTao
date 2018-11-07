﻿using BusinessEntity.Model;
using BusinessEntitys;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessComponent
{
    public class ShopCarBC
    {
        public List<VMClassCar> GetMyShopByopenId(string openid)
        {
            var da = new ShopCarDA();
            return da.GetMyShopByopenId(openid);
        }

        public int AddShopCar(ShopCarEntity car, string loginName)
        {
            var da = new ShopCarDA();
            return da.AddShopCar(car, loginName);
        }

        public int EnableShopCar(string shopCarId, int status)
        {
            var da = new ShopCarDA();
            return da.EnableShopCar(shopCarId, status);
        }


    }
}
