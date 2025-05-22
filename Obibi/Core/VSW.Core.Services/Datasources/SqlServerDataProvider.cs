using LinqToDB.DataProvider.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSW.Core.Services.Datasources
{
    public class SqlServerDataProvider : LinqToDB.DataProvider.SqlServer.SqlServerDataProvider
    {
        public SqlServerDataProvider(string name, SqlServerVersion version) : base(name, version) 
        {

        }
    }
}
