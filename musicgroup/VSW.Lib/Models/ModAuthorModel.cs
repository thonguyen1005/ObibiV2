using System;
using System.Collections.Generic;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModAuthorEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public override string Name { get; set; }

        [DataInfo]
        public string Code { get; set; }

        [DataInfo]
        public string File { get; set; }

        [DataInfo]
        public string Banner { get; set; }

        [DataInfo]
        public string Facebook { get; set; }
        [DataInfo]
        public string Zalo { get; set; }
        [DataInfo]
        public string Tiktok { get; set; }
        [DataInfo]
        public string Youtube { get; set; }

        [DataInfo]
        public string Position { get; set; }

        [DataInfo]
        public string Content { get; set; }

        [DataInfo]
        public string Story { get; set; }

        [DataInfo]
        public string PageTitle { get; set; }

        [DataInfo]
        public string PageDescription { get; set; }

        [DataInfo]
        public string PageKeywords { get; set; }

        #endregion Autogen by VSW
    }

    public class ModAuthorService : ServiceBase<ModAuthorEntity>
    {
        #region Autogen by VSW

        public ModAuthorService() : base("[Mod_Author]")
        {
        }

        private static ModAuthorService _instance;
        public static ModAuthorService Instance => _instance ?? (_instance = new ModAuthorService());

        #endregion Autogen by VSW

        public ModAuthorEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }

        public ModAuthorEntity GetByID_Cache(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle_Cache();
        }
    }
}