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
    }
}
