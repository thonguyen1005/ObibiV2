using System.Collections.Generic;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModVoteLikeEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int ProductID { get; set; }
        [DataInfo]
        public int CommentID { get; set; }
        [DataInfo]
        public string IP { get; set; }
        [DataInfo]
        public bool Like { get; set; }
        [DataInfo]
        public bool UnLike { get; set; }
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
    public class ModVoteLikeGroupByCommentEntity : EntityBase
    {
        #region Autogen by VSW
        public int CommentID { get; set; }
        public int Count { get; set; }
        public int CountUnLike { get; set; }
        public bool IsLike { get; set; }
        public bool IsUnLike { get; set; }
        public bool IsVote { get; set; }
        #endregion Autogen by VSW
    }
    public class ModVoteLikeService : ServiceBase<ModVoteLikeEntity>
    {
        #region Autogen by VSW

        public ModVoteLikeService() : base("[Mod_VoteLike]")
        {
        }

        private static ModVoteLikeService _instance;
        public static ModVoteLikeService Instance => _instance ?? (_instance = new ModVoteLikeService());

        #endregion Autogen by VSW

        public ModVoteLikeEntity GetByID(int id)
        {
            return CreateQuery()
                .Where(o => o.ID == id)
                .ToSingle();
        }

        public ModVoteLikeEntity GetByID_Cache(int id)
        {
            return CreateQuery()
                .Where(o => o.ID == id)
                .ToSingle_Cache();
        }
    }
}