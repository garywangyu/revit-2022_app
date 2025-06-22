using System.Windows.Forms;

namespace RevitPlugin
{
    internal static class Prompt
    {
        public static string? ShowDialog(string text, string caption, string defaultValue = "")
        {
            Form prompt = new Form()
            {
                Width = 400,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 10, Top=10, Width=360, Text=text };
            TextBox inputBox = new TextBox() { Left = 10, Top=40, Width=360, Text = defaultValue };
            Button confirmation = new Button() { Text = "OK", Left=210, Width=80, Top=70, DialogResult = DialogResult.OK };
            Button cancel = new Button() { Text = "Cancel", Left=300, Width=70, Top=70, DialogResult = DialogResult.Cancel };
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(inputBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(cancel);
            prompt.AcceptButton = confirmation;
            prompt.CancelButton = cancel;
            return prompt.ShowDialog() == DialogResult.OK ? inputBox.Text : null;
        }
    }
}
