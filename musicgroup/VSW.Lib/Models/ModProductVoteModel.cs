using System;
using System.Collections.Generic;

using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModProductVoteEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int ProductID { get; set; }
        [DataInfo]
        public int MenuID { get; set; }
        [DataInfo]
        public override string Name { get; set; }
        [DataInfo]
        public int Yes { get; set; }

        [DataInfo]
        public int No { get; set; }
        [DataInfo]
        public int Order { get; set; }
        [DataInfo]
        public bool Activity { get; set; }
        [DataInfo]
        public int TypeView { get; set; }
        [DataInfo]
        public string NameYes { get; set; }
        [DataInfo]
        public string NameNo { get; set; }
        #endregion

        private ModProductEntity _oProduct = null;
        public ModProductEntity getProduct()
        {
            if (_oProduct == null && ProductID > 0)
                _oProduct = ModProductService.Instance.GetByID(ProductID);

            if (_oProduct == null)
                _oProduct = new ModProductEntity();

            return _oProduct;
        }

        private WebMenuEntity _oMenu;

        public WebMenuEntity GetMenu()
        {
            if (_oMenu == null && MenuID > 0)
                _oMenu = WebMenuService.Instance.GetByID_Cache(MenuID);

            return _oMenu ?? (_oMenu = new WebMenuEntity());
        }


    }

    public class ModProductVoteService : ServiceBase<ModProductVoteEntity>
    {

        #region Autogen by VSW

        private ModProductVoteService()
            : base("[Mod_ProductVote]")
        {

        }

        private static ModProductVoteService _Instance = null;
        public static ModProductVoteService Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ModProductVoteService();

                return _Instance;
            }
        }

        #endregion

        public ModProductVoteEntity GetByID(int id)
        {
            return base.CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }
    }
}