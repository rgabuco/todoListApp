using PROJ.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PROJ.Services
{
    public class SearchAndFilterService
    {
        private DatabaseHelper dbHelper;

        public SearchAndFilterService(DatabaseHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public void FilterTasksByCategory(ListView listView, string selectedCategory)
        {
            var allItems = listView.Tag as List<ListViewItem>;
            if (allItems == null)
            {
                allItems = listView.Items.Cast<ListViewItem>().ToList();
                listView.Tag = allItems;
            }

            var filteredItems = allItems.Where(item =>
                selectedCategory == "All Categories" ||
                item.SubItems[1].Text.Equals(selectedCategory, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            listView.BeginUpdate();
            listView.Items.Clear();
            listView.Items.AddRange(filteredItems.ToArray());
            listView.EndUpdate();
        }

        public void FilterTasksByPriority(ListView listView, string selectedPriority)
        {
            var allItems = listView.Tag as List<ListViewItem>;
            if (allItems == null)
            {
                allItems = listView.Items.Cast<ListViewItem>().ToList();
                listView.Tag = allItems;
            }

            var filteredItems = allItems.Where(item =>
                selectedPriority == "All Priorities" ||
                item.SubItems[4].Text.Equals(selectedPriority, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            listView.BeginUpdate();
            listView.Items.Clear();
            listView.Items.AddRange(filteredItems.ToArray());
            listView.EndUpdate();
        }

        public void FilterTasksByStatus(ListView listView, string selectedStatus)
        {
            var allItems = listView.Tag as List<ListViewItem>;
            if (allItems == null)
            {
                allItems = listView.Items.Cast<ListViewItem>().ToList();
                listView.Tag = allItems;
            }

            var filteredItems = allItems.Where(item =>
                selectedStatus == "All Statuses" ||
                item.SubItems[5].Text.Equals(selectedStatus, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            listView.BeginUpdate();
            listView.Items.Clear();
            listView.Items.AddRange(filteredItems.ToArray());
            listView.EndUpdate();
        }

        public void SearchTasks(ListView listView, string query)
        {
            var allItems = listView.Tag as List<ListViewItem>;
            if (allItems == null)
            {
                allItems = listView.Items.Cast<ListViewItem>().ToList();
                listView.Tag = allItems;
            }

            var filteredItems = allItems.Where(item =>
                string.IsNullOrEmpty(query) ||
                item.Text.ToLower().Contains(query) ||
                item.SubItems.Cast<ListViewItem.ListViewSubItem>().Any(subItem => subItem.Text.ToLower().Contains(query))
            ).ToList();

            listView.BeginUpdate();
            listView.Items.Clear();
            listView.Items.AddRange(filteredItems.ToArray());
            listView.EndUpdate();
        }

        public void ClearStoredItems(ListView listView)
        {
            listView.Tag = null;
        }

        public void StoreCurrentItems(ListView listView)
        {
            var allItems = listView.Items.Cast<ListViewItem>().ToList();
            listView.Tag = allItems;
        }

        public void AddTaskToListView(ListView listView, TaskModel task)
        {
            ListViewItem item = new ListViewItem(task.TaskName)
            {
                Tag = task.Id // Store the task ID for later use
            };
            item.SubItems.Add(task.Category);
            item.SubItems.Add(task.Description);
            item.SubItems.Add(task.DueDate.ToString("yyyy-MM-dd"));
            item.SubItems.Add(task.PriorityLevel);
            item.SubItems.Add(task.Status);
            listView.Items.Add(item);
        }
    }
}
