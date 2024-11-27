using System;
using System.Windows.Forms;

namespace PROJ
{
    public partial class Form2 : Form
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();
        public bool IsEditMode { get; set; }
        public bool IsDeleteAction { get; private set; } = false;

        public string TaskName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DueDate { get; set; } = string.Empty;
        public string PriorityLevel { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        private const string CategoryFilePath = "categories.txt";


        // Store the selected item for editing purposes
        private ListViewItem? _selectedItem;

        // Constructor for adding a new task
        public Form2()
        {
            InitializeComponent();
            InitializeComboBox();
            IsEditMode = false;
        }

        // Constructor for editing an existing task
        public Form2(string taskName, string category, string description, string dueDate, string priorityLevel, string status, ListViewItem? selectedItem = null)
        {
            InitializeComponent();
            InitializeComboBox();

            IsEditMode = true; 

            // Set the selected item for updating
            _selectedItem = selectedItem ?? throw new ArgumentNullException(nameof(selectedItem));

            TaskName = taskName;
            Category = category;
            Description = description;
            DueDate = dueDate;
            PriorityLevel = priorityLevel;
            Status = status;

            // Pre-populate the form fields with the existing task data
            txtTaskName.Text = taskName;
            cmbCategory.SelectedItem = category;
            txtDescription.Text = description;
            dtpDueDate.Value = DateTime.Parse(dueDate);
            cmbPriorityLevel.SelectedItem = priorityLevel;
            cmbStatus.SelectedItem = status; 

            // Change the button text to "Save" and "Delete" when editing
            btnAdd.Text = "Save";
            btnCancel.Text = "Delete";
        }


        private void InitializeComboBox()
        {
            var categories = LoadCategoriesFromFile();
            if (categories.Any())
            {
                cmbCategory.Items.AddRange(categories.ToArray());
            }
            

            cmbPriorityLevel.Items.Add("Priority Level"); 
            cmbPriorityLevel.Items.Add("Low");
            cmbPriorityLevel.Items.Add("Medium");
            cmbPriorityLevel.Items.Add("High");

            cmbStatus.Items.Add("Status"); 
            cmbStatus.Items.Add("Starting");
            cmbStatus.Items.Add("In Progress");
            cmbStatus.Items.Add("Complete");


            if (cmbCategory.Items.Count > 0)
            {
                cmbCategory.SelectedIndex = 0;
            }
            cmbPriorityLevel.SelectedIndex = 0;
            cmbStatus.SelectedIndex = 0;

            
            cmbCategory.DrawMode = DrawMode.OwnerDrawFixed;
            cmbPriorityLevel.DrawMode = DrawMode.OwnerDrawFixed;
            cmbStatus.DrawMode = DrawMode.OwnerDrawFixed;


            Color customColor = ColorTranslator.FromHtml("#7974A8");


            cmbCategory.DrawItem += (sender, e) =>
            {
                if (e.Index < 0) return;
                if (e.Index == cmbCategory.SelectedIndex)
                {
                    e.Graphics.FillRectangle(new SolidBrush(customColor), e.Bounds);
                    e.Graphics.DrawString(cmbCategory.Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds);
                }
                else
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.White), e.Bounds);
                    e.Graphics.DrawString(cmbCategory.Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds);
                }
            };

            cmbPriorityLevel.DrawItem += (sender, e) =>
            {
                if (e.Index == cmbPriorityLevel.SelectedIndex)
                {
                    e.Graphics.FillRectangle(new SolidBrush(customColor), e.Bounds);
                    e.Graphics.DrawString(cmbPriorityLevel.Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds);
                }
                else
                {
                    if (cmbPriorityLevel.Items[e.Index].ToString() == "Priority Level")
                    {
                        e.Graphics.FillRectangle(new SolidBrush(Color.White), e.Bounds);
                        e.Graphics.DrawString(cmbPriorityLevel.Items[e.Index].ToString(), e.Font, Brushes.Gray, e.Bounds);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(new SolidBrush(Color.White), e.Bounds);
                        e.Graphics.DrawString(cmbPriorityLevel.Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds);
                    }
                }
            };

            cmbStatus.DrawItem += (sender, e) =>
            {
                if (e.Index == cmbStatus.SelectedIndex)
                {
                    e.Graphics.FillRectangle(new SolidBrush(customColor), e.Bounds);
                    e.Graphics.DrawString(cmbStatus.Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds);
                }
                else
                {
                    if (cmbStatus.Items[e.Index].ToString() == "Status")
                    {
                        e.Graphics.FillRectangle(new SolidBrush(Color.White), e.Bounds);
                        e.Graphics.DrawString(cmbStatus.Items[e.Index].ToString(), e.Font, Brushes.Gray, e.Bounds);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(new SolidBrush(Color.White), e.Bounds);
                        e.Graphics.DrawString(cmbStatus.Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds);
                    }
                }
            };
        }

        private void btnAdd_Click(object? sender, EventArgs e)
        {
            string taskName = txtTaskName.Text.Trim();
            string category = cmbCategory.SelectedItem?.ToString();
            string description = txtDescription.Text.Trim();
            DateTime dueDate = dtpDueDate.Value;
            string priorityLevel = cmbPriorityLevel.SelectedItem?.ToString();
            string status = cmbStatus.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(taskName) || string.IsNullOrWhiteSpace(category) ||
                string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(priorityLevel) ||
                string.IsNullOrWhiteSpace(status))
            {
                MessageBox.Show("Please fill in all fields.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (IsEditMode && _selectedItem != null)
                {
                    // Update the task in the database first
                    dbHelper.UpdateTask((int)_selectedItem.Tag, taskName, category, description, dueDate, priorityLevel, status);

                    // After updating the database, update the ListView item
                    _selectedItem.SubItems[0].Text = taskName;
                    _selectedItem.SubItems[1].Text = category;
                    _selectedItem.SubItems[2].Text = description;
                    _selectedItem.SubItems[3].Text = dueDate.ToString("yyyy-MM-dd");
                    _selectedItem.SubItems[4].Text = priorityLevel;
                    _selectedItem.SubItems[5].Text = status;

                    // Refresh the ListView in Form1 to reflect changes immediately
                    (this.Owner as Form1)?.RefreshListView();
                }
                else
                {
                    dbHelper.AddTask(taskName, category, description, dueDate, priorityLevel, status);

                    // Add the new task to the ListView in Form1
                    (this.Owner as Form1)?.AddTaskToListView(taskName, category, dueDate.ToString("yyyy-MM-dd"), description, priorityLevel, status, dbHelper.GetLastInsertedTaskId());
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            if (IsEditMode)
            {
                if (MessageBox.Show("Are you sure you want to delete this task?", "Delete Task", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    IsDeleteAction = true;
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
        private void btnManageCategories_Click(object sender, EventArgs e)
        {
            List<string> currentCategories = cmbCategory.Items.Cast<string>().ToList();

            using (var categoryForm = new CategoryForm(currentCategories))
            {
                if (categoryForm.ShowDialog() == DialogResult.OK)
                {
                    // Update ComboBox with the modified list of categories
                    cmbCategory.Items.Clear();
                    foreach (var category in categoryForm.Categories)
                    {
                        cmbCategory.Items.Add(category);
                    }

                    // Save updated categories to file
                    SaveCategoriesToFile(categoryForm.Categories);
                }
            }
        }
        private List<string> LoadCategoriesFromFile()
        {
            if (File.Exists(CategoryFilePath))
            {
                return File.ReadAllLines(CategoryFilePath).ToList();
            }
            return new List<string>();
        }
        private void SaveCategoriesToFile(List<string> categories)
        {
            File.WriteAllLines(CategoryFilePath, categories);
        }

    }
}
