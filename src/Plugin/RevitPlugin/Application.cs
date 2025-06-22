using Autodesk.Revit.UI;
using System.Reflection;
using Autodesk.Revit.Attributes;

namespace RevitPlugin
{
    public class Application : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication app)
        {
            // 外掛頁籤顯示名稱
            string tabName = "立立製作";
            try
            {
                app.CreateRibbonTab(tabName);
            }
            catch { }

            for (int i = 1; i <= 5; i++)
            {
                RibbonPanel panel = app.CreateRibbonPanel(tabName, $"Panel {i}");
                PulldownButtonData pulldownData = new PulldownButtonData($"Pulldown{i}", $"Pulldown{i}");
                PulldownButton pulldown = panel.AddItem(pulldownData) as PulldownButton;
                for (int j = 1; j <= 10; j++)
                {
                    var data = new PushButtonData($"PB{i}_{j}", $"{i}_{j}", Assembly.GetExecutingAssembly().Location, $"RevitPlugin.Commands.Command{i}_{j}");
                    pulldown.AddPushButton(data);
                }
            }

            RibbonPanel buttonPanel = app.CreateRibbonPanel(tabName, "Buttons");
            for (int i = 6; i <= 10; i++)
            {
                var data = new PushButtonData($"Button{i}", $"Button {i}", Assembly.GetExecutingAssembly().Location, $"RevitPlugin.Commands.Command{i}");
                buttonPanel.AddItem(data);
            }

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication app)
        {
            return Result.Succeeded;
        }
    }
}
