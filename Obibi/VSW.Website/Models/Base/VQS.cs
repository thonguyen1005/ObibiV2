using System;
using System.Collections.Generic;
using VSW.Core;

namespace VSW.Website.Models
{
    public class VQS
    {
        private List<string> listURL;
        public string BeginCode
        {
            get
            {
                return GetString(0);
            }
        }
        public int Count
        {
            get
            {
                return listURL.Count;
            }
        }
        public string EndCode
        {
            get
            {
                return GetString_End(0);
            }
        }
        public VQS()
        {
            listURL = new List<string>();
        }
        public VQS(string url)
        {
            listURL = new List<string>();
            bool flag = !string.IsNullOrEmpty(url);
            if (flag)
            {
                bool flag2 = url.EndsWith("/");
                if (flag2)
                {
                    url = url.Substring(0, url.Length - 1);
                }
                int num = url.IndexOf('.');
                bool flag3 = num > -1;
                if (flag3)
                {
                    url = url.Substring(0, num);
                }
                listURL.AddRange(url.Split(new char[]
                {
                    '/'
                }));
            }
            if (listURL.IsNotEmpty())
            {
                listURL = listURL.Where(o => o != "").ToList();
            }
        }
        public bool Equals(int index, string code)
        {
            return GetString(index).ToLower() == code.ToLower();
        }
        public string GetString(int index)
        {
            bool flag = Exists(index);
            string result;
            if (flag)
            {
                result = listURL[index].Trim();
            }
            else
            {
                result = string.Empty;
            }
            return result;
        }
        public string GetString_End(int index)
        {
            index = Count - index - 1;
            return GetString(index);
        }
        private bool Exists(int int_0)
        {
            return int_0 >= 0 && int_0 < Count;
        }

    }
}
