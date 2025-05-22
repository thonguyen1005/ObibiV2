using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSW.Website.Models
{
    /// <summary>
    /// Dùng chung cho các model Paging trả về, nếu không muốn tạo Class riêng theo nghiệp vụ
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class PageListModel<TModel> : BasePagedListModel<TModel>
    {
        public PageListModel()
        {
        }
    }
}
