using VSW.Core.Models;

namespace VSW.Lib.Models
{
    //public class ProductAjaxModel<T> where T : EntityBase
    public class ProductAjaxModel
    {
        #region Autogen by VSW

        public string Key { get; set; }

        public object Data { get; set; }
        public ProductAjaxModel(string _key, object _data)
        {
            Key = _key;
            Data = _data;
        }
        //public T Data { get; set; }

        //public ProductAjaxModel(string _key, T _data)
        //{
        //    Key = _key;
        //    Data = _data;
        //}
        #endregion Autogen by VSW
    }
}