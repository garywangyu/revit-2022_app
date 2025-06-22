using System;
using System.Windows.Forms;
using RevitPlugin.Managers;

namespace RevitPlugin
{
    public class DynamoManagerForm : Form
    {
        public DynamoManagerForm()
        {
            Text = "Dynamo 管理";
            Width = 420;
            Height = 380;

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 5,
                RowCount = 10,
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
            for (int i = 1; i < 5; i++)
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));

            for (int i = 1; i <= 10; i++)
            {
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
                string key = $"2_{i}";
                var label = new Label { Text = key, Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleCenter };
                var loadBtn = new Button { Text = "讀取", Dock = DockStyle.Fill };
                var exportBtn = new Button { Text = "匯出", Dock = DockStyle.Fill };
                var removeBtn = new Button { Text = "移除", Dock = DockStyle.Fill };
                var imgBtn = new Button { Text = "新增圖片", Dock = DockStyle.Fill };

                loadBtn.Click += (s, e) => LoadDynamo(key);
                exportBtn.Click += (s, e) => ExportDynamo(key);
                removeBtn.Click += (s, e) => RemoveDynamo(key);
                imgBtn.Click += (s, e) => AddImage(key);

                layout.Controls.Add(label, 0, i - 1);
                layout.Controls.Add(loadBtn, 1, i - 1);
                layout.Controls.Add(exportBtn, 2, i - 1);
                layout.Controls.Add(removeBtn, 3, i - 1);
                layout.Controls.Add(imgBtn, 4, i - 1);
            }

            Controls.Add(layout);
        }

        private void LoadDynamo(string key)
        {
            using (var dlg = new OpenFileDialog { Filter = "Dynamo Files (*.dyn)|*.dyn" })
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    DynamoManager.Instance.SetDynamoPath(key, dlg.FileName);
                    MessageBox.Show("已讀取 Dynamo 檔案", key);
                }
            }
        }

        private void ExportDynamo(string key)
        {
            var item = DynamoManager.Instance.GetItem(key);
            if (string.IsNullOrEmpty(item.DynamoPath) || !System.IO.File.Exists(item.DynamoPath))
            {
                MessageBox.Show("無可匯出的檔案", key);
                return;
            }
            using (var dlg = new SaveFileDialog { FileName = System.IO.Path.GetFileName(item.DynamoPath), Filter = "Dynamo Files (*.dyn)|*.dyn" })
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    DynamoManager.Instance.Export(key, dlg.FileName);
                    MessageBox.Show("已匯出", key);
                }
            }
        }

        private void RemoveDynamo(string key)
        {
            DynamoManager.Instance.Remove(key);
            MessageBox.Show("已移除", key);
        }

        private void AddImage(string key)
        {
            using (var dlg = new OpenFileDialog { Filter = "Image Files|*.png;*.jpg;*.bmp" })
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    DynamoManager.Instance.SetImagePath(key, dlg.FileName);
                    MessageBox.Show("已設定圖片", key);
                }
            }
        }
    }
}
