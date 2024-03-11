using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormSample.Model
{
    //出貨單相關的 Model
    public class ShippingOrder_DB_Model
    {
        public string DN_NO { get; set; } = "";
        public string Psi_PN { get; set; } = "";
        public string CUST_PN { get; set; } = "";             
        public string CustomerLotNO { get; set; } = "";
        public string Psi_Lot_No { get; set; } = "";
        public string QTY { get; set; } = "";
        public string SliceNo { get; set; } = "";
        public string CustomerCode { get; set; } = "";
    }
    
}
