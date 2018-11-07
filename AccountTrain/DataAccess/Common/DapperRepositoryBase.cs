using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using Common;
using BusinessEntity.Common;

namespace DataAccess.Common
{
	public class DapperRepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
		where TEntity : class, IEntity<TPrimaryKey>
	{
		/// <summary>
		/// 获取实体对应的表名
		/// </summary>
		protected EntityAttribute EntityAttributeInfo { get { return EntityHelper.DisplaySelfAttribute<TEntity>(); } }

		/// <summary>
		/// 通过主键获取实体数据
		/// </summary>
		/// <param name="id">主键</param>
		/// <returns>实体</returns>
		public TEntity Get(TPrimaryKey id)
		{
			using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("Id,");
				EntityAttributeInfo.ListColumnName.ForEach(r => { sb.AppendFormat("{0},", r); });

				string query = string.Format("SELECT {0} FROM {1} WITH(NOLOCK) WHERE Id=@id ", sb.ToString().TrimEnd(','), EntityAttributeInfo.TableName);
				return conn.Query<TEntity>(query, new { id = id }).SingleOrDefault();
			}
		}

		/// <summary>
		/// 获取所有数据
		/// </summary>
		/// <returns>数据集</returns>
        public List<TEntity> GetAllList(string orderby = "")
		{
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("Id,");
				EntityAttributeInfo.ListColumnName.ForEach(r => { sb.AppendFormat("{0},", r); });

				string query = string.Format("SELECT {0} FROM {1} WITH(NOLOCK)", sb.ToString().TrimEnd(','), EntityAttributeInfo.TableName);
				return conn.Query<TEntity>(query).ToList();
			}
		}

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns>数据集</returns>
        public List<TEntity> GetList(NameValueCollection param,string orderby="")
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Id,");
                EntityAttributeInfo.ListColumnName.ForEach(r => { sb.AppendFormat("{0},", r); });
                var objparma = new DynamicParameters();
                string strWhere = string.Empty;
                if (param != null && param.HasKeys())
                {
                    strWhere += " where 1=1 ";
                    for (int i = 0; i < param.Count; i++)
                    {
                        strWhere += " and "+param.GetKey(i) + "=@" + param.GetKey(i);
                        objparma.Add(param.GetKey(i), param.Get(i));
                    }
                }
                if (!string.IsNullOrEmpty(orderby)) orderby = " order by " + orderby;
                string query = string.Format("SELECT {0} FROM {1} WITH(NOLOCK) {2} {3}", sb.ToString().TrimEnd(','), EntityAttributeInfo.TableName, strWhere, orderby);
                return conn.Query<TEntity>(query, objparma).ToList();
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns>数据集</returns>
        public List<TEntity> GetList(string strWhere, string orderby = "")
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Id,");
                strWhere = string.Format("where 1=1 {0} {1}", (string.IsNullOrEmpty(strWhere) ? "" : "and"), strWhere);
                EntityAttributeInfo.ListColumnName.ForEach(r => { sb.AppendFormat("{0},", r); });
                if (!string.IsNullOrEmpty(orderby)) orderby = " order by " + orderby;
                string query = string.Format("SELECT {0} FROM {1} WITH(NOLOCK) {2} {3}", sb.ToString().TrimEnd(','), EntityAttributeInfo.TableName, strWhere, orderby);
                return conn.Query<TEntity>(query).ToList();
            }
        }

		/// <summary>
		/// 新增一条数据
		/// </summary>
		/// <param name="entity">对象</param>
		/// <returns>执行数量</returns>
		public int Insert(TEntity entity)
		{
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
			{
				StringBuilder sb=new StringBuilder ();
				StringBuilder sbParam = new StringBuilder();
				EntityAttributeInfo.InsertListColumnName.ForEach(r => { sb.AppendFormat("@{0},", r); sbParam.AppendFormat("{0},", r); });
				string query = string.Format("INSERT INTO {0}({1}) VALUES({2})", EntityAttributeInfo.TableName, sbParam.ToString().TrimEnd(','), sb.ToString().TrimEnd(','));
				return conn.Execute(query,entity);
			}
		}

		/// <summary>
		/// 新增一条数据返回主键ID
		/// </summary>
		/// <param name="entity">实体</param>
		/// <returns>实体的id</returns>
		public TPrimaryKey InsertAndGetId(TEntity entity)
		{
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
			{
				StringBuilder sb = new StringBuilder();
				StringBuilder sbParam = new StringBuilder();
				EntityAttributeInfo.InsertListColumnName.ForEach(r => { sb.AppendFormat("@{0},", r); sbParam.AppendFormat("{0},", r); });

				var param = new DynamicParameters(entity);
				param.Add("@Id", direction: ParameterDirection.Output);
				string query = string.Format("INSERT INTO {0}({1}) VALUES({2});SELECT @Id=SCOPE_IDENTITY()", EntityAttributeInfo.TableName, sbParam.ToString().TrimEnd(','), sb.ToString().TrimEnd(','));

				conn.Execute(query,param);
				return param.Get<TPrimaryKey>("@Id");
			}
		}

		/// <summary>
		///更新实体数据
		/// </summary>
		/// <param name="entity">实体</param>
		/// <returns>执行数量</returns>
		public int Update(TEntity entity)
		{
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
			{
				StringBuilder sb = new StringBuilder();
				EntityAttributeInfo.UpdateListColumnName.ForEach(r => sb.AppendFormat("{0}=@{0},", r));

				string query = string.Format("UPDATE {0} SET {1} WHERE ID=@Id ", EntityAttributeInfo.TableName, sb.ToString().TrimEnd(','));
				return conn.Execute(query, entity);
			}
		}

		/// <summary>
		/// 删除实体数据
		/// </summary>
		/// <param name="id">主键</param>
		/// <returns>执行数量</returns>
		public int Delete(TPrimaryKey id)
		{
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
			{
				string query = string.Format("DELETE FROM {0} WHERE ID=@Id ", EntityAttributeInfo.TableName);
				return conn.Execute(query, new { id = id });
			}
		}

        /// <summary>
        /// 获取datatable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
	    public DataTable QueryDataTable(string sql, params SqlParameter[] parameterValues)
	    {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                return conn.QueryDataTable(sql, parameterValues);
            }
	    }

	}
}
