using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using EcommApiCoreV3.BAL.Interface;
using EcommApiCoreV3.Controllers.Common;
using EcommApiCoreV3.Entities;
using static EcommApiCoreV3.Controllers.Common.SendEmails;

namespace EcommApiCoreV3.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("api/[controller]")]
    public class ReportController : BaseController<OrderController>
    {
        IOrderBAL _IOrderBAL;
        Common.Utilities _utilities = new Common.Utilities();
        public static string webRootPath;
        //IEmailTemplateBAL _IEmailTemplateBAL;
        IUsersBAL _usersBAL;
        private IConverter _converter;
        public ReportController(IWebHostEnvironment hostingEnvironment,
            IOrderBAL OrderBAL,
            //IEmailTemplateBAL emailTemplateBAL, 
            IUsersBAL usersBAL,
            IConverter converter)
        {
            _IOrderBAL = OrderBAL;
            webRootPath = hostingEnvironment.WebRootPath;
            //_IEmailTemplateBAL = emailTemplateBAL;
            _usersBAL = usersBAL;
            _converter = converter;
        }


        [HttpPost]
        [Route("GenerateOrderDetail")]
        public string GenerateOrderDetail([FromBody] Order obj)
        {
            string FileName = DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";

            List<Order> lst = this._IOrderBAL.GetAllOrder(obj).Result;
            for (int i = 0; i < lst.Count; i++)
            {
                obj.OrderId = lst[i].OrderId;
                lst[i].OrderDetails = this._IOrderBAL.GetAllOrderDetails(obj).Result;
                foreach (var item in lst[i].OrderDetails)
                {
                    if (item.SetNo > 0)
                        item.ProductImg = _utilities.ProductImage(item.ProductId, "productSetImage", webRootPath, item.SetNo);
                    else
                        item.ProductImg = _utilities.ProductImage(item.ProductId, "productColorImage", webRootPath, item.ProductSizeColorId);
                }
            }
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Order Details Report",
                Out = webRootPath + "\\ReportGenerate\\" + FileName
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = ReportTemplate.OrderDetailByFilterTemplate(lst),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            var file = _converter.Convert(pdf);

            //return Ok("Successfully created PDF document.");
            File(file, "application/pdf", FileName);
            return FileName;
        }


        [HttpPost]
        [Route("GenerateOrderInvoice")]
        public string GenerateOrderInvoice([FromBody] Order obj)
        {

            List<Order> lst = this._IOrderBAL.GetOrderByOrderId(obj).Result;
            lst[0].OrderDetails = this._IOrderBAL.GetSuccessOrderDetailsByOrderId(obj).Result;
            foreach (var item in lst[0].OrderDetails)
            {
                if (item.SetNo > 0)
                    item.ProductImg = _utilities.ProductImage(item.ProductId, "productSetImage", webRootPath, item.SetNo);
                else
                    item.ProductImg = _utilities.ProductImage(item.ProductId, "productColorImage", webRootPath, item.ProductSizeColorId);
            }
            string FileName = DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Order Invoice Report",
                Out = webRootPath + "\\ReportGenerate\\" + FileName
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = ReportTemplate.OrderInvoiceTemplate(lst),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            var file = _converter.Convert(pdf);

            //return Ok("Successfully created PDF document.");
            File(file, "application/pdf", FileName);
            //return File(file, "application/pdf");

            return FileName;

        }

        [HttpPost]
        [Route("GetAllOrder")]
        [AllowAnonymous]
        public async Task<List<Order>> GetAllOrder([FromBody] Order obj)
        {
            try
            {
                List<Order> lst = this._IOrderBAL.GetAllOrder(obj).Result;
                for (int i = 0; i < lst.Count; i++)
                {
                    obj.OrderId = lst[i].OrderId;
                    lst[i].OrderDetails = this._IOrderBAL.GetAllOrderDetails(obj).Result;
                    foreach (var item in lst[i].OrderDetails)
                    {
                        if (item.SetNo > 0)
                            item.ProductImg = _utilities.ProductImage(item.ProductId, "productSetImage", webRootPath, item.SetNo);
                        else
                            item.ProductImg = _utilities.ProductImage(item.ProductId, "productColorImage", webRootPath, item.ProductSizeColorId);
                    }
                }


                ////Create document  
                //Document doc = new Document();
                ////Create PDF Table  
                //PdfPTable tableLayout = new PdfPTable(4);
                ////Create a PDF file in specific path  
                //PdfWriter.GetInstance(doc, new System.IO.FileStream(webRootPath + "//Sample-PDF-File.pdf", System.IO.FileMode.Create));
                ////Open the PDF document  
                //doc.Open();
                ////Add Content to PDF  
                //doc.Add(Add_Content_To_PDF(tableLayout));
                //// Closing the document  
                //doc.Close();



                return await Task.Run(() => new List<Order>(lst));
            }
            catch (Exception ex)
            {
                Logger.LogError($"Something went wrong inside OrderController GetOrderByUserId action: {ex.Message}");
                return null;
            }
        }
        //    private PdfPTable Add_Content_To_PDF(PdfPTable tableLayout)
        //    {
        //        float[] headers = {
        //    20,
        //    20,
        //    30,
        //    30
        //}; //Header Widths  
        //        tableLayout.SetWidths(headers); //Set the pdf headers  
        //        tableLayout.WidthPercentage = 80; //Set the PDF File witdh percentage  
        //                                          //Add Title to the PDF file at the top  
        //        tableLayout.AddCell(new PdfPCell(new Phrase("Creating PDF file using iTextsharp", new Font(Font.NORMAL, 13, 1, new iTextSharp.text.BaseColor(153, 51, 0))))
        //        {
        //            Colspan = 4,
        //            Border = 0,
        //            PaddingBottom = 20,
        //            HorizontalAlignment = Element.ALIGN_CENTER
        //        });
        //        //Add header  
        //        AddCellToHeader(tableLayout, "Cricketer Name");
        //        AddCellToHeader(tableLayout, "Height");
        //        AddCellToHeader(tableLayout, "Born On");
        //        AddCellToHeader(tableLayout, "Parents");
        //        //Add body  
        //        AddCellToBody(tableLayout, "Sachin Tendulkar");
        //        AddCellToBody(tableLayout, "1.65 m");
        //        AddCellToBody(tableLayout, "April 24, 1973");
        //        AddCellToBody(tableLayout, "Ramesh Tendulkar, Rajni Tendulkar");
        //        AddCellToBody(tableLayout, "Mahendra Singh Dhoni");
        //        AddCellToBody(tableLayout, "1.75 m");
        //        AddCellToBody(tableLayout, "July 7, 1981");
        //        AddCellToBody(tableLayout, "Devki Devi, Pan Singh");
        //        AddCellToBody(tableLayout, "Virender Sehwag");
        //        AddCellToBody(tableLayout, "1.70 m");
        //        AddCellToBody(tableLayout, "October 20, 1978");
        //        AddCellToBody(tableLayout, "Aryavir Sehwag, Vedant Sehwag");
        //        AddCellToBody(tableLayout, "Virat Kohli");
        //        AddCellToBody(tableLayout, "1.75 m");
        //        AddCellToBody(tableLayout, "November 5, 1988");
        //        AddCellToBody(tableLayout, "Saroj Kohli, Prem Kohli");
        //        return tableLayout;
        //    }
        //    // Method to add single cell to the header  
        //    private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        //    {
        //        tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.NORMAL, 8, 1, iTextSharp.text.BaseColor.WHITE)))
        //        {
        //            HorizontalAlignment = Element.ALIGN_CENTER,
        //            Padding = 5,
        //            BackgroundColor = new iTextSharp.text.BaseColor(0, 51, 102)
        //        });
        //    }
        //    // Method to add single cell to the body  
        //    private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        //    {
        //        tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.NORMAL, 8, 1, iTextSharp.text.BaseColor.BLACK)))
        //        {
        //            HorizontalAlignment = Element.ALIGN_CENTER,
        //            Padding = 5,
        //            BackgroundColor = iTextSharp.text.BaseColor.WHITE
        //        });
        //    }
    }

    //public class PageEventHelper : PdfPageEventHelper
    //{
    //    public string Path { get; set; }
    //    public string HeaderText { get; set; }
    //    public override void OnEndPage(PdfWriter writer, Document document)
    //    {

    //        float[] headers = { 10, 35, 15 };  //Header Widths
    //        Font Arial_14 = FontFactory.GetFont("Calibri", 14, Font.BOLD, BaseColor.BLACK);
    //        Font Arial_8 = FontFactory.GetFont("Calibri", 9, Font.NORMAL, BaseColor.BLACK);
    //        Font Arial_8_Bold = FontFactory.GetFont("Calibri", 9, Font.BOLD, BaseColor.BLACK);
    //        Font Arial_10_Bold = FontFactory.GetFont("Calibri", 10, Font.BOLD, BaseColor.BLACK);
    //        Font Arial_8_Bold_White = FontFactory.GetFont("Calibri", 9, Font.BOLD, BaseColor.WHITE);
    //        float PageWidth = 550f;

    //        PdfPTable HeaderTable = new PdfPTable(3);

    //        HeaderTable.SetWidths(headers);
    //        //HeaderTable.WidthPercentage = 100;
    //        HeaderTable.TotalWidth = PageWidth;
    //        HeaderTable.LockedWidth = true;
    //        iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(Path + "Images/wish-report.jpg");
    //        gif.WidthPercentage = 100;
    //        PdfPCell cellWithImage = new PdfPCell() { Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_CENTER };
    //        cellWithImage.AddElement(gif);
    //        HeaderTable.AddCell(cellWithImage);
    //        //HeaderText = "Employee Summary Report";
    //        HeaderTable.AddCell(new PdfPCell(new Phrase(HeaderText, Arial_14)) { Border = 0, PaddingTop = 10, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_CENTER });
    //        HeaderTable.AddCell(new PdfPCell(new Phrase("Print Date: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm"), Arial_8)) { Border = 0, PaddingTop = 10, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });

    //        HeaderTable.WriteSelectedRows(0, -1, document.Left - 20, document.Top + 60, writer.DirectContent);
    //    }

    //}
}
