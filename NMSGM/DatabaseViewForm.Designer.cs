namespace NMSGM
{
    partial class DatabaseViewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseViewForm));
            this.olvBackups = new BrightIdeasSoftware.ObjectListView();
            this.olvBackupsTimestamp = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvBackupsComment = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvBackupType = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvBackupSeed = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvBackupsOnHold = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.btnExport = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnRestore = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.olvBackups)).BeginInit();
            this.SuspendLayout();
            // 
            // olvBackups
            // 
            this.olvBackups.AllColumns.Add(this.olvBackupsTimestamp);
            this.olvBackups.AllColumns.Add(this.olvBackupsComment);
            this.olvBackups.AllColumns.Add(this.olvBackupType);
            this.olvBackups.AllColumns.Add(this.olvBackupSeed);
            this.olvBackups.AllColumns.Add(this.olvBackupsOnHold);
            this.olvBackups.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
            this.olvBackups.CellEditUseWholeCell = false;
            this.olvBackups.CheckedAspectName = "";
            this.olvBackups.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvBackupsTimestamp,
            this.olvBackupsComment,
            this.olvBackupType,
            this.olvBackupSeed,
            this.olvBackupsOnHold});
            this.olvBackups.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvBackups.FullRowSelect = true;
            this.olvBackups.Location = new System.Drawing.Point(12, 23);
            this.olvBackups.Name = "olvBackups";
            this.olvBackups.ShowGroups = false;
            this.olvBackups.Size = new System.Drawing.Size(984, 446);
            this.olvBackups.TabIndex = 0;
            this.olvBackups.UseCompatibleStateImageBehavior = false;
            this.olvBackups.View = System.Windows.Forms.View.Details;
            this.olvBackups.CellEditFinished += new BrightIdeasSoftware.CellEditEventHandler(this.olvBackups_CellEditFinished);
            this.olvBackups.SubItemChecking += new System.EventHandler<BrightIdeasSoftware.SubItemCheckingEventArgs>(this.olvBackups_SubItemChecking);
            // 
            // olvBackupsTimestamp
            // 
            this.olvBackupsTimestamp.AspectName = "commitedTimeStamp";
            this.olvBackupsTimestamp.IsEditable = false;
            this.olvBackupsTimestamp.Text = "Timestamp";
            this.olvBackupsTimestamp.Width = 116;
            // 
            // olvBackupsComment
            // 
            this.olvBackupsComment.AspectName = "comment";
            this.olvBackupsComment.AutoCompleteEditor = false;
            this.olvBackupsComment.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.olvBackupsComment.CellEditUseWholeCell = true;
            this.olvBackupsComment.FillsFreeSpace = true;
            this.olvBackupsComment.Text = "Comment (doubleclick to edit)";
            this.olvBackupsComment.Width = 525;
            // 
            // olvBackupType
            // 
            this.olvBackupType.AspectName = "Type";
            this.olvBackupType.IsEditable = false;
            this.olvBackupType.Text = "Type";
            this.olvBackupType.Width = 62;
            // 
            // olvBackupSeed
            // 
            this.olvBackupSeed.AspectName = "decryptionSeed";
            this.olvBackupSeed.IsEditable = false;
            this.olvBackupSeed.Text = "Encryption Seed";
            this.olvBackupSeed.Width = 152;
            // 
            // olvBackupsOnHold
            // 
            this.olvBackupsOnHold.AspectName = "onHold";
            this.olvBackupsOnHold.AutoCompleteEditor = false;
            this.olvBackupsOnHold.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.olvBackupsOnHold.CheckBoxes = true;
            this.olvBackupsOnHold.Text = "on hold";
            this.olvBackupsOnHold.ToolTipText = "Items \"on hold\" will be excluded from automated database cleanup";
            this.olvBackupsOnHold.Width = 50;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(831, 475);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 2;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(912, 475);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Delete";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnRestore
            // 
            this.btnRestore.Location = new System.Drawing.Point(12, 475);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(108, 23);
            this.btnRestore.TabIndex = 4;
            this.btnRestore.Text = "Restore savegame";
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // DatabaseViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 510);
            this.Controls.Add(this.btnRestore);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.olvBackups);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DatabaseViewForm";
            this.Text = "No Man\'s SaveGame Manager: Database View";
            ((System.ComponentModel.ISupportInitialize)(this.olvBackups)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView olvBackups;
        private BrightIdeasSoftware.OLVColumn olvBackupsOnHold;
        private BrightIdeasSoftware.OLVColumn olvBackupsTimestamp;
        private System.Windows.Forms.Button btnExport;
        private BrightIdeasSoftware.OLVColumn olvBackupsComment;
        private BrightIdeasSoftware.OLVColumn olvBackupType;
        private BrightIdeasSoftware.OLVColumn olvBackupSeed;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnRestore;
    }
}