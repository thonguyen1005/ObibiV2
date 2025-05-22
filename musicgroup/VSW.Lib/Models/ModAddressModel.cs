using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModAddressEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public override string Name { get; set; }

        [DataInfo]
        public string Code { get; set; }

        [DataInfo]
        public string Email { get; set; }

        [DataInfo]
        public string Phone { get; set; }

        [DataInfo]
        public string Address { get; set; }

        [DataInfo]
        public string Facebook { get; set; }

        [DataInfo]
        public string FBName { get; set; }

        [DataInfo]
        public string Zalo { get; set; }

        [DataInfo]
        public string Map { get; set; }

        [DataInfo]
        public string MapAddress { get; set; }

        [DataInfo]
        public string LatLong { get; set; }

        [DataInfo]
        public DateTime Published { get; set; }

        [DataInfo]
        public DateTime Updated { get; set; }

        [DataInfo]
        public int Order { get; set; }

        [DataInfo]
        public bool Activity { get; set; }

        [DataInfo]
        public int ShowroomID { get; set; }
        [DataInfo]
        public int CityID { get; set; }
        [DataInfo]
        public int DistrictID { get; set; }
        [DataInfo]
        public int WardID { get; set; }
        public double MinValue { get; set; }
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
        #endregion Autogen by VSW
    }

    public class ModAddressService : ServiceBase<ModAddressEntity>
    {
        #region Autogen by VSW

        public ModAddressService() : base("[Mod_Address]")
        {
        }

        private static ModAddressService _instance;
        public static ModAddressService Instance => _instance ?? (_instance = new ModAddressService());

        #endregion Autogen by VSW

        public ModAddressEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }
        public ModAddressEntity GetByID_Cache(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle_Cache();
        }
        public bool Exists(string query)
        {
            return CreateQuery()
                           .Where(query)
                           .Count()
                           .ToValue()
                           .ToBool();
        }
        public List<ModAddressEntity> GetAllCache()
        {
            return CreateQuery().Where(o => o.Activity == true).ToList_Cache();
        }
        public List<ModAddressEntity> GetAllCacheAll()
        {
            return CreateQuery().ToList_Cache();
        }
        public ModAddressEntity GetShowroomNearest(string address)
        {
            double lat = 0;
            double lng = 0;
            string url = "https://maps.googleapis.com/maps/api/geocode/json?address=" + address + "&key=" + VSW.Lib.Global.Setting.keymap;
            System.Net.WebRequest request = System.Net.WebRequest.Create(url);
            using (System.Net.WebResponse response = (System.Net.HttpWebResponse)request.GetResponse())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8))
                {
                    var Obj = JObject.Parse(reader.ReadToEnd());
                    if (Obj["status"].ToString() == "OK")
                    {
                        lat = System.Convert.ToDouble(Obj["results"][0]["geometry"]["location"]["lat"].ToString());
                        lng = System.Convert.ToDouble(Obj["results"][0]["geometry"]["location"]["lng"].ToString());
                    }
                }
            }
            if (lat == 0 || lng == 0) return null;
            var item = new ModAddressEntity();
            var list = GetAllCache();
            try
            {
                double minvalue = 0;
                for (int i = 0; list != null && i < list.Count; i++)
                {
                    if (string.IsNullOrEmpty(list[i].LatLong)) continue;
                    string[] arr = list[i].LatLong.Split(',');
                    if (arr.Length != 2) continue;

                    double lats = System.Convert.ToDouble(arr[0]);
                    double lngs = System.Convert.ToDouble(arr[1]);
                    if (lats == 0 || lngs == 0) continue;

                    var radlat1 = Math.PI * lats / 180;
                    var radlat2 = Math.PI * lat / 180;
                    var radlng1 = Math.PI * lngs / 180;
                    var radlng2 = Math.PI * lng / 180;
                    var theta = lngs - lng;
                    var radtheta = Math.PI * theta / 180;
                    var dist = Math.Sin(radlat1) * Math.Sin(radlat2) + Math.Cos(radlat1) * Math.Cos(radlat2) * Math.Cos(radtheta);
                    dist = Math.Acos(dist);
                    dist = dist * 180 / Math.PI;
                    dist = dist * 60 * 1.1515;
                    if (minvalue == 0)
                    {
                        minvalue = dist;
                        item = list[i];
                    }
                    else
                    {
                        if (minvalue > dist)
                        {
                            minvalue = dist;
                            item = list[i];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                item = new ModAddressEntity();
            }
            return item;
        }
        public List<ModAddressEntity> GetAllCacheOrderAddress(string address)
        {
            var list = GetAllCacheAll();
            if (list == null) return null;
            if (string.IsNullOrEmpty(address)) return list;
            try
            {
                double lat = 0;
                double lng = 0;
                string url = "https://maps.googleapis.com/maps/api/geocode/json?address=" + address + "&key=" + VSW.Lib.Global.Setting.keymap;
                System.Net.WebRequest request = System.Net.WebRequest.Create(url);
                using (System.Net.WebResponse response = (System.Net.HttpWebResponse)request.GetResponse())
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8))
                    {
                        var Obj = JObject.Parse(reader.ReadToEnd());
                        if (Obj != null)
                        {
                            if (Obj["status"].ToString() == "OK")
                            {
                                lat = System.Convert.ToDouble(Obj["results"][0]["geometry"]["location"]["lat"].ToString());
                                lng = System.Convert.ToDouble(Obj["results"][0]["geometry"]["location"]["lng"].ToString());
                            }
                        }
                    }
                }
                if (lat == 0 || lng == 0) return list;
                for (int i = 0; list != null && i < list.Count; i++)
                {
                    list[i].MinValue = 1000000;
                    if (string.IsNullOrEmpty(list[i].LatLong)) continue;
                    string[] arr = list[i].LatLong.Split(',');
                    if (arr.Length != 2) continue;

                    double lats = System.Convert.ToDouble(arr[0]);
                    double lngs = System.Convert.ToDouble(arr[1]);
                    if (lats == 0 || lngs == 0) continue;

                    var radlat1 = Math.PI * lats / 180;
                    var radlat2 = Math.PI * lat / 180;
                    var radlng1 = Math.PI * lngs / 180;
                    var radlng2 = Math.PI * lng / 180;
                    var theta = lngs - lng;
                    var radtheta = Math.PI * theta / 180;
                    var dist = Math.Sin(radlat1) * Math.Sin(radlat2) + Math.Cos(radlat1) * Math.Cos(radlat2) * Math.Cos(radtheta);
                    dist = Math.Acos(dist);
                    dist = dist * 180 / Math.PI;
                    dist = dist * 60 * 1.1515;
                    list[i].MinValue = dist;
                }
            }
            catch (Exception ex)
            {
            }
            return list.OrderBy(o => o.MinValue).ToList();
        }
    }
}