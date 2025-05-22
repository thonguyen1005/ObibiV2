namespace VSW.Lib.Models
{
    public class OrderSendRequest
    {
        public string mrc_order_id { get; set; }
        public long total_amount { get; set; }
        public string customer_email { get; set; }
        public string customer_phone { get; set; }
        public string description { get; set; }
        public string url_success { get; set; }
        public string url_detail { get; set; }
        public long merchant_id { get; set; }
        public string webhooks { get; set; }
        public string customer_name { get; set; }
        public string customer_address { get; set; }
    }
}