using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Services
{
    public enum DatasourceType
    {
        MsSql = 0,
        MongoDb = 1,
        Redis = 2
    }

    public class DatasourcesSetting
    {
        public DatasourceItems Items { get; set; }

        public ConnectionItems Connections { get; set; }
    }

    public class ConnectionItems : KeyValueList<string, ConnectionItem>
    {
        public ConnectionItems() : base(x => x.Name)
        {

        }
    }

    public class DatasourceItems : KeyValueList<string, DatasourceItem>
    {
        public DatasourceItems() : base(x => x.Name)
        {

        }
    }

    public class DatasourceItem
    {
        public string Name { get; set; }

        public DatasourceType Type { get; set; }

        public ConnectionRef Connections { get; set; }

        public Parameters Parameters { get; set; }
    }


    public class ConnectionRef
    {
        public string Master { get; set; }

        public List<string> Slaves { get; set; }
    }


    public class ConnectionItem
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public Parameters Parameters { get; set; }
    }
}
