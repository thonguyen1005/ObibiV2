using System;
using System.Collections.Generic;

using VSW.Lib.MVC;
using VSW.Lib.Models;
using VSW.Lib.Global;

namespace VSW.Lib.CPControllers
{
    public class FormProductSizeImageController : CPController
    {
        public FormProductSizeImageController()
        {
            //khoi tao Service
            DataService = ModProductSizeService.Instance;
            CheckPermissions = true;
        }

        public void ActionIndex(FormProductSizeImageModel model)
        {
            var listFile = ModProductFileService.Instance.CreateQuery()
                                .Where(o => o.ProductID == model.ProductID)
                                .OrderByAsc(o => o.Order)
                                .ToList_Cache();

            ViewBag.Data = listFile;
            ViewBag.Model = model;
        }
    }

    public class FormProductSizeImageModel : DefaultModel
    {
        public int ProductID { get; set; }
        public int ID { get; set; }
    }
}

