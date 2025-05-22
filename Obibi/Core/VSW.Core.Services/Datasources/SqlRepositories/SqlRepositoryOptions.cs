using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Services
{
    public enum ReadMode
    {
        Master,
        Slave
    }

    public class SqlRepositoryOptions
    {
        public ReadMode ReadMode { get; set; }

        public SqlRepositoryOptions()
        {

        }
    }
}
