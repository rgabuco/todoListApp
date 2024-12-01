using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DotNetEnv;
using Newtonsoft.Json.Linq;

namespace PROJ
{
    public partial class Form1 : Form
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();
        private List<string> categories = new List<string>(); // Store category list
        private Panel weatherPanel = new Panel();
        private bool isOffline = true; // Flag to indicate offline mode

        public Form1()
        {
            InitializeComponent();
            InitializeWeatherPanel();
            InitializeListView();
            LoadWeatherData();

            listView1.KeyDown += ListView1_KeyDown;
            listView1.MouseClick += listView1_MouseClick;
            listView1.MultiSelect = false;
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
            if (isOffline)
            {
                // Use dummy data when offline
                string dummyJson = @"
            {
                'forecast': {
                    'forecastday': [
                        {
                            'date': '2023-10-01',
                            'day': {
                                'avgtemp_c': 15.0,
                                'condition': {
                                    'text': 'Sunny',
                                    'icon': '//cdn.weatherapi.com/weather/64x64/day/113.png'
                                }
                            }
                        },
                        {
                            'date': '2023-10-02',
                            'day': {
                                'avgtemp_c': 17.0,
                                'condition': {
                                    'text': 'Partly cloudy',
                                    'icon': '//cdn.weatherapi.com/weather/64x64/day/116.png'
                                }
                            }
                        },
                        {
                            'date': '2023-10-03',
                            'day': {
                                'avgtemp_c': 14.0,
                                'condition': {
                                    'text': 'Rainy',
                                    'icon': '//cdn.weatherapi.com/weather/64x64/day/308.png'
                                }
                            }
                        }
                    ]
                }
            }";
                JObject data = JObject.Parse(dummyJson);
                DisplayWeatherData(data);
            }
            else
            {
                string apiKey = Environment.GetEnvironmentVariable("API_KEY") ??
                               throw new InvalidOperationException("Weather API key is not set.");

                if (string.IsNullOrEmpty(apiKey))
                {
                    MessageBox.Show("Weather API key is not set.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                string city = "Calgary";
                string url = $"http://api.weatherapi.com/v1/forecast.json?key={apiKey}&q={city}&days=3";

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(url);
                        //MessageBox.Show($"Response Status Code: {response.StatusCode}");
                        if (response.IsSuccessStatusCode)
                        {
                            string json = await response.Content.ReadAsStringAsync();
                            //MessageBox.Show($"Response JSON: {json}");
                            JObject data = JObject.Parse(json);
                            DisplayWeatherData(data);
                        }
                        else
                        {
                            //MessageBox.Show("Failed to fetch weather data.");
                            DisplayErrorMessage("Failed to fetch weather data.");
                        }
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show($"Exception: {ex.Message}");
                        DisplayErrorMessage($"Exception: {ex.Message}");
                    }
                }
            }
        }

        private void DisplayWeatherData(JObject data)
        {
            // Clear existing controls in the weather panel
            weatherPanel.Controls.Clear();

            // Set the weather panel size and position
            int weatherPanelWidth = 343;
            int weatherPanelHeight = 165; // Adjusted to accommodate content
            weatherPanel.Size = new Size(weatherPanelWidth, weatherPanelHeight);
            weatherPanel.BackColor = Color.Transparent;

            // Specify the position of the first dayPanel
            int firstPanelX = 10; // X-coordinate of the top-left corner
            int firstPanelY = 20; // Y-coordinate of the top-left corner

            // Define dayPanel dimensions and spacing
            int dayPanelWidth = 100; // Width of each dayPanel
            int dayPanelHeight = 130; // Increased height to fit all content
            int spacing = 10; // Space between day panels

            JArray days = (JArray)data["forecast"]["forecastday"];

            for (int i = 0; i < 3; i++) // Loop through the first 3 days
            {
                JObject day = (JObject)days[i];
                string date = DateTime.Parse((string)day["date"]).ToString("ddd");
                string icon = (string)day["day"]["condition"]["icon"];
                string description = (string)day["day"]["condition"]["text"];
                string temp = ((double)day["day"]["avgtemp_c"]).ToString("0.0") + "°C";

                Panel dayPanel = new Panel
                {
                    Width = dayPanelWidth,
                    Height = dayPanelHeight,
                    BackColor = Color.LightSkyBlue,
                    BorderStyle = BorderStyle.FixedSingle,

                    // Set the position of the current dayPanel
                    Location = new Point(
                        firstPanelX + (dayPanelWidth + spacing) * i, // X-coordinate with spacing
                        firstPanelY // Y-coordinate remains the same
                    )
                };

                PictureBox iconBox = new PictureBox
                {
                    ImageLocation = $"http:{icon}",
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Size = new Size(40, 40), // Adjust icon size
                    Location = new Point((dayPanel.Width - 40) / 2, 5)
                };

                Label dateLabel = new Label
                {
                    Text = date,
                    Font = new Font("Arial", 9, FontStyle.Bold),
                    Location = new Point(0, 50),
                    Width = dayPanel.Width,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                Label descriptionLabel = new Label
                {
                    Text = description,
                    Font = new Font("Arial", 8),
                    Location = new Point(0, 70),
                    Width = dayPanel.Width,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                Label tempLabel = new Label
                {
                    Text = temp,
                    Font = new Font("Arial", 9, FontStyle.Bold),
                    ForeColor = Color.DarkBlue,
                    Location = new Point(0, 95),
                    Width = dayPanel.Width,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                // Add controls to the day panel
                dayPanel.Controls.Add(iconBox);
                dayPanel.Controls.Add(dateLabel);
                dayPanel.Controls.Add(descriptionLabel);
                dayPanel.Controls.Add(tempLabel);

                // Add day panel to the weather panel
                weatherPanel.Controls.Add(dayPanel);
            }

            // Add the weather panel to the form (if not already added)
            if (!this.Controls.Contains(weatherPanel))
            {
                this.Controls.Add(weatherPanel);
            }
        }

        private void DisplayErrorMessage(string message)
        {
            Label errorLabel = new Label
            {
                Text = message,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Red
            };
            weatherPanel.Controls.Add(errorLabel);
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
            // Right-click to open the EditTask method
             if (e.Button == MouseButtons.Right)
             {
                 ListViewItem clickedItem = listView1.GetItemAt(e.X, e.Y);
            
                 if (clickedItem != null)
                 {
                     // Open the EditTask method when right-clicking
                     EditTask(clickedItem);
                 }
             }
             else if (e.Button == MouseButtons.Left)
             {
                 // Left-click to highlight/select the row
                 ListViewItem clickedItem = listView1.GetItemAt(e.X, e.Y);
            
                 if (clickedItem != null)
                 {
                     if (Control.ModifierKeys == Keys.Control)
                     {
                         // If the Ctrl key is pressed, toggle the selection of the clicked item
                         clickedItem.Selected = !clickedItem.Selected;
                     }
                     else
                     {
                         // If Ctrl is not pressed, allow single selection (unselect other items)
                         foreach (ListViewItem item in listView1.Items)
                         {
                             item.Selected = false;  // Deselect all items
                         }
                         clickedItem.Selected = true;  // Select the clicked item
                     }
                 }
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

        private void BtnExpotToCsv_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                saveFileDialog.Title = "Save as CSV";
                saveFileDialog.FileName = "tasks.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                        {
                            // Write the header line
                            for (int i = 0; i < listView1.Columns.Count; i++)
                            {
                                writer.Write(listView1.Columns[i].Text);
                                if (i < listView1.Columns.Count - 1)
                                {
                                    writer.Write(",");
                                }
                            }
                            writer.WriteLine();

                            // Write the data lines
                            foreach (ListViewItem item in listView1.Items)
                            {
                                for (int i = 0; i < item.SubItems.Count; i++)
                                {
                                    writer.Write(item.SubItems[i].Text);
                                    if (i < item.SubItems.Count - 1)
                                    {
                                        writer.Write(",");
                                    }
                                }
                                writer.WriteLine();
                            }
                        }

                        MessageBox.Show("Save successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}

