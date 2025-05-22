using System;

namespace VSW.Lib.Models
{
    public class OrderSuccessResponse
    {
        public string id { get; set; }
        public string mrc_order_id { get; set; }
        public string txn_id { get; set; }
        public long total_amount { get; set; }
        public int stat { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string checksum { get; set; }
    }
}