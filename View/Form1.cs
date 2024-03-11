using Dapper;
using NLog;
using Oracle.ManagedDataAccess.Client;
using ShippingOrderCOAFilter.Controller;
using ShippingOrderCOAFilter.Model;
using ShippingOrderCOAFilter.View;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net.Mail;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows.Forms;
using static System.Windows.Forms.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace ShippingOrderCOAFilter
{
    public partial class Form1 : Form
    {
        #region Initial Values

        // 初始化 ToolTip 控制項
        private System.Windows.Forms.ToolTip toolTip1 = new System.Windows.Forms.ToolTip();
        // 初始化關於 Form 用的 Model
        // 所有涉及到 Form 的參數幾乎都放在這裡
        private FormFormatModel FormModel = new FormFormatModel();
        // 宣告 Controller
        MainFormHandler LogicAgent;
        // 初始化關於 log 文件
        private static Logger logger = LogManager.GetCurrentClassLogger();

        #region Delegate Samples

        // 在 Form1 中建立委派的實例
        // About TextBox
        // 更新
        public UpdateTextBoxDelegate UpdateShippingOrderNoTextBoxDelegate;
        public UpdateTextBoxDelegate UpdateCOA_textBoxDelegate;
        public UpdateTextBoxDelegate UpdateFilterResult_TextBoxDelegate;
        public UpdateTextBoxDelegate UpdateCheckResult_TextBoxDelegate;
        // 變色
        public UpdateTextBoxColorDelegate UpdateFilterResult_TextBoxColorDelegate;
        public UpdateTextBoxColorDelegate UpdateCheckResult_TextBoxColorDelegate;
        // About ListView
        public UpdateListViewDelegate UpdateListViewInfoDelegate;
        public RefreshListViewDelegate RefreshListViewInfoDelegate;
        // About ClearEvent
        public ClearDelegate ClearAllItemsDelegate;
        public ClearPartialDelegate ClearPartialShippingOrderDelegate;
        // About Logger
        public UpdateLoggerDelegate UpdateLoggerInfoDelegate;

        #endregion

        #endregion

        #region Delegate Initialize

        #region 關於 TextBox 的委派

        /// <summary>
        /// 委派：初始化委派物件，同種類同一項
        /// </summary>
        /// <returns></returns>
        public delegate void UpdateTextBoxDelegate(string text);
        /// <summary>
        /// 委派：ShippingOrderNoTextBox 變更
        /// </summary>
        /// <returns></returns>
        private void UpdateShippingOrderNoTextBox(string text)
        {
            ShippingOrderNoTextBox.Text = text;
        }
        /// <summary>
        /// 委派：COA_textBox 變更
        /// </summary>
        /// <returns></returns>
        private void UpdateCOA_textBox(string text)
        {
            COA_textBox.Text = text;
        }
        /// <summary>
        /// 委派：FilterResult_TextBox 變更
        /// </summary>
        /// <returns></returns>
        private void UpdateFilterResult_TextBox(string text)
        {
            FilterResult_TextBox.Text = text;
        }
        /// <summary>
        /// 委派：CheckResult_TextBox 變更
        /// </summary>
        /// <returns></returns>
        private void UpdateCheckResult_TextBox(string text)
        {
            CheckResult_TextBox.Text = text;
        }

        #endregion

        #region 關於 TextBox 變色的委派

        /// <summary>
        /// 委派：初始化委派物件，同種類同一項
        /// </summary>
        /// <returns></returns>
        public delegate void UpdateTextBoxColorDelegate(Color ChangedColor);
        /// <summary>
        /// 委派：FilterResult_TextBox 變色
        /// </summary>
        /// <returns></returns>
        private void UpdateFilterResult_TextBoxColor(Color ChangedColor)
        {
            FilterResult_TextBox.BackColor = ChangedColor;
        }
        /// <summary>
        /// 委派：FilterResult_TextBox 變色
        /// </summary>
        /// <returns></returns>
        private void UpdateCheckResult_TextBoxColor(Color ChangedColor)
        {
            CheckResult_TextBox.BackColor = ChangedColor;
        }

        #endregion

        #region 關於 listView1 的委派

        /// <summary>
        /// 委派：初始化委派物件，同種類同一項
        /// </summary>
        /// <returns></returns>
        public delegate void UpdateListViewDelegate(ListViewItem item);
        public delegate void RefreshListViewDelegate(ListViewItem item, string[] RefreshedItems);
        /// <summary>
        /// 委派：listView1Item 變更
        /// </summary>
        /// <returns></returns>
        private void UpdateListViewItem(ListViewItem item)
        {
            // 將整個項目添加到ListView中
            listView1.Items.Add(item);
        }
        /// <summary>
        /// 委派：listView1Item 刷新
        /// </summary>
        /// <returns></returns>
        private void RefreshListViewItem(ListViewItem TargetItem, string[] RefreshedItems)
        {
            for (int i = 0; i < RefreshedItems.Length; i++)
            {
                TargetItem.SubItems[i].Text = RefreshedItems[i];
            }
            listView1.Refresh();
        }

        #endregion

        #region 關於 Logger 的委派

        /// <summary>
        /// 委派：初始化委派物件，同種類同一項
        /// </summary>
        /// <returns></returns>
        public delegate void UpdateLoggerDelegate(string Level, string Record);
        /// <summary>
        /// 委派：Logger 寫入
        /// </summary>
        /// <returns></returns>
        private void UpdateLogger(string Level, string Record)
        {
            // 將整個項目添加到ListView中
            WriteLog_LocalFile(Level, Record, "MainFormHandler");
        }

        #endregion

        #region 關於清理事件的委派

        /// <summary>
        /// 委派：初始化委派物件，同種類同一項
        /// </summary>
        /// <returns></returns>
        public delegate void ClearDelegate();
        public delegate void ClearPartialDelegate(List<string> RemoveList);
        /// <summary>
        /// 委派：清理事件
        /// </summary>
        /// <returns></returns>
        private void ClearAll_Delegate()
        {
            ClearAll();
        }
        private void ClearPartialShippingOrder_Delegate(List<string> RemoveList)
        {
            ClearPartialShippingOrder(RemoveList);
        }

        #endregion

        #endregion

        /// <summary>
        /// 初始化畫面
        /// </summary>
        /// <returns></returns>
        public Form1()
        {
            InitializeComponent();
            // 在 Form1 的建構函式中初始化委派
            InitializeDelegate();
        }

        /// <summary>
        /// 初始化委派列表
        /// </summary>
        /// <returns></returns>
        private void InitializeDelegate()
        {
            // TextBox
            UpdateShippingOrderNoTextBoxDelegate = new UpdateTextBoxDelegate(UpdateShippingOrderNoTextBox);
            UpdateCOA_textBoxDelegate = new UpdateTextBoxDelegate(UpdateCOA_textBox);
            UpdateFilterResult_TextBoxDelegate = new UpdateTextBoxDelegate(UpdateFilterResult_TextBox);
            UpdateCheckResult_TextBoxDelegate = new UpdateTextBoxDelegate(UpdateCheckResult_TextBox);
            UpdateFilterResult_TextBoxColorDelegate = new UpdateTextBoxColorDelegate(UpdateFilterResult_TextBoxColor);
            UpdateCheckResult_TextBoxColorDelegate = new UpdateTextBoxColorDelegate(UpdateCheckResult_TextBoxColor);
            // ListView
            UpdateListViewInfoDelegate = new UpdateListViewDelegate(UpdateListViewItem);
            RefreshListViewInfoDelegate = new RefreshListViewDelegate(RefreshListViewItem);
            listView1.MouseClick += new MouseEventHandler(this.listView1_ItemActivate);
            // Logger
            UpdateLoggerInfoDelegate = new UpdateLoggerDelegate(UpdateLogger);
            // Clear
            ClearAllItemsDelegate = new ClearDelegate(ClearAll_Delegate);
            ClearPartialShippingOrderDelegate = new ClearPartialDelegate(ClearPartialShippingOrder_Delegate);
        }

        /// <summary>
        /// 初始進入的函式，初始化放在這裡
        /// </summary>
        /// <returns></returns>
        private void Form1_Load(object sender, EventArgs e)
        {
            WriteLog_LocalFile("info", "程式啟動", MethodBase.GetCurrentMethod()?.Name);

            WriteLog_LocalFile("info", "設定提示文字", MethodBase.GetCurrentMethod()?.Name);
            toolTip1.SetToolTip(SendMail, "發送核對結果的提示報告");
            toolTip1.SetToolTip(Clear, "清除出貨單資訊與核對結果");
            toolTip1.SetToolTip(ScanShipoutRecipt, "從資料庫搜尋出貨單資訊");
            toolTip1.SetToolTip(OpenDirectory, "開啟存放 COA 的資料夾");
            toolTip1.SetToolTip(COA_Preview, "開啟 COA 資料預覽畫面，來自 DB 的 COA 資料");
            toolTip1.SetToolTip(ShippingOrderPreview, "開啟出貨單資料預覽畫面，可以獨立刪除出貨單");
            toolTip1.SetToolTip(FilterShippingOrder, "依據出貨單批號檢查資料夾內檔名是否相同");
            toolTip1.SetToolTip(Check_COA_with_Order, "核對出貨單與 COA 資訊是否一致");
            toolTip1.SetToolTip(Output_COA, "把篩完的 COA 檔案搬到指定資料夾");

            WriteLog_LocalFile("info", "初始化列表格", MethodBase.GetCurrentMethod()?.Name);
            // 添加欄位
            listView1.Columns.Add("出貨單號");
            listView1.Columns.Add("CustomerLotNo");
            listView1.Columns.Add("片數");
            listView1.Columns.Add("各片清單");
            listView1.Columns.Add("廠商代號");
            // 讓該欄位可以被選取
            listView1.FullRowSelect = true;
            // 設定列自動調整
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            // 初始化關於 Form 用的 Handler(Agent)
            // **要在 Form 被建立以後才能建立，因為它要建構一個額外的 Form，如果放到初始設定會造成無限循環
            WriteLog_LocalFile("info", "建構 LogicAgent", MethodBase.GetCurrentMethod()?.Name);
            LogicAgent = new MainFormHandler();
        }

        #region 取得元件上的值

        /// <summary>
        /// 取得 COA_TextBox 上的值
        /// </summary>
        /// <returns></returns>
        public string ReturnCOA_TextBoxText()
        {
            return COA_textBox.Text;
        }

        /// <summary>
        /// 取得 ListView1 上的 Items
        /// </summary>
        /// <returns></returns>
        public ListViewItemCollection ReturnListView1Items()
        {
            return listView1.Items;
        }

        #endregion

        /// <summary>
        /// 對著 ShippingOrderNoTextBox 按 Enter 同按按鈕的功能
        /// </summary>
        /// <returns></returns>
        private void ShippingOrderNoTextBox_TextChanged(object sender, KeyPressEventArgs e)
        {            
            // 檢查按下的按鍵是否是 Enter 鍵（ASCII 13）
            if (e.KeyChar == (char)Keys.Enter && sender == ShippingOrderNoTextBox)
            {
                ScanShipoutRecipt_Click(sender, e);
                // 取消 Enter 鍵的預設動作（防止換行）
                e.Handled = true;
            }
        }

        /// <summary>
        /// 「搜尋出貨單」按鈕按下後引發的事件
        /// </summary>
        /// <returns></returns>
        private void ScanShipoutRecipt_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("此處輸入出貨單號，DEMO 版固定以 123 舉例", "DEMO 版事例");
                if (!String.IsNullOrEmpty(ShippingOrderNoTextBox.Text))
                {
                    WriteLog_LocalFile("info", "搜尋出貨單", MethodBase.GetCurrentMethod()?.Name);
                    ShippingOrderNoTextBox.Text = "123";
                    FormModel = LogicAgent.ScanShipoutRecipt(FormModel, ShippingOrderNoTextBox.Text, this);
                }
                else
                {
                    MessageBox.Show("請輸入需求單號", "空白需求單號");
                    WriteLog_LocalFile("info", "空白需求單號", MethodBase.GetCurrentMethod()?.Name);
                }
                // 設定列自動調整
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                ShippingOrderNoTextBox.Text = "";
            }
            catch (Exception ex)
            {
                WriteLog_LocalFile("error", "搜尋出貨單異常，以下回報給IT \r\n {ex}", MethodBase.GetCurrentMethod()?.Name);
                MessageBox.Show($"搜尋出貨單異常，以下回報給IT \r\n {ex}", "搜尋出貨單異常");
                return;
            }
        }

        /// <summary>
        /// 開啟存放 COA 的資料夾
        /// </summary>
        /// <returns></returns>
        private void OpenDirectory_Click(object sender, EventArgs e)
        {
            try
            {
                WriteLog_LocalFile("info", "開啟資料夾", MethodBase.GetCurrentMethod()?.Name);
                using (var fbd = new FolderBrowserDialog())
                {
                    // 設定預設開啟的路徑
                    //fbd.SelectedPath = @"";
                    DialogResult _result = fbd.ShowDialog();
                    string COA_Dir = fbd.SelectedPath;
                    COA_textBox.Text = COA_Dir;
                }
                DialogResult result = MessageBox.Show("需要轉換原始 COA 檔案，一個檔案要 5~10 秒\r\n" +
                    "請耐心等待提示窗出現再繼續作業", "確認", MessageBoxButtons.YesNo);

                // 根據使用者的選擇執行不同的邏輯
                if (result == DialogResult.Yes)
                {
                    // 使用者選擇是，執行下一個函式                    
                    WriteLog_LocalFile("info", "取得 COA 檔案的資料", MethodBase.GetCurrentMethod()?.Name);
                    FormModel = LogicAgent.GetCOA_FileInfo(FormModel, this);
                    WriteLog_LocalFile("info", "取得來自 DB 的 COA 資料", MethodBase.GetCurrentMethod()?.Name);
                    FormModel = LogicAgent.GetCOA_From_DB(FormModel, this);
                    MessageBox.Show("轉換結束！", "確認");
                }
                else
                {
                    COA_textBox.Text = "";
                    // 使用者選擇否，回到主畫面
                    // 在這裡可以加入回到主畫面的相關邏輯
                    return;
                }

            }
            catch (Exception ex)
            {
                WriteLog_LocalFile("error", ex.ToString(), MethodBase.GetCurrentMethod()?.Name);
                return;
            }
        }

        /// <summary>
        /// 開啟 COA 資料預覽畫面，來自 DB 的 COA 資料
        /// </summary>
        /// <returns></returns>
        private void COA_Preview_Click(object sender, EventArgs e)
        {
            try
            {
                WriteLog_LocalFile("info", "開啟 COA 資料預覽畫面", MethodBase.GetCurrentMethod()?.Name);
                if (String.IsNullOrEmpty(COA_textBox.Text))
                {
                    MessageBox.Show("請選擇正確檔案路徑", "空白 COA 檔案路徑");
                }
                else
                {
                    // 實例化 SecondForm
                    SubForm secondForm = new SubForm(COA_textBox.Text, FormModel.COA_List);
                    // 設定 MainForm 為 SecondForm 的主視窗
                    secondForm.Owner = this;
                    // 顯示 SecondForm
                    secondForm.Show();
                }
            }
            catch (Exception ex)
            {
                WriteLog_LocalFile("warn", $"開啟 COA 資料預覽畫面異常，以下回報給IT\r\n {ex}", MethodBase.GetCurrentMethod()?.Name);
                MessageBox.Show($"開啟 COA 資料預覽畫面異常，以下回報給IT\r\n {ex}", "開啟視窗異常");
                return;
            }
        }

        /// <summary>
        /// 開啟出貨單資料預覽畫面
        /// </summary>
        /// <returns></returns>
        private void ShippingOrderPreview_Click(object sender, EventArgs e)
        {
            try
            {
                WriteLog_LocalFile("info", "開啟出貨單預覽畫面", MethodBase.GetCurrentMethod()?.Name);
                if (FormModel.ShippingOrderNums.Count == 0)
                {
                    MessageBox.Show("尚無出貨單資料", "未找到出貨單");
                }
                else
                {
                    // 實例化 SecondForm
                    SubForm2 secondForm = new SubForm2(FormModel, this);
                    // 設定 MainForm 為 SecondForm 的主視窗
                    secondForm.Owner = this;
                    // 顯示 SecondForm
                    secondForm.Show();
                }
            }
            catch (Exception ex)
            {
                WriteLog_LocalFile("warn", $"開啟出貨單資料預覽畫面異常，以下回報給IT\r\n {ex}", MethodBase.GetCurrentMethod()?.Name);
                MessageBox.Show($"開啟出貨單資料預覽畫面異常，以下回報給IT\r\n {ex}", "開啟視窗異常");
                return;
            }
        }

        /// <summary>
        /// 依據出貨單批號檢查資料夾內檔名是否相同
        /// </summary>
        /// <returns></returns>
        private void Filter_Click(object sender, EventArgs e)
        {
            // ShippingOrderList => 出貨單資料 + 它的資訊
            // COA_LocalFile_List => 本地端 COA 檔案 + 它的資訊
            WriteLog_LocalFile("info", "依據出貨單批號檢查資料夾內檔名是否相同", MethodBase.GetCurrentMethod()?.Name);
            try
            {
                FormModel = LogicAgent.CompareShippingOrderListWithLocal_COA_File(FormModel, this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"比對出貨單與本地 COA 檔案異常，以下回報給IT\r\n {ex}", "比對出貨單與本地 COA 檔案異常");
                WriteLog_LocalFile("error", $"比對出貨單與本地 COA 檔案異常，以下回報給IT\r\n {ex}", MethodBase.GetCurrentMethod()?.Name);
                return;
            }
        }

        /// <summary>
        /// 核對出貨單與 COA 資訊
        /// </summary>
        /// <returns></returns>
        private void Check_COA_with_Order_Click(object sender, EventArgs e)
        {
            // ShippingOrderList => 出貨單資料 + 它的資訊
            // COA_List => 本地端 COA 檔案 + 它的資訊
            WriteLog_LocalFile("info", "核對出貨單與 COA 資訊", MethodBase.GetCurrentMethod()?.Name);
            try
            {
                FormModel = LogicAgent.Compare_COA_with_Order(FormModel, this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"比對出貨單與 COA 內容異常，以下回報給IT\r\n {ex}", "比對出貨單與 COA 內容異常");
                WriteLog_LocalFile("error", $"比對出貨單與 COA 內容異常，以下回報給IT\r\n {ex}", MethodBase.GetCurrentMethod()?.Name);
                return;
            }
        }

        /// <summary>
        /// 把篩完的 COA 檔案搬到指定資料夾
        /// </summary>
        /// <returns></returns>
        private void Output_COA_Click(object sender, EventArgs e)
        {
            try
            {
                WriteLog_LocalFile("info", "把篩完的 COA 檔案搬到指定資料夾", MethodBase.GetCurrentMethod()?.Name);
                FormModel = LogicAgent.MoveFilesToOrderedDirectory(FormModel, this);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"輸出檔案異常，以下回報給IT\r\n {ex}", "輸出檔案異常");
                WriteLog_LocalFile("error", $"輸出檔案異常，以下回報給IT\r\n {ex}", MethodBase.GetCurrentMethod()?.Name);
                return;
            }
        }

        /// <summary>
        /// 寄送結果 Mail
        /// </summary>
        /// <returns></returns>
        private void SendMail_Click(object sender, EventArgs e)
        {
            try
            {
                // 使用 MessageBox 顯示確認對話框
                DialogResult result = MessageBox.Show("是否發送 Mail？", "確認", MessageBoxButtons.YesNo);

                // 根據使用者的選擇執行不同的邏輯
                if (result == DialogResult.Yes)
                {
                    // 使用者選擇是，執行下一個函式
                    WriteLog_LocalFile("info", "寄送結果 Mail", MethodBase.GetCurrentMethod()?.Name);
                    LogicAgent.SendMailEvent(FormModel, this);
                }
                else
                {
                    // 使用者選擇否，回到主畫面
                    // 在這裡可以加入回到主畫面的相關邏輯
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"寄送 Mail 異常，以下回報給IT\r\n {ex}", "寄送 Mail 異常");
                WriteLog_LocalFile("error", $"寄送 Mail 異常，以下回報給IT\r\n {ex}", MethodBase.GetCurrentMethod()?.Name);
                return;
            }
        }

        /// <summary>
        /// 清除按鈕觸發後的事件
        /// </summary>
        /// <returns></returns>
        private void Clear_Click(object sender, EventArgs e)
        {
            try
            {
                WriteLog_LocalFile("info", "清理主畫面項目", MethodBase.GetCurrentMethod()?.Name);
                ClearAll();
            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// 清除所有已填入資料
        /// </summary>
        /// <returns></returns>
        private void ClearAll()
        {
            try
            {
                WriteLog_LocalFile("info", "清除所有已填入資料", MethodBase.GetCurrentMethod()?.Name);
                listView1.Items.Clear();
                ShippingOrderNoTextBox.Text = "";
                COA_textBox.Text = "";
                //初始化 FormFormatModel
                FormModel = new FormFormatModel();
                TestBoxReset();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"清除資料異常，以下回報給IT \r\n {ex}", "清除資料異常");
                throw;
            }
        }

        /// <summary>
        /// 清除出貨單部分資料
        /// </summary>
        /// <returns></returns>
        private void ClearPartialShippingOrder(List<string> RemoveList)
        {
            try
            {
                WriteLog_LocalFile("info", "清除部分需求單", MethodBase.GetCurrentMethod()?.Name);
                foreach (var BeRemoved in RemoveList)
                {
                    foreach (ListViewItem item in listView1.Items)
                    {
                        if (item.Tag.ToString().Contains(BeRemoved))
                        {
                            // 刪除 Form2 中的項目
                            listView1.Items.Remove(item);
                            FormModel.ShippingOrderList.Remove(item.Tag.ToString());
                            FormModel.ExistingKeysOfShippingOrderAndCustPN.Remove(item.Tag.ToString());
                            FormModel.ShippingOrderNums.Remove(BeRemoved);
                        }
                    }
                }
                FormModel.OutputList = new HashSet<string>();
                FormModel.CustomerList = new HashSet<string>();
                FormModel.COA_Mapping_Lot_List = new Dictionary<string, string>();
                TestBoxReset();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"清除資料異常，以下回報給IT \r\n {ex}", "清除資料異常");
                throw;
            }
        }

        /// <summary>
        /// 寫程式運行的 log
        /// </summary>
        /// <returns></returns>
        public static void WriteLog_LocalFile(string level, string Message, string? layer)
        {
            try
            {
                switch (level)
                {
                    case "info":
                        logger.Info($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - [{layer}] - {Message}");
                        break;
                    case "warn":
                        logger.Warn($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - [{layer}] - {Message}");
                        break;
                    case "error":
                        logger.Error($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - [{layer}] - {Message}");
                        break;
                    default:
                        throw new Exception("不支援的Log格式");
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 點擊 listView1 上的 item 自動複製到剪貼簿
        /// </summary>
        /// <returns></returns>
        private void listView1_ItemActivate(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.ListView listview = (System.Windows.Forms.ListView)sender;
            ListViewItem lstrow = listview.GetItemAt(e.X, e.Y);
            ListViewItem.ListViewSubItem lstcol = lstrow.GetSubItemAt(e.X, e.Y);
            string strText = lstcol.Text;
            // 在這裡處理 ListView 中 Item 被激活的事件邏輯
            try
            {
                Clipboard.SetDataObject(strText);
                //補加的 => 位置修正
                toolTip1.Show($"已複製 {strText}", this, e.X + 500, e.Y + 100);

                // 延遲 0.3 秒後自動隱藏ToolTip
                Thread.Sleep(300);
                toolTip1.Hide(this);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 初始化 TestBox
        /// </summary>
        /// <returns></returns>
        private void TestBoxReset()
        {
            FilterResult_TextBox.Text = FormModel.InitialFilterTextBox();
            CheckResult_TextBox.Text = FormModel.InitialCheckTextBox();
            FilterResult_TextBox.BackColor = Color.White;
            CheckResult_TextBox.BackColor = Color.White;
        }
    }
}