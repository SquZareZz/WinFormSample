using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormSample.Model
{
    //找 COA DB 相關的 Model
    public class COA_DB_Model
    {
        // PRODUCTNO = Psi PN
        // CUST_PN = CUST_PN = Customer PN
        // CustomerLotNO = CUSTOMERLOTNO
        // LOTNO = Psi Lot No
        // QTY = PASSWAFERNUM
        // SliceNo = PASSSLOTS
        public string PRODUCTNO { get; set; } = "";
        public string CUST_PN { get; set; } = "";
        public string CUSTOMERLOTNO { get; set; } = "";
        public string LOTNO { get; set; } = "";
        public string PASSWAFERNUM { get; set; } = "";
        public string PASSSLOTS { get; set; } = "";
        public string CUSTOMERENAME { get; set; } = "";

    }
}
