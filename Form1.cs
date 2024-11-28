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
        private List<string> categories = new List<string>(); // Store the category list

        public Form1()
        {
            InitializeComponent();
            InitializeListView();
        }

        private void Form1_Load_1(object? sender, EventArgs e)
        {
            // Load tasks from the database into the ListView
            if (listView1 != null)
            {
                dbHelper.LoadTasks(listView1);
            }

            LoadCategories(); // Load categories from file
            UpdateCategoryDropdown(); // Populate the Category ComboBox
            cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged; // Attach event handler

        }
        private void LoadCategories()
        {
            string filePath = "categories.txt";
            if (File.Exists(filePath))
            {
                categories = File.ReadAllLines(filePath).ToList(); // Load from file
            }
            else
            {
                categories = new List<string>(); // No default categories except "All Categories"
            }

            // Ensure "All Categories" is always the first category
            if (!categories.Contains("All Categories", StringComparer.OrdinalIgnoreCase))
            {
                categories.Insert(0, "All Categories");
            }
        }

        private void UpdateCategoryDropdown()
        {
            cmbCategory.Items.Clear();

            // Add "All Categories" as the first option
            if (!categories.Contains("All Categories", StringComparer.OrdinalIgnoreCase))
            {
                categories.Insert(0, "All Categories");
            }

            cmbCategory.Items.AddRange(categories.ToArray()); // Add categories to ComboBox
            cmbCategory.SelectedIndex = 0; // Default to "All Categories"
        }


        private void InitializeListView()
        {
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.OwnerDraw = true;

            listView1.Columns.Clear();

            listView1.Columns.Add("Task Name", 150, HorizontalAlignment.Left);
            listView1.Columns.Add("Category", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Description", 400, HorizontalAlignment.Left);
            listView1.Columns.Add("Due Date", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Priority Level", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Status", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Actions", 100, HorizontalAlignment.Left);
            

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
            Form2 F2 = new Form2();
            F2.Owner = this;
            F2.ShowDialog();
        }

        public void AddTaskToListView(string taskName, string category, string dueDate, string description, string priorityLevel, string status, int taskId)
        {
            ListViewItem item = new ListViewItem(taskName)
            {
                Tag = taskId // Store the task ID in the Tag property
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

            ListViewItem clickedItem = listView1.GetItemAt(e.X, e.Y);
            if (clickedItem != null)
            {
                EditTask(clickedItem);
            }
        }

        private void EditTask(ListViewItem item)
        {
            Form2 form2 = new Form2(item.SubItems[0].Text, item.SubItems[1].Text, item.SubItems[2].Text,
                                     item.SubItems[3].Text, item.SubItems[4].Text, item.SubItems[5].Text, item);

            DialogResult result = form2.ShowDialog();

            if (result == DialogResult.OK)
            {
                RefreshListView();
            }
            else if (result == DialogResult.Cancel)
            {
                if (form2.IsDeleteAction)
                {
                    listView1.Items.Remove(item);

                    int taskId = (int)item.Tag;
                    dbHelper.DeleteTask(taskId); // Call DeleteTask method in DatabaseHelper
                }
            }
        }
        private void cmbCategory_SelectedIndexChanged(object? sender, EventArgs e)
        {
            string selectedCategory = cmbCategory.SelectedItem.ToString();

            listView1.Items.Clear(); // Clear the ListView

            var tasks = dbHelper.GetTasks(); // Fetch tasks
            var filteredTasks = tasks.Where(task =>
                selectedCategory == "All" || task.Category.Equals(selectedCategory, StringComparison.OrdinalIgnoreCase)
            );

            foreach (var task in filteredTasks)
            {
                AddTaskToListView(task.TaskName, task.Category, task.DueDate.ToString("yyyy-MM-dd"), task.Description, task.PriorityLevel, task.Status, task.Id);
            }
        }

        public void RefreshListView()
        {
            listView1.Items.Clear();
            dbHelper.LoadTasks(listView1); // Reload tasks from the database
        }
        private void btnManageCategories_Click(object sender, EventArgs e)
        {
            using (var categoryForm = new CategoryForm(categories))
            {
                if (categoryForm.ShowDialog() == DialogResult.OK)
                {
                    categories = categoryForm.Categories; // Get updated categories
                    UpdateCategoryDropdown(); // Refresh ComboBox
                }
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            cmbCategory.SelectedIndex = 0; // Reset category filter to "All"
            RefreshListView(); // Reload all tasks
        }

    }
}

