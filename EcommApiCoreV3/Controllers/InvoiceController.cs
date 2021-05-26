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
        ConvertInToWord _convertInToWord = new ConvertInToWord();
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
                //string Invoice_Template = webRootPath + "\\Template\\GSTInvoice.docx";
                string Invoice_Template = webRootPath + (lst[0].State.ToLower() == "delhi" ? "\\Template\\LocalGSTInvoice.docx" : "\\Template\\CentralGSTInvoice.docx");
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
                Invoice_doc.Replace("[MobileNo]", lst[0].Phone, false, true);

                //Invoice_doc.Replace("[CompanyName]", "Company Name: " + lst[0].CompanyName, false, true);
                if (lst[0].ListUsers[0].BusinessLicenseType == "GSTIN")
                    Invoice_doc.Replace("[GSTNo]", lst[0].ListUsers[0].GSTNo, false, true);
                else
                    Invoice_doc.Replace("[GSTNo]", "", false, true);
                if (lst[0].ListUsers[0].BusinessLicenseType == "BusinessPAN")
                    Invoice_doc.Replace("[PANNo]", lst[0].ListUsers[0].GSTNo, false, true);
                else
                    Invoice_doc.Replace("[PANNo]", "", false, true);
                if (lst[0].ListUsers[0].BusinessLicenseType == "AadharCard")
                    Invoice_doc.Replace("[AadharCard]", lst[0].ListUsers[0].AadharCard, false, true);
                else
                    Invoice_doc.Replace("[AadharCard]", "", false, true);

                Table table1 = Invoice_doc.Sections[0].Tables[0] as Spire.Doc.Table;
                int NextRowNumber = 0;

                int TotalQty = 0;
                double TotalDiscountAmount = 0;
                double TotalAmountWithoutGST = 0;
                double TotalAmount = 0;
                double TotalCGSTAmount = 0;
                double TotalSGSTAmount = 0;
                double TotalIGSTAmount = 0;
                for (int i = 0; i < lst[0].OrderDetails.Count; i++)
                {
                    TotalQty += lst[0].OrderDetails[i].Quantity;
                    TotalDiscountAmount += lst[0].OrderDetails[i].SalePrice - Convert.ToDouble(lst[0].OrderDetails[i].AdditionalDiscountAmount);
                    TotalAmountWithoutGST += lst[0].OrderDetails[i].Quantity * TotalDiscountAmount;
                    TotalAmount += lst[0].OrderDetails[i].Quantity * TotalDiscountAmount + lst[0].OrderDetails[i].GSTAmount;

                    TotalIGSTAmount += lst[0].OrderDetails[i].GSTAmount;
                    TotalCGSTAmount += lst[0].OrderDetails[i].GSTAmount / 2;
                    TotalSGSTAmount += lst[0].OrderDetails[i].GSTAmount / 2;

                }

                Invoice_doc.Replace("[RoundOff]", Convert.ToDecimal("0." + TotalAmount.ToString("0.00").Split('.')[1]).ToString("0.00"), false, true);
                Invoice_doc.Replace("[TotalAmount]", Convert.ToInt32(TotalAmount).ToString("0.00"), false, true);
                Invoice_doc.Replace("[RupessInWord]", _convertInToWord.ConvertToWords(Convert.ToInt32(TotalAmount).ToString("0.00")), false, true);

                if (lst[0].State.ToLower() == "delhi")
                {
                    for (int i = 0; i < lst[0].OrderDetails.Count; i++)
                    {
                        NextRowNumber = i + 1;
                        TableRow row = table1.AddRow(false, 13);
                        table1.Rows.Insert(NextRowNumber, row);

                        //Sr.No
                        table1[NextRowNumber, 0].AddParagraph().AppendText((i + 1).ToString()).CharacterFormat.FontSize = 8;

                        //Product
                        table1[NextRowNumber, 1].AddParagraph().AppendText(lst[0].OrderDetails[i].ProductName).CharacterFormat.FontSize = 8;

                        //HSN Code
                        table1[NextRowNumber, 2].AddParagraph().AppendText(lst[0].OrderDetails[i].HSNCode).CharacterFormat.FontSize = 8;
                        table1[NextRowNumber, 2].CellFormat.VerticalAlignment = VerticalAlignment.Middle;

                        //Quantity
                        TextAlign(table1, NextRowNumber, 3, lst[0].OrderDetails[i].Quantity.ToString("0.00"), false);

                        // PCS
                        TextAlign(table1, NextRowNumber, 4, "PCS", false);

                        //Rate
                        TextAlign(table1, NextRowNumber, 5, lst[0].OrderDetails[i].SalePrice.ToString("0.00"), false);

                        //Discount
                        TextAlign(table1, NextRowNumber, 6, lst[0].OrderDetails[i].AdditionalDiscount.ToString("0.00"), false);

                        //Discount Rate
                        double DiscountedRate = (lst[0].OrderDetails[i].SalePrice - Convert.ToDouble(lst[0].OrderDetails[i].AdditionalDiscountAmount));
                        TextAlign(table1, NextRowNumber, 7, DiscountedRate.ToString("0.00"), false);

                        //Amount (Wihtout GST)
                        //double GST = lst[0].OrderDetails[i].Quantity * ((lst[0].OrderDetails[i].SalePrice - Convert.ToDouble(lst[0].OrderDetails[i].AdditionalDiscountAmount)) * lst[0].OrderDetails[i].GSTRate / 100);
                        TextAlign(table1, NextRowNumber, 8, ((lst[0].OrderDetails[i].Quantity * DiscountedRate) - lst[0].OrderDetails[i].GSTAmount).ToString("0.00"), false);

                        //GST Rate in %
                        TextAlign(table1, NextRowNumber, 9, (lst[0].OrderDetails[i].GSTRate).ToString("0.00"), false);

                        //CGST
                        TextAlign(table1, NextRowNumber, 10, (lst[0].OrderDetails[i].GSTAmount / 2).ToString("0.00"), false);

                        //SGST
                        TextAlign(table1, NextRowNumber, 11, (lst[0].OrderDetails[i].GSTAmount / 2).ToString("0.00"), false);

                        ////IGST
                        //TextAlign(table1, NextRowNumber, 12, lst[0].OrderDetails[i].GSTAmount.ToString("0.00"), false);

                        //Total
                        TextAlign(table1, NextRowNumber, 12, ((lst[0].OrderDetails[i].Quantity * DiscountedRate) + lst[0].OrderDetails[i].GSTAmount).ToString("0.00"), false);
                    }

                    //SubTotal
                    TableRow row1 = table1.AddRow(true, 13);
                    NextRowNumber = table1.Rows.Count - 1;
                    table1.Rows.Insert(NextRowNumber, row1);
                    //table1.AddRow(true, 8);

                    //SR.No
                    //table1[NextRowNumber, 0].AddParagraph().AppendText("Total").CharacterFormat.FontSize = 8;
                    TextAlign(table1, NextRowNumber, 0, "Total", true);

                    //Product Name
                    table1[NextRowNumber, 1].AddParagraph().AppendText("").CharacterFormat.FontSize = 8;

                    //HSN Code
                    table1[NextRowNumber, 2].AddParagraph().AppendText("").CharacterFormat.FontSize = 8;

                    //Quantity
                    TextAlign(table1, NextRowNumber, 3, TotalQty.ToString("0.00"), true);

                    //Unit
                    table1[NextRowNumber, 4].AddParagraph().AppendText("").CharacterFormat.FontSize = 8;

                    //Rate
                    table1[NextRowNumber, 5].AddParagraph().AppendText("").CharacterFormat.FontSize = 8;

                    //Discount
                    table1[NextRowNumber, 6].AddParagraph().AppendText("").CharacterFormat.FontSize = 8;

                    //Discounted Rate
                    TextAlign(table1, NextRowNumber, 7, TotalDiscountAmount.ToString("0.00"), true);

                    //Amount (Wihtout GST)
                    TextAlign(table1, NextRowNumber, 8, TotalAmountWithoutGST.ToString("0.00"), true);

                    //GST Rate in %
                    table1[NextRowNumber, 9].AddParagraph().AppendText("").CharacterFormat.FontSize = 8;

                    //CGST
                    TextAlign(table1, NextRowNumber, 10, TotalCGSTAmount.ToString("0.00"), true);

                    //SGST
                    TextAlign(table1, NextRowNumber, 11, TotalSGSTAmount.ToString("0.00"), true);

                    ////IGST
                    //TextAlign(table1, NextRowNumber, 12, TotalIGSTAmount.ToString("0.00"), true);

                    //Total
                    TextAlign(table1, NextRowNumber, 12, TotalAmount.ToString("0.00"), true);


                    // GroupBy GSTRate
                    Table GST_table = Invoice_doc.Sections[0].Tables[1] as Spire.Doc.Table;
                    Table nestedTable = GST_table[0, 1].AddTable(true);
                    nestedTable.AddRow(2);
                    //nestedTable.ResetCells(2, 2);
                    //nestedTable.AutoFitBehavior(AutoFitBehaviorType.wdAutoFitContents);
                    for (int i = 0; i < lst[0].OrderGSTGroup.Count; i++)
                    {
                        TextAlign(nestedTable, nestedTable.Rows.Count - 1, 0, "SGST " + (lst[0].OrderGSTGroup[i].GSTRate/2).ToString("0.00") + "%", true);
                        TextAlign(nestedTable, nestedTable.Rows.Count - 1, 1, (lst[0].OrderGSTGroup[i].GSTAmount/2).ToString("0.00"), true);
                        nestedTable.AddRow(true, 2);
                        TextAlign(nestedTable, nestedTable.Rows.Count - 1, 0, "CGST " + (lst[0].OrderGSTGroup[i].GSTRate / 2).ToString("0.00") + "%", true);
                        TextAlign(nestedTable, nestedTable.Rows.Count - 1, 1, (lst[0].OrderGSTGroup[i].GSTAmount / 2).ToString("0.00"), true);
                        nestedTable.AddRow(true, 2);
                    }

                    // Total
                    TextAlign(nestedTable, nestedTable.Rows.Count - 1, 0, "Total", true);
                    TextAlign(nestedTable, nestedTable.Rows.Count - 1, 1, TotalIGSTAmount.ToString("0.00"), true);

                    // GroupBy HSN and GSTRate
                    Table HSNGST_table = Invoice_doc.Sections[0].Tables[2] as Spire.Doc.Table;
                    for (int j = 0; j < lst[0].OrderHSNGroup.Count; j++)
                    {
                        TableRow row_HSN = HSNGST_table.AddRow(false, 8);
                        NextRowNumber = HSNGST_table.Rows.Count - 1;
                        HSNGST_table.Rows.Insert(NextRowNumber, row_HSN);

                        //HSN
                        TextAlign(HSNGST_table, NextRowNumber, 0, lst[0].OrderHSNGroup[j].HSNCode, false);

                        //Qty
                        TextAlign(HSNGST_table, NextRowNumber, 1, lst[0].OrderHSNGroup[j].Quantity.ToString("0.00"), false);

                        //Taxable Value
                        double TaxableValue = Convert.ToDouble(lst[0].OrderHSNGroup[j].Quantity * (lst[0].OrderHSNGroup[j].SalePrice - Convert.ToDouble(lst[0].OrderHSNGroup[j].AdditionalDiscountAmount)));
                        TextAlign(HSNGST_table, NextRowNumber, 2, TaxableValue.ToString("0.00"), false);

                        //Central Rate
                        TextAlign(HSNGST_table, NextRowNumber, 3, (lst[0].OrderHSNGroup[j].GSTRate / 2).ToString("0.00"), false);

                        //Central Amount
                        TextAlign(HSNGST_table, NextRowNumber, 4, (lst[0].OrderHSNGroup[j].GSTAmount / 2).ToString("0.00"), false);

                        //State Rate
                        TextAlign(HSNGST_table, NextRowNumber, 5, (lst[0].OrderHSNGroup[j].GSTRate / 2).ToString("0.00"), false);

                        //State Amount
                        TextAlign(HSNGST_table, NextRowNumber, 6, (lst[0].OrderHSNGroup[j].GSTAmount / 2).ToString("0.00"), false);

                        //Tax Amount
                        TextAlign(HSNGST_table, NextRowNumber, 7, lst[0].OrderHSNGroup[j].GSTAmount.ToString("0.00"), false);
                    }

                    // Total
                    TableRow row_HSN1 = HSNGST_table.AddRow(true, 8);
                    NextRowNumber = HSNGST_table.Rows.Count - 1;
                    HSNGST_table.Rows.Insert(NextRowNumber, row_HSN1);

                    //HSN
                    TextAlign(HSNGST_table, NextRowNumber, 0, "Total", true);

                    //Qty
                    TextAlign(HSNGST_table, NextRowNumber, 1, TotalQty.ToString("0.00"), true);

                    //Taxable Value
                    TextAlign(HSNGST_table, NextRowNumber, 2, TotalAmountWithoutGST.ToString("0.00"), true);

                    //Central Rate
                    TextAlign(HSNGST_table, NextRowNumber, 3, "", true);

                    //Central Amount
                    TextAlign(HSNGST_table, NextRowNumber, 4, (TotalIGSTAmount / 2).ToString("0.00"), true);

                    //State Rate
                    TextAlign(HSNGST_table, NextRowNumber, 5, "", true);

                    //State Amount
                    TextAlign(HSNGST_table, NextRowNumber, 6, (TotalIGSTAmount / 2).ToString("0.00"), true);

                    //Tax Amount
                    TextAlign(HSNGST_table, NextRowNumber, 7, TotalIGSTAmount.ToString("0.00"), true);
                }
                else
                {
                    for (int i = 0; i < lst[0].OrderDetails.Count; i++)
                    {
                        NextRowNumber = i + 1;
                        TableRow row = table1.AddRow(false, 12);
                        table1.Rows.Insert(NextRowNumber, row);

                        //Sr.No
                        table1[NextRowNumber, 0].AddParagraph().AppendText((i + 1).ToString()).CharacterFormat.FontSize = 8;

                        //Product
                        table1[NextRowNumber, 1].AddParagraph().AppendText(lst[0].OrderDetails[i].ProductName).CharacterFormat.FontSize = 8;

                        //HSN Code
                        table1[NextRowNumber, 2].AddParagraph().AppendText(lst[0].OrderDetails[i].HSNCode).CharacterFormat.FontSize = 8;
                        table1[NextRowNumber, 2].CellFormat.VerticalAlignment = VerticalAlignment.Middle;

                        //Quantity
                        TextAlign(table1, NextRowNumber, 3, lst[0].OrderDetails[i].Quantity.ToString("0.00"), false);

                        // PCS
                        TextAlign(table1, NextRowNumber, 4, "PCS", false);

                        //Rate
                        TextAlign(table1, NextRowNumber, 5, lst[0].OrderDetails[i].SalePrice.ToString("0.00"), false);

                        //Discount
                        TextAlign(table1, NextRowNumber, 6, lst[0].OrderDetails[i].AdditionalDiscount.ToString("0.00"), false);

                        //Discount Rate
                        double DiscountedRate = (lst[0].OrderDetails[i].SalePrice - Convert.ToDouble(lst[0].OrderDetails[i].AdditionalDiscountAmount));
                        TextAlign(table1, NextRowNumber, 7, DiscountedRate.ToString("0.00"), false);

                        //Amount (Wihtout GST)
                        //double GST = lst[0].OrderDetails[i].Quantity * ((lst[0].OrderDetails[i].SalePrice - Convert.ToDouble(lst[0].OrderDetails[i].AdditionalDiscountAmount)) * lst[0].OrderDetails[i].GSTRate / 100);
                        TextAlign(table1, NextRowNumber, 8, ((lst[0].OrderDetails[i].Quantity * DiscountedRate) - lst[0].OrderDetails[i].GSTAmount).ToString("0.00"), false);

                        //GST Rate in %
                        TextAlign(table1, NextRowNumber, 9, (lst[0].OrderDetails[i].GSTRate).ToString("0.00"), false);

                        //IGST
                        TextAlign(table1, NextRowNumber, 10, lst[0].OrderDetails[i].GSTAmount.ToString("0.00"), false);

                        //Total
                        TextAlign(table1, NextRowNumber, 11, ((lst[0].OrderDetails[i].Quantity * DiscountedRate) + lst[0].OrderDetails[i].GSTAmount).ToString("0.00"), false);
                    }

                    //SubTotal
                    TableRow row1 = table1.AddRow(true, 12);
                    NextRowNumber = table1.Rows.Count - 1;
                    table1.Rows.Insert(NextRowNumber, row1);
                    //table1.AddRow(true, 8);

                    //SR.No
                    //table1[NextRowNumber, 0].AddParagraph().AppendText("Total").CharacterFormat.FontSize = 8;
                    TextAlign(table1, NextRowNumber, 0, "Total", true);

                    //Product Name
                    table1[NextRowNumber, 1].AddParagraph().AppendText("").CharacterFormat.FontSize = 8;

                    //HSN Code
                    table1[NextRowNumber, 2].AddParagraph().AppendText("").CharacterFormat.FontSize = 8;

                    //Quantity
                    TextAlign(table1, NextRowNumber, 3, TotalQty.ToString("0.00"), true);

                    //Unit
                    table1[NextRowNumber, 4].AddParagraph().AppendText("").CharacterFormat.FontSize = 8;

                    //Rate
                    table1[NextRowNumber, 5].AddParagraph().AppendText("").CharacterFormat.FontSize = 8;

                    //Discount
                    table1[NextRowNumber, 6].AddParagraph().AppendText("").CharacterFormat.FontSize = 8;

                    //Discounted Rate
                    TextAlign(table1, NextRowNumber, 7, TotalDiscountAmount.ToString("0.00"), true);

                    //Amount (Wihtout GST)
                    TextAlign(table1, NextRowNumber, 8, TotalAmountWithoutGST.ToString("0.00"), true);

                    //GST Rate in %
                    table1[NextRowNumber, 9].AddParagraph().AppendText("").CharacterFormat.FontSize = 8;

                    //IGST
                    TextAlign(table1, NextRowNumber, 10, TotalIGSTAmount.ToString("0.00"), true);

                    //Total
                    TextAlign(table1, NextRowNumber, 11, TotalAmount.ToString("0.00"), true);


                    // GroupBy GSTRate
                    Table GST_table = Invoice_doc.Sections[0].Tables[1] as Spire.Doc.Table;
                    Table nestedTable = GST_table[0, 1].AddTable(true);
                    nestedTable.AddRow(2);
                    //nestedTable.ResetCells(2, 2);
                    //nestedTable.AutoFitBehavior(AutoFitBehaviorType.wdAutoFitContents);
                    for (int i = 0; i < lst[0].OrderGSTGroup.Count; i++)
                    {
                        TextAlign(nestedTable, i, 0, "IGST " + lst[0].OrderGSTGroup[i].GSTRate.ToString("0.00") + "%", true);
                        TextAlign(nestedTable, i, 1, lst[0].OrderGSTGroup[i].GSTAmount.ToString("0.00"), true);
                        nestedTable.AddRow(true, 2);
                    }

                    // Total
                    TextAlign(nestedTable, nestedTable.Rows.Count - 1, 0, "Total", true);
                    TextAlign(nestedTable, nestedTable.Rows.Count - 1, 1, TotalIGSTAmount.ToString("0.00"), true);

                    // GroupBy HSN and GSTRate
                    Table HSNGST_table = Invoice_doc.Sections[0].Tables[2] as Spire.Doc.Table;
                    for (int j = 0; j < lst[0].OrderHSNGroup.Count; j++)
                    {
                        TableRow row_HSN = HSNGST_table.AddRow(false, 6);
                        NextRowNumber = HSNGST_table.Rows.Count - 1;
                        HSNGST_table.Rows.Insert(NextRowNumber, row_HSN);

                        //HSN
                        TextAlign(HSNGST_table, NextRowNumber, 0, lst[0].OrderHSNGroup[j].HSNCode, false);

                        //Qty
                        TextAlign(HSNGST_table, NextRowNumber, 1, lst[0].OrderHSNGroup[j].Quantity.ToString("0.00"), false);

                        //Taxable Value
                        double TaxableValue = Convert.ToDouble(lst[0].OrderHSNGroup[j].Quantity * (lst[0].OrderHSNGroup[j].SalePrice - Convert.ToDouble(lst[0].OrderHSNGroup[j].AdditionalDiscountAmount)));
                        TextAlign(HSNGST_table, NextRowNumber, 2, TaxableValue.ToString("0.00"), false);

                        //Central Rate
                        TextAlign(HSNGST_table, NextRowNumber, 3, lst[0].OrderHSNGroup[j].GSTRate.ToString("0.00"), false);

                        //Central Amount
                        TextAlign(HSNGST_table, NextRowNumber, 4, lst[0].OrderHSNGroup[j].GSTAmount.ToString("0.00"), false);

                        //Tax Amount
                        TextAlign(HSNGST_table, NextRowNumber, 5, lst[0].OrderHSNGroup[j].GSTAmount.ToString("0.00"), false);
                    }

                    // Total
                    TableRow row_HSN1 = HSNGST_table.AddRow(true, 6);
                    NextRowNumber = HSNGST_table.Rows.Count - 1;
                    HSNGST_table.Rows.Insert(NextRowNumber, row_HSN1);

                    //HSN
                    TextAlign(HSNGST_table, NextRowNumber, 0, "Total", true);

                    //Qty
                    TextAlign(HSNGST_table, NextRowNumber, 1, TotalQty.ToString("0.00"), true);

                    //Taxable Value
                    TextAlign(HSNGST_table, NextRowNumber, 2, TotalAmountWithoutGST.ToString("0.00"), true);

                    //Integrated  Rate
                    TextAlign(HSNGST_table, NextRowNumber, 3, "", true);

                    //Integrated  Amount
                    TextAlign(HSNGST_table, NextRowNumber, 4, TotalIGSTAmount.ToString("0.00"), true);

                    //Tax Amount
                    TextAlign(HSNGST_table, NextRowNumber, 5, TotalIGSTAmount.ToString("0.00"), true);
                }



                Invoice_doc.SaveToFile(Temp_Path + Invoice_File + ".docx", FileFormat.Docx);
                //Convert Word to PDF
                Invoice_doc.SaveToFile(Temp_Path + Invoice_File + ".PDF", FileFormat.PDF);

                System.IO.File.Delete(Temp_Path + Invoice_File + ".docx");
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
        public void TextAlign(Table table1, int i, int rowNumber, string value, bool IsBold)
        {
            Paragraph p = table1[i, rowNumber].AddParagraph();
            TextRange TR = p.AppendText(value);
            //Format Cells
            p.Format.HorizontalAlignment = HorizontalAlignment.Right;
            TR.CharacterFormat.FontSize = 8;
            TR.CharacterFormat.Bold = IsBold;
            table1[i, rowNumber].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
        }
    }
}
