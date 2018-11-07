using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 枚举帮助类
    /// </summary>
    public static class EnumHelper
    {
        #region 私有方法
        private static DescriptionAttribute[] GetDescriptAttr(this FieldInfo fieldInfo)
        {
            if (fieldInfo != null)
            {
                return (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            }
            return null;
        }
        #endregion 私有方法
        /// <summary>
        ///判断枚举是否包括枚举常数名称
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="enumName">枚举常数名称</param>
        /// <returns>是否包括枚举常数名称</returns>
        /// 创建  人:
        /// 创建时间:2015-06-04 17:17
        /// 备注说明:<c>null</c>
        public static bool CheckedContainEnumName<T>(string enumName) where T : struct, IConvertible
        {
            bool _result = false;
            if (typeof(T).IsEnum)
            {
                string[] _enumnName = Enum.GetNames(typeof(T));
                if (_enumnName != null)
                {
                    for (int i = 0; i < _enumnName.Length; i++)
                    {
                        if (string.Compare(_enumnName[i], enumName, true) == 0)
                        {
                            _result = true;
                            break;
                        }
                    }
                }
            }
            return _result;
        }
        /// <summary>
        /// 从枚举中获取Description
        /// </summary>
        /// <param name="targetEnum">需要获取枚举描述的枚举</param>
        /// <returns>描述内容</returns>
        public static string GetDescription(this Enum targetEnum)
        {
            string _description = string.Empty;
            FieldInfo _fieldInfo = targetEnum.GetType().GetField(targetEnum.ToString());
            DescriptionAttribute[] _attributes = _fieldInfo.GetDescriptAttr();
            if (_attributes != null && _attributes.Length > 0)
                _description = _attributes[0].Description;
            else
                _description = targetEnum.ToString();
            return _description;
        }
        /// <summary>
        /// 根据Description获取枚举
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="description">枚举描述</param>
        /// <returns>枚举</returns>
        public static T ParseEnumDescription<T>(this string description, T defaultValue) where T : struct, IConvertible
        {
            if (typeof(T).IsEnum)
            {
                Type _type = typeof(T);
                foreach (FieldInfo field in _type.GetFields())
                {
                    DescriptionAttribute[] _description = field.GetDescriptAttr();
                    if (_description != null && _description.Length > 0)
                    {
                        if (string.Compare(_description[0].Description, description, true) == 0)
                        {
                            defaultValue = (T)field.GetValue(null);
                            break;
                        }
                    }
                    else
                    {
                        if (string.Compare(field.Name, description, true) == 0)
                        {
                            defaultValue = (T)field.GetValue(null);
                            break;
                        }
                    }
                }
            }
            return defaultValue;
        }
        /// <summary>
        /// 将枚举常数名称转换成枚举
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="enumName">枚举常数名称</param>
        /// <returns></returns>
        /// 创建  人:
        /// 创建时间:2015-06-05 9:16
        /// 备注说明:<c>null</c>
        public static T ParseEnumName<T>(this string enumName) where T : struct, IConvertible
        {
            return (T)Enum.Parse(typeof(T), enumName, true);
        }
    }
}