using EcomSiteMVC.Utilities.ExternalServices.PdfService.Model;
using iText.Html2pdf;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;

namespace EcomSiteMVC.Utilities.ExternalServices.PdfService.Service
{
    public class PdfService : IPdfService
    {
        public PdfResponseModel GenerateOrderPdfFromHtml(PdfRequestModel request)
        {
            var fullHtmlContent = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <title>{request.Title}</title>
                    <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css"" rel=""stylesheet"">
                    <style>
                        /* Remove modal-specific styles that might affect PDF */
                        .modal {{ 
                            position: static;
                            display: block;
                        }}
                        .modal-dialog {{
                            margin: 0;
                            max-width: none;
                        }}
                        /* Hide close button and footer in PDF */
                        .btn-close, .modal-footer {{
                            display: none;
                        }}
                    </style>
                </head>
                <body>
                    {request.HtmlContent}
                </body>
                </html>";

            using var memoryStream = new MemoryStream();
            var writer = new PdfWriter(memoryStream);
            var pdf = new PdfDocument(writer);
            pdf.SetDefaultPageSize(PageSize.A4);

            var converterProperties = new ConverterProperties();
            HtmlConverter.ConvertToPdf(fullHtmlContent, pdf, converterProperties);

            return new PdfResponseModel
            {
                FileContents = memoryStream.ToArray(),
                FileName = $"{(request.Title ?? "receipt")}.pdf",
                ContentType = "application/pdf"
            };
        }
    }
}
