using System;
using System.Collections.Generic;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModVoteEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int ProductID { get; set; }

        [DataInfo]
        public override string Name { get; set; }

        [DataInfo]
        public string Email { get; set; }

        [DataInfo]
        public string Phone { get; set; }

        [DataInfo]
        public double Vote { get; set; }
        [DataInfo]
        public string Type { get; set; }
        [DataInfo]
        public int WebUserID { get; set; }

        [DataInfo]
        public string IP { get; set; }

        [DataInfo]
        public DateTime Created { get; set; }
        [DataInfo]
        public int ParentID { get; set; }
        [DataInfo]
        public string Content { get; set; }
        [DataInfo]
        public bool Activity { get; set; }
        [DataInfo]
        public bool DaMua { get; set; }
        [DataInfo]
        public bool GioiThieu { get; set; }
        [DataInfo]
        public DateTime NgayMua { get; set; }
        [DataInfo]
        public string AdminNote { get; set; }
        [DataInfo]
        public string ChucVu { get; set; }
        [DataInfo]
        public string OrderCode { get; set; }

        [DataInfo]
        public int OrderID { get; set; }
        #endregion Autogen by VSW
        private string _oObject;
        public string GetObject()
        {
            if (Type == "Product")
                _oObject = ModProductService.Instance.GetByID_Cache(ProductID)?.Name+"(Sản phẩm)";
            else if (Type == "News")
                _oObject = ModNewsService.Instance.GetByID_Cache(ProductID)?.Name + "(Tin tức)";
            else if (Type == "Page")
                _oObject = SysPageService.Instance.GetByID_Cache(ProductID)?.Name + "(Trang)";
            return _oObject;
        }
        private ModWebUserEntity _oWebUser;
        public ModWebUserEntity GetWebUser()
        {
            if (_oWebUser == null && WebUserID > 0)
                _oWebUser = ModWebUserService.Instance.GetByID(WebUserID);

            return _oWebUser ?? (_oWebUser = new ModWebUserEntity());
        }
        private List<ModVoteFileEntity> _oGetFile;
        public List<ModVoteFileEntity> GetFile()
        {
            if (_oGetFile == null)
                _oGetFile = ModVoteFileService.Instance.CreateQuery()
                                                .Where(o => o.CommentID == ID)
                                                .OrderByAsc(o => o.Order)
                                                .ToList_Cache();

            return _oGetFile ?? (_oGetFile = new List<ModVoteFileEntity>());
        }
    }
    public class ModVoteCountEntity : EntityBase
    {
        #region Autogen by VSW
        [DataInfo]
        public double Vote { get; set; }
        [DataInfo]
        public int CountVote { get; set; }
        #endregion Autogen by VSW
    }
    public class ModVoteService : ServiceBase<ModVoteEntity>
    {
        #region Autogen by VSW

        public ModVoteService() : base("[Mod_Vote]")
        {
        }

        private static ModVoteService _instance;
        public static ModVoteService Instance => _instance ?? (_instance = new ModVoteService());

        #endregion Autogen by VSW

        public ModVoteEntity GetByID(int id)
        {
            return CreateQuery()
                .Where(o => o.ID == id)
                .ToSingle();
        }

        public ModVoteEntity GetByID_Cache(int id)
        {
            return CreateQuery()
                .Where(o => o.ID == id)
                .ToSingle_Cache();
        }
    }
}