using System;

namespace BusinessEntity.Common
{
    /// <summary>
	/// Ĭ�ϵ��������ͣ�INT��
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