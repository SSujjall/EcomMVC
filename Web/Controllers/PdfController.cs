using EcomSiteMVC.Utilities.ExternalServices.PdfService.Model;
using EcomSiteMVC.Utilities.ExternalServices.PdfService.Service;
using Microsoft.AspNetCore.Mvc;

namespace EcomSiteMVC.Web.Controllers
{
    public class PdfController(IPdfService _pdfService) : Controller
    {
        [HttpPost]
        public IActionResult GenerateSingleOrderPdf([FromBody] PdfRequestModel request)
        {
            try
            {
                var result = _pdfService.GenerateOrderPdfFromHtml(request);
                return File(result.FileContents, result.ContentType, result.FileName);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
