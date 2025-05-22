using LinqToDB.Data;
using LinqToDB.DataProvider.SqlServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
namespace VSW.Core.Services
{
    public static class DatasourceExtensions
    {
        public static DatasourceItem GetDatasouceItem(string dbName)
        {
            var setting = CoreService.GetConfigWithSection<DatasourcesSetting>();
            if (setting == null || setting.Items.IsEmpty())
            {
                return null;
            }

            return setting.Items[dbName];
        }

        public static Tuple<DatasourceItem, List<ConnectionItem>> GetDatasouceWithConnections(string dbName)
        {
            var setting = CoreService.GetConfigWithSection<DatasourcesSetting>();
            if (setting == null || setting.Items.IsEmpty())
            {
                return null;
            }

            var item = setting.Items[dbName];

            if (item == null)
            {
                return new Tuple<DatasourceItem, List<ConnectionItem>>(item, new List<ConnectionItem>());
            }

            var cnnNames = new List<string>();
            if (item.Connections != null)
            {
                if (item.Connections.Master.IsNotEmpty())
                {
                    if (!cnnNames.Contains(item.Connections.Master))
                    {
                        cnnNames.Add(item.Connections.Master);
                    }
                }

                if (item.Connections.Slaves.IsNotEmpty())
                {
                    cnnNames.AddRange(item.Connections.Slaves);
                }
            }

            var connections = setting.Connections.FindAll(x => cnnNames.Contains(x.Name));
            return new Tuple<DatasourceItem, List<ConnectionItem>>(item, connections);
        }

        public static Tuple<DatasourceItem, List<ConnectionItem>> GetMongoDbSetting()
        {
            var ds = CoreService.GetConfigWithSection<DatasourcesSetting>();
            //Register Redis
            var dbItem = ds.Items.FirstOrDefault(x => x.Type == DatasourceType.MongoDb);

            if (dbItem == null)
            {
                throw new Exception("Not found config information of MongoDb");
            }

            return GetDatasouceWithConnections(dbItem.Name);
        }

        public static Tuple<DatasourceItem, List<ConnectionItem>> GetRedisSetting()
        {
            var ds = CoreService.GetConfigWithSection<DatasourcesSetting>();
            //Register Redis
            var dbItem = ds.Items.FirstOrDefault(x => x.Type == DatasourceType.Redis);

            if (dbItem == null)
            {
                throw new Exception("Not found config information of Redis");
            }

            return GetDatasouceWithConnections(dbItem.Name);
        }

        public static ConnectionItem GetConnectionItem(string dbName, ReadMode mode = ReadMode.Master)
        {
            var setting = CoreService.GetConfigWithSection<DatasourcesSetting>();
            if (setting == null || setting.Items.IsEmpty() || !setting.Items.Contains(dbName))
            {
                return null;
            }

            var item = setting.Items[dbName];
            var name = GetConnectionName(item, mode);
            return setting.Connections[name];
        }

        public static ConnectionItem GetConnectionItem(this DatasourceItem ds, List<ConnectionItem> items, ReadMode mode = ReadMode.Master)
        {
            var name = GetConnectionName(ds, mode);
            return items.FirstOrDefault(x => x.Name == name);
        }

        public static string GetConnectionName(DatasourceItem item, ReadMode mode = ReadMode.Master)
        {
            if (mode == ReadMode.Master)
            {
                return item.Connections.Master;
            }

            if (item.Connections.Slaves.IsEmpty())
            {
                return item.Connections.Master;
            }

            if (item.Connections.Slaves.Count == 1)
            {
                return item.Connections.Slaves[0];
            }

            var index = RandomHelper.Next(0, item.Connections.Slaves.Count - 1);
            return item.Connections.Slaves[index];
        }

        public static IDbConnection EnsureOpen(this IDbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            return connection;
        }
    }
}
