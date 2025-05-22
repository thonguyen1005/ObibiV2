using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModWebUserFileEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int WebUserID { get; set; }

        [DataInfo]
        public string File { get; set; }

        [DataInfo]
        public int Order { get; set; }

        [DataInfo]
        public bool Activity { get; set; }

        #endregion Autogen by VSW

        private ModWebUserEntity _oWebUser;
        public ModWebUserEntity GetWebUser()
        {
            if (_oWebUser == null && WebUserID > 0)
                _oWebUser = ModWebUserService.Instance.GetByID_Cache(WebUserID);

            return _oWebUser ?? (_oWebUser = new ModWebUserEntity());
        }
    }

    public class ModWebUserFileService : ServiceBase<ModWebUserFileEntity>
    {
        #region Autogen by VSW

        public ModWebUserFileService() : base("[Mod_WebUserFile]")
        {
        }

        private static ModWebUserFileService _instance;
        public static ModWebUserFileService Instance => _instance ?? (_instance = new ModWebUserFileService());

        #endregion Autogen by VSW

        public ModWebUserFileEntity GetByID(int id)
        {
            return CreateQuery()
                .Where(o => o.ID == id)
                .ToSingle();
        }

        public ModWebUserFileEntity GetByID_Cache(int id)
        {
            return CreateQuery()
                .Where(o => o.ID == id)
                .ToSingle_Cache();
        }

        public bool Exists(int WebUserID, string file)
        {
            return CreateQuery()
                .Where(o => o.WebUserID == WebUserID && o.File == file)
                .Count()
                .ToValue_Cache()
                .ToBool();
        }

        public List<ModWebUserFileEntity> GetAll_Cache(int WebUserID)
        {
            return CreateQuery()
                .Where(o => o.WebUserID == WebUserID)
                .ToList_Cache();
        }

        public void InsertOrUpdate(int WebUserID, string files)
        {
            if (string.IsNullOrEmpty(files)) return;
            string[] arrFile = files.Split('|');
            if (arrFile == null || arrFile.Length == 0)
            {
                Delete(o => o.WebUserID == WebUserID);
                return;
            }

            var listInDb = CreateQuery()
                                .Where(o => o.WebUserID == WebUserID)
                                .ToList_Cache();

            for (var i = 0; listInDb != null && i < listInDb.Count; i++)
            {
                if (System.Array.IndexOf(arrFile, listInDb[i].File) < 0)
                    Delete(listInDb[i]);
            }
            string allfiles = "";
            foreach (var file in arrFile)
            {
                if (string.IsNullOrEmpty(file)) continue;
                string file_new = file;
                if (file.IndexOf("----------") > -1)
                {
                    string pathfolder = "~/Data/upload/images/UserID" + WebUserID + "/Post/";
                    if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(pathfolder)))
                        System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(pathfolder));

                    string filename = System.IO.Path.GetFileNameWithoutExtension(file);
                    string ext = System.IO.Path.GetExtension(file);
                    string[] arrfile = Regex.Split(filename, "----------");
                    file_new = pathfolder + arrfile[0] + ext;
                    System.IO.Path.GetFileNameWithoutExtension(file_new);
                    System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(file), System.Web.HttpContext.Current.Server.MapPath(file_new));
                    if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(file)))
                    {
                        System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath(file));
                    }
                }

                var item = CreateQuery().Where(o => o.WebUserID == WebUserID && o.File == file_new).ToSingle_Cache();
                if (item != null) continue;

                item = new ModWebUserFileEntity()
                {
                    WebUserID = WebUserID,
                    File = file_new
                };

                Save(item);
                allfiles += (!string.IsNullOrEmpty(file_new) ? "|" : "") + file_new;
            }
            if (!string.IsNullOrEmpty(allfiles))
            {
                var webUser = ModWebUserService.Instance.GetByID(WebUserID);
                if (webUser != null)
                {
                    webUser.Files = allfiles;
                    ModWebUserService.Instance.Save(webUser, o => o.Files);
                }
            }
        }
    }
}