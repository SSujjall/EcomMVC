using Newtonsoft.Json;

namespace EcomSiteMVC.Extensions.KhaltiPaymentService.Model
{
    public class KhaltiRequestModel
    {
        [JsonProperty("return_url")]
        public string ReturnUrl { get; set; }

        [JsonProperty("website_url")]
        public string WebsiteUrl { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("purchase_order_id")]
        public string PurchaseOrderId { get; set; }

        [JsonProperty("purchase_order_name")]
        public string PurchaseOrderName { get; set; }

        [JsonProperty("customer_info")]
        public CustomerInfo CustomerInfo { get; set; }

        [JsonProperty("product_details")]
        public List<ProductDetail> ProductDetails { get; set; }

        [JsonProperty("merchant_username")]
        public string MerchantUsername { get; set; }

        [JsonProperty("merchant_extra")]
        public string MerchantExtra { get; set; }
    }

    public class CustomerInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }
    }

    public class ProductDetail
    {
        [JsonProperty("identity")]
        public string Identity { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("total_price")]
        public int TotalPrice { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("unit_price")]
        public int UnitPrice { get; set; }
    }
}
