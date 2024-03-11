using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingOrderCOAFilter.Model
{
    public class FormFormatModel
    {
        #region 屬性

        // 初始化出貨單內容清單
        public Dictionary<string, ShippingOrder_DB_Model> ShippingOrderList = new Dictionary<string, ShippingOrder_DB_Model>();
        // 使用 HashSet 追蹤已存在的 出貨單 + "," + Cust_PN
        public HashSet<string> ExistingKeysOfShippingOrderAndCustPN = new HashSet<string>();
        // 使用 HashSet 追蹤出貨單數量
        public HashSet<string>ShippingOrderNums = new HashSet<string>();
        // 初始化 COA 內容清單
        public Dictionary<string, COA_DB_Model> COA_List = new Dictionary<string, COA_DB_Model>();
        // 初始化 COA 本地端檔案清單
        public Dictionary<string, COA_LocalFile_Model> COA_LocalFile_List = new Dictionary<string, COA_LocalFile_Model>();
        // 初始化 SQL 相關 Model
        public SQL_Condition_Model SQL_Model = new SQL_Condition_Model();
        // 初始化輸出檔案清單
        public HashSet<string> OutputList = new HashSet<string>();
        // 初始化廠商清單
        public HashSet<string> CustomerList = new HashSet<string>();
        // 初始化 COA Lot 名稱與檔名對應清單
        public Dictionary<string, string> COA_Mapping_Lot_List = new Dictionary<string, string>();

        // Filter 項屬性值
        internal string Filter_ShippingOrder { get; set; } = "";
        internal string Filter_FilteredItem { get; set; } = "";
        internal string Filter_UnfilteredItem { get; set; } = "";

        // Check 項屬性值
        //提供 Form 的輸出格式
        internal string Check_ShippingOrder { get; set; } = "";
        internal string Check_FilteredItem { get; set; } = "";
        internal string Check_UnfilteredItem { get; set; } = "";

        // Mail 項屬性值
        internal string[] MailList { get; set; } = new string[] { "MyMail@Name.com",
                                                 };
        internal string Mail_CSS_Format_Header { get; set; } = @"<br>
                                                        <html>
                                                            <head>
                                                                <style>
                                                                    #customers { font-family: ""Microsoft JhengHei UI"", Arial; border-collapse: collapse; width: 100%;} /* 字型和欄位寬 */
                                                                    #customers td, #customers th { border: 2px solid #000000; padding: 8px; width: auto;} /* 表格格線設定 */
                                                                    #customers th { padding-top: 12px; padding-bottom: 12px; text-align: left; background-color: #808080; color: white;}
                                                                    /* ↑上下間距與文字方向 */
                                                                </style>
                                                            </head>
                                                            <body>
                                                                <div style = overflow - x:auto;> 
                                                                <table id=customers>
                                                                    <tr><th>Customer</th><th>Pass/Fail</th><th>DN No</th><th>Psi PN</th><th>Customer PN</th><th>Customer Lot No</th><th>Psi Lot No</th><th>Qty</th><th>Wafer ID</th></tr> 
                                                                    <!--↑標題設定幾行-->";
        internal string Mail_CSS_Format_Footer { get; set; } = @"</table>
                                                                </div></body></html>";

        #endregion

        #region 方法

        internal string FilterResult()
        {
            return $"篩選結果：\r\n出貨單： {Filter_ShippingOrder} 張\r\n已篩選： {Filter_FilteredItem} lot\r\n" +
                $"未篩選： {Filter_UnfilteredItem} lot\r\n";
        }
        internal string CheckResult()
        {
            return $"核對結果：\r\nCOA： {Check_ShippingOrder} lot\r\nPass： {Check_FilteredItem} lot\r\n" +
                $"Fail： {Check_UnfilteredItem} lot\r\n";
        }
        internal string InitialFilterTextBox()
        {
            return $"篩選結果：\r\n出貨單：\r\n已篩選：\r\n未篩選：\r\n";
        }
        internal string InitialCheckTextBox()
        {
            return $"核對結果：\r\nCOA：\r\nPass：\r\nFail：\r\n";
        }

        #endregion
    }
}
