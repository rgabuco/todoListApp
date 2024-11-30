namespace PROJ
{
    partial class CategoryForm
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
            btnAddCategory = new Button();
            btnDeleteCategory = new Button();
            txtNewCategory = new TextBox();
            lstCategories = new ListBox();
            btnSave = new Button();
            SuspendLayout();
            // 
            // btnAddCategory
            // 
            btnAddCategory.Location = new Point(232, 34);
            btnAddCategory.Name = "btnAddCategory";
            btnAddCategory.Size = new Size(75, 23);
            btnAddCategory.TabIndex = 0;
            btnAddCategory.Text = "Add";
            btnAddCategory.UseVisualStyleBackColor = true;
            btnAddCategory.Click += btnAddCategory_Click;
            // 
            // btnDeleteCategory
            // 
            btnDeleteCategory.Location = new Point(232, 199);
            btnDeleteCategory.Name = "btnDeleteCategory";
            btnDeleteCategory.Size = new Size(75, 23);
            btnDeleteCategory.TabIndex = 1;
            btnDeleteCategory.Text = "Delete";
            btnDeleteCategory.UseVisualStyleBackColor = true;
            btnDeleteCategory.Click += btnDeleteCategory_Click;
            // 
            // txtNewCategory
            // 
            txtNewCategory.Location = new Point(30, 34);
            txtNewCategory.Name = "txtNewCategory";
            txtNewCategory.Size = new Size(178, 23);
            txtNewCategory.TabIndex = 2;
            // 
            // lstCategories
            // 
            lstCategories.FormattingEnabled = true;
            lstCategories.ItemHeight = 15;
            lstCategories.Location = new Point(30, 83);
            lstCategories.Name = "lstCategories";
            lstCategories.Size = new Size(178, 139);
            lstCategories.TabIndex = 3;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(232, 261);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 4;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // CategoryForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(337, 309);
            Controls.Add(btnSave);
            Controls.Add(lstCategories);
            Controls.Add(txtNewCategory);
            Controls.Add(btnDeleteCategory);
            Controls.Add(btnAddCategory);
            Name = "CategoryForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CatgeroyForm";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnAddCategory;
        private Button btnDeleteCategory;
        private TextBox txtNewCategory;
        private ListBox lstCategories;
        private Button btnSave;
    }
}