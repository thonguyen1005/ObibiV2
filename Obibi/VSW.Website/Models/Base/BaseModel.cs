using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace VSW.Website.Models
{
    public class BaseModel
    {
        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public BaseModel()
        {
        }
        //Các dữ liệu thay đổi
        public string DataChange { get; set; }
        #endregion

    }
}
