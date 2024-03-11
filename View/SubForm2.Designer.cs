namespace ShippingOrderCOAFilter.View
{
    partial class SubForm2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ShippingOrderCheckedListBox = new CheckedListBox();
            Close = new Button();
            Delete = new Button();
            SuspendLayout();
            // 
            // ShippingOrderCheckedListBox
            // 
            ShippingOrderCheckedListBox.FormattingEnabled = true;
            ShippingOrderCheckedListBox.Location = new Point(12, 12);
            ShippingOrderCheckedListBox.Name = "ShippingOrderCheckedListBox";
            ShippingOrderCheckedListBox.Size = new Size(253, 364);
            ShippingOrderCheckedListBox.TabIndex = 1;
            // 
            // Close
            // 
            Close.Location = new Point(163, 397);
            Close.Name = "Close";
            Close.Size = new Size(72, 52);
            Close.TabIndex = 2;
            Close.Text = "Close";
            Close.UseVisualStyleBackColor = true;
            Close.Click += Close_Click;
            // 
            // Delete
            // 
            Delete.Location = new Point(51, 397);
            Delete.Name = "Delete";
            Delete.Size = new Size(72, 52);
            Delete.TabIndex = 3;
            Delete.Text = "Delete";
            Delete.UseVisualStyleBackColor = true;
            Delete.Click += Delete_Click;
            // 
            // SubForm2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(284, 461);
            Controls.Add(Delete);
            Controls.Add(Close);
            Controls.Add(ShippingOrderCheckedListBox);
            Name = "SubForm2";
            Text = "出貨單項目預覽";
            ResumeLayout(false);
        }

        #endregion

        private CheckedListBox ShippingOrderCheckedListBox;
        private Button Close;
        private Button Delete;
    }
}