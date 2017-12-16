namespace Textual_Affective_Properties_Analyzer
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.NormDataGrid = new System.Windows.Forms.DataGridView();
            this.AddColumnButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.EncodingDropdown = new System.Windows.Forms.ComboBox();
            this.FilenameLabel = new System.Windows.Forms.Label();
            this.StartButton = new System.Windows.Forms.Button();
            this.ScanSubfolderCheckbox = new System.Windows.Forms.CheckBox();
            this.BgWorker = new System.ComponentModel.BackgroundWorker();
            this.FolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.PunctuationBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.WordCaseSensitiveCheckbox = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.LoadNormsButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.StopListLabel = new System.Windows.Forms.Label();
            this.FunctionWordTextBox = new System.Windows.Forms.TextBox();
            this.StopListButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.CountsAndSumsCheckbox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.NormDataGrid)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // NormDataGrid
            // 
            this.NormDataGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.NormDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.NormDataGrid.Location = new System.Drawing.Point(12, 12);
            this.NormDataGrid.MaximumSize = new System.Drawing.Size(450, 410);
            this.NormDataGrid.MinimumSize = new System.Drawing.Size(450, 420);
            this.NormDataGrid.Name = "NormDataGrid";
            this.NormDataGrid.Size = new System.Drawing.Size(450, 420);
            this.NormDataGrid.TabIndex = 0;
            this.NormDataGrid.VirtualMode = true;
            this.NormDataGrid.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.NormDataGrid_DataError);
            this.NormDataGrid.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.NormDataGrid_UserAddedRow);
            // 
            // AddColumnButton
            // 
            this.AddColumnButton.Location = new System.Drawing.Point(31, 29);
            this.AddColumnButton.Name = "AddColumnButton";
            this.AddColumnButton.Size = new System.Drawing.Size(143, 23);
            this.AddColumnButton.TabIndex = 1;
            this.AddColumnButton.Text = "Add New Column to Sheet";
            this.AddColumnButton.UseVisualStyleBackColor = true;
            this.AddColumnButton.Click += new System.EventHandler(this.AddColumnButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Text Encoding:";
            // 
            // EncodingDropdown
            // 
            this.EncodingDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.EncodingDropdown.FormattingEnabled = true;
            this.EncodingDropdown.Location = new System.Drawing.Point(9, 41);
            this.EncodingDropdown.Name = "EncodingDropdown";
            this.EncodingDropdown.Size = new System.Drawing.Size(180, 21);
            this.EncodingDropdown.TabIndex = 11;
            // 
            // FilenameLabel
            // 
            this.FilenameLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FilenameLabel.Location = new System.Drawing.Point(12, 449);
            this.FilenameLabel.Name = "FilenameLabel";
            this.FilenameLabel.Size = new System.Drawing.Size(856, 16);
            this.FilenameLabel.TabIndex = 15;
            this.FilenameLabel.Text = "Waiting to analyze texts...";
            // 
            // StartButton
            // 
            this.StartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartButton.Location = new System.Drawing.Point(687, 333);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(152, 47);
            this.StartButton.TabIndex = 14;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // ScanSubfolderCheckbox
            // 
            this.ScanSubfolderCheckbox.AutoSize = true;
            this.ScanSubfolderCheckbox.Location = new System.Drawing.Point(709, 386);
            this.ScanSubfolderCheckbox.Name = "ScanSubfolderCheckbox";
            this.ScanSubfolderCheckbox.Size = new System.Drawing.Size(108, 17);
            this.ScanSubfolderCheckbox.TabIndex = 13;
            this.ScanSubfolderCheckbox.Text = "Scan subfolders?";
            this.ScanSubfolderCheckbox.UseVisualStyleBackColor = true;
            // 
            // BgWorker
            // 
            this.BgWorker.WorkerSupportsCancellation = true;
            this.BgWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BgWorker_DoWork);
            this.BgWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BgWorker_RunWorkerCompleted);
            // 
            // FolderBrowser
            // 
            this.FolderBrowser.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.FolderBrowser.ShowNewFolderButton = false;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.FileName = "Repeatalizer.csv";
            this.saveFileDialog.Filter = "CSV Files|*.csv";
            this.saveFileDialog.Title = "Please choose where to save your output";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(173, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Characters to Remove from Words:";
            // 
            // PunctuationBox
            // 
            this.PunctuationBox.AcceptsTab = true;
            this.PunctuationBox.Location = new System.Drawing.Point(7, 89);
            this.PunctuationBox.Name = "PunctuationBox";
            this.PunctuationBox.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.PunctuationBox.Size = new System.Drawing.Size(180, 20);
            this.PunctuationBox.TabIndex = 16;
            this.PunctuationBox.Text = ";:\"@#$%^&\t*(){}\\|,/<>`~[].?!";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CountsAndSumsCheckbox);
            this.groupBox1.Controls.Add(this.WordCaseSensitiveCheckbox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.EncodingDropdown);
            this.groupBox1.Controls.Add(this.PunctuationBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(665, 136);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(202, 174);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Text Analysis Options";
            // 
            // WordCaseSensitiveCheckbox
            // 
            this.WordCaseSensitiveCheckbox.AutoSize = true;
            this.WordCaseSensitiveCheckbox.Location = new System.Drawing.Point(7, 123);
            this.WordCaseSensitiveCheckbox.Name = "WordCaseSensitiveCheckbox";
            this.WordCaseSensitiveCheckbox.Size = new System.Drawing.Size(184, 17);
            this.WordCaseSensitiveCheckbox.TabIndex = 20;
            this.WordCaseSensitiveCheckbox.Text = "Word analyses are case-sensitive";
            this.WordCaseSensitiveCheckbox.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.LoadNormsButton);
            this.groupBox2.Controls.Add(this.AddColumnButton);
            this.groupBox2.Location = new System.Drawing.Point(665, 22);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 100);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Norm Options";
            // 
            // LoadNormsButton
            // 
            this.LoadNormsButton.Location = new System.Drawing.Point(31, 62);
            this.LoadNormsButton.Name = "LoadNormsButton";
            this.LoadNormsButton.Size = new System.Drawing.Size(143, 23);
            this.LoadNormsButton.TabIndex = 2;
            this.LoadNormsButton.Text = "Load Norms from File";
            this.LoadNormsButton.UseVisualStyleBackColor = true;
            this.LoadNormsButton.Click += new System.EventHandler(this.LoadNormsButton_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "Norms.txt";
            this.openFileDialog.Filter = "Tab-delimited Text files|*.txt";
            // 
            // StopListLabel
            // 
            this.StopListLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.StopListLabel.Location = new System.Drawing.Point(491, 10);
            this.StopListLabel.Name = "StopListLabel";
            this.StopListLabel.Size = new System.Drawing.Size(150, 28);
            this.StopListLabel.TabIndex = 21;
            this.StopListLabel.Text = "Words to Omit (Case Insensitive)";
            this.StopListLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FunctionWordTextBox
            // 
            this.FunctionWordTextBox.Location = new System.Drawing.Point(491, 41);
            this.FunctionWordTextBox.Multiline = true;
            this.FunctionWordTextBox.Name = "FunctionWordTextBox";
            this.FunctionWordTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.FunctionWordTextBox.Size = new System.Drawing.Size(150, 362);
            this.FunctionWordTextBox.TabIndex = 20;
            // 
            // StopListButton
            // 
            this.StopListButton.Location = new System.Drawing.Point(491, 409);
            this.StopListButton.Name = "StopListButton";
            this.StopListButton.Size = new System.Drawing.Size(150, 23);
            this.StopListButton.TabIndex = 3;
            this.StopListButton.Text = "Load Stop List";
            this.StopListButton.UseVisualStyleBackColor = true;
            this.StopListButton.Click += new System.EventHandler(this.StopListButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(709, 409);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(102, 23);
            this.CancelButton.TabIndex = 22;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // CountsAndSumsCheckbox
            // 
            this.CountsAndSumsCheckbox.AutoSize = true;
            this.CountsAndSumsCheckbox.Location = new System.Drawing.Point(6, 146);
            this.CountsAndSumsCheckbox.Name = "CountsAndSumsCheckbox";
            this.CountsAndSumsCheckbox.Size = new System.Drawing.Size(188, 17);
            this.CountsAndSumsCheckbox.TabIndex = 21;
            this.CountsAndSumsCheckbox.Text = "Include counts and sums in output";
            this.CountsAndSumsCheckbox.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 471);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.StopListButton);
            this.Controls.Add(this.StopListLabel);
            this.Controls.Add(this.FunctionWordTextBox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.FilenameLabel);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.ScanSubfolderCheckbox);
            this.Controls.Add(this.NormDataGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(900, 510);
            this.MinimumSize = new System.Drawing.Size(900, 510);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TAPA: Textual Affective Properties Analyzer";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.NormDataGrid)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView NormDataGrid;
        private System.Windows.Forms.Button AddColumnButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox EncodingDropdown;
        private System.Windows.Forms.Label FilenameLabel;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.CheckBox ScanSubfolderCheckbox;
        private System.ComponentModel.BackgroundWorker BgWorker;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowser;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox PunctuationBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button LoadNormsButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.CheckBox WordCaseSensitiveCheckbox;
        private System.Windows.Forms.Label StopListLabel;
        private System.Windows.Forms.TextBox FunctionWordTextBox;
        private System.Windows.Forms.Button StopListButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.CheckBox CountsAndSumsCheckbox;
    }
}

