using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetEnv;

namespace PROJ
{
    public class DatabaseHelper
    {
        private string connectionString;
        public DatabaseHelper()
        {
            // Load environment variables from the .env file
            Env.Load();

            connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") ??
                           throw new InvalidOperationException("Connection string is not set.");

            if (string.IsNullOrEmpty(connectionString))
            {
                MessageBox.Show("Connection string is not set.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AddTask(string taskName, string category, string description, DateTime dueDate, string priorityLevel, string status)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Tasks (TaskName, Category, Description, DueDate, PriorityLevel, Status) VALUES (@taskName, @category, @description, @dueDate, @priorityLevel, @status)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@taskName", taskName);
                        command.Parameters.AddWithValue("@category", category);
                        command.Parameters.AddWithValue("@description", description);
                        command.Parameters.AddWithValue("@dueDate", dueDate);
                        command.Parameters.AddWithValue("@priorityLevel", priorityLevel);
                        command.Parameters.AddWithValue("@status", status);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding task: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void UpdateTask(int taskId, string taskName, string category, string description, DateTime dueDate, string priorityLevel, string status)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Tasks SET TaskName = @taskName, Category = @category, Description = @description, " +
                               "DueDate = @dueDate, PriorityLevel = @priorityLevel, Status = @status WHERE Id = @taskId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@taskId", taskId);
                    command.Parameters.AddWithValue("@taskName", taskName);
                    command.Parameters.AddWithValue("@category", category);
                    command.Parameters.AddWithValue("@description", description);
                    command.Parameters.AddWithValue("@dueDate", dueDate);
                    command.Parameters.AddWithValue("@priorityLevel", priorityLevel);
                    command.Parameters.AddWithValue("@status", status);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteTask(int taskId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Start a transaction to ensure the action is done atomically
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Delete the specific task by its Id
                        string deleteQuery = "DELETE FROM Tasks WHERE Id = @taskId";
                        using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection, transaction))
                        {
                            deleteCommand.Parameters.AddWithValue("@taskId", taskId);
                            deleteCommand.ExecuteNonQuery();
                        }

                        // Commit the transaction if everything is successful
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Rollback if there is any error
                        transaction.Rollback();
                        MessageBox.Show($"Error deleting task: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting task: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void LoadTasks(ListView listView)
        {
            listView.Items.Clear(); // Clear existing items before loading new ones.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Tasks";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ListViewItem item = new ListViewItem(reader["TaskName"].ToString())
                            {
                                Tag = reader["Id"] // Assuming Id is the primary key in the Tasks table
                            };
                            item.SubItems.Add(reader["Category"].ToString());
                            item.SubItems.Add(reader["Description"].ToString());
                            item.SubItems.Add(Convert.ToDateTime(reader["DueDate"]).ToString("yyyy-MM-dd"));
                            item.SubItems.Add(reader["PriorityLevel"].ToString());
                            item.SubItems.Add(reader["Status"].ToString());
                            item.SubItems.Add("Edit/Delete");

                            listView.Items.Add(item);
                        }
                    }
                }
            }
        }


        public int GetLastInsertedTaskId()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT IDENT_CURRENT('Tasks')"; // Get last inserted ID
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }


    }
}
