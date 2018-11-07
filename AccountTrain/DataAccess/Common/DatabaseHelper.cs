using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Common
{
	public class DatabaseHelper
	{
		#region 返回sql注释
		/// <summary>
		/// 返回sql注释
		/// </summary>
		/// <param name="newSt">调用方法new StackTrace(true)</param>
		/// <param name="author">sql作者</param>
		/// <param name="sqlDesc">sql说明</param>
		/// <returns>信息</returns>
		public static string GetSqlComments(StackTrace newSt, SqlAuthor author, string sqlDesc)
		{
			try
			{
				StackFrame stackFrame = newSt.GetFrame(0);
				StringBuilder commetBuilder = new StringBuilder();
				commetBuilder.AppendFormat("/*Flat:TCCCT/Author:{0}/For:{1}/File:///{2}/Fun:{3}*/", author.ToString(), sqlDesc, stackFrame.GetFileName(), stackFrame.GetMethod().Name);
				return commetBuilder.ToString();
			}
			catch
			{
				return "";
			}
		}
		#endregion
	}

}
