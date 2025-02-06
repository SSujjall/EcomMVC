using EcomSiteMVC.Utilities.ExternalServices.PdfService.Model;

namespace EcomSiteMVC.Utilities.ExternalServices.PdfService.Service
{
    public interface IPdfService
    {
        PdfResponseModel GenerateOrderPdfFromHtml(PdfRequestModel request);
    }
}
