using System;
using System.Collections.Generic;

using VSW.Lib.MVC;
using VSW.Lib.Models;
using VSW.Lib.Global;

namespace VSW.Lib.CPControllers
{
    public class FormVoteFileController : CPController
    {
        public FormVoteFileController()
        {
            //khoi tao Service
            DataService = ModVoteFileService.Instance;
            CheckPermissions = true;
        }

        public void ActionIndex(ModVoteFileModel model)
        {
            // sap xep tu dong
            string orderBy = AutoSort(model.Sort, "[Order] ASC");

            // tao danh sach
            var dbQuery = ModVoteFileService.Instance.CreateQuery()
                                            .Where(model.ProductID > 0, o => o.ProductID == model.ProductID)
                                            .Take(model.PageSize)
                                            .OrderBy(orderBy)
                                            .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }

    }

    public class ModVoteFileModel : DefaultModel
    {
        private int _LangID = 1;
        public int LangID
        {
            get { return _LangID; }
            set { _LangID = value; }
        }

        public int ProductID { get; set; }
    }
}

