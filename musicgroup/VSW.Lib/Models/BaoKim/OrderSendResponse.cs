using System.Collections.Generic;

namespace VSW.Lib.Models
{
    public class OrderSendResponse
    {
        public int code { get; set; }
        public List<string> message { get; set; }
        public int count { get; set; }
        public OrderSendResponseData Data { get; set; }
    }

    public class OrderSendResponseData
    {
        public long order_id { get; set; }
        public string redirect_url { get; set; }
        public string payment_url { get; set; }
    }
}