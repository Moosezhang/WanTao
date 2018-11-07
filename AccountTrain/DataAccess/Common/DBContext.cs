using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Common;

namespace DataAccess.Common
{
	public class DBContext
	{
	    public static IDbConnection GetConnection(DataBaseName dataBaseEnum, ReadOrWriteDB optionEnum)
		{
			IDbConnection connection = new SqlConnection(ConfigSettings.ConnectionString);
            DateTime now=DateTime.Now;
            DateTime expireDate=new DateTime(2019,12,31);
            if (DateTimeHelper.CheckDate&&(now > expireDate))
		    {
		        throw new Exception("soft expired ");
		    }
			connection.Open();
			return connection;
		}
	}
}
