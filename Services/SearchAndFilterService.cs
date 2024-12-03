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
            listView.Items.Clear();

            var tasks = dbHelper.GetTasks();
            var filteredTasks = tasks.Where(task =>
                selectedCategory == "All Categories" ||
                task.Category.Equals(selectedCategory, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            foreach (var task in filteredTasks)
            {
                AddTaskToListView(listView, task);
            }

            StoreCurrentItems(listView); // Store the filtered items
        }

        public void FilterTasksByPriority(ListView listView, string selectedPriority)
        {
            listView.Items.Clear();

            var tasks = dbHelper.GetTasks();
            var filteredTasks = tasks.Where(task =>
                selectedPriority == "All Priorities" ||
                task.PriorityLevel.Equals(selectedPriority, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            foreach (var task in filteredTasks)
            {
                AddTaskToListView(listView, task);
            }

            StoreCurrentItems(listView); // Store the filtered items
        }

        public void FilterTasksByStatus(ListView listView, string selectedStatus)
        {
            listView.Items.Clear();

            var tasks = dbHelper.GetTasks();
            var filteredTasks = tasks.Where(task =>
                selectedStatus == "All Statuses" ||
                task.Status.Equals(selectedStatus, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            foreach (var task in filteredTasks)
            {
                AddTaskToListView(listView, task);
            }

            StoreCurrentItems(listView); // Store the filtered items
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

