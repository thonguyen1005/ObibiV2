using System.Collections.Generic;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModVoteFileEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int ProductID { get; set; }
        [DataInfo]
        public int CommentID { get; set; }

        [DataInfo]
        public string File { get; set; }

        [DataInfo]
        public int Order { get; set; }
        [DataInfo]
        public bool Show { get; set; }
        [DataInfo]
        public bool Activity { get; set; }
        public ModVoteEntity Commnet { get; set; }

        #endregion Autogen by VSW

        private ModProductEntity _oProduct;
        public ModProductEntity GetProduct()
        {
            if (_oProduct == null && ProductID > 0)
                _oProduct = ModProductService.Instance.GetByID_Cache(ProductID);

            return _oProduct ?? (_oProduct = new ModProductEntity());
        }
        private ModVoteEntity _oCommnet;
        public ModVoteEntity GetCommnet()
        {
            if (_oCommnet == null && CommentID > 0)
                _oCommnet = ModVoteService.Instance.GetByID_Cache(CommentID);

            return _oCommnet ?? (_oCommnet = new ModVoteEntity());
        }
    }

    public class ModVoteFileService : ServiceBase<ModVoteFileEntity>
    {
        #region Autogen by VSW

        public ModVoteFileService() : base("[Mod_VoteFile]")
        {
        }

        private static ModVoteFileService _instance;
        public static ModVoteFileService Instance => _instance ?? (_instance = new ModVoteFileService());

        #endregion Autogen by VSW

        public ModVoteFileEntity GetByID(int id)
        {
            return CreateQuery()
                .Where(o => o.ID == id)
                .ToSingle();
        }

        public ModVoteFileEntity GetByID_Cache(int id)
        {
            return CreateQuery()
                .Where(o => o.ID == id)
                .ToSingle_Cache();
        }

        public bool Exists(int productID, string file)
        {
            return CreateQuery()
                .Where(o => o.ProductID == productID && o.File == file)
                .Count()
                .ToValue_Cache()
                .ToBool();
        }

    }
}