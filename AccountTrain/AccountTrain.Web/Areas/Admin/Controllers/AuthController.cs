using AccountTrain.Web.Common;
using BusinessComponent;
using BusinessEntity.Common;
using BusinessEntity.Model;
using BusinessEntitys;
using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountTrain.Web.Areas.Admin.Controllers
{
    public class AuthController : BaseController
    {
        //
        // GET: /Admin/Auth/
        public ActionResult Index()
        {
            return View();
        }

        #region 用户管理
        public ActionResult User()
        {
            return View();
        }

        //查询用户信息
        public ActionResult GetUserByCondition(string userName)
        {
            try
            {
                return Json(new UserBC().GetUserByCondition(userName), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<VMUserInfo>(), JsonRequestBehavior.AllowGet);
            }
        }

        //查询用户信息
        public ActionResult GetUserByKey(string userId)
        {
            try
            {
                return Json(new UserBC().GetUserByKey(userId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new UserEntity(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetUserInfo()
        {
            return Json(new UserBC().GetUserByKey(CurrentUserInfo.Id), JsonRequestBehavior.AllowGet);
        }
      
        /// <summary>
        /// 用户保存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult SaveUser(UserEntity user)
        {
            try
            {
                var result = new UserBC().SaveUser(user, CurrentUserInfo.Account);
                if (result == 0)
                    return Json(string.Empty);
                return Json("保存成功");
            }
            catch (Exception ex)
            {
                return Json(string.Empty);
            }
        }

        /// <summary>
        /// 用户启用/禁用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult EnableUser(string userId, int status)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    return Json(string.Empty);
                var result = new UserBC().EnableUser(userId, status);
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
        #endregion

        #region 角色管理
        public ActionResult Role()
        {
            return View();
        }

        public ActionResult GetRoles()
        {
            try
            {
                return Json(new RoleBC().GetRoles(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<VMUserInfo>(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetMenu()
        {
            try 
	        {
                return Json(SystemMenuHelper.GetSystemMenuList(), JsonRequestBehavior.AllowGet);
	        }
	        catch (Exception ex)
	        {

                return Json(new List<SystemMenuInfo>(), JsonRequestBehavior.AllowGet);
	        }
            
        }

        /// <summary>
        /// 根据角色ID获取已配置菜单列表
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public ActionResult GetMenusByRoleId(string RoleId)
        {
            try
            {
                List<string> menus=new List<string>();
                var result=new RoleBC().GetMenusByRoleId(RoleId);
                if(result!=null && result.Count>0)
                {
                    foreach (var item in result)
	                {
                        menus.Add(item.Menu_Id);
	                }
                }

                return Json(menus, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new List<string>(), JsonRequestBehavior.AllowGet);
            }
        }


        //查询角色信息
        public ActionResult GetRoleByKey(string roleId)
        {
            try
            {
                return Json(new RoleBC().GetRoleByKey(roleId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new UserEntity(), JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// 用户保存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult SaveRole(RoleEntity role)
        {
            try
            {
                var result = new RoleBC().SaveRole(role, CurrentUserInfo.Account);
                if (result == 0)
                    return Json(string.Empty);
                return Json("保存成功");
            }
            catch (Exception ex)
            {
                return Json(string.Empty);
            }
        }

        /// <summary>
        /// 用户启用/禁用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult EnableRole(string roleId, int status)
        {
            try
            {
                if (string.IsNullOrEmpty(roleId))
                    return Json(string.Empty);
                var result = new RoleBC().EnableRole(roleId, status);
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

        /// <summary>
        /// 保存角色菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult SaveRoleMenu(RoleMenuModel model)
        {
            try
            {
                var menusList = JsonConvert.DeserializeObject<List<string>>(model.menus);
                List<string> menusModel = new List<string>();
                if (menusList != null && menusList.Count > 0)
                {
                    foreach (var item in menusList)
                    {                        
                        var menuEntity = new AuthBC().GetMenuByMenuId(item);
                        if (!string.IsNullOrEmpty(menuEntity.Menu_FatherId))
                        {
                            menusModel.Add(item);
                        }                        
                    }
                    menusModel = menusModel.Distinct().ToList();
                    new RoleBC().DeleteRoleMenuByRoleId(model.roleId);
                    foreach (var item in menusModel)
                    {
                        new RoleBC().SaveRoleMenu(model.roleId, item, CurrentUserInfo.Account);
                    }
                    return Json("保存成功");
                }
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                
                 return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        
	}

    public class RoleMenuModel
    {
        public string roleId {get;set;}
        public string menus { get; set; }
    }
}