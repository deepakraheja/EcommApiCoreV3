using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcommApiCoreV3.Entities;

namespace EcommApiCoreV3.Controllers.Common
{
    public static class ReportTemplate
    {
        public static string OrderDetailByFilterTemplate(List<Order> lst)
        {
            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>Order Details Report</h1></div>
                                <table align='center'>
                                    <tr>
                                        <th>OrderNumber</th>
                                        <th>OrderDate</th>
                                        <th>Name</th>
                                        <th>MobileNo</th>
                                    </tr>");
            foreach (var lstOrder in lst)
            {
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                  </tr>", lstOrder.OrderNumber, lstOrder.OrderDate, lstOrder.FName + " " + lstOrder.LName, lstOrder.Phone);
            }
            sb.Append(@"
                                </table>
                            </body>
                        </html>");
            return sb.ToString();
        }

        public static string OrderInvoiceTemplate(List<Order> lst)
        {
            
           string Orderdate = DateTime.Parse(lst[0].OrderDate, new System.Globalization.CultureInfo("en-US")).ToString("MMM dd, yyyy");

            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>Invoice Report</h1></div>
                                    <div class='order-summary mb-5'>
                                        <div class='row'>
                                            <div class='col-md-6 mb-3'>
                                                <h4>summery</h4>
                                                <ul class='order-detail'>
                                                    <li><span>order ID:</span>" + lst[0].OrderNumber + @"</li>
                                                    <li><span>Order Date:</span>" + Orderdate + @"</li>
                                                    <li><span>Order Total:</span>"
                                                       + lst[0].TotalAmount +
                                                     @"</li>
                                                </ul>
                                            </div>
                                            <div class='col-md-6 mb-3'>
                                                <h4>shipping address</h4>
                                                <ul class='order-detail'>
                                                    <li>" + lst[0].Address + @", " + lst[0].City + @"</li>
                                                    <li>" + lst[0].State + @"</li>
                                                    <li>" + lst[0].Country + @", " + lst[0].ZipCode + @"</li>
                                                    <li>Contact No. " + lst[0].Phone + @" </li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                    <h4 class='mb-4'>Your Order Details</h4>
                                    <div class='order-box'>
                                        <div class='table-responsive-sm'>
                                            <table class='table table-bordered table-hover'>
                                                <thead class='thead-dark'>
                                                    <tr>
                                                        <th class='thead-dark'>
                                                            Product
                                                        </th>
                                                        <th class='thead-dark'>
                                                            Price
                                                        </th>
                                                        <th class='thead-dark'>
                                                            Qty
                                                        </th>
                                                        <th class='thead-dark'>
                                                            GST Rate(%)
                                                        </th>
                                                        <th class='thead-dark'>
                                                            GST Amount
                                                        </th>
                                                        <th class='thead-dark'>
                                                            Total
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>");
            foreach (var lstOrder in lst[0].OrderDetails)
            {
                sb.AppendFormat(@"<tr>
                                    <td>
                                        {0}
                                    </td>
                                    <td>
                                        {1}
                                    </td>
                                    <td>
                                        {2}
                                    </td>
                                    <td>
                                        {3}%
                                    </td>
                                    <td>
                                        {4}
                                    </td>
                                    <td>
                                        <span
                                            style='float: right; font-size: 16px; line-height: 20px;  font-weight: 400;'>
                                            {5}</span>
                                    </td>
                                </tr>", lstOrder.ProductName, lstOrder.Price, lstOrder.Quantity, lstOrder.GSTRate, lstOrder.GSTAmount, ((lstOrder.Price * lstOrder.Quantity) + lstOrder.GSTAmount));
            }

            double TotalGSTAmount = 0;
            foreach (var item in lst[0].OrderDetails)
            {
                TotalGSTAmount += item.GSTAmount;
            }

            double TotalAmount = 0;
            foreach (var element in lst[0].OrderDetails)
            {
                TotalAmount += (element.Price * element.Quantity) + element.GSTAmount;
            }

            sb.AppendFormat(@"<tr>
                                    <td colspan='4'>
                                        <b>Subtotal</b>
                                    </td>
                                    <td>
                                        <b>{0}</b>
                                    </td>
                                    <td>
                                        <span
                                            style='float: right; font-size: 16px; line-height: 20px; color: var(--theme-deafult); font-weight: 400;'>
                                            {1}</span>
                                    </td>
                                </tr>", TotalGSTAmount, TotalAmount);

            sb.AppendFormat(@"<tr>
                                    <td colspan='4'>
                                        <b> Packing</b>
                                    </td>
                                    <td colspan='2'>
                                        <span
                                            style='float: right; font-size: 16px; line-height: 20px; color: var(--theme-deafult); font-weight: 400;'>
                                            <b>Free Packing</b></span>
                                    </td>
                                </tr>
                                <tr class='total'>
                                    <td colspan='5'>
                                        <b>Total</b>
                                    </td>
                                    <td>
                                        <span
                                            style='float: right; font-size: 16px; line-height: 20px; color: var(--theme-deafult); font-weight: 400;'>
                                            {0}</span>
                                    </td>
                                </tr>", TotalAmount);

            sb.Append(@"
                                </tbody>
                        </table>
                    </div>
                </div> 
                            </body>
                        </html>");
            return sb.ToString();
        }
    }
}
