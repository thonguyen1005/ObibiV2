using System;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModOrderProcessEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }
        [DataInfo]
        public int OrderID { get; set; }
        [DataInfo]
        public int StatusID { get; set; }
        [DataInfo]
        public DateTime Date { get; set; }
        
        #endregion Autogen by VSW

        private ModOrderEntity _oOrder;

        public ModOrderEntity GetOrder()
        {
            if (_oOrder == null && OrderID > 0)
                _oOrder = ModOrderService.Instance.GetByID(OrderID);

            return _oOrder ?? (_oOrder = new ModOrderEntity());
        }

        private WebMenuEntity _oStatus;
        public WebMenuEntity GetStatus()
        {
            if (_oStatus == null && StatusID > 0)
                _oStatus = WebMenuService.Instance.GetByID_Cache(StatusID);

            return _oStatus ?? (_oStatus = new WebMenuEntity());
        }
    }

    public class ModOrderProcessService : ServiceBase<ModOrderProcessEntity>
    {
        #region Autogen by VSW

        private ModOrderProcessService() : base("[Mod_OrderProcess]")
        {
        }

        private static ModOrderProcessService _instance;
        public static ModOrderProcessService Instance => _instance ?? (_instance = new ModOrderProcessService());

        #endregion Autogen by VSW

        public ModOrderProcessEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }
    }
}