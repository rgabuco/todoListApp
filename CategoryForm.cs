using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROJ
{
    public partial class CategoryForm : Form
    {
        public List<string> Categories { get; private set; }

        public CategoryForm(List<string> existingCategories)
        {
            InitializeComponent();
            LoadCategoriesFromFile(); // Load existing categories from file
            Categories = new List<string>(existingCategories);

            // Populate the ListBox with existing categories
            foreach (var category in Categories)
            {
                lstCategories.Items.Add(category);
            }
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string newCategory = txtNewCategory.Text.Trim();

            if (string.IsNullOrWhiteSpace(newCategory))
            {
                MessageBox.Show("Please enter a valid category.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (lstCategories.Items.Contains(newCategory))
            {
                MessageBox.Show("This category already exists.", "Duplicate Category", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lstCategories.Items.Add(newCategory);
            Categories.Add(newCategory);
            txtNewCategory.Clear();
        }
        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            if (lstCategories.SelectedItem == null)
            {
                MessageBox.Show("Please select a category to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string categoryToDelete = lstCategories.SelectedItem.ToString();

            if (MessageBox.Show($"Are you sure you want to delete '{categoryToDelete}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                lstCategories.Items.Remove(categoryToDelete);
                Categories.Remove(categoryToDelete);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveCategoriesToFile(); // Save the categories to the file
            this.DialogResult = DialogResult.OK; // Close the form and save changes
            this.Close();
        }

        private void SaveCategoriesToFile()
        {
            string filePath = "categories.txt"; // Path to the file
            System.IO.File.WriteAllLines(filePath, Categories); // Save the list to the file
        }

        private void LoadCategoriesFromFile()
        {
            string filePath = "categories.txt";
            if (System.IO.File.Exists(filePath)) // Check if the file exists
            {
                Categories = System.IO.File.ReadAllLines(filePath).ToList(); // Load the list from the file
            }
        }


    }

}

