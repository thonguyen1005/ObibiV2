using Microsoft.AspNetCore.Mvc.Rendering;
using VSW.Website.DataBase.Models;

namespace VSW.Website.Models
{
    public class MViewCartModel
    {
        public MViewCartModel()
        {
        }
        public int Index { get; set; }
        public int ProductID { get; set; }
        public int SizeID { get; set; }
        public int ColorID { get; set; }

        private int _quantity = 1;
        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }
    }
}
