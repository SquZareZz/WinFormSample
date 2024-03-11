using WinFormSample.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinFormSample.View
{
    public partial class SubForm : Form
    {
        #region Initial Values

        // 新增一個成員變數用來存放從 Form1 傳遞過來的值
        private string Address = "";
        public Dictionary<string, COA_DB_Model> FileDict = new Dictionary<string, COA_DB_Model>();

        #endregion

        public SubForm()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // 新增一個接受參數的建構子
        public SubForm(string AddressFromForm1, Dictionary<string, COA_DB_Model> FileDictFromForm1) : this()
        {
            // 在這裡處理 Form1 傳遞過來的值
            Address = AddressFromForm1;
            FileDict = FileDictFromForm1;
            //接收到值以後處理 List 事件
            DisplayFileList();
        }
        private void DisplayFileList()
        {
            PreviewFiles.Columns.Add("抓到的檔案");
            PreviewFiles.Columns.Add("CUST_PN");
            PreviewFiles.Columns.Add("CustomerLotNO");
            PreviewFiles.Columns.Add("QTY");
            PreviewFiles.Columns.Add("SliceNo");
            var FileList = Directory.GetFiles(Address);
            foreach (var file in FileDict)
            {
                string LocalFile = Path.GetFileName(FileList.Where(x => x.Contains(file.Key[1])).First());
                ListViewItem item = new ListViewItem(new[] { LocalFile,file.Value.CUST_PN,
                file.Value.CUSTOMERLOTNO,file.Value.PASSWAFERNUM,file.Value.PASSSLOTS});
                PreviewFiles.Items.Add(item);
            }
            // 設定列自動調整
            PreviewFiles.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
    }
}
