using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VSW.Lib.Global
{
    public class CompareItem
    {
        public int ProductID { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is CompareItem)) return base.Equals(obj);

            var temp = (CompareItem)obj;

            return ProductID.Equals(temp.ProductID);
        }

        public override int GetHashCode()
        {
            return (ProductID).GetHashCode();
        }
    }

    public class Compare
    {
        private readonly List<CompareItem> _listItem = new List<CompareItem>();
        private readonly string _cookieKey = "VSW_Compare";

        public ReadOnlyCollection<CompareItem> Items => _listItem.AsReadOnly();

        public int Count => _listItem.Count;

        public Compare()
            : this(string.Empty)
        {
        }

        public Compare(string serviceName)
        {
            _cookieKey += serviceName;

            if (ObjectCookies<List<CompareItem>>.Exists(_cookieKey))
                _listItem = ObjectCookies<List<CompareItem>>.GetValue(_cookieKey);

            if (_listItem == null)
                _listItem = new List<CompareItem>();
        }

        public bool Exists(CompareItem item)
        {
            return _listItem.Contains(item);
        }

        public void Add(CompareItem item)
        {
            Remove(item);

            _listItem.Add(item);
        }

        public CompareItem Find(CompareItem item)
        {
            return _listItem.Find(o => o.Equals(item));
        }

        public void Remove(CompareItem item)
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
                ObjectCookies<List<CompareItem>>.SetValue(_cookieKey, _listItem);
            else
                ObjectCookies<List<CompareItem>>.Remove(_cookieKey);
        }
    }
}