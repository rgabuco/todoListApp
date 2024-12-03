using PROJ.Database;
using PROJ.Services;
using System;
using System.Windows.Forms;

namespace PROJ
{
    public partial class TaskForm : Form
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();
        private SearchAndFilterService searchAndFilterService;
        public bool IsEditMode { get; set; }
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
        public TaskForm(SearchAndFilterService searchAndFilterService)
        {
            InitializeComponent();
            InitializeComboBox();
            IsEditMode = false;
            this.searchAndFilterService = searchAndFilterService;
        }

        // Constructor for editing a task
        public TaskForm(string taskName, string category, string description, string dueDate, string priorityLevel, string status, ListViewItem? selectedItem, SearchAndFilterService searchAndFilterService)
        {
            InitializeComponent();
            InitializeComboBox();

            IsEditMode = true;
            this.searchAndFilterService = searchAndFilterService;

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

            // Change the button text to "Save" when editing
            btnAdd.Text = "Save";
        }

        private void InitializeComboBox()
        {
            var categories = LoadCategoriesFromFile();
            if (categories.Any())
            {
                cmbCategory.Items.AddRange(categories.ToArray());
            }

            // Add items for Priority Level and Status
            cmbPriorityLevel.Items.AddRange(new string[] { "Low", "Medium", "High" });
            cmbStatus.Items.AddRange(new string[] { "Not Started", "In Progress", "Completed" });

            // Set placeholder text
            cmbCategory.Text = "Categories";
            cmbPriorityLevel.Text = "Priority Level";
            cmbStatus.Text = "Status";

            // These events handle clearing placeholder text
            cmbPriorityLevel.DropDown += CmbPriorityLevel_DropDown;
            cmbStatus.DropDown += CmbStatus_DropDown;
            cmbPriorityLevel.Leave += CmbPriorityLevel_Leave;
            cmbStatus.Leave += CmbStatus_Leave;
            cmbPriorityLevel.DropDown += CmbPriorityLevel_DropDown;
            cmbPriorityLevel.Leave += CmbPriorityLevel_Leave;

            // These customize the look of the dropdowns
            cmbCategory.DrawMode = DrawMode.OwnerDrawFixed;
            cmbPriorityLevel.DrawMode = DrawMode.OwnerDrawFixed;
            cmbStatus.DrawMode = DrawMode.OwnerDrawFixed;

            Color customColor = ColorTranslator.FromHtml("#7974A8");

            cmbCategory.DrawItem += (sender, e) =>
            {
                e.DrawBackground();

                Color backgroundColor = ColorTranslator.FromHtml("#7974A8"); // Purple background
                Color textColor = Color.White; // White text color for contrast

                if (e.State.HasFlag(DrawItemState.Selected))
                {
                    // Apply purple background even when the item is selected
                    e.Graphics.FillRectangle(new SolidBrush(backgroundColor), e.Bounds);
                }
                else
                {
                    e.Graphics.FillRectangle(new SolidBrush(backgroundColor), e.Bounds); // Default background
                }

                if (e.Index >= 0)
                {
                    // Draw actual item text
                    e.Graphics.DrawString(cmbCategory.Items[e.Index].ToString(), e.Font, new SolidBrush(textColor), e.Bounds);
                }
                else
                {
                    // Draw placeholder text
                    e.Graphics.DrawString("Categories", e.Font, new SolidBrush(textColor), e.Bounds);
                }

                e.DrawFocusRectangle();
            };

            cmbPriorityLevel.DrawItem += (sender, e) =>
            {
                e.DrawBackground();

                Color backgroundColor = ColorTranslator.FromHtml("#7974A8"); // Purple background
                Color textColor = Color.White; // White text color for contrast

                if (e.State.HasFlag(DrawItemState.Selected))
                {
                    e.Graphics.FillRectangle(new SolidBrush(backgroundColor), e.Bounds);
                }
                else
                {
                    e.Graphics.FillRectangle(new SolidBrush(backgroundColor), e.Bounds); // Default background
                }

                if (e.Index >= 0)
                {
                    e.Graphics.DrawString(cmbPriorityLevel.Items[e.Index].ToString(), e.Font, new SolidBrush(textColor), e.Bounds);
                }
                else
                {
                    e.Graphics.DrawString("Priority Level", e.Font, new SolidBrush(textColor), e.Bounds);
                }

                e.DrawFocusRectangle();
            };

            cmbStatus.DrawItem += (sender, e) =>
            {
                e.DrawBackground();

                Color backgroundColor = ColorTranslator.FromHtml("#7974A8"); // Purple background
                Color textColor = Color.White;

                if (e.State.HasFlag(DrawItemState.Selected))
                {
                    e.Graphics.FillRectangle(new SolidBrush(backgroundColor), e.Bounds);
                }
                else
                {
                    e.Graphics.FillRectangle(new SolidBrush(backgroundColor), e.Bounds);
                }

                if (e.Index >= 0)
                {
                    e.Graphics.DrawString(cmbStatus.Items[e.Index].ToString(), e.Font, new SolidBrush(textColor), e.Bounds);
                }
                else
                {
                    e.Graphics.DrawString("Status", e.Font, new SolidBrush(textColor), e.Bounds);
                }

                e.DrawFocusRectangle();
            };
        }

        private void btnAdd_Click(object? sender, EventArgs e)
        {
            // Get all the info from the input fields

            string taskName = txtTaskName.Text.Trim();
            string category = cmbCategory.SelectedItem?.ToString();
            string description = txtDescription.Text.Trim();
            DateTime dueDate = dtpDueDate.Value;
            string priorityLevel = cmbPriorityLevel.SelectedItem?.ToString();
            string status = cmbStatus.SelectedItem?.ToString();

            // Check if the inputs are empty
            if (string.IsNullOrWhiteSpace(taskName) || string.IsNullOrWhiteSpace(category) ||
                string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(priorityLevel) ||
                string.IsNullOrWhiteSpace(status))
            {
                MessageBox.Show("Please fill in all fields.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cmbCategory.SelectedItem == null)
            {
                MessageBox.Show("Please select a category.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var parentForm = this.Owner as Form1;

                if (IsEditMode && _selectedItem != null)
                {
                    // Update the task in the database
                    dbHelper.UpdateTask((int)_selectedItem.Tag, taskName, category, description, dueDate, priorityLevel, status);

                    // Update the ListView with the new details
                    _selectedItem.SubItems[0].Text = taskName;
                    _selectedItem.SubItems[1].Text = category;
                    _selectedItem.SubItems[2].Text = description;
                    _selectedItem.SubItems[3].Text = dueDate.ToString("yyyy-MM-dd");
                    _selectedItem.SubItems[4].Text = priorityLevel;
                    _selectedItem.SubItems[5].Text = status;

                    // Refresh the main form
                    parentForm?.RefreshListView();
                    parentForm?.UpdateCategoryDropdown();
                }
                else
                {
                    // Add a new task to the database
                    dbHelper.AddTask(taskName, category, description, dueDate, priorityLevel, status);

                    // Add the new task to the ListView using SearchAndFilterService
                    searchAndFilterService.AddTaskToListView(
                        parentForm?.listView1,
                        new TaskModel
                        {
                            TaskName = taskName,
                            Category = category,
                            Description = description,
                            DueDate = dueDate,
                            PriorityLevel = priorityLevel,
                            Status = status,
                            Id = dbHelper.GetLastInsertedTaskId()
                        }
                    );

                    // Refresh the categories
                    parentForm?.RefreshListView();
                    parentForm?.UpdateCategoryDropdown();
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
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnManageCategories_Click(object sender, EventArgs e)
        {
            // Open the category management form
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

        private void CmbPriorityLevel_DropDown(object sender, EventArgs e)
        {
            if (cmbPriorityLevel.Text == "Priority Level")
            {
                cmbPriorityLevel.Text = ""; // Clear placeholder when dropdown is clicked
            }
        }

        private void CmbStatus_DropDown(object sender, EventArgs e)
        {
            if (cmbStatus.Text == "Status")
            {
                cmbStatus.Text = ""; // Clear placeholder when dropdown is clicked
            }
        }

        private void CmbPriorityLevel_Leave(object sender, EventArgs e)
        {
            if (cmbPriorityLevel.SelectedIndex == -1 || string.IsNullOrWhiteSpace(cmbPriorityLevel.Text))
            {
                cmbPriorityLevel.Text = "Priority Level"; // Reset placeholder if nothing is selected
            }
        }

        private void CmbStatus_Leave(object sender, EventArgs e)
        {
            if (cmbStatus.SelectedIndex == -1 || string.IsNullOrWhiteSpace(cmbStatus.Text))
            {
                cmbStatus.Text = "Status"; // Reset placeholder if nothing is selected
            }
        }

        private void CmbCategory_DropDown(object sender, EventArgs e)
        {
            if (cmbCategory.Text == "Categories")
            {
                cmbCategory.Text = ""; // Clear placeholder when dropdown is clicked
            }
        }

        private void CmbCategory_Leave(object sender, EventArgs e)
        {
            if (cmbCategory.SelectedIndex == -1 || string.IsNullOrWhiteSpace(cmbCategory.Text))
            {
                cmbCategory.Text = "Categories"; // Reset placeholder if nothing is selected
            }
        }
    }
}
