
namespace WinFormSample
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            ShippingOrderNoTextBox = new TextBox();
            ScanShipoutRecipt = new Button();
            COA_textBox = new TextBox();
            OpenDirectory = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            listView1 = new ListView();
            Clear = new Button();
            COA_Preview = new Button();
            CheckResult_TextBox = new TextBox();
            FilterResult_TextBox = new TextBox();
            SendMail = new Button();
            FilterShippingOrder = new Button();
            Check_COA_with_Order = new Button();
            Output_COA = new Button();
            ShippingOrderPreview = new Button();
            SuspendLayout();
            // 
            // ShippingOrderNoTextBox
            // 
            ShippingOrderNoTextBox.Font = new Font("Microsoft JhengHei UI", 24F, FontStyle.Regular, GraphicsUnit.Point);
            ShippingOrderNoTextBox.Location = new Point(88, 61);
            ShippingOrderNoTextBox.Name = "ShippingOrderNoTextBox";
            ShippingOrderNoTextBox.Size = new Size(222, 48);
            ShippingOrderNoTextBox.TabIndex = 0;
            ShippingOrderNoTextBox.KeyPress += ShippingOrderNoTextBox_TextChanged;
            // 
            // ScanShipoutRecipt
            // 
            ScanShipoutRecipt.Image = (Image)resources.GetObject("ScanShipoutRecipt.Image");
            ScanShipoutRecipt.ImageAlign = ContentAlignment.MiddleLeft;
            ScanShipoutRecipt.Location = new Point(341, 61);
            ScanShipoutRecipt.Name = "ScanShipoutRecipt";
            ScanShipoutRecipt.Size = new Size(125, 53);
            ScanShipoutRecipt.TabIndex = 1;
            ScanShipoutRecipt.Text = "搜尋出貨單";
            ScanShipoutRecipt.TextAlign = ContentAlignment.MiddleRight;
            ScanShipoutRecipt.UseVisualStyleBackColor = true;
            ScanShipoutRecipt.Click += ScanShipoutRecipt_Click;
            // 
            // COA_textBox
            // 
            COA_textBox.BackColor = SystemColors.HighlightText;
            COA_textBox.Enabled = false;
            COA_textBox.Font = new Font("Microsoft JhengHei UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            COA_textBox.Location = new Point(88, 207);
            COA_textBox.Name = "COA_textBox";
            COA_textBox.Size = new Size(378, 34);
            COA_textBox.TabIndex = 2;
            // 
            // OpenDirectory
            // 
            OpenDirectory.Image = (Image)resources.GetObject("OpenDirectory.Image");
            OpenDirectory.ImageAlign = ContentAlignment.MiddleLeft;
            OpenDirectory.Location = new Point(341, 145);
            OpenDirectory.Name = "OpenDirectory";
            OpenDirectory.Size = new Size(125, 54);
            OpenDirectory.TabIndex = 3;
            OpenDirectory.Text = "選擇資料夾";
            OpenDirectory.TextAlign = ContentAlignment.MiddleRight;
            OpenDirectory.UseVisualStyleBackColor = true;
            OpenDirectory.Click += OpenDirectory_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft JhengHei UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(132, 19);
            label1.Name = "label1";
            label1.Size = new Size(133, 30);
            label1.TabIndex = 5;
            label1.Text = "出貨單單號";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft JhengHei UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(132, 153);
            label2.Name = "label2";
            label2.Size = new Size(144, 30);
            label2.TabIndex = 6;
            label2.Text = "COA 資料夾";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft JhengHei UI", 24F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(835, 8);
            label3.Name = "label3";
            label3.Size = new Size(178, 41);
            label3.TabIndex = 7;
            label3.Text = "出貨單資訊";
            // 
            // listView1
            // 
            listView1.Font = new Font("Microsoft JhengHei UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            listView1.Location = new Point(567, 61);
            listView1.Name = "listView1";
            listView1.Size = new Size(665, 411);
            listView1.TabIndex = 8;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = System.Windows.Forms.View.Details;
            // 
            // Clear
            // 
            Clear.Font = new Font("Microsoft JhengHei UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            Clear.Image = (Image)resources.GetObject("Clear.Image");
            Clear.ImageAlign = ContentAlignment.MiddleLeft;
            Clear.Location = new Point(1107, 504);
            Clear.Name = "Clear";
            Clear.Size = new Size(125, 55);
            Clear.TabIndex = 9;
            Clear.Text = "Reset";
            Clear.TextAlign = ContentAlignment.MiddleRight;
            Clear.UseVisualStyleBackColor = true;
            Clear.Click += Clear_Click;
            // 
            // COA_Preview
            // 
            COA_Preview.Image = (Image)resources.GetObject("COA_Preview.Image");
            COA_Preview.ImageAlign = ContentAlignment.MiddleLeft;
            COA_Preview.Location = new Point(124, 265);
            COA_Preview.Name = "COA_Preview";
            COA_Preview.Size = new Size(140, 56);
            COA_Preview.TabIndex = 10;
            COA_Preview.Text = "COA 資料預覽";
            COA_Preview.TextAlign = ContentAlignment.MiddleRight;
            COA_Preview.UseVisualStyleBackColor = true;
            COA_Preview.Click += COA_Preview_Click;
            // 
            // CheckResult_TextBox
            // 
            CheckResult_TextBox.BackColor = SystemColors.HighlightText;
            CheckResult_TextBox.Enabled = false;
            CheckResult_TextBox.Font = new Font("Microsoft JhengHei UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point);
            CheckResult_TextBox.ForeColor = SystemColors.InfoText;
            CheckResult_TextBox.Location = new Point(835, 504);
            CheckResult_TextBox.Multiline = true;
            CheckResult_TextBox.Name = "CheckResult_TextBox";
            CheckResult_TextBox.Size = new Size(222, 150);
            CheckResult_TextBox.TabIndex = 11;
            CheckResult_TextBox.Text = "核對結果：\rCOA：\r\nPass：\r\nFail：\r\n";
            // 
            // FilterResult_TextBox
            // 
            FilterResult_TextBox.BackColor = SystemColors.HighlightText;
            FilterResult_TextBox.Enabled = false;
            FilterResult_TextBox.Font = new Font("Microsoft JhengHei UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point);
            FilterResult_TextBox.ForeColor = SystemColors.ActiveCaptionText;
            FilterResult_TextBox.Location = new Point(567, 504);
            FilterResult_TextBox.Multiline = true;
            FilterResult_TextBox.Name = "FilterResult_TextBox";
            FilterResult_TextBox.Size = new Size(222, 150);
            FilterResult_TextBox.TabIndex = 12;
            FilterResult_TextBox.Text = "篩選結果：\r\n出貨單：\r\n已篩選：\r\n未篩選：";
            // 
            // SendMail
            // 
            SendMail.Font = new Font("Microsoft JhengHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            SendMail.Image = (Image)resources.GetObject("SendMail.Image");
            SendMail.ImageAlign = ContentAlignment.MiddleLeft;
            SendMail.Location = new Point(1107, 599);
            SendMail.Name = "SendMail";
            SendMail.Size = new Size(125, 55);
            SendMail.TabIndex = 13;
            SendMail.Text = "發送郵件";
            SendMail.TextAlign = ContentAlignment.MiddleRight;
            SendMail.UseVisualStyleBackColor = true;
            SendMail.Click += SendMail_Click;
            // 
            // FilterShippingOrder
            // 
            FilterShippingOrder.Font = new Font("Microsoft JhengHei UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            FilterShippingOrder.Location = new Point(124, 364);
            FilterShippingOrder.Name = "FilterShippingOrder";
            FilterShippingOrder.Size = new Size(304, 70);
            FilterShippingOrder.TabIndex = 14;
            FilterShippingOrder.Text = "篩選資料正確出貨單";
            FilterShippingOrder.UseVisualStyleBackColor = true;
            FilterShippingOrder.Click += Filter_Click;
            // 
            // Check_COA_with_Order
            // 
            Check_COA_with_Order.Font = new Font("Microsoft JhengHei UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            Check_COA_with_Order.Location = new Point(124, 463);
            Check_COA_with_Order.Name = "Check_COA_with_Order";
            Check_COA_with_Order.Size = new Size(304, 70);
            Check_COA_with_Order.TabIndex = 15;
            Check_COA_with_Order.Text = "核對 COA 與出貨單資料";
            Check_COA_with_Order.UseVisualStyleBackColor = true;
            Check_COA_with_Order.Click += Check_COA_with_Order_Click;
            // 
            // Output_COA
            // 
            Output_COA.Font = new Font("Microsoft JhengHei UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            Output_COA.Location = new Point(124, 568);
            Output_COA.Name = "Output_COA";
            Output_COA.Size = new Size(304, 70);
            Output_COA.TabIndex = 16;
            Output_COA.Text = "轉出 COA 資料";
            Output_COA.UseVisualStyleBackColor = true;
            Output_COA.Click += Output_COA_Click;
            // 
            // ShippingOrderPreview
            // 
            ShippingOrderPreview.Image = (Image)resources.GetObject("ShippingOrderPreview.Image");
            ShippingOrderPreview.ImageAlign = ContentAlignment.MiddleLeft;
            ShippingOrderPreview.Location = new Point(341, 265);
            ShippingOrderPreview.Name = "ShippingOrderPreview";
            ShippingOrderPreview.Size = new Size(125, 56);
            ShippingOrderPreview.TabIndex = 17;
            ShippingOrderPreview.Text = "出貨單預覽";
            ShippingOrderPreview.TextAlign = ContentAlignment.MiddleRight;
            ShippingOrderPreview.UseVisualStyleBackColor = true;
            ShippingOrderPreview.Click += ShippingOrderPreview_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1264, 681);
            Controls.Add(ShippingOrderPreview);
            Controls.Add(Output_COA);
            Controls.Add(Check_COA_with_Order);
            Controls.Add(FilterShippingOrder);
            Controls.Add(SendMail);
            Controls.Add(FilterResult_TextBox);
            Controls.Add(CheckResult_TextBox);
            Controls.Add(COA_Preview);
            Controls.Add(Clear);
            Controls.Add(listView1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(OpenDirectory);
            Controls.Add(COA_textBox);
            Controls.Add(ScanShipoutRecipt);
            Controls.Add(ShippingOrderNoTextBox);
            Name = "Form1";
            Text = "COA 與出貨單比對程式";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox ShippingOrderNoTextBox;
        private Button ScanShipoutRecipt;
        private TextBox COA_textBox;
        private Button OpenDirectory;
        private Label label1;
        private Label label2;
        private Label label3;
        private ListView listView1;
        private Button Clear;
        private Button COA_Preview;
        private TextBox CheckResult_TextBox;
        private TextBox FilterResult_TextBox;
        private Button SendMail;
        private Button FilterShippingOrder;
        private Button Check_COA_with_Order;
        private Button Output_COA;
        private Button ShippingOrderPreview;
    }
}