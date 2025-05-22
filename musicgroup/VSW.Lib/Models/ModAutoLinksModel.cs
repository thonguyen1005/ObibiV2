using System;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModAutoLinksEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public override string Name { get; set; }
        [DataInfo]
        public string Code { get; set; }
        [DataInfo]
        public string Title { get; set; }

        [DataInfo]
        public string Link { get; set; }
        [DataInfo]
        public string Target { get; set; }

        [DataInfo]
        public int Quantity { get; set; }

        [DataInfo]
        public DateTime Published { get; set; }

        [DataInfo]
        public int Order { get; set; }

        [DataInfo]
        public bool Activity { get; set; }

        #endregion Autogen by VSW
    }

    public class ModAutoLinksService : ServiceBase<ModAutoLinksEntity>
    {
        #region Autogen by VSW

        private ModAutoLinksService() : base("[Mod_AutoLinks]")
        {
        }

        private static ModAutoLinksService _instance;
        public static ModAutoLinksService Instance => _instance ?? (_instance = new ModAutoLinksService());

        #endregion Autogen by VSW

        public ModAutoLinksEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }
    }
}