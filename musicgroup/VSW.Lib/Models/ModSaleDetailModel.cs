using System;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModSaleDetailEntity : EntityBase
    {

        #region Autogen by HL

        [DataInfo]
        public override int ID { get; set; }
        [DataInfo]
        public int OrderID { get; set; }
        [DataInfo]
        public int SaleID { get; set; }
        [DataInfo]
        public long Money { get; set; }
        [DataInfo]
        public DateTime Published { get; set; }
        #endregion

        private ModOrderEntity _oOrder = null;
        public ModOrderEntity getOrder()
        {
            if (_oOrder == null && OrderID > 0)
                _oOrder = ModOrderService.Instance.GetByID_Cache(OrderID);

            if (_oOrder == null)
                _oOrder = new ModOrderEntity();

            return _oOrder;
        }
        private ModSaleEntity _oSale = null;
        public ModSaleEntity getSale()
        {
            if (_oSale == null && SaleID > 0)
                _oSale = ModSaleService.Instance.GetByID_Cache(SaleID);

            if (_oSale == null)
                _oSale = new ModSaleEntity();

            return _oSale;
        }
    }

    public class ModSaleDetailService : ServiceBase<ModSaleDetailEntity>
    {
        #region Autogen by HL

        private ModSaleDetailService()
            : base("[Mod_SaleDetail]")
        {

        }
        private static ModSaleDetailService _Instance = null;
        public static ModSaleDetailService Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ModSaleDetailService();

                return _Instance;
            }
        }
        #endregion

        public ModSaleDetailEntity GetByID(int id)
        {
            return base.CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }

    }
}