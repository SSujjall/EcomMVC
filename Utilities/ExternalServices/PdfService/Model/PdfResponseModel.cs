namespace EcomSiteMVC.Utilities.ExternalServices.PdfService.Model
{
    public class PdfResponseModel
    {
        public byte[] FileContents { get; set; } = Array.Empty<byte>();
        public string FileName { get; set; } = "document.pdf";
        public string ContentType { get; set; } = "application/pdf";
    }
}
