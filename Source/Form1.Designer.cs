namespace PDFtab
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.lvDoc = new System.Windows.Forms.ListView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.pbStatus = new System.Windows.Forms.ToolStripProgressBar();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnExecuteCSV = new System.Windows.Forms.Button();
            this.btnDestination = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkOpen = new System.Windows.Forms.CheckBox();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.btnLog = new System.Windows.Forms.Button();
            this.chkDestination = new System.Windows.Forms.CheckBox();
            this.chkTitle = new System.Windows.Forms.CheckBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.chkFrame = new System.Windows.Forms.CheckBox();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvDoc
            // 
            this.lvDoc.HideSelection = false;
            this.lvDoc.Location = new System.Drawing.Point(12, 46);
            this.lvDoc.Name = "lvDoc";
            this.lvDoc.Size = new System.Drawing.Size(461, 333);
            this.lvDoc.TabIndex = 0;
            this.lvDoc.UseCompatibleStateImageBehavior = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.toolStripStatusLabel2,
            this.pbStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 385);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(565, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(16, 17);
            this.lblStatus.Text = "...";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(332, 17);
            this.toolStripStatusLabel2.Spring = true;
            // 
            // pbStatus
            // 
            this.pbStatus.Name = "pbStatus";
            this.pbStatus.Size = new System.Drawing.Size(200, 16);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(12, 17);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 2;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            // 
            // btnExecuteCSV
            // 
            this.btnExecuteCSV.Location = new System.Drawing.Point(479, 220);
            this.btnExecuteCSV.Name = "btnExecuteCSV";
            this.btnExecuteCSV.Size = new System.Drawing.Size(75, 23);
            this.btnExecuteCSV.TabIndex = 3;
            this.btnExecuteCSV.Text = "Convert";
            this.btnExecuteCSV.UseVisualStyleBackColor = true;
            // 
            // btnDestination
            // 
            this.btnDestination.Enabled = false;
            this.btnDestination.Location = new System.Drawing.Point(404, 17);
            this.btnDestination.Name = "btnDestination";
            this.btnDestination.Size = new System.Drawing.Size(69, 23);
            this.btnDestination.TabIndex = 4;
            this.btnDestination.Text = "Destination";
            this.btnDestination.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(479, 249);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // chkOpen
            // 
            this.chkOpen.AutoSize = true;
            this.chkOpen.Location = new System.Drawing.Point(479, 128);
            this.chkOpen.Name = "chkOpen";
            this.chkOpen.Size = new System.Drawing.Size(74, 17);
            this.chkOpen.TabIndex = 6;
            this.chkOpen.Text = "auto open";
            this.chkOpen.UseVisualStyleBackColor = true;
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.Location = new System.Drawing.Point(479, 298);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(75, 23);
            this.btnOpenFolder.TabIndex = 7;
            this.btnOpenFolder.Text = "Output";
            this.btnOpenFolder.UseVisualStyleBackColor = true;
            // 
            // btnLog
            // 
            this.btnLog.Location = new System.Drawing.Point(479, 356);
            this.btnLog.Name = "btnLog";
            this.btnLog.Size = new System.Drawing.Size(75, 23);
            this.btnLog.TabIndex = 8;
            this.btnLog.Text = "log";
            this.btnLog.UseVisualStyleBackColor = true;
            // 
            // chkDestination
            // 
            this.chkDestination.AutoSize = true;
            this.chkDestination.Location = new System.Drawing.Point(346, 21);
            this.chkDestination.Name = "chkDestination";
            this.chkDestination.Size = new System.Drawing.Size(62, 17);
            this.chkDestination.TabIndex = 9;
            this.chkDestination.Text = "change";
            this.chkDestination.UseVisualStyleBackColor = true;
            // 
            // chkTitle
            // 
            this.chkTitle.AutoSize = true;
            this.chkTitle.Location = new System.Drawing.Point(479, 80);
            this.chkTitle.Name = "chkTitle";
            this.chkTitle.Size = new System.Drawing.Size(68, 17);
            this.chkTitle.TabIndex = 10;
            this.chkTitle.Text = "Headtitle";
            this.chkTitle.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(479, 327);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 11;
            this.btnClear.Text = "clear";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // chkFrame
            // 
            this.chkFrame.AutoSize = true;
            this.chkFrame.Location = new System.Drawing.Point(479, 57);
            this.chkFrame.Name = "chkFrame";
            this.chkFrame.Size = new System.Drawing.Size(45, 17);
            this.chkFrame.TabIndex = 12;
            this.chkFrame.Text = "Grid";
            this.chkFrame.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 407);
            this.Controls.Add(this.chkFrame);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.chkTitle);
            this.Controls.Add(this.chkDestination);
            this.Controls.Add(this.btnLog);
            this.Controls.Add(this.btnOpenFolder);
            this.Controls.Add(this.chkOpen);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDestination);
            this.Controls.Add(this.btnExecuteCSV);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.lvDoc);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PDFtab";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvDoc;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripProgressBar pbStatus;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnExecuteCSV;
        private System.Windows.Forms.Button btnDestination;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkOpen;
        private System.Windows.Forms.Button btnOpenFolder;
        private System.Windows.Forms.Button btnLog;
        private System.Windows.Forms.CheckBox chkDestination;
        private System.Windows.Forms.CheckBox chkTitle;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.CheckBox chkFrame;
    }
}

