using System;
using System.Collections.Generic;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModSaleEntity : EntityBase
    {

        #region Autogen by HL

        [DataInfo]
        public override int ID { get; set; }
        [DataInfo]
        public override string Name { get; set; }
        [DataInfo]
        public string Code { get; set; }
        [DataInfo]
        public DateTime DateStart { get; set; }
        [DataInfo]
        public DateTime DateEnd { get; set; }
        [DataInfo]
        public long Money { get; set; }
        [DataInfo]
        public bool Status { get; set; }
        [DataInfo]
        public DateTime Published { get; set; }
        [DataInfo]
        public double Percent { get; set; }
        [DataInfo]
        public bool Auto { get; set; }
        [DataInfo]
        public int Count { get; set; }
        [DataInfo]
        public int CountUse { get; set; }
        [DataInfo]
        public string Content { get; set; }
        [DataInfo]
        public bool Activity { get; set; }
        [DataInfo]
        public long MoneyMin { get; set; }
        #endregion

        private List<ModSaleDetailEntity> _oGetDetail;
        public List<ModSaleDetailEntity> GetDetail()
        {
            _oGetDetail = new List<ModSaleDetailEntity>();

            if (_oGetDetail == null || _oGetDetail.Count < 1)
            {
                _oGetDetail = ModSaleDetailService.Instance.CreateQuery()
                                                .Where(o => o.SaleID == ID)
                                                .ToList_Cache();
            }

            return _oGetDetail;
        }
        public string GetOrder(int maxId)
        {

            if (maxId <= 1) return "0000001";

            var result = string.Empty;
            for (var i = 1; i <= (7 - maxId.ToString().Length); i++)
            {
                result += "0";
            }

            return result + (maxId + 1);
        }
    }

    public class ModSaleService : ServiceBase<ModSaleEntity>
    {
        #region Autogen by HL

        private ModSaleService()
            : base("[Mod_Sale]")
        {

        }
        private static ModSaleService _Instance = null;
        public static ModSaleService Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ModSaleService();

                return _Instance;
            }
        }
        #endregion

        public ModSaleEntity GetByID(int id)
        {
            return base.CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }
        public ModSaleEntity GetByID_Cache(int id)
        {
            return base.CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle_Cache();
        }
    }
}