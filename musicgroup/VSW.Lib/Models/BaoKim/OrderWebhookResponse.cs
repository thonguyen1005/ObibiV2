using System.Collections.Generic;

namespace VSW.Lib.Models
{
    public class OrderWebhookResponse
    {
        public OrderResponseData order { get; set; }
        public TxnResponseData txn { get; set; }
        public List<string> dataToken { get; set; } = new List<string>();
        public string sign { get; set; }
    }

    public class OrderResponseData
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public string mrc_order_id { get; set; }
        public int txn_id { get; set; }
        public object ref_no { get; set; }
        public int merchant_id { get; set; }
        public string total_amount { get; set; }
        public string description { get; set; }
        public object items { get; set; }
        public string url_success { get; set; }
        public object url_cancel { get; set; }
        public string url_detail { get; set; }
        public string stat { get; set; }
        public string lang { get; set; }
        public string bpm_id { get; set; }
        public int type { get; set; }
        public int accept_qrpay { get; set; }
        public int accept_bank { get; set; }
        public int accept_cc { get; set; }
        public int accept_ib { get; set; }
        public int accept_ewallet { get; set; }
        public int accept_installments { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string webhooks { get; set; }
        public string customer_name { get; set; }
        public string customer_email { get; set; }
        public string customer_phone { get; set; }
        public object customer_address { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }

    public class TxnResponseData
    {
        public int id { get; set; }
        public string reference_id { get; set; }
        public int user_id { get; set; }
        public int merchant_id { get; set; }
        public int order_id { get; set; }
        public string mrc_order_id { get; set; }
        public string total_amount { get; set; }
        public string amount { get; set; }
        public string fee_amount { get; set; }
        public string bank_fee_amount { get; set; }
        public string bank_fix_fee_amount { get; set; }
        public int fee_payer { get; set; }
        public int bank_fee_payer { get; set; }
        public object auth_code { get; set; }

        private string _authTime = string.Empty;

        public string auth_time
        {
            get => _authTime;
            set
            {
                _authTime = value ?? string.Empty;
            }
        }

        public object ref_no { get; set; }
        public string bpm_id { get; set; }
        public string bank_ref_no { get; set; }
        public int bpm_type { get; set; }
        public string gateway { get; set; }
        public int stat { get; set; }
        public object init_token { get; set; }
        public string description { get; set; }
        public string customer_email { get; set; }
        public string customer_phone { get; set; }
        public string completed_at { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public object deleted_at { get; set; }
    }
}