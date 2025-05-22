using System;
using System.IO;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModGHNEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public string WardCode { get; set; }

        [DataInfo]
        public int DistrictID { get; set; }

        [DataInfo]
        public string WardName { get; set; }
        [DataInfo]
        public string Code { get; set; }

        #endregion Autogen by VSW
    }
}