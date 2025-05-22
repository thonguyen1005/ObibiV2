using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModAdvEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int MenuID { get; set; }

        [DataInfo]
        public override string Name { get; set; }

        [DataInfo]
        public string AdvCode { get; set; }

        [DataInfo]
        public string File { get; set; }

        [DataInfo]
        public string Summary { get; set; }

        [DataInfo]
        public int Width { get; set; }

        [DataInfo]
        public int Height { get; set; }

        [DataInfo]
        public string AddInTag { get; set; }

        [DataInfo]
        public string URL { get; set; }

        [DataInfo]
        public string Target { get; set; }

        [DataInfo]
        public int Order { get; set; }

        [DataInfo]
        public bool Activity { get; set; }

        #endregion Autogen by VSW

        private WebMenuEntity _oMenu;

        public WebMenuEntity GetMenu()
        {
            if (_oMenu == null && MenuID > 0)
                _oMenu = WebMenuService.Instance.GetByID_Cache(MenuID);

            return _oMenu ?? (_oMenu = new WebMenuEntity());
        }
    }

    public class ModAdvService : ServiceBase<ModAdvEntity>
    {
        #region Autogen by VSW

        public ModAdvService() : base("[Mod_Adv]")
        {
        }

        private static ModAdvService _instance;
        public static ModAdvService Instance => _instance ?? (_instance = new ModAdvService());

        #endregion Autogen by VSW

        public ModAdvEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }

        public bool Exists(string query)
        {
            return CreateQuery()
                           .Where(query)
                           .Count()
                           .ToValue()
                           .ToBool();
        }
    }
}