using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using EcommApiCoreV3.BAL.Interface;
using EcommApiCoreV3.Controllers.Common;
using EcommApiCoreV3.Entities;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using System.Drawing;

namespace EcommApiCoreV3.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("api/[controller]")]
    public class InvoiceController : BaseController<InvoiceController>
    {
        IOrderBAL _IOrderBAL;
        Utilities _utilities = new Utilities();
        public static string webRootPath;
        IUsersBAL _usersBAL;
        public InvoiceController(IWebHostEnvironment hostingEnvironment,
            IOrderBAL OrderBAL, IUsersBAL usersBAL)
        {
            _IOrderBAL = OrderBAL;
            webRootPath = hostingEnvironment.WebRootPath;
            _usersBAL = usersBAL;
        }

        [HttpPost]
        [Route("OrderInvoiceByGUID")]
        public string OrderInvoiceByGUID([FromBody] Order obj)
        {
            try
            {
                List<Order> lst = this._IOrderBAL.GetPrintOrderByGUID(obj).Result;


                string Temp_Path = webRootPath + "\\TempPDF\\";
                string Invoice_File = "Invoice_" + lst[0].OrderNumber + "_" + DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss");
                string Invoice_Template = webRootPath + (Convert.ToDecimal(lst[0].ListUsers[0].AdditionalDiscount == "" ? "0" : lst[0].ListUsers[0].AdditionalDiscount) > 0 ? "\\Template\\AdditionalDiscountGSTInvoice.docx" : "\\Template\\GSTInvoice.docx");
                Document Invoice_doc = new Document(Invoice_Template);
                // Find and replace text in the document

                Invoice_doc.Replace("[OrderNumber]", lst[0].OrderNumber, false, true);
                Invoice_doc.Replace("[OrderDate]", lst[0].OrderDate, false, true);
                Invoice_doc.Replace("[InvoiceNumber]", (lst[0].InvoiceNo), false, true);
                //Invoice_doc.Replace("[InvoiceNumber]", ("INV-" + lst[0].OrderId.ToString("000") + "-" + lst[0].OrderNumber), false, true);
                Invoice_doc.Replace("[ShippingName]", lst[0].FName + " " + lst[0].LName, false, true);
                Invoice_doc.Replace("[ShippingAddress]", lst[0].Address + ", " + lst[0].City + ", " + lst[0].State + ", " + lst[0].Country + "-" + lst[0].ZipCode, false, true);
                Invoice_doc.Replace("[ShippingState]", lst[0].State, false, true);
                Invoice_doc.Replace("[BillingName]", lst[0].FName + " " + lst[0].LName, false, true);
                Invoice_doc.Replace("[BillingAddress]", lst[0].Address + ", " + lst[0].City + ", " + lst[0].State + ", " + lst[0].Country + "-" + lst[0].ZipCode, false, true);
                Invoice_doc.Replace("[BillingState]", lst[0].State, false, true);
                Invoice_doc.Replace("[dis]", lst[0].OrderDetails[0].AdditionalDiscount.ToString() + "%", false, true);
                Table table1 = Invoice_doc.Sections[0].Tables[0] as Spire.Doc.Table;
                int NextRowNumber = 0;
                if ((Convert.ToDecimal(lst[0].ListUsers[0].AdditionalDiscount == "" ? "0" : lst[0].ListUsers[0].AdditionalDiscount) > 0))
                {
                    for (int i = 0; i < lst[0].OrderDetails.Count; i++)
                    {
                        NextRowNumber = i + 1;
                        TableRow row = table1.AddRow(false, 13);
                        table1.Rows.Insert(NextRowNumber, row);

                        //table1.AddRow(true, 10);
                        //Sr.No
                        table1[NextRowNumber, 0].AddParagraph().AppendText((i + 1).ToString()).CharacterFormat.FontSize = 8;
                        //Product
                        table1[NextRowNumber, 1].AddParagraph().AppendText(lst[0].OrderDetails[i].ProductName).CharacterFormat.FontSize = 8;

                        //HSN Code
                        table1[NextRowNumber, 2].AddParagraph().AppendText(lst[0].OrderDetails[i].HSNCode).CharacterFormat.FontSize = 8;
                        table1[NextRowNumber, 2].CellFormat.VerticalAlignment = VerticalAlignment.Middle;

                        //Quantity
                        //table1[NextRowNumber, 2].AddParagraph().AppendText(lst[0].OrderDetails[i].Quantity.ToString()).CharacterFormat.FontSize = 8;
                        TextAlign(table1, NextRowNumber, 3, lst[0].OrderDetails[i].Quantity.ToString());

                        // PCS
                        TextAlign(table1, NextRowNumber, 4, "PCS");

                        //Gross Amount
                        //table1[NextRowNumber, 3].AddParagraph().AppendText(lst[0].OrderDetails[i].SalePrice.ToString("0.00")).CharacterFormat.FontSize = 8;
                        TextAlign(table1, NextRowNumber, 5, lst[0].OrderDetails[i].SalePrice.ToString("0.00"));

                       
                        //Discount
                        //table1[NextRowNumber, 4].AddParagraph().AppendText(lst[0].OrderDetails[i].AdditionalDiscountAmount.ToString("0.00")).CharacterFormat.FontSize = 8;
                        TextAlign(table1, NextRowNumber, 6, lst[0].OrderDetails[i].AdditionalDiscountAmount.ToString("0.00"));

                        //Taxable Value
                        double GST = lst[0].OrderDetails[i].Quantity * ((lst[0].OrderDetails[i].SalePrice - Convert.ToDouble(lst[0].OrderDetails[i].AdditionalDiscountAmount)) * 18 / 100);
                        //table1[NextRowNumber, 5].AddParagraph().AppendText(((lst[0].OrderDetails[i].Quantity) * (lst[0].OrderDetails[i].SalePrice - Convert.ToDouble(lst[0].OrderDetails[i].AdditionalDiscountAmount)) - GST).ToString("0.00")).CharacterFormat.FontSize = 8;
                        TextAlign(table1, NextRowNumber, 7, ((lst[0].OrderDetails[i].Quantity) * (lst[0].OrderDetails[i].SalePrice - Convert.ToDouble(lst[0].OrderDetails[i].AdditionalDiscountAmount)) - GST).ToString("0.00"));

                        //CGST
                        //table1[NextRowNumber, 6].AddParagraph().AppendText((GST / 2).ToString("0.00")).CharacterFormat.FontSize = 8;
                        TextAlign(table1, NextRowNumber, 8, (GST / 2).ToString("0.00"));

                        //SGST
                        //table1[NextRowNumber, 7].AddParagraph().AppendText((GST / 2).ToString("0.00")).CharacterFormat.FontSize = 8;
                        TextAlign(table1, NextRowNumber, 9, (GST / 2).ToString("0.00"));

                        //IGST
                        //table1[NextRowNumber, 8].AddParagraph().AppendText(GST.ToString("0.00")).CharacterFormat.FontSize = 8;
                        TextAlign(table1, NextRowNumber, 10, GST.ToString("0.00"));

                        //Total
                        //table1[NextRowNumber, 9].AddParagraph().AppendText((lst[0].OrderDetails[i].Quantity * (lst[0].OrderDetails[i].SalePrice - Convert.ToDouble(lst[0].OrderDetails[i].AdditionalDiscountAmount))).ToString("0.00")).CharacterFormat.FontSize = 8;
                        TextAlign(table1, NextRowNumber, 11, (lst[0].OrderDetails[i].Quantity * (lst[0].OrderDetails[i].SalePrice - Convert.ToDouble(lst[0].OrderDetails[i].AdditionalDiscountAmount))).ToString("0.00"));
                    }
                }
                //if (lst[0].ListUsers[0].IsPersonal == false)
                //{
                //    Invoice_doc.Replace("[CompanyName]", "Company Name: " + lst[0].CompanyName, false, true);
                //    if (lst[0].BusinessLicenseType == "GSTIN")
                //        Invoice_doc.Replace("[GSTNo]", ("GST Number: " + lst[0].BusinessLicenseNo), false, true);
                //    if (lst[0].BusinessLicenseType == "BusinessPAN")
                //        Invoice_doc.Replace("[GSTNo]", ("Business PAN: " + lst[0].BusinessLicenseNo), false, true);
                //    if (lst[0].BusinessLicenseType == "AadharCard")
                //        Invoice_doc.Replace("[GSTNo]", ("Aadhar Card: " + lst[0].BusinessLicenseNo), false, true);
                //    for (int i = 0; i < lst[0].OrderDetails.Count; i++)
                //    {
                //        NextRowNumber = i + 1;
                //        TableRow row = table1.AddRow(true, 10);
                //        table1.Rows.Insert(NextRowNumber, row);
                //        //table1.AddRow(true, 8);
                //        //Product
                //        table1[NextRowNumber, 0].AddParagraph().AppendText(lst[0].OrderDetails[i].ProductName).CharacterFormat.FontSize = 8;
                //        //HSN Code + IGST
                //        table1[NextRowNumber, 1].AddParagraph().AppendText("HSN: " + lst[0].OrderDetails[i].HSNCode).CharacterFormat.FontSize = 8;
                //        table1[NextRowNumber, 1].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                //        //Quantity
                //        //table1[NextRowNumber, 2].AddParagraph().AppendText(lst[0].OrderDetails[i].Quantity.ToString()).CharacterFormat.FontSize = 8;
                //        TextAlign(table1, NextRowNumber, 2, lst[0].OrderDetails[i].Quantity.ToString());
                //        //Gross Amount
                //        //table1[NextRowNumber, 3].AddParagraph().AppendText(lst[0].OrderDetails[i].SalePrice.ToString("0.00")).CharacterFormat.FontSize = 8;
                //        TextAlign(table1, NextRowNumber, 3, lst[0].OrderDetails[i].SalePrice.ToString("0.00"));
                //        //Discount
                //        //table1[NextRowNumber, 4].AddParagraph().AppendText(lst[0].OrderDetails[i].AdditionalDiscountAmount.ToString("0.00")).CharacterFormat.FontSize = 8;
                //        TextAlign(table1, NextRowNumber, 4, lst[0].OrderDetails[i].AdditionalDiscountAmount.ToString("0.00"));
                //        //Taxable Value
                //        //table1[NextRowNumber, 5].AddParagraph().AppendText((lst[0].OrderDetails[i].Quantity * (lst[0].OrderDetails[i].SalePrice - Convert.ToDouble(lst[0].OrderDetails[i].AdditionalDiscountAmount))).ToString("0.00")).CharacterFormat.FontSize = 8;
                //        TextAlign(table1, NextRowNumber, 5, (lst[0].OrderDetails[i].Quantity * (lst[0].OrderDetails[i].SalePrice - Convert.ToDouble(lst[0].OrderDetails[i].AdditionalDiscountAmount))).ToString("0.00"));
                //        //CGST
                //        //table1[NextRowNumber, 6].AddParagraph().AppendText((lst[0].OrderDetails[i].GSTAmount / 2).ToString("0.00")).CharacterFormat.FontSize = 8;
                //        TextAlign(table1, NextRowNumber, 6, (lst[0].OrderDetails[i].GSTAmount / 2).ToString("0.00"));
                //        //SGST
                //        //table1[NextRowNumber, 7].AddParagraph().AppendText((lst[0].OrderDetails[i].GSTAmount / 2).ToString("0.00")).CharacterFormat.FontSize = 8;
                //        TextAlign(table1, NextRowNumber, 7, (lst[0].OrderDetails[i].GSTAmount / 2).ToString("0.00"));
                //        //IGST
                //        //table1[NextRowNumber, 8].AddParagraph().AppendText(lst[0].OrderDetails[i].GSTAmount.ToString("0.00")).CharacterFormat.FontSize = 8;
                //        TextAlign(table1, NextRowNumber, 8, lst[0].OrderDetails[i].GSTAmount.ToString("0.00"));
                //        //Total
                //        //table1[NextRowNumber, 9].AddParagraph().AppendText(((lst[0].OrderDetails[i].Quantity * (lst[0].OrderDetails[i].SalePrice - Convert.ToDouble(lst[0].OrderDetails[i].AdditionalDiscountAmount))) + Convert.ToDouble(lst[0].OrderDetails[i].GSTAmount)).ToString("0.00")).CharacterFormat.FontSize = 8;
                //        TextAlign(table1, NextRowNumber, 9, ((lst[0].OrderDetails[i].Quantity * (lst[0].OrderDetails[i].SalePrice - Convert.ToDouble(lst[0].OrderDetails[i].AdditionalDiscountAmount))) + Convert.ToDouble(lst[0].OrderDetails[i].GSTAmount)).ToString("0.00"));
                //    }
                //}

                //int TotalQty = 0;
                //decimal TotalAdditionalDiscountAmount = 0;
                //decimal TotalAmount = 0;
                //double TotalCGSTAmount = 0;
                //double TotalSGSTAmount = 0;
                //double TotalIGSTAmount = 0;
                //for (int i = 0; i < lst[0].OrderDetails.Count; i++)
                //{
                //    TotalQty += lst[0].OrderDetails[i].Quantity;
                //    TotalAdditionalDiscountAmount += lst[0].OrderDetails[i].AdditionalDiscountAmount;
                //    TotalAmount += Convert.ToDecimal(lst[0].OrderDetails[i].SalePrice * lst[0].OrderDetails[i].Quantity) - lst[0].OrderDetails[i].AdditionalDiscountAmount;
                //    if (lst[0].ListUsers[0].IsPersonal == true)
                //    {
                //        TotalIGSTAmount += lst[0].OrderDetails[i].Quantity * ((lst[0].OrderDetails[i].SalePrice - Convert.ToDouble(lst[0].OrderDetails[i].AdditionalDiscountAmount)) * 18 / 100);
                //        TotalCGSTAmount += (lst[0].OrderDetails[i].Quantity * ((lst[0].OrderDetails[i].SalePrice - Convert.ToDouble(lst[0].OrderDetails[i].AdditionalDiscountAmount)) * 18 / 100)) / 2;
                //        TotalSGSTAmount += (lst[0].OrderDetails[i].Quantity * ((lst[0].OrderDetails[i].SalePrice - Convert.ToDouble(lst[0].OrderDetails[i].AdditionalDiscountAmount)) * 18 / 100)) / 2;
                //    }
                //    if (lst[0].ListUsers[0].IsPersonal == false)
                //    {
                //        TotalIGSTAmount += lst[0].OrderDetails[i].GSTAmount;
                //        TotalCGSTAmount += lst[0].OrderDetails[i].GSTAmount / 2;
                //        TotalSGSTAmount += lst[0].OrderDetails[i].GSTAmount / 2;
                //    }
                //}

                ////SubTotal
                //TableRow row1 = table1.AddRow(true, 10);
                //NextRowNumber = table1.Rows.Count - 1;
                //table1.Rows.Insert(NextRowNumber, row1);
                ////table1.AddRow(true, 8);
                ////Product
                //table1[NextRowNumber, 0].AddParagraph().AppendText("SubTotal").CharacterFormat.FontSize = 8;
                ////HSN Code + IGST
                //table1[NextRowNumber, 1].AddParagraph().AppendText("").CharacterFormat.FontSize = 8;
                ////Quantity
                ////table1[NextRowNumber, 2].AddParagraph().AppendText(TotalQty.ToString()).CharacterFormat.FontSize = 8;
                //TextAlign(table1, NextRowNumber, 2, TotalQty.ToString());
                ////Gross Amount
                //table1[NextRowNumber, 3].AddParagraph().AppendText("").CharacterFormat.FontSize = 8;
                ////Discount
                ////table1[NextRowNumber, 4].AddParagraph().AppendText(TotalAdditionalDiscountAmount.ToString("0.00")).CharacterFormat.FontSize = 8;
                //TextAlign(table1, NextRowNumber, 4, TotalAdditionalDiscountAmount.ToString("0.00"));
                //if (lst[0].ListUsers[0].IsPersonal == true)
                //{
                //    //Taxable Value
                //    //table1[NextRowNumber, 5].AddParagraph().AppendText((TotalAmount - Convert.ToDecimal(TotalIGSTAmount)).ToString("0.00")).CharacterFormat.FontSize = 8;
                //    TextAlign(table1, NextRowNumber, 5, (TotalAmount - Convert.ToDecimal(TotalIGSTAmount)).ToString("0.00"));
                //    //Total
                //    //table1[NextRowNumber, 9].AddParagraph().AppendText(TotalAmount.ToString("0.00")).CharacterFormat.FontSize = 8;
                //    TextAlign(table1, NextRowNumber, 9, TotalAmount.ToString("0.00"));
                //}
                ////CGST
                ////table1[NextRowNumber, 6].AddParagraph().AppendText(TotalCGSTAmount.ToString("0.00")).CharacterFormat.FontSize = 8;
                //TextAlign(table1, NextRowNumber, 6, TotalCGSTAmount.ToString("0.00"));
                ////SGST
                ////table1[NextRowNumber, 7].AddParagraph().AppendText(TotalSGSTAmount.ToString("0.00")).CharacterFormat.FontSize = 8;
                //TextAlign(table1, NextRowNumber, 7, TotalSGSTAmount.ToString("0.00"));
                ////IGST
                ////table1[NextRowNumber, 8].AddParagraph().AppendText(TotalIGSTAmount.ToString("0.00")).CharacterFormat.FontSize = 8;
                //TextAlign(table1, NextRowNumber, 8, TotalIGSTAmount.ToString("0.00"));
                //if (lst[0].ListUsers[0].IsPersonal == false)
                //{
                //    //Taxable Value
                //    //table1[NextRowNumber, 5].AddParagraph().AppendText(TotalAmount.ToString("0.00")).CharacterFormat.FontSize = 8;
                //    TextAlign(table1, NextRowNumber, 5, TotalAmount.ToString("0.00"));
                //    //Total
                //    //table1[NextRowNumber, 9].AddParagraph().AppendText((TotalAmount + Convert.ToDecimal(TotalIGSTAmount)).ToString("0.00")).CharacterFormat.FontSize = 8;
                //    TextAlign(table1, NextRowNumber, 9, (TotalAmount + Convert.ToDecimal(TotalIGSTAmount)).ToString("0.00"));
                //}

                //Total
                TableRow row2 = table1.AddRow(true, 13);
                NextRowNumber = table1.Rows.Count - 1;
                table1.Rows.Insert(NextRowNumber, row2);
                //table1.AddRow(true, 8);
                //Product
                table1[NextRowNumber, 0].AddParagraph().AppendText("Total").CharacterFormat.FontSize = 8;
                //HSN Code + IGST
                table1[NextRowNumber, 1].AddParagraph().AppendText("").CharacterFormat.FontSize = 8;
                //Quantity
                table1[NextRowNumber, 2].AddParagraph().AppendText("").CharacterFormat.FontSize = 8;
                //Gross Amount
                table1[NextRowNumber, 3].AddParagraph().AppendText("").CharacterFormat.FontSize = 8;
                //Discount
                table1[NextRowNumber, 4].AddParagraph().AppendText("").CharacterFormat.FontSize = 8;
                //Taxable Value
                table1[NextRowNumber, 5].AddParagraph().AppendText("").CharacterFormat.FontSize = 8;
                //CGST
                table1[NextRowNumber, 6].AddParagraph().AppendText("").CharacterFormat.FontSize = 8;
                //SGST
                table1[NextRowNumber, 7].AddParagraph().AppendText("").CharacterFormat.FontSize = 8;
                //IGST
                table1[NextRowNumber, 8].AddParagraph().AppendText("").CharacterFormat.FontSize = 8;
                //if (lst[0].ListUsers[0].IsPersonal == true)
                //{
                //    //Total
                //    //table1[NextRowNumber, 9].AddParagraph().AppendText(TotalAmount.ToString("0.00")).CharacterFormat.FontSize = 8;
                //    TextAlign(table1, NextRowNumber, 9, TotalAmount.ToString("0.00"));
                //}
                //if (lst[0].ListUsers[0].IsPersonal == false)
                //{
                //    //Total
                //    //table1[NextRowNumber, 9].AddParagraph().AppendText((TotalAmount + Convert.ToDecimal(TotalIGSTAmount)).ToString("0.00")).CharacterFormat.FontSize = 8;
                //    TextAlign(table1, NextRowNumber, 9, (TotalAmount + Convert.ToDecimal(TotalIGSTAmount)).ToString("0.00"));
                //}
                //for (int i = 0; i < table1.Rows.Count; i++)
                //{
                //    if (lst[0].State.ToLower() != "uttar pradesh")
                //    {
                //        table1.Rows[i].Cells.RemoveAt(7);
                //        table1.Rows[i].Cells.RemoveAt(6);
                //    }
                //    else
                //    {
                //        table1.Rows[i].Cells.RemoveAt(8);
                //    }
                //    if (lst[0].OrderDetails[0].AdditionalDiscountAmount == 0)
                //    {
                //        table1.Rows[i].Cells.RemoveAt(4);
                //    }
                //}

                Invoice_doc.SaveToFile(Temp_Path + Invoice_File + ".docx", FileFormat.Docx);
                //Convert Word to PDF
                Invoice_doc.SaveToFile(Temp_Path + Invoice_File + ".PDF", FileFormat.PDF);
                //System.IO.File.Delete(Temp_Path + Invoice_File + ".docx");
                return Invoice_File + ".PDF";

                //return Invoice_File;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside InvoiceController OrderInvoiceByGUID action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside InvoiceController OrderInvoiceByGUID action: {ex.Message}");
                return null;
            }
        }
        public void TextAlign(Table table1, int i, int rowNumber, string value)
        {
            Paragraph p = table1[i, rowNumber].AddParagraph();
            TextRange TR = p.AppendText(value);
            //Format Cells
            p.Format.HorizontalAlignment = HorizontalAlignment.Right;
            TR.CharacterFormat.FontSize = 8;
            table1[i, rowNumber].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
        }
    }
}
