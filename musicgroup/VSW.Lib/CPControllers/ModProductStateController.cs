using System;
using System.Configuration;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.CPControllers
{
    [CPModuleInfo(Name = "Vị trí sản phẩm",
        Description = "Quản lý - Vị trí sản phẩm",
        Code = "ModProductState",
        Access = 31,
        Order = 10,
        ShowInMenu = true,
        CssClass = "pencil")]
    public class ModProductStateController : CPController
    {
        public ModProductStateController()
        {
            //khoi tao Service
            DataService = ModProductStateService.Instance;
            CheckPermissions = true;
        }

        public void ActionIndex(ModProductStateModel model)
        {
            //sap xep tu dong
            string orderBy = AutoSort(model.Sort, "[ID]");

            //tao danh sach
            var dbQuery = ModProductStateService.Instance.CreateQuery()
                                    .Where(!string.IsNullOrEmpty(model.SearchText), o => (o.Name.Contains(model.SearchText)))
                                    .Take(model.PageSize)
                                    .OrderBy(orderBy)
                                    .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }

        public void ActionAdd(ModProductStateModel model)
        {
            if (model.RecordID > 0)
            {
                _item = ModProductStateService.Instance.GetByID(model.RecordID);

                //khoi tao gia tri mac dinh khi update
            }
            else
            {
                _item = new ModProductStateEntity
                {
                    Value = GetMaxValue()
                };

                //khoi tao gia tri mac dinh khi insert
            }

            ViewBag.Data = _item;
            ViewBag.Model = model;
        }

        public void ActionSave(ModProductStateModel model)
        {
            if (ValidSave(model))
                SaveRedirect();
        }

        public void ActionApply(ModProductStateModel model)
        {
            if (ValidSave(model))
                ApplyRedirect(model.RecordID, _item.ID);
        }

        public void ActionSaveNew(ModProductStateModel model)
        {
            if (ValidSave(model))
                SaveNewRedirect(model.RecordID, _item.ID);
        }

        #region private func

        private ModProductStateEntity _item;

        private bool ValidSave(ModProductStateModel model)
        {
            TryUpdateModel(_item);

            //chong hack
            _item.ID = model.RecordID;

            ViewBag.Data = _item;
            ViewBag.Model = model;

            CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;

            //kiem tra quyen han
            if ((model.RecordID < 1 && !CPViewPage.UserPermissions.Add) || (model.RecordID > 0 && !CPViewPage.UserPermissions.Edit))
                CPViewPage.Message.ListMessage.Add("Quyền hạn chế.");

            //kiem tra chuyen muc
            if (string.IsNullOrEmpty(_item.Name))
                CPViewPage.Message.ListMessage.Add("Nhập tên vị trí.");

            if (CPViewPage.Message.ListMessage.Count != 0) return false;

            try
            {
                //save
                ModProductStateService.Instance.Save(_item);

                //// Add an Application Setting.
                //var listState = ModProductStateService.Instance.CreateQuery().ToList();
                //string value = "";
                //for (int i = 0; listState != null && i < listState.Count; i++)
                //{
                //    value += (!string.IsNullOrEmpty(value) ? "," : "") + listState[i].Name + "|" + listState[i].Value.ToString();
                //}
                //if (!string.IsNullOrEmpty(value))
                //{
                //    ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
                //    configFileMap.ExeConfigFilename = CPViewPage.Server.MapPath("~/Web.config");
                //    System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

                //    config.AppSettings.Settings.Remove("Mod.ProductState");
                //    config.AppSettings.Settings.Add("Mod.ProductState",
                //               value);
                //    // Save the changes in App.config file.
                //    config.Save(ConfigurationSaveMode.Modified);

                //    // Force a reload of a changed section.
                //    ConfigurationManager.RefreshSection("appSettings");

                //    //config.AppSettings.Settings["Mod.ProductState"].Value = value;
                //    //config.Save();

                    
                //}
            }
            catch (Exception ex)
            {
                Error.Write(ex);
                CPViewPage.Message.ListMessage.Add(ex.Message);
                return false;
            }

            return true;
        }

        private static int GetMaxValue()
        {
            int value = ModProductStateService.Instance.CreateQuery()
                    .Max(o => o.Value)
                    .ToValue().ToInt(0);
            if (value == 0) return 1;
            return value * 2;

        }

        #endregion private func
    }

    public class ModProductStateModel : DefaultModel
    {
        private int _LangID = 1;

        public int LangID
        {
            get => _LangID;
            set => _LangID = value;
        }
        public string SearchText { get; set; }
        public int MenuID { get; set; }
    }
}