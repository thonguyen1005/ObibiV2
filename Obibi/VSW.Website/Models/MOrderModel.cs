using Microsoft.AspNetCore.Mvc.Rendering;
using VSW.Website.DataBase.Entities;
using VSW.Website.DataBase.Models;

namespace VSW.Website.Models
{
    public class MOrderModel
    {
        public MOrderModel()
        {
            lstCity = new List<SelectListItem>();
            lstProduct = new List<ModProductModel>();
            lstClassify = new List<MOD_PRODUCTCLASSIFYEntity>();
            lstClassifyDetail = new List<MOD_PRODUCTCLASSIFYDETAILEntity>();
            lstClassifyDetailPrice = new List<MOD_PRODUCTCLASSIFYDETAILPRICEEntity>();
        }

        public List<SelectListItem> lstCity { get; set; }
        public List<ModProductModel> lstProduct { get; set; }
        public List<MOD_PRODUCTCLASSIFYEntity> lstClassify { get; set; }
        public List<MOD_PRODUCTCLASSIFYDETAILEntity> lstClassifyDetail { get; set; }
        public List<MOD_PRODUCTCLASSIFYDETAILPRICEEntity> lstClassifyDetailPrice { get; set; }
        public int TotalQuantity { get; set; }
        public long TotalPrice { get; set; }
    }
}
