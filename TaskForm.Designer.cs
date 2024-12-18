﻿namespace PROJ
{
    partial class TaskForm
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
            txtTaskName = new TextBox();
            cmbCategory = new ComboBox();
            btnAdd = new Button();
            dtpDueDate = new DateTimePicker();
            cmbPriorityLevel = new ComboBox();
            txtDescription = new TextBox();
            btnCancel = new Button();
            cmbStatus = new ComboBox();
            btnManageCategories = new Button();
            SuspendLayout();
            // 
            // txtTaskName
            // 
            txtTaskName.Location = new Point(27, 47);
            txtTaskName.Name = "txtTaskName";
            txtTaskName.PlaceholderText = "Task Name...";
            txtTaskName.Size = new Size(227, 23);
            txtTaskName.TabIndex = 0;
            // 
            // cmbCategory
            // 
            cmbCategory.BackColor = Color.FromArgb(121, 116, 168);
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategory.FormattingEnabled = true;
            cmbCategory.Location = new Point(27, 93);
            cmbCategory.Name = "cmbCategory";
            cmbCategory.Size = new Size(164, 23);
            cmbCategory.TabIndex = 1;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.FromArgb(121, 116, 168);
            btnAdd.Location = new Point(30, 313);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(75, 23);
            btnAdd.TabIndex = 2;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // dtpDueDate
            // 
            dtpDueDate.Location = new Point(27, 134);
            dtpDueDate.Name = "dtpDueDate";
            dtpDueDate.Size = new Size(227, 23);
            dtpDueDate.TabIndex = 3;
            // 
            // cmbPriorityLevel
            // 
            cmbPriorityLevel.BackColor = Color.FromArgb(121, 116, 168);
            cmbPriorityLevel.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPriorityLevel.FormattingEnabled = true;
            cmbPriorityLevel.Location = new Point(27, 174);
            cmbPriorityLevel.Name = "cmbPriorityLevel";
            cmbPriorityLevel.Size = new Size(108, 23);
            cmbPriorityLevel.TabIndex = 4;
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(30, 209);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.PlaceholderText = "Description";
            txtDescription.Size = new Size(224, 73);
            txtDescription.TabIndex = 5;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(179, 313);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // cmbStatus
            // 
            cmbStatus.BackColor = Color.FromArgb(121, 116, 168);
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.FormattingEnabled = true;
            cmbStatus.Location = new Point(148, 174);
            cmbStatus.Name = "cmbStatus";
            cmbStatus.Size = new Size(106, 23);
            cmbStatus.TabIndex = 8;
            // 
            // btnManageCategories
            // 
            btnManageCategories.Location = new Point(200, 93);
            btnManageCategories.Name = "btnManageCategories";
            btnManageCategories.Size = new Size(54, 23);
            btnManageCategories.TabIndex = 10;
            btnManageCategories.Text = "+";
            btnManageCategories.UseVisualStyleBackColor = true;
            btnManageCategories.Click += btnManageCategories_Click;
            // 
            // TaskForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(299, 417);
            Controls.Add(btnManageCategories);
            Controls.Add(cmbStatus);
            Controls.Add(btnCancel);
            Controls.Add(txtDescription);
            Controls.Add(cmbPriorityLevel);
            Controls.Add(dtpDueDate);
            Controls.Add(btnAdd);
            Controls.Add(cmbCategory);
            Controls.Add(txtTaskName);
            Name = "TaskForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Task Form";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtTaskName;
        private ComboBox cmbCategory;
        private Button btnAdd;
        private DateTimePicker dtpDueDate;
        private ComboBox cmbPriorityLevel;
        private TextBox txtDescription;
        private Button btnCancel;
        private ComboBox cmbStatus;
        //private TextBox textBox1;
        private Button btnManageCategories;
    }
}