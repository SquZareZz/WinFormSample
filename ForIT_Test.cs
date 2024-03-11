using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;


namespace ShippingOrderCOAFilter
{
    internal class ForIT_Test
    {
        //測試單元用的程式碼
        internal void GeneratePDF(string CustomerName, string Cust_PN, string CustomerLotNo)
        {
            var pdfFilePath = Path.Combine("D:\\",
                $"{CustomerName}.{Cust_PN}.{CustomerLotNo}.pdf");
            File.Create(pdfFilePath);
        }
    }
}
