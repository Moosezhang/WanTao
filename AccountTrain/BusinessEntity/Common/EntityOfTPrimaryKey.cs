using System;
using System.Collections.Generic;

namespace BusinessEntity.Common
{
	/// <summary>
	/// 实现基本的接口。.
	/// 一个实体可以继承这个类的直接实现来识别接口.
	/// </summary>
	/// <typeparam name="TPrimaryKey">实体的主键的类型</typeparam>
	[Serializable]
	public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
	{
		/// <summary>
		/// 主键.
		/// </summary>
		public virtual TPrimaryKey Id { get; set; }

		/// <summary>
		/// 判断这个实体是短暂的 (它没有id).
		/// </summary>
		/// <returns> 如果这个实体是短暂的就返回true</returns>
		public virtual bool IsTransient()
		{
			return EqualityComparer<TPrimaryKey>.Default.Equals(Id, default(TPrimaryKey));
		}

		/// <重写/>
		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is Entity<TPrimaryKey>))
			{
				return false;
			}
			//Same instances must be considered as equal
			if (ReferenceEquals(this, obj))
			{
				return true;
			}
			//Transient objects are not considered as equal
			var other = (Entity<TPrimaryKey>)obj;
			if (IsTransient() && other.IsTransient())
			{
				return false;
			}
			//Must have a IS-A relation of types or must be same type
			var typeOfThis = GetType();
			var typeOfOther = other.GetType();
			if (!typeOfThis.IsAssignableFrom(typeOfOther) && !typeOfOther.IsAssignableFrom(typeOfThis))
			{
				return false;
			}
			return Id.Equals(other.Id);
		}

		/// <重写/>
		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		/// <重写/>
		public static bool operator ==(Entity<TPrimaryKey> left, Entity<TPrimaryKey> right)
		{
			if (Equals(left, null))
			{
				return Equals(right, null);
			}

			return left.Equals(right);
		}

		/// <重写/>
		public static bool operator !=(Entity<TPrimaryKey> left, Entity<TPrimaryKey> right)
		{
			return !(left == right);
		}

		/// <重写/>
		public override string ToString()
		{
			return string.Format("[{0} {1}]", GetType().Name, Id);
		}
	}
}
