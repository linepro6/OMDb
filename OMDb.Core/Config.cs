﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMDb.Core
{
    public static class Config
    {
        internal static readonly List<string> SqliteConnectionStrings = new();
        public static bool AddConnectionString(string str, string configId, bool needCodeFirst)
        {
            SqliteConnectionStrings.Add(str);
            return Services.DbService.AddDb(str, configId, needCodeFirst);
        }
        public static bool AddDbFile(string filePath, string configId,bool needCodeFirst)
        {
            SqliteConnectionStrings.Add(@"DataSource=" + filePath);
            return Services.DbService.AddDb(@"DataSource=" + filePath, configId, needCodeFirst);
        }
    }
}
