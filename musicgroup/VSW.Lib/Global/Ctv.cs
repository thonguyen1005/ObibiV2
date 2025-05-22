using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VSW.Lib.Global
{
    public class CtvItem
    {
        public int ProductID { get; set; }
        public string ctv { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is CtvItem)) return base.Equals(obj);

            var temp = (CtvItem)obj;

            return ProductID.Equals(temp.ProductID) && ctv.Equals(temp.ctv);
        }

        public override int GetHashCode()
        {
            return (ProductID + "-" + ctv).GetHashCode();
        }
    }

    public class Ctv
    {
        private readonly List<CtvItem> _listItem = new List<CtvItem>();
        private readonly string _cookieKey = "VSW_Ctv";

        public ReadOnlyCollection<CtvItem> Items => _listItem.AsReadOnly();

        public int Count => _listItem.Count;

        public Ctv()
            : this(string.Empty)
        {
        }

        public Ctv(string serviceName)
        {
            _cookieKey += serviceName;

            if (ObjectCookies<List<CtvItem>>.Exists(_cookieKey))
                _listItem = ObjectCookies<List<CtvItem>>.GetValue(_cookieKey);

            if (_listItem == null)
                _listItem = new List<CtvItem>();
        }

        public bool Exists(CtvItem item)
        {
            return _listItem.Contains(item);
        }

        public void Add(CtvItem item)
        {
            Remove(item);

            _listItem.Add(item);
        }

        public CtvItem Find(CtvItem item)
        {
            return _listItem.Find(o => o.Equals(item));
        }

        public void Remove(CtvItem item)
        {
            if (Exists(item))
                _listItem.Remove(item);
        }

        public void RemoveAll()
        {
            _listItem.Clear();
        }

        public void Save()
        {
            if (_listItem.Count > 0)
                ObjectCookies<List<CtvItem>>.SetValue(_cookieKey, _listItem);
            else
                ObjectCookies<List<CtvItem>>.Remove(_cookieKey);
        }
    }
}