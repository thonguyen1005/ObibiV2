using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core
{
    public static class JsonSettings
    {
        public static Lazy<JsonSerializerSettings> Tolerant = new Lazy<JsonSerializerSettings>(InitTolerantSetting, true);
        public static Lazy<JsonSerializerSettings> Strict = new Lazy<JsonSerializerSettings>(InitStrictSetting, true);

        public static Func<List<JsonConverter>> GetCustomConverters { get; set; }

        public const bool UseStrictAsDefault = false;

        private static JsonSerializerSettings InitTolerantSetting()
        {
            var setting = new JsonSerializerSettings();
            setting.NullValueHandling = NullValueHandling.Ignore;
            setting.MissingMemberHandling = MissingMemberHandling.Ignore;
            setting.TypeNameHandling = TypeNameHandling.None;
            setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            setting.PreserveReferencesHandling = PreserveReferencesHandling.None;
            setting.DefaultValueHandling = DefaultValueHandling.Ignore;
            setting.Converters.Add(new IsoDateTimeConverter());
            setting.Converters.Add(new JsonSafeInt64Converter());

            if(GetCustomConverters != null)
            {
                var converters = GetCustomConverters();
                if(converters.IsNotEmpty())
                {
                    foreach(var c in converters)
                    {
                        setting.Converters.Add(c);
                    }
                }                
            }
            return setting;
        }

        private static JsonSerializerSettings InitStrictSetting()
        {
            var setting = new JsonSerializerSettings();
            setting.NullValueHandling = NullValueHandling.Ignore;
            setting.MissingMemberHandling = MissingMemberHandling.Error;
            setting.TypeNameHandling = TypeNameHandling.None;
            setting.ReferenceLoopHandling = ReferenceLoopHandling.Error;
            setting.PreserveReferencesHandling = PreserveReferencesHandling.None;
            setting.Converters.Add(new IsoDateTimeConverter());
            setting.Converters.Add(new JsonSafeInt64Converter());

            if (GetCustomConverters != null)
            {
                var converters = GetCustomConverters();
                if (converters.IsNotEmpty())
                {
                    foreach (var c in converters)
                    {
                        setting.Converters.Add(c);
                    }
                }
            }

            return setting;
        }

        private static JsonSerializerSettings _apiJsonSettingUtcTimeMode;

        public static void Init()
        {
            _apiJsonSettingUtcTimeMode = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore             
            };
        }


        public static void ConfigApiSetting(this JsonSerializerSettings setting)
        {
            setting.Converters.Add(new IsoDateTimeConverter());
            setting.Converters.Add(new JsonSafeInt64Converter());
            if (GetCustomConverters != null)
            {
                var converters = GetCustomConverters();
                if (converters.IsNotEmpty())
                {
                    foreach (var c in converters)
                    {
                        setting.Converters.Add(c);
                    }
                }
            }
        }

    }
}
