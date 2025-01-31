using Newtonsoft.Json;

namespace EcomSiteMVC.Extensions.KhaltiPaymentService.Model
{
    public class PaymentInitiateResponse
    {
        [JsonProperty("pidx")]
        public string Pidx { get; set; }

        [JsonProperty("payment_url")]
        public string PaymentUrl { get; set; }

        [JsonProperty("expires_at")]
        public DateTime ExpiresAt { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }

    public class PaymentLookupResponse
    {
        [JsonProperty("pidx")]
        public string Pidx { get; set; }

        [JsonProperty("total_amount")]
        public int TotalAmount { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("transaction_id")]
        public string TransactionId { get; set; }

        [JsonProperty("fee")]
        public int Fee { get; set; }

        [JsonProperty("refunded")]
        public bool Refunded { get; set; }
    }
}
