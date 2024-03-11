namespace ShippingOrderCOAFilter.View
{
    partial class SubForm
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
            Close = new Button();
            PreviewFiles = new ListView();
            SuspendLayout();
            // 
            // Close
            // 
            Close.Location = new Point(488, 330);
            Close.Name = "Close";
            Close.Size = new Size(72, 52);
            Close.TabIndex = 0;
            Close.Text = "Close";
            Close.UseVisualStyleBackColor = true;
            Close.Click += Close_Click;
            // 
            // PreviewFiles
            // 
            PreviewFiles.Location = new Point(25, 25);
            PreviewFiles.Name = "PreviewFiles";
            PreviewFiles.Size = new Size(439, 357);
            PreviewFiles.TabIndex = 1;
            PreviewFiles.UseCompatibleStateImageBehavior = false;
            PreviewFiles.View = System.Windows.Forms.View.Details;
            // 
            // SubForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 411);
            Controls.Add(PreviewFiles);
            Controls.Add(Close);
            Name = "SubForm";
            Text = "COA 資料預覽";
            ResumeLayout(false);
        }

        #endregion

        private Button Close;
        private ListView PreviewFiles;
    }
}