﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
	public class ConfigSettings
	{
        public static string ConnectionString { get { return ConfigurationManager.ConnectionStrings["AccountTrian"].ConnectionString; } }
		
	}
}
