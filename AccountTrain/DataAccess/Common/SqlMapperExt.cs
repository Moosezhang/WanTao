using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Common
{
    public static class SqlMapperExt
    {
        public static DataTable QueryDataTable(this IDbConnection cnn, string sql, params SqlParameter[] parmas)
        {
            return SqlHelper.ExecuteDataset(SqlHelper.GetConnSting(), CommandType.Text, sql, parmas).Tables[0];
        }
    }
}
