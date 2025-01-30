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
}
