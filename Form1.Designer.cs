namespace PROJ
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
            listView1 = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            columnHeader4 = new ColumnHeader();
            columnHeader5 = new ColumnHeader();
            columnHeader6 = new ColumnHeader();
            columnHeader7 = new ColumnHeader();
            columnHeader8 = new ColumnHeader();
            btnNewTask = new Button();
            cmbCategory = new ComboBox();
            cmbPriority = new ComboBox();
            cmbStatus = new ComboBox();
            textBox1 = new TextBox();
            btnRefresh = new Button();
            label1 = new Label();
            btnExpotToCsv = new Button();
            SuspendLayout();
            // 
            // listView1
            // 
            listView1.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4, columnHeader5, columnHeader6, columnHeader7, columnHeader8 });
            listView1.Location = new Point(12, 260);
            listView1.Name = "listView1";
            listView1.Size = new Size(1053, 283);
            listView1.TabIndex = 0;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Task Name";
            columnHeader1.Width = 150;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Category";
            columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Description";
            columnHeader3.Width = 350;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "Due Date";
            columnHeader4.Width = 100;
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "Priority Level";
            columnHeader5.Width = 100;
            // 
            // columnHeader6
            // 
            columnHeader6.Text = "Status";
            columnHeader6.Width = 100;
            // 
            // columnHeader7
            // 
            columnHeader7.Text = "";
            // 
            // columnHeader8
            // 
            columnHeader8.Text = "Actions";
            columnHeader8.Width = 100;
            // 
            // btnNewTask
            // 
            btnNewTask.BackColor = Color.FromArgb(121, 116, 168);
            btnNewTask.Font = new Font("Segoe UI", 10F);
            btnNewTask.Location = new Point(12, 163);
            btnNewTask.Name = "btnNewTask";
            btnNewTask.Size = new Size(89, 32);
            btnNewTask.TabIndex = 1;
            btnNewTask.Text = "New Task";
            btnNewTask.UseVisualStyleBackColor = false;
            btnNewTask.Click += btnNewTask_Click;
            // 
            // cmbCategory
            // 
            cmbCategory.FormattingEnabled = true;
            cmbCategory.Location = new Point(23, 214);
            cmbCategory.Name = "cmbCategory";
            cmbCategory.Size = new Size(121, 23);
            cmbCategory.TabIndex = 2;
            // 
            // cmbPriority
            // 
            cmbPriority.FormattingEnabled = true;
            cmbPriority.Location = new Point(557, 214);
            cmbPriority.Name = "cmbPriority";
            cmbPriority.Size = new Size(121, 23);
            cmbPriority.TabIndex = 3;
            // 
            // cmbStatus
            // 
            cmbStatus.FormattingEnabled = true;
            cmbStatus.Location = new Point(727, 214);
            cmbStatus.Name = "cmbStatus";
            cmbStatus.Size = new Size(121, 23);
            cmbStatus.TabIndex = 4;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(213, 214);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(282, 23);
            textBox1.TabIndex = 5;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(943, 216);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(75, 23);
            btnRefresh.TabIndex = 6;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(904, 18);
            label1.Name = "label1";
            label1.Size = new Size(143, 15);
            label1.TabIndex = 7;
            label1.Text = "3 days Weather Forecast";
            // 
            // btnExpotToCsv
            // 
            btnExpotToCsv.Font = new Font("Segoe UI", 9F);
            btnExpotToCsv.Location = new Point(117, 163);
            btnExpotToCsv.Name = "btnExpotToCsv";
            btnExpotToCsv.Size = new Size(97, 32);
            btnExpotToCsv.TabIndex = 8;
            btnExpotToCsv.Text = "Export to CSV";
            btnExpotToCsv.UseVisualStyleBackColor = true;
            btnExpotToCsv.Click += BtnExpotToCsv_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1093, 564);
            Controls.Add(btnExpotToCsv);
            Controls.Add(label1);
            Controls.Add(btnRefresh);
            Controls.Add(textBox1);
            Controls.Add(cmbStatus);
            Controls.Add(cmbPriority);
            Controls.Add(cmbCategory);
            Controls.Add(btnNewTask);
            Controls.Add(listView1);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Welcome To Task Management";
            Load += Form1_Load_1;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        public ListView listView1;
        private Button btnNewTask;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
        private ColumnHeader columnHeader7;
        private ColumnHeader columnHeader8;
        private ComboBox cmbCategory;
        private ComboBox cmbPriority;
        private ComboBox cmbStatus;
        private TextBox textBox1;
        private Button btnRefresh;
        private Label label1;
        private Button btnExpotToCsv;
    }
}
