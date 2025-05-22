using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace VSW.Core
{
    public class PagingResult<T>
    {
        public List<T> Items { get; set; }

        public int TotalCount { get; set; }

        public int PageIndex { get; set; }

        public int TotalPage { get; set; }

        public int PageSize { get; set; }


        public PagingResult(List<T> items, int totalCount, int page, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageIndex = page;
            PageSize = pageSize;
            TotalPage = TotalCount.Ceiling(PageSize);
        }

        /// <summary>
        /// Nếu Mapping Function = null --> Sẽ sử dụng hàm MapToList mặc định của hệ thống
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="mapping"></param>
        /// <returns></returns>
        public PagingResult<TDestination> ToResult<TDestination>(Func<T, TDestination> mapping = null)
        {
            List<TDestination> lst = null;
            if (mapping != null)
            {
                lst = new List<TDestination>();
                foreach (var obj in Items)
                {
                    lst.Add(mapping(obj));
                }
            }
            else
            {
                lst = Items.MapToList<TDestination>();
            }

            var rs = new PagingResult<TDestination>(lst, TotalCount, PageIndex, PageSize);
            return rs;
        }
    }

    public class PagingInfo
    {
        public int Size { get; set; }

        public int Index { get; set; }

        public PagingInfo()
        {

        }

        public PagingInfo(int size, int index)
        {
            Size = size;
            Index = index;
        }

        public int GetSkip()
        {
            return (Index < 1 ? 0 : (Index - 1)) * Size;
        }

        public int GetTake()
        {
            return Index < 1 ? 0 : Size;
        }
    }
}
