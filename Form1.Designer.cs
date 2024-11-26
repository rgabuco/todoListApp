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
            btnNewTask = new Button();
            SuspendLayout();
            // 
            // listView1
            // 
            listView1.Location = new Point(12, 63);
            listView1.Name = "listView1";
            listView1.Size = new Size(653, 283);
            listView1.TabIndex = 0;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            // 
            // btnNewTask
            // 
            btnNewTask.BackColor = Color.FromArgb(121, 116, 168);
            btnNewTask.Location = new Point(12, 24);
            btnNewTask.Name = "btnNewTask";
            btnNewTask.Size = new Size(75, 23);
            btnNewTask.TabIndex = 1;
            btnNewTask.Text = "New Task";
            btnNewTask.UseVisualStyleBackColor = false;
            btnNewTask.Click += btnNewTask_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(677, 358);
            Controls.Add(btnNewTask);
            Controls.Add(listView1);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Welcome To Task Management";
            Load += Form1_Load_1;
            ResumeLayout(false);
        }

        #endregion

        private ListView listView1;
        private Button btnNewTask;
    }
}
