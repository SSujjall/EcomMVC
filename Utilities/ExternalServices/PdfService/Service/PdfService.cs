using EcomSiteMVC.Utilities.ExternalServices.PdfService.Model;
using iText.Html2pdf;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;

namespace EcomSiteMVC.Utilities.ExternalServices.PdfService.Service
{
    public class PdfService : IPdfService
    {
        private readonly string _htmlPrefix;
        private readonly string _htmlSuffix = "</body></html>";
        private readonly ConverterProperties _converterProperties = new ConverterProperties();

        public PdfService()
        {
            /** bootstrap is not supported by iText7 when generating pdf, 
             * so we use the already available bootstrap classes and put css in them to make it look matching.
             **/
            var customCssStyles = @"
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
            _htmlPrefix = $"<!DOCTYPE html><html><head><meta charset='UTF-8'><style>{customCssStyles}</style><title>";
        }
        public PdfResponseModel GenerateOrderPdfFromHtml(PdfRequestModel request)
        {
            var title = request.Title ?? "receipt";
            var fullHtmlContent = $"{_htmlPrefix}{title}</title></head><body>{request.HtmlContent}{_htmlSuffix}";

            using var memoryStream = new MemoryStream(1024 * 50);
            var writerProperties = new WriterProperties().SetCompressionLevel(CompressionConstants.NO_COMPRESSION); // trades file size for faster generation — remove this line if you need smaller files
            using var writer = new PdfWriter(memoryStream);
            using var pdf = new PdfDocument(writer);

            HtmlConverter.ConvertToPdf(fullHtmlContent, pdf, _converterProperties);

            return new PdfResponseModel
            {
                FileContents = memoryStream.ToArray(),
                FileName = $"{(request.Title ?? "receipt")}.pdf",
                ContentType = "application/pdf"
            };
        }
    }
}
