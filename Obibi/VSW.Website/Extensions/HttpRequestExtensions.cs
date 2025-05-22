using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VSW.Core;

namespace VSW.Website
{
    public static class HttpRequestExtensions
    {
        public static HttpContext GetContext()
        {
            var accessor = CoreService.ServiceProvider.GetService<IHttpContextAccessor>();
            if(accessor == null)
            {
                return null;
            }

            return accessor.HttpContext;
        }

        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Headers != null)
            {
                string header = request.Headers["X-Requested-With"];
                return header.IsNotEmpty() && header.EqualsIgnoreCase("XmlHttpRequest");
            }    

            return false;
        }

        public static bool IsAjaxRequest(this HttpContext context)
        {
            return context.Request.IsAjaxRequest();
        }

        public static NameValueCollection GetFormAsCollection(this HttpRequest request)
        {
            var r = new NameValueCollection();
            var form = request.Form;
            if(!request.HasFormContentType)
            {
                return r;
            }    

            foreach (var x in form)
            {
                r.Add(x.Key, x.Value);
            }
            return r;
        }

        public static NameValueCollection GetQueryAsCollection(this HttpRequest request)
        {
            var r = new NameValueCollection();
            foreach (var x in request.Query)
            {
                r.Add(x.Key, x.Value);
            }
            return r;
        }

        /// <summary>
        /// Get remote ip address, optionally allowing for x-forwarded-for header check
        /// </summary>
        /// <param name="context">Http context</param>
        /// <returns>{clientIp, proxyIp}</returns>
        public static (IPAddress, IPAddress) GetRemoteIPAddresses(this HttpContext context)
        {
            IPAddress clientIp;
            IPAddress proxyIp;

            string header = context.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                context.Request.Headers["CF-Connecting-IP"].FirstOrDefault();

            // có proxy
            if (header != null)
            {
                if (IPAddress.TryParse(header, out clientIp))
                {
                    if (clientIp.IsIPv4MappedToIPv6)
                    {
                        clientIp = clientIp.MapToIPv4();
                    }
                }
                else
                {
                    clientIp = null;
                }
                proxyIp = context.Connection.RemoteIpAddress;
                if (proxyIp.IsIPv4MappedToIPv6)
                {
                    proxyIp = proxyIp.MapToIPv4();
                }
                if (clientIp == null)
                {
                    clientIp = proxyIp;
                }
                return (clientIp, proxyIp);
            }
            // không có proxy
            proxyIp = null;
            clientIp = context.Connection.RemoteIpAddress;
            if (clientIp.IsIPv4MappedToIPv6)
            {
                clientIp = clientIp.MapToIPv4();
            }
            return (clientIp, proxyIp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns>{clientIp, proxyIp}</returns>
        public static (string, string) GetRemoteIpAddresses(this HttpRequest request)
        {
            var (clientIp, proxyIp) = GetRemoteIPAddresses(request.HttpContext);
            string clientIpAsStr = clientIp.ToString();
            string proxyIpAsStr = proxyIp?.ToString();
            return (clientIpAsStr, proxyIpAsStr);
        }

        /// <summary>
        /// Trả về ip của client, bỏ qua proxy nếu có
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetClientIpAddress(this HttpRequest request)
        {
            return GetClientIpAddress(request.HttpContext);
        }

        public static string GetClientIpAddress(this HttpContext context)
        {
            var (clientIp, _) = GetRemoteIPAddresses(context);
            string clientIpAsStr = clientIp.ToString();
            return clientIpAsStr;
        }
    }
}
