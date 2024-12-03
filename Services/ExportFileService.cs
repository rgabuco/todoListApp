using System;
using System.IO;
using System.Windows.Forms;

namespace PROJ.Services
{
    public class ExportFileService
    {
        public void ExportListViewToCsv(ListView listView, string fileName)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    // Write the header line
                    for (int i = 0; i < listView.Columns.Count; i++)
                    {
                        writer.Write(listView.Columns[i].Text);
                        if (i < listView.Columns.Count - 1)
                        {
                            writer.Write(",");
                        }
                    }
                    writer.WriteLine();

                    // Write the data lines
                    foreach (ListViewItem item in listView.Items)
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
