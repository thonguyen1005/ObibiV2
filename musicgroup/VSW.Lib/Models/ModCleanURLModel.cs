using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModCleanURLEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public string Code { get; set; }

        [DataInfo]
        public string Type { get; set; }

        [DataInfo]
        public int Value { get; set; }

        [DataInfo]
        public int MenuID { get; set; }

        [DataInfo]
        public int LangID { get; set; }

        #endregion Autogen by VSW

        private WebMenuEntity _oMenu;

        public WebMenuEntity GetMenu()
        {
            if (_oMenu == null && MenuID > 0) _oMenu = WebMenuService.Instance.GetByID_Cache(MenuID);

            return _oMenu ?? (_oMenu = new WebMenuEntity());
        }
    }

    public class ModCleanURLService : ServiceBase<ModCleanURLEntity>
    {
        #region Autogen by VSW

        private ModCleanURLService() : base("[Mod_CleanURL]")
        {
        }

        private static ModCleanURLService _instance;
        public static ModCleanURLService Instance => _instance ?? (_instance = new ModCleanURLService());

        #endregion Autogen by VSW

        public ModCleanURLEntity GetByCode(string code, int langId)
        {
            return CreateQuery().Where(o => o.Code == code && o.LangID == langId).ToSingle();
        }

        public bool CheckCode(string code, int langId, int value)
        {
            if (code == null) return true;

            code = code.Trim();

            return CreateQuery()
                        .Where(o => o.Code == code && o.LangID == langId && o.Value != value)
                        .Count()
                        .ToValue().ToBool();
        }
        public bool CheckCode(string code, string type, int value, int langId)
        {
            if (code == null) return true;

            code = code.Trim();

            return CreateQuery()
                        .Where(o => o.Code == code)
                        .Where(type != string.Empty && value > 0, o => o.Type != type && o.Value != value && o.LangID != langId)
                        .Count()
                        .ToValue().ToBool();
        }

        public void InsertOrUpdate(string code, string type, int value, int menuId, int langId)
        {
            if (code == null) return;

            code = code.Trim();

            if (string.IsNullOrEmpty(code) || code == "-") return;

            var clearUrl = CreateQuery().Where(o => o.Type == type && o.Value == value && o.LangID == langId).ToSingle();
            if (clearUrl != null)
            {
                clearUrl.Code = code;
                clearUrl.MenuID = menuId;
                clearUrl.LangID = langId;

                Save(clearUrl, o => new { o.Code, o.MenuID });
            }
            else
            {
                clearUrl = new ModCleanURLEntity()
                {
                    Code = code,
                    Type = type,
                    Value = value,
                    MenuID = menuId,
                    LangID = langId
                };

                Save(clearUrl);
            }
        }
    }
}