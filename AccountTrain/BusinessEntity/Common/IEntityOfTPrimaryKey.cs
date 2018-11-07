using System;
namespace BusinessEntity.Common
{
	/// <summary>
	/// 定义基础实体类型的接口。系统中的所有实体必须实现此接口。
	/// </summary>
	/// <typeparam name="TPrimaryKey">实体的主键的类型</typeparam>
	public interface IEntity<TPrimaryKey>
	{
		/// <summary>
		/// 主键
		/// </summary>
		TPrimaryKey Id { get; set; }

		/// <summary>
		/// 判断这个实体是短暂的 (它没有id).
		/// </summary>
		/// <returns> 如果这个实体是短暂的就返回true</returns>
		bool IsTransient();

	}
}
