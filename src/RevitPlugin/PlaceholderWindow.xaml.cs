using System.Windows;

namespace RevitPlugin
{
    public partial class PlaceholderWindow : Window
    {
        public PlaceholderWindow(string tag)
        {
            InitializeComponent();
            MessageText.Text = $"{tag} 待製作中";
        }
    }
}
