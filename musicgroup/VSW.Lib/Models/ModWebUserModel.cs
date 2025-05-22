using System;
using System.Collections.Generic;
using VSW.Core.Models;
using VSW.Lib.Global;

namespace VSW.Lib.Models
{
    public class ModWebUserEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }
        [DataInfo]
        public string UserName { get; set; }
        [DataInfo]
        public string Password { get; set; }
        [DataInfo]
        public string TempPassword { get; set; }
        [DataInfo]
        public override string Name { get; set; }
        [DataInfo]
        public string Code { get; set; }
        [DataInfo]
        public string File { get; set; }
        [DataInfo]
        public string Files { get; set; }
        [DataInfo]
        public string Email { get; set; }
        [DataInfo]
        public string Phone { get; set; }
        [DataInfo]
        public string Address { get; set; }
        [DataInfo]
        public string Content { get; set; }
        [DataInfo]
        public string Facebook { get; set; }
        [DataInfo]
        public string Skype { get; set; }
        [DataInfo]
        public string Zalo { get; set; }
        [DataInfo]
        public string IP { get; set; }
        [DataInfo]
        public DateTime Created { get; set; }
        [DataInfo]
        public bool Activity { get; set; }

        [DataInfo]
        public DateTime Birthday { get; set; }
        [DataInfo]
        public bool Gender { get; set; }
        [DataInfo]
        public int Type { get; set; }
        [DataInfo]
        public string CMND { get; set; }
        [DataInfo]
        public int CityID { get; set; }
        [DataInfo]
        public int DistrictID { get; set; }
        [DataInfo]
        public int WardID { get; set; }
        [DataInfo]
        public string Website { get; set; }
        [DataInfo]
        public string CompanyName { get; set; }
        [DataInfo]
        public string CompanyAddress { get; set; }
        [DataInfo]
        public string CompanyTax { get; set; }
        [DataInfo]
        public int Type2 { get; set; }
        [DataInfo]
        public long Point { get; set; }
        #endregion Autogen by VSW
        private List<ModWebUserFileEntity> _oGetFile = null;
        public List<ModWebUserFileEntity> GetFile()
        {
            if (_oGetFile == null)
            {
                _oGetFile = ModWebUserFileService.Instance.CreateQuery()
                                            .Where(o => o.WebUserID == ID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();
            }

            return _oGetFile;
        }
        private List<string> _oGetFile2;
        public List<string> GetFile2(string file)
        {
            if ((_oGetFile2 == null || _oGetFile2.Count < 1) && !string.IsNullOrEmpty(file))
            {
                _oGetFile2 = new List<string>();

                string[] ArrFile = file.Split('|');
                for (int i = 0; ArrFile != null && i < ArrFile.Length; i++)
                {
                    if (!string.IsNullOrEmpty(ArrFile[i]))
                        _oGetFile2.Add(ArrFile[i]);
                }
            }

            return _oGetFile2;
        }
        private WebMenuEntity _oGetCity;
        public WebMenuEntity GetCity()
        {
            if (_oGetCity == null && CityID > 0)
                _oGetCity = WebMenuService.Instance.GetByID_Cache(CityID);

            return _oGetCity ?? (_oGetCity = new WebMenuEntity());
        }
        private WebMenuEntity _oGetDistrict;
        public WebMenuEntity GetDistrict()
        {
            if (_oGetDistrict == null && DistrictID > 0)
                _oGetDistrict = WebMenuService.Instance.GetByID_Cache(DistrictID);

            return _oGetDistrict ?? (_oGetDistrict = new WebMenuEntity());
        }
        private WebMenuEntity _oGetWard;
        public WebMenuEntity GetWard()
        {
            if (_oGetWard == null && WardID > 0)
                _oGetWard = WebMenuService.Instance.GetByID_Cache(WardID);

            return _oGetWard ?? (_oGetWard = new WebMenuEntity());
        }
        private ModWebUserMenuEntity _oWebUserMenu;
        public ModWebUserMenuEntity WebUserMenu
        {
            get
            {
                if (_oWebUserMenu == null && Type2 > 0)
                    _oWebUserMenu = ModWebUserMenuService.Instance.GetByID_Cache(Type2);

                return _oWebUserMenu ?? (_oWebUserMenu = new ModWebUserMenuEntity());
            }
        }
    }

    public class ModWebUserService : ServiceBase<ModWebUserEntity>
    {
        #region Autogen by VSW

        private ModWebUserService() : base("[Mod_WebUser]")
        {
        }

        private static ModWebUserService _instance;
        public static ModWebUserService Instance => _instance ?? (_instance = new ModWebUserService());

        #endregion Autogen by VSW

        public ModWebUserEntity GetByID(int id)
        {
            return CreateQuery()
                   .Where(o => o.ID == id)
                   .ToSingle();
        }
        public string GetNameByID(int id)
        {
            string name = "";
            var item = CreateQuery()
                   .Where(o => o.ID == id)
                   .ToSingle();
            if (item != null)
            {
                name = item.Name;
            }
            return name;
        }
        public ModWebUserEntity GetByID_Cache(int id)
        {
            return CreateQuery()
                   .Where(o => o.ID == id)
                   .ToSingle_Cache();
        }
        private string RemoveNotUserNameChar(string s)
        {
            string _s = string.Empty;
            string charSpecial = "_.@";
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];

                if ((int)c == 160)
                    c = ' ';

                if (Char.IsLetterOrDigit(c) || Char.IsWhiteSpace(c))
                    _s += c.ToString();
                else if (charSpecial.IndexOf(c) > -1)
                    _s += c.ToString();
            }
            return _s;
        }
        public string GetUserName(string s)
        {
            s = RemoveNotUserNameChar(Data.RemoveVietNamese(s));

            return s.Trim().Replace(" ", "-")
                .Replace("'", "")
                .Replace("/", "-")
                .Replace("*", "-")
                .Replace("\\", "-")
                .Replace("--", "-")
                .Replace("--", "-").ToLower();
        }
        public int GetCount()
        {
            return CreateQuery()
                .Select(o => o.ID)
                .Count()
                .ToValue_Cache()
                .ToInt(0);
        }
        public ModWebUserEntity GetByEmail(string email)
        {
            return CreateQuery()
                    .Where(o => o.Email == email)
                    .ToSingle_Cache();
        }
        public ModWebUserEntity GetByPhone(string phone)
        {
            return CreateQuery()
                    .Where(o => o.Phone == phone)
                    .ToSingle_Cache();
        }
        public ModWebUserEntity GetByUserName(string UserName)
        {
            return CreateQuery()
                    .Where(o => o.UserName == UserName)
                    .ToSingle_Cache();
        }
        public bool CheckEmail(string email, int id)
        {
            return CreateQuery()
                   .Where(o => o.Email == email && o.ID != id)
                   .Count()
                   .ToValue_Cache()
                   .ToBool();
        }
        public bool CheckUserName(string username, int id)
        {
            return base.CreateQuery()
               .Where(o => o.UserName == username && o.ID != id)
               .Count()
               .ToValue().ToBool();
        }
        public ModWebUserEntity GetForLogin(int id)
        {
            if (id < 1) return null;

            return CreateQuery()
                      .Where(o => o.ID == id)
                      .ToSingle_Cache();
        }
        public ModWebUserEntity GetForLogin(string email, string md5_passwd)
        {
            var webUser = CreateQuery()
                                .Where(o => ((o.Email == email && o.Email != "") || (o.UserName == email && o.UserName != "")) && (o.Password == md5_passwd || o.TempPassword == md5_passwd))
                                .ToSingle_Cache();

            if (webUser != null && webUser.TempPassword != string.Empty)
            {
                if (webUser.TempPassword == md5_passwd) webUser.Password = md5_passwd;
                webUser.TempPassword = string.Empty;
                Save(webUser);
            }
            return webUser;
        }
    }
}