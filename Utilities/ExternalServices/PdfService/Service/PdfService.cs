using EcomSiteMVC.Utilities.ExternalServices.PdfService.Model;
using iText.Html2pdf;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;

namespace EcomSiteMVC.Utilities.ExternalServices.PdfService.Service
{
    public class PdfService : IPdfService
    {
        private string CustomCssStyles;
        public PdfService()
        {
            /** bootstrap is not supported by iText7 when generating pdf, 
             * so we use the already available bootstrap classes and put css in them to make it look matching.
             **/
            CustomCssStyles = @"
            /* Remove modal-specific styles that might affect PDF */
                .modal { 
                    position: static;
                    display: block;
                }
                .modal-dialog {
                    margin: 0;
                    max-width: none;
                }
                /* Hide close button and footer in PDF */
                .btn-close, .modal-footer {
                    display: none;
                }
                        
                /* Main Style Starts Here*/
                .modal-content {
                    background-color: #fff;
                    border-radius: 5px;
                    padding: 20px;
                    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
                }

                .modal-header {
                    border-bottom: 1px solid #ddd;
                    margin-bottom: 10px;
                }

                .modal-title {
                    font-size: 1.5rem;
                    font-weight: bold;
                }

                .modal-body {
                    font-size: 1rem;
                }

                .row {
                    display: flex;
                    flex-direction: row;
                }

                .mb-4 {
                    margin-bottom: 1.5rem;
                }

                .table-responsive {
                    overflow-x: auto;
                }

                .table {
                    width: 100%;
                    border-collapse: collapse;
                    margin-top: 10px;
                }

                .table th, .table td {
                    padding: 10px;
                    border: 1px solid #ddd;
                    text-align: left;
                }

                .text-end {
                    text-align: right;
                    font-size: 25px;
                }
            ";
        }
        public PdfResponseModel GenerateOrderPdfFromHtml(PdfRequestModel request)
        {
            var fullHtmlContent = $@"
                <!DOCTYPE html>
                <html>
                <head> 
                    <meta charset='UTF-8'>
                    <title>{request.Title}</title>
                    <style>
                        {CustomCssStyles}
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
