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
    public partial class SubForm2 : Form
    {
        List<string> selectedItems = new List<string>();
        Form1 Form1Instance;
        public SubForm2()
        {
            InitializeComponent();
            ShippingOrderCheckedListBox.CheckOnClick = true;
        }

        // 新增一個接受參數的建構子
        public SubForm2(FormFormatModel FormModel, Form1 Instance) : this()
        {
            foreach (var item in FormModel.ShippingOrderNums)
            {
                ShippingOrderCheckedListBox.Items.Add(item);
            }
            Form1Instance = Instance;
        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            // 在預覽中刪除這些項目
            RemoveItemsFromListView();
            Form1Instance.ClearPartialShippingOrderDelegate.Invoke(selectedItems);
            // 刪完自動關閉
            this.Close();
        }
        private void RemoveItemsFromListView()
        {
            // 要刪除的項目的索引集合
            List<int> itemsToDelete = new List<int>();
            for (int i = ShippingOrderCheckedListBox.Items.Count - 1; i >= 0; i--)
            {
                if (ShippingOrderCheckedListBox.GetItemChecked(i))
                {
                    // 如果項目被打勾，記錄索引
                    itemsToDelete.Add(i);
                }
            }
            foreach (var index in itemsToDelete)
            {
                selectedItems.Add(ShippingOrderCheckedListBox.Items[index].ToString());
                ShippingOrderCheckedListBox.Items.RemoveAt(index);
            }
        }
    }
}
