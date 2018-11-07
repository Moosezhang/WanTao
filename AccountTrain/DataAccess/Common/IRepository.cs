using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Common
{
	/// <summary>
	/// 基础仓储接口
	/// </summary>
	/// <typeparam name="TEntity">实体类</typeparam>
	/// <typeparam name="TPrimaryKey">主键</typeparam>
	public interface IRepository<TEntity, TPrimaryKey>
	{
		/// <summary>
		/// 通过主键获取实体数据
		/// </summary>
		/// <param name="id">主键</param>
		/// <returns>实体</returns>
		TEntity Get(TPrimaryKey id);

		/// <summary>
		/// 获取所有数据
		/// </summary>
		/// <returns>数据集</returns>
        List<TEntity> GetAllList(string orderby = "");

	    /// <summary>
	    /// 获取数据
	    /// </summary>
	    /// <returns>数据集</returns>
        List<TEntity> GetList(NameValueCollection param, string orderby = "");

		/// <summary>
		/// 新增一条数据
		/// </summary>
		/// <param name="entity">对象</param>
		/// <returns>执行数量</returns>
		int Insert(TEntity entity);

		/// <summary>
		/// 新增一条数据返回主键ID
		/// </summary>
		/// <param name="entity">实体</param>
		/// <returns>实体的id</returns>
		TPrimaryKey InsertAndGetId(TEntity entity);

		/// <summary>
		///更新实体数据
		/// </summary>
		/// <param name="entity">实体</param>
		/// <returns>执行数量</returns>
		int Update(TEntity entity);

		/// <summary>
		/// 删除实体数据
		/// </summary>
		/// <param name="id">主键</param>
		/// <returns>执行数量</returns>
		int Delete(TPrimaryKey id);

	    /// <summary>
	    /// 获取datatable
	    /// </summary>
	    /// <param name="sql"></param>
	    /// <param name="parameterValues"></param>
	    /// <returns></returns>
        DataTable QueryDataTable(string sql, params SqlParameter[] parameterValues);

	    /// <summary>
	    /// 获取数据
	    /// </summary>
	    /// <returns>数据集</returns>
	    List<TEntity> GetList(string strWhere, string orderby = "");

	}
}
