using Dapper;
using Oracle.ManagedDataAccess.Client;
using ShippingOrderCOAFilter.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ShippingOrderCOAFilter.Controller
{
    // 負責處理所有主程式的邏輯控制，以 MVC 結構來說是它的 C(Controller)
    public class MainFormHandler
    {
        // 有任何變數操作 => 調整 FormModel，傳回 View
        /// <summary>
        /// 搜尋出貨單
        /// </summary>
        /// <returns></returns>
        public FormFormatModel ScanShipoutRecipt(FormFormatModel FormModel, string ShippingOrderNo, Form1 Form1Instance)
        {
            try
            {
                MessageBox.Show("此處會用到 DB 查詢，透過 Dapper 套件將查詢內容映射到方法模型(Class)\r\n" +
                    "SearchRes = connection.Query<ShippingOrder_DB_Model> \r\n" +
                    "DEMO 版以固定模型內容表示", "DEMO 版事例");
                var SearchRes = new List<ShippingOrder_DB_Model>();
                string sqlQuery = FormModel.SQL_Model.ShippingOrder_DB_SQL_Condition();
                // 使用 Dapper 執行查詢，將結果映射到 Model 類型的 List
                // 沒查到 => 回上一步重來 ； 有查到 => 輸入進 Hashset
                Form1Instance.UpdateLoggerInfoDelegate.Invoke("info", "查找資料庫");
                SearchRes.Add(new ShippingOrder_DB_Model()
                {
                    DN_NO = "123",
                    Psi_PN = "SAMPLE",
                    CUST_PN = "SAMPLE_CUST_PN",
                    CustomerLotNO = "SAMPLE_LotNO",
                    Psi_Lot_No = "SAMPLE_LotNO2",
                    QTY = "25",
                    SliceNo = "1-25",
                    CustomerCode = "SAMPLE_Code"
                });
                if (SearchRes.Count == 0)
                {
                    MessageBox.Show("找不到資料庫上的需求單號，請確認輸入正確", "錯誤需求單號");
                    Form1Instance.UpdateLoggerInfoDelegate.Invoke("warn", "找不到資料庫上的需求單號，請確認輸入正確");
                    Form1Instance.UpdateShippingOrderNoTextBoxDelegate.Invoke("");
                    return FormModel;
                }
                Form1Instance.UpdateLoggerInfoDelegate.Invoke("info", "整理片號資訊");
                foreach (var LineData in SearchRes)
                {
                    if (FormModel.ShippingOrderList.ContainsKey(ShippingOrderNo + "," + LineData.CustomerLotNO))
                    {
                        // 查到的資料全部轉大寫
                        var FormatLineData = LotValueToUpper(LineData);
                        FormModel = UpdateSliceNo(FormatLineData, ShippingOrderNo, FormModel, Form1Instance);
                    }
                    else
                    {
                        // 查到的資料全部轉大寫
                        var FormatLineData = LotValueToUpper(LineData);
                        // Cust_PN、片數、各片清單
                        FormModel.ShippingOrderList[ShippingOrderNo + "," + FormatLineData.CustomerLotNO] = FormatLineData;
                    }
                }
                Form1Instance.UpdateLoggerInfoDelegate.Invoke("info", "資訊上傳到 listView");
                MessageBox.Show("此處會確認資料是否重複，避免用戶輸入重複出貨單\r\n", "DEMO 版事例");
                foreach (var ResultItem in FormModel.ShippingOrderList.Values)
                {
                    //如果資料已經存在
                    if (FormModel.ExistingKeysOfShippingOrderAndCustPN.Contains(ResultItem.DN_NO + "," + ResultItem.CustomerLotNO))
                    {
                        foreach (ListViewItem tempListViewItem in Form1Instance.ReturnListView1Items())
                        {
                            if (tempListViewItem.Tag != null && tempListViewItem.Tag.ToString() == ShippingOrderNo + "," + ResultItem.CustomerLotNO)
                            {
                                Form1Instance.RefreshListViewInfoDelegate(tempListViewItem,
                                    new[] { ShippingOrderNo, ResultItem.CustomerLotNO,ResultItem.QTY,
                                        ResultItem.SliceNo,ResultItem.CustomerCode});
                                break;
                            }
                        }
                    }
                    //如果資料不存在
                    else
                    {
                        ListViewItem item = new ListViewItem(new[] { ShippingOrderNo, ResultItem.CustomerLotNO,
                                ResultItem.QTY, ResultItem.SliceNo,ResultItem.CustomerCode}); // 添加項目
                                                                                              //Tag 要有出貨單號和 LotID
                        item.Tag = ShippingOrderNo + "," + ResultItem.CustomerLotNO;

                        // 將整個項目添加到ListView中
                        Form1Instance.UpdateListViewInfoDelegate(item);
                        FormModel.ExistingKeysOfShippingOrderAndCustPN.Add(ShippingOrderNo + "," + ResultItem.CustomerLotNO);
                    }

                }
                FormModel.ShippingOrderNums.Add(ShippingOrderNo);
                return FormModel;
            }
            catch (Exception ex)
            {
                MessageBox.Show("找不到資料庫上的需求單號，請確認輸入正確", "錯誤需求單號");
                MessageBox.Show(ex.ToString());
                Form1Instance.UpdateLoggerInfoDelegate.Invoke("error", ex.ToString());
                return FormModel;
            }
        }

        /// <summary>
        /// 把出貨單上的 Lot 資料均轉換成大寫
        /// </summary>
        /// <returns></returns>
        private ShippingOrder_DB_Model LotValueToUpper(ShippingOrder_DB_Model ToDo)
        {
            ToDo.CUST_PN = ToDo.CUST_PN.ToUpper();
            ToDo.CustomerLotNO = ToDo.CustomerLotNO.ToUpper();
            ToDo.Psi_Lot_No = ToDo.Psi_Lot_No.ToUpper();
            ToDo.Psi_PN = ToDo.Psi_PN.ToUpper();
            return ToDo;
        }

        /// <summary>
        /// 「搜尋出貨單」按鈕按下後，如果有重複批號，合併它的片號資料
        /// </summary>
        /// <returns></returns>
        private FormFormatModel UpdateSliceNo(ShippingOrder_DB_Model Model, string ShippingOrderNo, FormFormatModel FormModel, Form1 Form1Instance)
        {
            try
            {
                MessageBox.Show("資料庫查到數筆資料，合併其中的片號資料(SliceNo)", "DEMO 版事例");
                //片數可能 => 1~25 => 0 ~ 25 長度 26
                //有 Lot => True
                var CheckList = new bool[26];
                //歷史資料
                var HistoryRecord = FormModel.ShippingOrderList[ShippingOrderNo + "," + Model.CustomerLotNO].SliceNo;
                //會用逗號分隔
                var SliceRange = HistoryRecord.Split(',');
                var SliceResult = "";
                Form1Instance.UpdateLoggerInfoDelegate.Invoke("info", "確認歷史資料有幾片(CheckList)");
                if (Int32.TryParse(HistoryRecord, out int temp))
                {
                    CheckList[temp] = true;
                }
                else
                {
                    //輸入歷史資料
                    foreach (var Slices in SliceRange)
                    {
                        var nums = Slices.Split("-");
                        if (nums.Length == 1)
                        {
                            CheckList[Int32.Parse(nums[0])] = true;
                        }
                        else
                        {
                            for (int i = Int32.Parse(nums[0]); i <= Int32.Parse(nums[1]); i++)
                            {
                                CheckList[i] = true;
                            }
                        }
                    }
                }
                //輸入當前資料
                SliceRange = Model.SliceNo.Split(',');
                Form1Instance.UpdateLoggerInfoDelegate.Invoke("info", "確認當前資料有幾片(CheckList)");
                foreach (var Slices in SliceRange)
                {
                    var nums = Slices.Split("-");
                    if (nums.Length == 1)
                    {
                        CheckList[Int32.Parse(nums[0])] = true;
                    }
                    else
                    {
                        for (int i = Int32.Parse(nums[0]); i <= Int32.Parse(nums[1]); i++)
                        {
                            CheckList[i] = true;
                        }
                    }
                }
                //不一定是第一片開始，所以要找第一個為 True 的
                int StartIndex = Array.FindIndex(CheckList, element => element);
                SliceResult = StartIndex.ToString();
                bool FirstFlag = true;
                Form1Instance.UpdateLoggerInfoDelegate.Invoke("info", "重整片號資訊");
                for (int i = StartIndex; i < CheckList.Length; i++)
                {
                    //如果已經有頭，且看到尾的時候
                    if (CheckList[i] == false && FirstFlag)
                    {
                        SliceResult = StartIndex != i - 1 ? SliceResult + "-" + (i - 1) + "," :
                           SliceResult + ",";

                        FirstFlag = false;
                    }
                    //如果沒有頭，但有看到 true 的時候，定義出新的頭
                    else if (CheckList[i] == true && !FirstFlag)
                    {
                        StartIndex = i;
                        FirstFlag = true;
                        SliceResult = SliceResult + i;
                    }
                    //如果到底了都沒看到尾的時候
                    else if (i == CheckList.Length - 1 && CheckList[i] == true)
                    {
                        //判斷左邊有沒有逗號
                        SliceResult = SliceResult.Last() == ',' ? $"{SliceResult}{StartIndex}-{i}"
                            : $"{SliceResult}-{i}";
                    }
                }
                //把可能出現的最後一個逗號拿掉
                SliceResult = SliceResult.Trim(',');
                //更新該 Lot QTY
                FormModel.ShippingOrderList[ShippingOrderNo + "," + Model.CustomerLotNO].QTY = CheckList.Count(x => x == true).ToString();
                //更新該 Lot 片號清單
                FormModel.ShippingOrderList[ShippingOrderNo + "," + Model.CustomerLotNO].SliceNo = SliceResult;

                return FormModel;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新片數異常，以下回報給IT \r\n {ex}", "更新片數異常");
                Form1Instance.UpdateLoggerInfoDelegate.Invoke("error", $"更新片數異常，以下回報給IT \r\n {ex}");
                return FormModel;
            }
        }

        /// <summary>
        /// 取得 COA 資料
        /// </summary>
        /// <returns></returns>
        public FormFormatModel GetCOA_FileInfo(FormFormatModel FormModel, Form1 Form1Instance)
        {
            MessageBox.Show("此處會比對 COA txt 的資訊\r\n", "DEMO 版事例");
            string[] files = Directory.GetFiles(Form1Instance.ReturnCOA_TextBoxText(), "*.txt");
            // Initialize
            FormModel.COA_LocalFile_List = new Dictionary<string, COA_LocalFile_Model>();
            foreach (string file in files)
            {
                try
                {
                    var FileName = Path.GetFileName(file);
                    var ModelTemp = new COA_LocalFile_Model();
                    // 取得路徑下所有的 txt 檔案
                    var LineData = File.ReadAllLines(file);
                    // 使用 LINQ 找到包含 XXX 的第一項的索引
                    // 統一使用大寫
                    ModelTemp.CUST_PN = LineData[Array.FindIndex(LineData, line => line.Contains("Product:")) + 1].ToUpper();
                    ModelTemp.CustomerLotNO = LineData[Array.FindIndex(LineData, line => line.Contains("LotID:")) + 1].ToUpper();
                    ModelTemp.CustomerName = LineData[Array.FindIndex(LineData, line => line.Contains("Customer:")) + 1].ToUpper();
                    FormModel.COA_Mapping_Lot_List[ModelTemp.CustomerLotNO] = file;
                    FormModel.COA_LocalFile_List[ModelTemp.CustomerLotNO] = ModelTemp;
                }
                catch
                {
                    MessageBox.Show($"解析不到檔案：{Path.GetFileName(file)}",
                        $"未找到資料：{Path.GetFileName(file)}");
                    Form1Instance.UpdateLoggerInfoDelegate.Invoke("error", $"解析不到檔案：{Path.GetFileName(file)}");
                    continue;
                }
            }
            return FormModel;
        }

        /// <summary>
        /// 取得來自 DB 的 COA 資料
        /// </summary>
        /// <returns></returns>
        public FormFormatModel GetCOA_From_DB(FormFormatModel FormModel, Form1 Form1Instance)
        {
            try
            {
                MessageBox.Show("此處會用到 DB 查詢，透過 Dapper 套件將查詢內容映射到方法模型(Class)\r\n" +
                    "SearchRes_Pass = connection.Query<COA_DB_Model> \r\n" +
                    "DEMO 版以固定模型內容表示", "DEMO 版事例");
                // Initialize
                FormModel.COA_List = new Dictionary<string, COA_DB_Model>();
                //Pass 的和 Fail 的都要找，因為有些客戶會出貨 Fail 的項
                string sqlQuery_Pass = FormModel.SQL_Model.COA_DB_SQL_Condition_Pass();
                foreach (var LocalFile in FormModel.COA_LocalFile_List)
                {
                    // 使用 Dapper 執行查詢，將結果映射到 Model 類型的 List
                    // 沒查到 => 回上一步重來 ； 有查到 => 輸入進 Hashset
                    Form1Instance.UpdateLoggerInfoDelegate.Invoke("info", "查詢資料庫資訊");
                    var SearchRes_Pass = new List<COA_DB_Model>() {
                        new COA_DB_Model() { PRODUCTNO = "SAMPLE_PN", CUST_PN = "SAMPLE_CUST_PN",
                        CUSTOMERLOTNO = "SAMPLE_LotNO", LOTNO = "SAMPLE_LotNO", PASSWAFERNUM = "25",
                        PASSSLOTS = "1-25", CUSTOMERENAME = "SAMPLE_Code"} };
                    if (SearchRes_Pass == null)
                    {
                        Form1Instance.UpdateLoggerInfoDelegate.Invoke("warn", $"找不到資料庫上的{LocalFile.Key}");
                        continue;
                    }
                    if (SearchRes_Pass != null)
                    {
                        foreach (var item in SearchRes_Pass)
                        {
                            var FormattedItem = COA_DB_ValueToUpper(item);
                            FormModel.COA_List[LocalFile.Key] = FormattedItem;
                        }
                    }
                }
                return FormModel;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"調用資料庫異常，以下回報給IT\r\n {ex}", "調用資料庫異常");
                Form1Instance.UpdateLoggerInfoDelegate.Invoke("error", $"調用資料庫異常，以下回報給IT\r\n {ex}");
                throw;
            }
        }

        /// <summary>
        /// 把來自 DB 的 COA 資料均轉換成大寫
        /// </summary>
        /// <returns></returns>
        private COA_DB_Model COA_DB_ValueToUpper(COA_DB_Model ToDo)
        {
            ToDo.CUSTOMERENAME = ToDo.CUSTOMERENAME.ToUpper();
            ToDo.CUSTOMERLOTNO = ToDo.CUSTOMERLOTNO.ToUpper();
            ToDo.CUST_PN = ToDo.CUST_PN.ToUpper();
            ToDo.LOTNO = ToDo.LOTNO.ToUpper();
            ToDo.PRODUCTNO = ToDo.PRODUCTNO.ToUpper();
            return ToDo;
        }

        /// <summary>
        /// 依據出貨單批號檢查資料夾內檔名是否相同
        /// </summary>
        /// <returns></returns>
        public FormFormatModel CompareShippingOrderListWithLocal_COA_File(FormFormatModel FormModel, Form1 Form1Instance)
        {
            try
            {
                int CheckingCounter = 0;
                if (FormModel.ShippingOrderList.Count == 0)
                {
                    MessageBox.Show("未找到出貨單資訊，無法比對", "無出貨單");
                    Form1Instance.UpdateLoggerInfoDelegate.Invoke("warn", "未找到出貨單資訊，無法比對");
                    return FormModel;
                }
                if (FormModel.COA_LocalFile_List.Count == 0)
                {
                    MessageBox.Show("未找到本地 COA 檔案，無法比對", "無 COA 檔案");
                    Form1Instance.UpdateLoggerInfoDelegate.Invoke("warn", "未找到本地 COA 檔案，無法比對");
                    return FormModel;
                }
                Form1Instance.UpdateLoggerInfoDelegate.Invoke("info", "確認目標項目是否變色");
                foreach (var item in FormModel.ShippingOrderList)
                {
                    if (FormModel.COA_LocalFile_List.ContainsKey(item.Key.Split(',')[1]))
                    {
                        bool CheckOK = true;
                        // 確認數值是否正確
                        if (FormModel.COA_LocalFile_List[item.Key.Split(',')[1]].CUST_PN != item.Value.CUST_PN) CheckOK = false;
                        if (FormModel.COA_LocalFile_List[item.Key.Split(',')[1]].CustomerLotNO != item.Value.CustomerLotNO) CheckOK = false;
                        // 全部正確統計進去
                        if (CheckOK) CheckingCounter++;
                        ListViewItem? targetItem = null;
                        // 要目標項存在，才找得到項目
                        foreach (ListViewItem temp in Form1Instance.ReturnListView1Items())
                        {
                            if (temp.Tag != null && temp.Tag.ToString().Contains(item.Key.Split(',')[1]))
                            {
                                targetItem = temp;
                                // 如果找到了目標項目，則設定其前景色和背景色
                                targetItem.BackColor = System.Drawing.Color.Yellow; // 設定背景色
                            }
                        }

                    }
                }
                Form1Instance.UpdateLoggerInfoDelegate.Invoke("info", "更新 MainForm 上的列表項目");
                MessageBox.Show("出貨單資訊如果篩選不到顯示白底；篩選得到顯示黃底\r\n" +
                    "篩選如果篩選不到顯示紅底；篩選得到顯示綠底", "DEMO 版事例");
                FormModel.Filter_ShippingOrder = FormModel.ShippingOrderNums.Count.ToString();
                FormModel.Filter_FilteredItem = CheckingCounter.ToString();
                FormModel.Filter_UnfilteredItem = (FormModel.ShippingOrderList.Count - CheckingCounter).ToString();
                Form1Instance.UpdateFilterResult_TextBoxDelegate(FormModel.FilterResult());
                //淡綠色 => 144, 238, 144 淡紅色 => 255, 182, 193
                var ToDoColor = Int32.Parse(FormModel.Filter_UnfilteredItem) > 0 ?
                     Color.FromArgb(255, 182, 193) : Color.FromArgb(144, 238, 144);
                Form1Instance.UpdateFilterResult_TextBoxColorDelegate(ToDoColor);
                return FormModel;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 核對出貨單與 COA 資訊
        /// </summary>
        /// <returns></returns>
        public FormFormatModel Compare_COA_with_Order(FormFormatModel FormModel, Form1 Form1Instance)
        {
            try
            {
                // 計算有篩到的個數
                int CheckingCounter = 0;
                if (FormModel.ShippingOrderList.Count == 0)
                {
                    MessageBox.Show("未找到出貨單資訊，無法比對", "無出貨單");
                    Form1Instance.UpdateLoggerInfoDelegate.Invoke("warn", "未找到出貨單資訊，無法比對");
                    return FormModel;
                }
                if (FormModel.COA_List.Count == 0)
                {
                    MessageBox.Show("未找到本地 COA 檔案，無法比對", "無 COA 檔案");
                    Form1Instance.UpdateLoggerInfoDelegate.Invoke("warn", "未找到本地 COA 檔案，無法比對");
                    return FormModel;
                }
                Form1Instance.UpdateLoggerInfoDelegate.Invoke("info", "確認目標項目是否變色");
                //item => 出貨清單，從第一到最後
                foreach (var item in FormModel.ShippingOrderList)
                {
                    // COA 只抓 Pass 的資料，如果是 Fail 單上的資料，CheckOK 會轉 False，因為 QTY 等對不上
                    // 如果 QTY 對得上，還要確認片號資訊是否一樣，有可能 Pass/Fail QTY 都一樣也說不定
                    if (FormModel.COA_List.ContainsKey(item.Key.Split(',')[1]))
                    {
                        CheckingCounter++;
                        bool CheckOK = true;
                        // 確認數值是否正確
                        if (FormModel.COA_List[item.Key.Split(',')[1]].CUST_PN != item.Value.CUST_PN) CheckOK = false;
                        if (FormModel.COA_List[item.Key.Split(',')[1]].CUSTOMERLOTNO != item.Value.CustomerLotNO) CheckOK = false;
                        if (FormModel.COA_List[item.Key.Split(',')[1]].PASSWAFERNUM != item.Value.QTY) CheckOK = false;
                        if (FormModel.COA_List[item.Key.Split(',')[1]].PASSSLOTS != item.Value.SliceNo) CheckOK = false;
                        // 全部正確統計進去
                        if (CheckOK)
                        {
                            FormModel.OutputList.Add(item.Key);
                        }
                        foreach (ListViewItem temp in Form1Instance.ReturnListView1Items())
                        {
                            if (temp.Tag != null && temp.Tag.ToString() == item.Key)
                            {
                                temp.ForeColor = CheckOK ? Color.Green : Color.Red; // 設定背景色
                                break;
                            }
                        }

                    }
                }
                Form1Instance.UpdateLoggerInfoDelegate.Invoke("info", "確認目標項目是否變色");
                MessageBox.Show("出貨單資訊如果篩選不到顯示紅字；篩選得到顯示綠字\r\n" +
                    "篩選如果篩選不到顯示紅底；篩選得到顯示綠底", "DEMO 版事例");
                FormModel.Check_ShippingOrder = FormModel.Filter_FilteredItem;
                FormModel.Check_FilteredItem = FormModel.OutputList.Count.ToString();
                FormModel.Check_UnfilteredItem = (CheckingCounter - FormModel.OutputList.Count).ToString();
                Form1Instance.UpdateCheckResult_TextBoxDelegate(FormModel.CheckResult());
                //淡綠色 => 144, 238, 144 淡紅色 => 255, 182, 193
                var ToDoColor = FormModel.OutputList.Count < Int32.Parse(FormModel.Filter_FilteredItem) ?
                     Color.FromArgb(255, 182, 193) : Color.FromArgb(144, 238, 144);
                Form1Instance.UpdateCheckResult_TextBoxColorDelegate(ToDoColor);
                return FormModel;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 把篩完的 COA 檔案搬到指定資料夾
        /// </summary>
        /// <returns></returns>
        public FormFormatModel MoveFilesToOrderedDirectory(FormFormatModel FormModel, Form1 Form1Instance)
        {
            string OutputPath = "";
            if (FormModel.OutputList.Count < 1)
            {
                MessageBox.Show($"輸出清單是空的，請比對完確認後再輸出", "空白輸出清單");
                Form1Instance.UpdateLoggerInfoDelegate.Invoke("warn", "輸出清單是空的，請比對完確認後再輸出");
                return FormModel;
            }
            if (String.IsNullOrEmpty(Form1Instance.ReturnCOA_TextBoxText()))
            {
                MessageBox.Show($"源頭路徑是空的，請比對完確認後再輸出", "空白源頭路徑");
                Form1Instance.UpdateLoggerInfoDelegate.Invoke("warn", "源頭路徑是空的，請比對完確認後再輸出");
                return FormModel;
            }
            MessageBox.Show("搬運用戶指定的檔案到指定路徑下\r\n", "DEMO 版事例");
            Form1Instance.UpdateLoggerInfoDelegate.Invoke("info", "輸出完成");
            Process.Start("explorer.exe", OutputPath);
            return FormModel;
        }

        /// <summary>
        /// 寄送結果 Mail
        /// </summary>
        /// <returns></returns>
        public void SendMailEvent(FormFormatModel FormModel, Form1 Form1Instance)
        {
            try
            {
                MessageBox.Show("這個功能會失敗，已對 Mail 地址相關去識別化\r\n", "DEMO 版事例");
                if (FormModel.OutputList.Count < 1)
                {
                    MessageBox.Show($"輸出清單是空的，請比對完確認後再發 Mail", "空白輸出清單");
                    Form1Instance.UpdateLoggerInfoDelegate.Invoke("warn", "輸出清單是空的，請比對完確認後再發 Mail");
                    return;
                }
                if (String.IsNullOrEmpty(Form1Instance.ReturnCOA_TextBoxText()))
                {
                    MessageBox.Show($"COA 源頭路徑是空的，請比對完確認後再發 Mail", "空白源頭路徑");
                    Form1Instance.UpdateLoggerInfoDelegate.Invoke("warn", "源頭路徑是空的，請比對完確認後再發 Mail");
                    return;
                }
                Form1Instance.UpdateLoggerInfoDelegate.Invoke("info", $"Set e-mail recipients");
                string date = DateTime.Today.AddDays(0).ToShortDateString();
                MailMessage msg = new MailMessage();
                //收件者，以逗號分隔不同收件者 ex "test@gmail.com,test2@gmail.com"
                msg.To.Add(string.Join(",", FormModel.MailList.ToArray()));
                msg.From = new MailAddress("YourCompany@Name.com", "YourCompany", System.Text.Encoding.UTF8);
                //郵件標題 「日期 COA report check list - 客戶」 
                msg.Subject = $"{date} COA report check list";
                //郵件標題編碼
                msg.SubjectEncoding = System.Text.Encoding.UTF8;
                //郵件表頭
                msg.Body += FormModel.Mail_CSS_Format_Header;
                //郵件表身

                foreach (var ShippingOrderItem in FormModel.ShippingOrderList)
                {
                    string BodyTemp = "";
                    msg.Body += "<tr>";
                    BodyTemp += $"<td>{ShippingOrderItem.Value.CustomerCode}</td>";
                    //有沒有檢查通過
                    if (FormModel.OutputList.Contains(ShippingOrderItem.Key))
                    {
                        BodyTemp += $"<td>Pass</td>";
                    }
                    else
                    {
                        BodyTemp += $"<td>Fail</td>";
                    }
                    BodyTemp += $"<td>{ShippingOrderItem.Value.DN_NO}</td>";
                    BodyTemp += $"<td>{ShippingOrderItem.Value.Psi_PN}</td>";
                    BodyTemp += $"<td>{ShippingOrderItem.Value.CUST_PN}</td>";
                    BodyTemp += $"<td>{ShippingOrderItem.Value.CustomerLotNO}</td>";
                    BodyTemp += $"<td>{ShippingOrderItem.Value.Psi_Lot_No}</td>";
                    BodyTemp += $"<td>{ShippingOrderItem.Value.QTY}</td>";
                    BodyTemp += $"<td>{ShippingOrderItem.Value.SliceNo}</td>";
                    msg.Body += BodyTemp;
                    msg.Body += "</tr>";
                }

                //郵件表尾
                msg.Body += FormModel.Mail_CSS_Format_Footer;

                msg.IsBodyHtml = true;
                msg.BodyEncoding = System.Text.Encoding.UTF8;//郵件內容編碼 
                msg.Priority = MailPriority.Normal;//郵件優先級 
                SmtpClient MySmtp = new SmtpClient("Name.com", 25);
                MySmtp.Send(msg);
                Form1Instance.UpdateLoggerInfoDelegate.Invoke("info", "已寄送出貨 Mail，請查收");
                MessageBox.Show("已寄送出貨 Mail，請查收", "Mail 發送完成");
            }
            catch (Exception ex)
            {
                Form1Instance.UpdateLoggerInfoDelegate.Invoke("error", "Crash on SendMail");
                MessageBox.Show($"寄送郵件異常，以下回報給IT \r\n {ex}", "寄送郵件異常");
                return;
            }
        }
    }
}
