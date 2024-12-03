using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DotNetEnv;
using Newtonsoft.Json.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using PROJ.Services;
using PROJ.Database;

namespace PROJ
{
    public partial class Form1 : Form
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();
        private List<string> categories = new List<string>(); // Store category list
        private Panel weatherPanel = new Panel();
        private bool isOffline = false; // Flag to indicate offline mode
        private WeatherService weatherService;
        private SearchAndFilterService searchAndFilterService;
        private ExportFileService exportFileService = new ExportFileService();
        private const string PlaceholderText = "Search tasks...";

        public Form1()
        {
            InitializeComponent();
            InitializeWeatherPanel();
            InitializeListView();
            weatherService = new WeatherService(weatherPanel, isOffline);
            searchAndFilterService = new SearchAndFilterService(dbHelper);
            LoadWeatherData();

            listView1.KeyDown += ListView1_KeyDown;
            listView1.MouseClick += listView1_MouseClick;

            // Set placeholder text
            textBox1.Text = PlaceholderText;
            textBox1.ForeColor = Color.Gray;
            textBox1.GotFocus += RemovePlaceholderText;
            textBox1.GotFocus += (s, e) => searchAndFilterService.ClearStoredItems(listView1); // Clear stored items when TextBox gets focus
            textBox1.LostFocus += SetPlaceholderText;

            // Store current items when ComboBox loses focus
            cmbCategory.LostFocus += (s, e) => searchAndFilterService.StoreCurrentItems(listView1);
            cmbPriority.LostFocus += (s, e) => searchAndFilterService.StoreCurrentItems(listView1);
            cmbStatus.LostFocus += (s, e) => searchAndFilterService.StoreCurrentItems(listView1);
        }

        private void InitializeWeatherPanel()
        {
            weatherPanel.Dock = DockStyle.None; // Ensure DockStyle is set to None to manually set the position
            weatherPanel.BackColor = Color.LightBlue;
            weatherPanel.BorderStyle = BorderStyle.FixedSingle; // Add border outline

            // Set the location of the top-left corner of the weatherPanel
            weatherPanel.Location = new Point(720, 25);

            this.Controls.Add(weatherPanel);
        }

        private async void LoadWeatherData()
        {
            await weatherService.LoadWeatherData();
        }

        private void Form1_Load_1(object? sender, EventArgs e)
        {
            dbHelper.LoadTasks(listView1);

            textBox1.TextChanged += textBox1_TextChanged;

            // Populate cmbPriority with priority levels
            cmbPriority.Items.AddRange(new string[] { "All Priorities", "Low", "Medium", "High" });
            cmbPriority.SelectedIndex = 0;

            // Populate cmbStatus with status levels
            cmbStatus.Items.AddRange(new string[] { "All Statuses", "Not Started", "In Progress", "Completed" });
            cmbStatus.SelectedIndex = 0;

            LoadCategories(); // Load categories from file
            UpdateCategoryDropdown(); // to update the dropdown
            cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged; //handle categoryfilter changes
            cmbStatus.SelectedIndexChanged += cmbStatus_SelectedIndexChanged; // handle status filter changes
            cmbPriority.SelectedIndexChanged += cmbPriority_SelectedIndexChanged; // handle priority level filter changes
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
            //listView1.MultiSelect = true;

            // Add columns to show task details

            listView1.Columns.Clear();

            listView1.Columns.Add("Task Name", 200, HorizontalAlignment.Left);
            listView1.Columns.Add("Category", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Description", 450, HorizontalAlignment.Left);
            listView1.Columns.Add("Due Date", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Priority Level", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Status", 100, HorizontalAlignment.Left);

            // Add custom drawing for rows and headers
            listView1.DrawColumnHeader += listView1_DrawColumnHeader;
            listView1.DrawItem += listView1_DrawItem;
            listView1.DrawSubItem += listView1_DrawSubItem;
            listView1.MouseClick += listView1_MouseClick;
            listView1.MouseClick += listView1_MouseUp;
            listView1.MouseDoubleClick += listView1_MouseDoubleClick;
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


            
            e.DrawDefault = true;
        }

        private void listView1_DrawSubItem(object? sender, DrawListViewSubItemEventArgs e)
        {
            
            e.DrawDefault = true;

        }

        private void btnNewTask_Click(object? sender, EventArgs e)
        {
            // Open the form to add a new task
            TaskForm F2 = new TaskForm(searchAndFilterService);
            F2.Owner = this;
            F2.ShowDialog();
        }

        private bool isRightClickHandled = false;

        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && !isRightClickHandled)
            {
                isRightClickHandled = true; // Prevent multiple triggers

                // Get the clicked item in the ListView
                ListViewItem clickedItem = listView1.GetItemAt(e.X, e.Y);

                if (clickedItem != null)
                {
                    // Show the confirmation dialog only if at least one item is selected
                    if (listView1.SelectedItems.Count > 0)
                    {
                        DialogResult result = MessageBox.Show(
                            "Are you sure you want to delete the selected task(s)?",
                            "Delete Task",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question
                        );

                        if (result == DialogResult.Yes)
                        {
                            // Loop through all selected items and remove them
                            foreach (ListViewItem item in listView1.SelectedItems)
                            {
                                // Remove from ListView
                                listView1.Items.Remove(item);

                                // Delete from the database
                                dbHelper.DeleteTask((int)item.Tag);  // Assuming Tag holds the task ID
                            }

                            // Refresh ListView and other controls
                            RefreshListView();
                            UpdateCategoryDropdown();
                            MessageBox.Show("Selected task(s) deleted.");
                        }
                        else
                        {
                            MessageBox.Show("Task deletion canceled.");
                        }
                    }
                }

                // Reset the flag to allow handling of future right-clicks
                isRightClickHandled = false;
            }
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Left-click to highlight/select the row
                ListViewItem clickedItem = listView1.GetItemAt(e.X, e.Y);

                if (clickedItem != null)
                {
                    if (Control.ModifierKeys == Keys.Control)
                    {
                        clickedItem.Selected = !clickedItem.Selected;
                    }
                    else
                    {
                        foreach (ListViewItem item in listView1.Items)
                        {
                            item.Selected = false;
                        }
                        clickedItem.Selected = true;
                    }
                }
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Left double-click to open the EditTask method
            ListViewItem clickedItem = listView1.GetItemAt(e.X, e.Y);

            if (clickedItem != null)
            {
                // Open the EditTask method when double-clicking
                TaskForm form2 = new TaskForm(
                    clickedItem.SubItems[0].Text,
                    clickedItem.SubItems[1].Text,
                    clickedItem.SubItems[2].Text,
                    clickedItem.SubItems[3].Text,
                    clickedItem.SubItems[4].Text,
                    clickedItem.SubItems[5].Text,
                    clickedItem,
                    searchAndFilterService);

                DialogResult result = form2.ShowDialog();

                if (result == DialogResult.OK)
                {
                    RefreshListView();
                    UpdateCategoryDropdown();
                }
            }
        }

        private void ListView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                // Allow multi-selection when the Ctrl key is pressed
                listView1.MultiSelect = true;
            }
            else
            {
                // Disable multi-selection when the Ctrl key is not pressed
                listView1.MultiSelect = false;
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
            searchAndFilterService.FilterTasksByCategory(listView1, selectedCategory);
        }

        public void RefreshListView()
        {
            // Reloads the tasks from the database
            listView1.Items.Clear();
            dbHelper.LoadTasks(listView1);
            searchAndFilterService.StoreCurrentItems(listView1); // Store the current items
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
            cmbPriority.SelectedIndex = 0;
            cmbStatus.SelectedIndex = 0;
            textBox1.Clear();
            SetPlaceholderText(textBox1, EventArgs.Empty); // Set placeholder text if the TextBox is empty
            RefreshListView(); // Reload all tasks
        }

        private void BtnExpotToCsv_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                saveFileDialog.Title = "Save as CSV";
                saveFileDialog.FileName = "tasks.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    exportFileService.ExportListViewToCsv(listView1, saveFileDialog.FileName);
                }
            }
        }

        // Filters tasks with search input
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != PlaceholderText)
            {
                string query = textBox1.Text.Trim().ToLower();
                searchAndFilterService.SearchTasks(listView1, query);
            }
        }

        // Filter tasks with Priority Level
        private void cmbPriority_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedPriority = cmbPriority.SelectedItem?.ToString() ?? "All Priorities";
            searchAndFilterService.FilterTasksByPriority(listView1, selectedPriority);
        }

        // Filter tasks with task Status
        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedStatus = cmbStatus.SelectedItem?.ToString() ?? "All Statuses";
            searchAndFilterService.FilterTasksByStatus(listView1, selectedStatus);
        }

        private void RemovePlaceholderText(object sender, EventArgs e)
        {
            if (textBox1.Text == PlaceholderText)
            {
                textBox1.Text = string.Empty;
                textBox1.ForeColor = Color.Black;
            }
        }

        private void SetPlaceholderText(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = PlaceholderText;
                textBox1.ForeColor = Color.Gray;
            }
        }
    }
}
