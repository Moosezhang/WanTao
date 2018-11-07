using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Routing;
using System.Security.Principal;
using System.Data;
using System.Collections;
using System.Web.Optimization;
using System.IO;
using System.Text;
using System.Web.UI;
using BusinessEntity.Common;
using BusinessComponent;
using BusinessEntitys;

namespace AccountTrain.Web.Common
{
    public static class SystemMenuHelper
    {
       

        /// <summary>
        /// 获得系统菜单列表
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static IList<SystemMenuInfo> GetSystemMenuList(this HtmlHelper htmlHelper)
        {
            //获取角色用户下所有菜单
            IList<SystemMenuInfo> curMenuList = new List<SystemMenuInfo>();
            List<MenuEntity> moduleMenus = new List<MenuEntity>();
            try
            {
                var CurrentUser = CacheManager.Instance.CurrentUser;
                if (CurrentUser == null)
                    return curMenuList;
                AuthBC bc = new AuthBC();
                IList<MenuEntity> moduleList = bc.GetMenusByLoginName(CurrentUser.Account);
                //抓取父菜单Id
                List<MenuEntity> fatherIds = new List<MenuEntity>();
                foreach (var item in moduleList)
                {
                    MenuEntity entity = new AuthBC().GetMenuByMenuId(item.Menu_FatherId);
                    fatherIds.Add(entity);
                }

                var ml = fatherIds.Distinct().ToList();
                if (ml != null && ml.Count() > 0)
                {
                    moduleMenus = ml.OrderBy(p => p.Menu_Order).ToList();
                    for (int i = 0; i < moduleMenus.Count; i++)
                    {
                        var curModule = moduleMenus[i];
                        SystemMenuInfo tempMenu = new SystemMenuInfo
                        {
                            MenuId = curModule.Menu_Id.ToString(),
                            MenuName = curModule.Menu_Title,
                            Url = curModule.Menu_Url,
                            SubMenus = new List<SystemMenuInfo>()
                        };

                        var subl = moduleList.Where(p => p.Menu_FatherId == curModule.Menu_Id.ToString());
                        if (subl == null)
                            tempMenu.SubMenus.Add(new SystemMenuInfo());
                        var subMenus = subl.OrderBy(a => a.Menu_Order).ToList();
                        foreach (var item in subMenus)
                        {

                            SystemMenuInfo tempSubMenu = new SystemMenuInfo
                            {
                                MenuId = item.Menu_Id.ToString(),
                                MenuName = item.Menu_Title,
                                Url = item.Menu_Url
                            };
                            tempMenu.SubMenus.Add(tempSubMenu);
                        }

                        if (!curMenuList.Any(p => p.MenuName == tempMenu.MenuName))
                        {
                            curMenuList.Add(tempMenu);
                        }


                    }
                    return curMenuList;
                }
                else
                {
                    return curMenuList;
                }
            }
            catch (Exception ex)
            {
                return curMenuList;
            }     
        }

        /// <summary>
        /// 获得所有系统菜单列表
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static IList<SystemMenuInfo> GetSystemMenuList()
        {
            //获取角色用户下所有菜单
            IList<SystemMenuInfo> curMenuList = new List<SystemMenuInfo>();
            List<MenuEntity> moduleMenus = new List<MenuEntity>();
            try
            {
                var CurrentUser = CacheManager.Instance.CurrentUser;
                if (CurrentUser == null)
                    return curMenuList;
                AuthBC bc = new AuthBC();
                IList<MenuEntity> moduleList = bc.GetAllMenus();
                var ml = moduleList.Where(p => p.Menu_FatherId == null);
                if (ml != null && ml.Count() > 0)
                {
                    moduleMenus = ml.OrderBy(p => p.Menu_Order).ToList();
                    for (int i = 0; i < moduleMenus.Count; i++)
                    {
                        var curModule = moduleMenus[i];
                        SystemMenuInfo tempMenu = new SystemMenuInfo
                        {
                            MenuId = curModule.Menu_Id.ToString(),
                            MenuName = curModule.Menu_Title,
                            Url = curModule.Menu_Url,
                            SubMenus = new List<SystemMenuInfo>()
                        };

                        var subl = moduleList.Where(p => p.Menu_FatherId == curModule.Menu_Id.ToString());
                        if (subl == null)
                            tempMenu.SubMenus.Add(new SystemMenuInfo());
                        var subMenus = subl.OrderBy(a => a.Menu_Order).ToList();
                        foreach (var item in subMenus)
                        {

                            SystemMenuInfo tempSubMenu = new SystemMenuInfo
                            {
                                MenuId = item.Menu_Id.ToString(),
                                MenuName = item.Menu_Title,
                                Url = item.Menu_Url
                            };
                            tempMenu.SubMenus.Add(tempSubMenu);
                        }

                        if (!curMenuList.Any(p => p.MenuName == tempMenu.MenuName))
                        {
                            curMenuList.Add(tempMenu);
                        }


                    }
                    return curMenuList;
                }
                else
                {
                    return curMenuList;
                }
            }
            catch (Exception ex)
            {
                return curMenuList;
            }
        }  
    }
}