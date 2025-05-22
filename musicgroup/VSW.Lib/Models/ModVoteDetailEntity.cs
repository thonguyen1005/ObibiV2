using System;
using System.Collections.Generic;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModVoteDetailEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }
        [DataInfo]
        public int ProductID { get; set; }
        [DataInfo]
        public int ProductVoteID { get; set; }
        [DataInfo]
        public int CommentID { get; set; }
        [DataInfo]
        public bool Vote { get; set; }
        [DataInfo]
        public bool Activity { get; set; }

        #endregion Autogen by VSW
        private ModProductVoteEntity _oProductVote;
        public ModProductVoteEntity GetProductVote()
        {
            if (_oProductVote == null && ProductVoteID > 0)
                _oProductVote = ModProductVoteService.Instance.GetByID(ProductVoteID);

            return _oProductVote ?? (_oProductVote = new ModProductVoteEntity());
        }
    }

    public class ModVoteDetailService : ServiceBase<ModVoteDetailEntity>
    {
        #region Autogen by VSW

        public ModVoteDetailService() : base("[Mod_VoteDetail]")
        {
        }

        private static ModVoteDetailService _instance;
        public static ModVoteDetailService Instance => _instance ?? (_instance = new ModVoteDetailService());

        #endregion Autogen by VSW

        public ModVoteDetailEntity GetByID(int id)
        {
            return CreateQuery()
                .Where(o => o.ID == id)
                .ToSingle();
        }

        public ModVoteDetailEntity GetByID_Cache(int id)
        {
            return CreateQuery()
                .Where(o => o.ID == id)
                .ToSingle_Cache();
        }
    }
}