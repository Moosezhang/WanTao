using BusinessEntity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Common
{
	/// <summary>
	/// 实体表特性信息
	/// </summary>
	public class EntityAttribute 
	{
		/// <summary>
		/// 表名
		/// </summary>
		public string TableName { get; set; }

		/// <summary>
		/// 字段名
		/// </summary>
		public List<string> ListColumnName { get; set; }

		/// <summary>
		/// 新增字段名
		/// </summary>
		public List<string> InsertListColumnName { get; set; }

        /// <summary>
        /// 修改字段名
        /// </summary>
        public List<string> UpdateListColumnName { get; set; }
	
	}


	public class EntityHelper
	{
		/// <summary>
		/// 通过反射取自定义属性
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public static EntityAttribute DisplaySelfAttribute<T>() where T : class
		{
			string tableName = string.Empty;
			List<string> listColumnName = new List<string>();
			List<string> insertListColumnName = new List<string>();
            List<string> updateListColumnName = new List<string>();
			Type objType = typeof(T);

			//取属性上的自定义特性
			foreach (PropertyInfo propInfo in objType.GetProperties())
			{
				object[] objAttrs = propInfo.GetCustomAttributes(typeof(EnitityMappingAttribute), true);
				if (objAttrs.Length > 0)
				{
					EnitityMappingAttribute attr = objAttrs[0] as EnitityMappingAttribute;
					if (attr != null)
					{
                        listColumnName.Add(attr.ColumnName); //列名			
                        if (attr.InsterIgnoreColumn == false) 
						{
                            insertListColumnName.Add(attr.ColumnName);                  	
						}  
                        if (attr.UpdateIgnoreColumn == false) 
						{
                            updateListColumnName.Add(attr.ColumnName);            	
						} 
					}
				}
			}

			//取类上的自定义特性
			object[] objs = objType.GetCustomAttributes(typeof(EnitityMappingAttribute), true);
			foreach (object obj in objs)
			{
				EnitityMappingAttribute attr = obj as EnitityMappingAttribute;
				if (attr != null)
				{

					tableName = attr.TableName;//表名只有获取一次
					break;
				}
			}
			if (string.IsNullOrEmpty(tableName))
			{
				tableName = objType.Name.Replace("Entity","");
			}

            return new EntityAttribute { TableName = tableName, ListColumnName = listColumnName, InsertListColumnName = insertListColumnName, UpdateListColumnName = updateListColumnName };
		}
	}
}
