using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Common
{
	/// <summary>
    /// 自定义特性 属性或者类可用  支持继承
    /// </summary>
	[AttributeUsage(AttributeTargets.Property|AttributeTargets.Class, Inherited = true)]
    public class EnitityMappingAttribute : Attribute
    {
		private string tableName;
		/// <summary>
		/// 实体实际对应的表名
		/// </summary>
		public string TableName
		{
			get { return tableName; }
			set { tableName = value; }
		}

		private string columnName;
		/// <summary>
		/// 列名
		/// </summary>
		public string ColumnName
		{
			get { return columnName; }
			set { columnName = value; }
		}

        private bool insterIgnoreColumn = false;
		/// <summary>
		/// 新增忽略字段
		/// </summary>
		public bool InsterIgnoreColumn
		{
            get { return insterIgnoreColumn; }
            set { insterIgnoreColumn = value; }
		}

        private bool updateIgnoreColumn = false;
        /// <summary>
        /// 修改忽略字段
        /// </summary>
        public bool UpdateIgnoreColumn
        {
            get { return updateIgnoreColumn; }
            set { updateIgnoreColumn = value; }
        }
	
	}
}
