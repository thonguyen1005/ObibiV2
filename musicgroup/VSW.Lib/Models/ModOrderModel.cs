using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using VSW.Core.Models;
using VSW.Lib.Global;

namespace VSW.Lib.Models
{
    public class ModOrderEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int StatusID { get; set; }

        [DataInfo]
        public int WebUserID { get; set; }

        [DataInfo]
        public string Code { get; set; }

        [DataInfo]
        public long Total { get; set; }

        [DataInfo]
        public override string Name { get; set; }

        [DataInfo]
        public string Email { get; set; }

        [DataInfo]
        public string Phone { get; set; }

        [DataInfo]
        public string Address { get; set; }

        [DataInfo]
        public int CityID { get; set; }
        [DataInfo]
        public int DistrictID { get; set; }
        [DataInfo]
        public int WardID { get; set; }

        [DataInfo]
        public string Title { get; set; }

        [DataInfo]
        public string Content { get; set; }

        [DataInfo]
        public int Payment { get; set; }

        [DataInfo]
        public bool StatusPay { get; set; }

        [DataInfo]
        public string BankPay { get; set; }

        [DataInfo]
        public bool Invoice { get; set; }
        [DataInfo]
        public string CompanyName { get; set; }
        [DataInfo]
        public string CompanyAddress { get; set; }
        [DataInfo]
        public string CompanyTax { get; set; }
        [DataInfo]
        public int Receive { get; set; }
        [DataInfo]
        public int ReceiveValue { get; set; }

        [DataInfo]
        public string IP { get; set; }

        [DataInfo]
        public DateTime Created { get; set; }
        [DataInfo]
        public string SaleCode { get; set; }
        [DataInfo]
        public long SaleMoney { get; set; }
        [DataInfo]
        public double SalePercent { get; set; }
        [DataInfo]
        public long Fee { get; set; }
        [DataInfo]
        public string PayError { get; set; }
        [DataInfo]
        public string PayTranID { get; set; }
        [DataInfo]
        public DateTime ShipDate { get; set; }
        [DataInfo]
        public string ShipNote { get; set; }
        [DataInfo]
        public string OrderNote { get; set; }
        [DataInfo]
        public string ShipPartner { get; set; }
        [DataInfo]
        public string ShipLabel { get; set; }
        [DataInfo]
        public long ShipFee { get; set; }
        [DataInfo]
        public long ShipInsuranceFee { get; set; }
        [DataInfo]
        public string ShipPickTime { get; set; }
        [DataInfo]
        public string ShipDeliverTime { get; set; }
        [DataInfo]
        public bool ShipCheck { get; set; }
        [DataInfo]
        public int ShipType { get; set; }
        [DataInfo]
        public long SaleCustomer { get; set; }
        [DataInfo]
        public long SaleMoneyPoint { get; set; }
        [DataInfo]
        public long Point { get; set; }
        [DataInfo]
        public bool OrderNews { get; set; }
        [DataInfo]
        public string Address2 { get; set; }

        [DataInfo]
        public int CityID2 { get; set; }
        [DataInfo]
        public int DistrictID2 { get; set; }
        [DataInfo]
        public int WardID2 { get; set; }
        [DataInfo]
        public bool AddressMore { get; set; }
        #endregion Autogen by VSW

        private WebMenuEntity _oStatus;
        public WebMenuEntity GetStatus()
        {
            if (_oStatus == null && StatusID > 0)
                _oStatus = WebMenuService.Instance.GetByID_Cache(StatusID);

            return _oStatus ?? (_oStatus = new WebMenuEntity());
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
        private ModAddressEntity _oReceiveValue;
        public ModAddressEntity GetReceiveValue()
        {
            if (_oReceiveValue == null && ReceiveValue > 0)
                _oReceiveValue = ModAddressService.Instance.GetByID_Cache(ReceiveValue);

            return _oReceiveValue ?? (_oReceiveValue = new ModAddressEntity());
        }
        private List<ModOrderDetailEntity> _oGetOrderDetail;
        public List<ModOrderDetailEntity> GetOrderDetail()
        {
            if (_oGetOrderDetail == null)
            {
                _oGetOrderDetail = ModOrderDetailService.Instance.CreateQuery()
                                                    .Where(o => o.OrderID == ID)
                                                    .ToList_Cache();
            }

            return _oGetOrderDetail ?? (_oGetOrderDetail = new List<ModOrderDetailEntity>());
        }
        private ModWebUserEntity _oWebUser;
        public ModWebUserEntity GetWebUser()
        {
            if (_oWebUser == null && WebUserID > 0)
                _oWebUser = ModWebUserService.Instance.GetByID(WebUserID);

            return _oWebUser ?? (_oWebUser = new ModWebUserEntity());
        }
        private WebMenuEntity _oGetCity2;
        public WebMenuEntity GetCity2()
        {
            if (_oGetCity2 == null && CityID2 > 0)
                _oGetCity2 = WebMenuService.Instance.GetByID_Cache(CityID2);

            return _oGetCity2 ?? (_oGetCity2 = new WebMenuEntity());
        }
        private WebMenuEntity _oGetDistrict2;
        public WebMenuEntity GetDistrict2()
        {
            if (_oGetDistrict2 == null && DistrictID2 > 0)
                _oGetDistrict2 = WebMenuService.Instance.GetByID_Cache(DistrictID2);

            return _oGetDistrict2 ?? (_oGetDistrict2 = new WebMenuEntity());
        }
        private WebMenuEntity _oGetWard2;
        public WebMenuEntity GetWard2()
        {
            if (_oGetWard2 == null && WardID2 > 0)
                _oGetWard2 = WebMenuService.Instance.GetByID_Cache(WardID2);

            return _oGetWard2 ?? (_oGetWard2 = new WebMenuEntity());
        }
    }
    public class ServiceGHN
    {
        public int service_id { get; set; }
        public int service_type_id { get; set; }
    }
    public class ModOrderService : ServiceBase<ModOrderEntity>
    {
        #region Autogen by VSW

        private ModOrderService() : base("[Mod_Order]")
        {
        }

        private static ModOrderService _instance;
        public static ModOrderService Instance => _instance ?? (_instance = new ModOrderService());

        #endregion Autogen by VSW

        public ModOrderEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }
        public ModOrderEntity GetByID_Cache(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle_Cache();
        }
        public ModOrderEntity GetByCode(string code)
        {
            return base.CreateQuery()
               .Where(o => o.Code == code)
               .ToSingle();
        }
    }
}