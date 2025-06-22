using System.Windows.Forms;
using RevitPlugin.Managers;

namespace RevitPlugin
{
    public class ButtonImageForm : Form
    {
        public ButtonImageForm()
        {
            Text = "按鈕圖片設定";
            Width = 360;
            Height = 360;

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 10,
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30f));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70f));

            for (int i = 1; i <= 10; i++)
            {
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
                string key = $"1_{i}";
                var label = new Label { Text = key, Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleCenter };
                var btn = new Button { Text = "選擇圖片", Dock = DockStyle.Fill };
                btn.Click += (s, e) => ChooseImage(key);

                layout.Controls.Add(label, 0, i - 1);
                layout.Controls.Add(btn, 1, i - 1);
            }

            Controls.Add(layout);
        }

        private void ChooseImage(string key)
        {
            using var dlg = new OpenFileDialog { Filter = "Image Files|*.png;*.jpg;*.bmp" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ButtonImageManager.Instance.SetPath(key, dlg.FileName);
                MessageBox.Show("已設定圖片", key);
            }
        }
    }
}
