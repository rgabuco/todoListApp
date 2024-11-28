using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Threading.Tasks;

namespace PROJ
{
    public partial class Form1 : Form
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();
        private List<string> categories = new List<string>(); // Store category list

        public Form1()
        {
            InitializeComponent();
            InitializeListView();
        }

        private void Form1_Load_1(object? sender, EventArgs e)
        {
           
            dbHelper.LoadTasks(listView1);

            LoadCategories(); // Load categories from file
            UpdateCategoryDropdown(); // to update the dropdown
            cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged; //handle categoryfilter changes

        }
        private void LoadCategories()
        {
            string filePath = "categories.txt";//where we save the categories
            if (File.Exists(filePath))
            {
                // Read categories from the file and add them to the list
                categories = File.ReadAllLines(filePath).ToList(); 
            }
            else
            {
                categories = new List<string>(); // No other default categories except "All Categories"
            }

            // Make sure "All Categories" is always the first category
            if (!categories.Contains("All Categories", StringComparer.OrdinalIgnoreCase))
            {
                categories.Insert(0, "All Categories");
            }
        }
        public void UpdateCategoryDropdown()
        {
            try
            {
                // Get categories from tasks in the database
                var taskCategories = dbHelper.GetDistinctCategoriesFromTasks();

                // Get categories from the file
                string filePath = "categories.txt";
                var fileCategories = File.Exists(filePath)
                    ? File.ReadAllLines(filePath).ToList()
                    : new List<string>();

                // Combine task categories and file categories
                var allCategories = new HashSet<string>(taskCategories);
                foreach (var category in fileCategories)
                {
                    allCategories.Add(category);
                }

                // Clear the dropdown and add combined categories
                cmbCategory.BeginInvoke((Action)(() =>
                {
                    cmbCategory.Items.Clear();
                    cmbCategory.Items.Add("All Categories");
                    cmbCategory.Items.AddRange(allCategories.ToArray());
                    cmbCategory.SelectedIndex = 0; // start with "All Categories"
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating category dropdown: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void InitializeListView()
        {
            // Set up the task list display
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.OwnerDraw = true;

            // Add columns to show task details

            listView1.Columns.Clear();

            listView1.Columns.Add("Task Name", 150, HorizontalAlignment.Left);
            listView1.Columns.Add("Category", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Description", 400, HorizontalAlignment.Left);
            listView1.Columns.Add("Due Date", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Priority Level", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Status", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Actions", 100, HorizontalAlignment.Left);

            // Add custom drawing for rows and headers
            listView1.DrawColumnHeader += listView1_DrawColumnHeader;
            listView1.DrawItem += listView1_DrawItem;
            listView1.DrawSubItem += listView1_DrawSubItem;
            listView1.MouseClick += listView1_MouseClick;
        }


        private void listView1_DrawColumnHeader(object? sender, DrawListViewColumnHeaderEventArgs e)
        {
            if (e == null) return;

            using (Brush brush = new SolidBrush(ColorTranslator.FromHtml("#7974A8")))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }

            TextRenderer.DrawText(e.Graphics, e.Header.Text, e.Font, e.Bounds, Color.Black,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
        
    }

        private void listView1_DrawItem(object? sender, DrawListViewItemEventArgs e)
        {
            Color rowColor = (e.ItemIndex % 2 == 0) ? Color.LightGray : Color.White;
            using (Brush brush = new SolidBrush(rowColor))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }
            e.DrawDefault = true;
        }

        private void listView1_DrawSubItem(object? sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void btnNewTask_Click(object? sender, EventArgs e)
        {
            // Open the form to add a new task
            Form2 F2 = new Form2();
            F2.Owner = this;
            F2.ShowDialog();
        }

        public void AddTaskToListView(string taskName, string category, string dueDate, string description, string priorityLevel, string status, int taskId)
        {
            ListViewItem item = new ListViewItem(taskName)
            {
                Tag = taskId // Store the task ID for later use
            };
            item.SubItems.Add(category);
            item.SubItems.Add(description);
            item.SubItems.Add(dueDate);
            item.SubItems.Add(priorityLevel);
            item.SubItems.Add(status);
            item.SubItems.Add("Edit/Delete");
            listView1.Items.Add(item);

        }


        private void listView1_MouseClick(object? sender, MouseEventArgs e)
        {
            // Manage clicks on tasks to edit them
            ListViewItem clickedItem = listView1.GetItemAt(e.X, e.Y);
            if (clickedItem != null)
            {
                EditTask(clickedItem);
            }
        }

        private void EditTask(ListViewItem item)
        {
            // Open the edit form with task details
            Form2 form2 = new Form2(
                item.SubItems[0].Text,
                item.SubItems[1].Text,
                item.SubItems[2].Text,
                item.SubItems[3].Text,
                item.SubItems[4].Text,
                item.SubItems[5].Text,
                item);

            DialogResult result = form2.ShowDialog();

            if (result == DialogResult.OK)
            {
                // Update the list if some changes were made
                RefreshListView();
                UpdateCategoryDropdown();
            }
            else if (result == DialogResult.Cancel && form2.IsDeleteAction)
            {
                // Remove the task if it was deleted
                listView1.Items.Remove(item);
                dbHelper.DeleteTask((int)item.Tag);
                RefreshListView();
                UpdateCategoryDropdown();
            }
        }

        private void cmbCategory_SelectedIndexChanged(object? sender, EventArgs e)
        {
            //manages category filtering
            if (cmbCategory.SelectedItem == null)
            {
                return;
            }

            string selectedCategory = cmbCategory.SelectedItem.ToString();

            listView1.Items.Clear();

            var tasks = dbHelper.GetTasks();
            var filteredTasks = tasks.Where(task =>
                selectedCategory == "All Categories" ||
                task.Category.Equals(selectedCategory, StringComparison.OrdinalIgnoreCase)
            );

            foreach (var task in filteredTasks)
            {
                AddTaskToListView(task.TaskName, task.Category, task.DueDate.ToString("yyyy-MM-dd"), task.Description, task.PriorityLevel, task.Status, task.Id);
            }
        }


        public void RefreshListView()
        {
            //reloads the tasks from the database
            listView1.Items.Clear();
            dbHelper.LoadTasks(listView1); 
            listView1.Refresh(); 

        }
        private void btnManageCategories_Click(object sender, EventArgs e)
        {
            // Open the category form
            using (var categoryForm = new CategoryForm(categories))
            {
                if (categoryForm.ShowDialog() == DialogResult.OK)
                {
                    categories = categoryForm.Categories; // Get updated categories
                    UpdateCategoryDropdown(); 
                }
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            cmbCategory.SelectedIndex = 0; 
            RefreshListView(); // Reload all tasks
        }

    }
}

