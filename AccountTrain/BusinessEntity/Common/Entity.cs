using System;

namespace BusinessEntity.Common
{
    /// <summary>
	/// 默认的主键类型（INT）
    /// </summary>
    [Serializable]
    public abstract class Entity : Entity<int>, IEntity
    {
		public Entity()
		{
			if (IsTransient())
			{
				
			}
		}

		
		
    }
}