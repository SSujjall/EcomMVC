using System.Net.Http;
using System.Text;
using CloudinaryDotNet;
using EcomSiteMVC.Extensions.KhaltiPaymentService.Config;
using EcomSiteMVC.Extensions.KhaltiPaymentService.Model;
using MimeKit;
using Newtonsoft.Json;

namespace EcomSiteMVC.Extensions.KhaltiPaymentService.Service
{
    public sealed class KhaltiService : IKhaltiService
    {
        private readonly HttpClient _httpClient;
        private readonly KhaltiConfig _khaltiConfig;

        public KhaltiService(HttpClient httpClient, KhaltiConfig khaltiConfig)
        {
            _httpClient = httpClient;
            _khaltiConfig = khaltiConfig;
        }

        public async Task<PaymentInitiateResponse> InitiatePayment(KhaltiRequestModel requestModel)
        {
            var initiateUrl = _khaltiConfig.InitiateUrl.ToString();

            var request = JsonConvert.SerializeObject(requestModel);
            var content = new StringContent(request, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Key {_khaltiConfig.SecretKey}");

            var apiResponse = await _httpClient.PostAsync(initiateUrl, content);

            var responseString = await apiResponse.Content.ReadAsStringAsync();
            if (!apiResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Khalti API Error: {responseString}");
            }

            var response = JsonConvert.DeserializeObject<PaymentInitiateResponse>(responseString);
            return response;
        }
    }
}
