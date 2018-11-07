using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
	/// <summary>
	/// 加入些特性后，在实体差异比较中会忽略该属性
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class IngoreCompareAttribute : Attribute
	{
		public IngoreCompareAttribute()
		{
			Flag = true;
		}

		public bool Flag { get; set; }
	}

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class MetricAttribute : Attribute
	{
		public MetricAttribute()
		{
		}

		public string Code { get; set; }
	}

	/// <summary>
	/// 不验证用户登陆信息
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class IngoreValidateAttribute : Attribute
	{

	}
}
