using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VSW.Website.Global
{
    public class CartItem
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public int ColorID { get; set; }
        public int SizeID { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is CartItem)) return base.Equals(obj);

            var temp = (CartItem)obj;

            return ProductID.Equals(temp.ProductID) && ColorID.Equals(temp.ColorID) && SizeID.Equals(temp.SizeID);
        }

        public override int GetHashCode()
        {
            return (ProductID + "-" + ColorID + "-" + SizeID).GetHashCode();
        }
    }

    public class Cart
    {
        private readonly List<CartItem> _listItem = new List<CartItem>();
        private readonly string _cookieKey = "VSW_Cart";

        public ReadOnlyCollection<CartItem> Items => _listItem.AsReadOnly();

        public int Count => _listItem.Count;

        public Cart()
            : this(string.Empty)
        {
        }

        public Cart(string serviceName)
        {
            _cookieKey += serviceName;

            if (ObjectCookies<List<CartItem>>.Exists(_cookieKey))
                _listItem = ObjectCookies<List<CartItem>>.GetValue(_cookieKey);

            if (_listItem == null)
                _listItem = new List<CartItem>();
        }

        public bool Exists(CartItem item)
        {
            return _listItem.Contains(item);
        }

        public void Add(CartItem item)
        {
            Remove(item);

            _listItem.Add(item);
        }

        public CartItem Find(CartItem item)
        {
            return _listItem.Find(o => o.Equals(item));
        }

        public void Remove(CartItem item)
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
                ObjectCookies<List<CartItem>>.SetValue(_cookieKey, _listItem);
            else
                ObjectCookies<List<CartItem>>.Remove(_cookieKey);
        }
    }
}