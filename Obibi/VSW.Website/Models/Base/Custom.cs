using System;
using System.Collections.Generic;
using VSW.Core;

namespace VSW.Website.Models
{
    public class Custom
    {
        public Custom()
        {

        }
        public Custom(string value)
        {
            _item = new Dictionary<string, string>();
            bool flag = value == string.Empty;
            if (!flag)
            {
                string[] array = value.Split(new char[]
                {
                    '\n'
                });
                bool flag2 = false;
                for (int i = 0; i < array.Length; i++)
                {
                    string text = array[i].Trim();
                    bool flag3 = text == string.Empty || text.StartsWith("//");
                    if (!flag3)
                    {
                        bool flag4 = text == "/*";
                        if (flag4)
                        {
                            flag2 = true;
                        }
                        else
                        {
                            bool flag5 = text == "*/";
                            if (flag5)
                            {
                                flag2 = false;
                            }
                            else
                            {
                                bool flag6 = !flag2;
                                if (flag6)
                                {
                                    int num = text.IndexOf('=');
                                    bool flag7 = num > -1;
                                    if (flag7)
                                    {
                                        string value2 = text.Substring(num + 1);
                                        text = text.Substring(0, num);
                                        SetValue(text, value2);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private Dictionary<string, string> _item = new Dictionary<string, string>();
        public string[] AllKeys
        {
            get
            {
                string[] array = new string[0];
                bool flag = _item.Keys != null;
                if (flag)
                {
                    array = new string[_item.Keys.Count];
                    _item.Keys.CopyTo(array, 0);
                }
                return array;
            }
        }
        public int Count
        {
            get
            {
                return _item.Count;
            }
        }
        public string this[string Key]
        {
            get
            {
                return GetValue(Key);
            }
            set
            {
                SetValue(Key, value);
            }
        }
        public bool Exists(string key)
        {
            return _item.ContainsKey(key);
        }
        public string GetValue(string key)
        {
            bool flag = _item.ContainsKey(key);
            if (flag)
            {
                return _item[key];
            }
            return "";
        }
        public void Remove(string key)
        {
            bool flag = Exists(key);
            if (flag)
            {
                _item.Remove(key);
            }
        }
        public void SetValue(string key, string value)
        {
            bool flag = !string.IsNullOrEmpty(key) && value != null;
            if (flag)
            {
                _item[key] = value;
            }
        }

        public Custom GetCustom(string vswID)
        {
            Custom custom = new Custom();
            if (vswID.IsEmpty()) return custom;
            string[] allKeys = AllKeys;
            for (int i = 0; i < allKeys.Length; i++)
            {
                bool flag2 = allKeys[i].StartsWith(vswID + ".");
                if (flag2)
                {
                    custom.SetValue(allKeys[i].Replace(vswID + ".", string.Empty), GetValue(allKeys[i]));
                }
            }
            return custom;
        }
    }
}
