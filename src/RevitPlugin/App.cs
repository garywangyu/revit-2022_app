using Autodesk.Revit.UI;
using System;

namespace RevitPlugin
{
    public class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            string tabName = "MyPlugin";
            try { application.CreateRibbonTab(tabName); } catch { }
            RibbonPanel panel = application.CreateRibbonPanel(tabName, "Main");

            string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            // Create 5 dropdowns
            for (int i = 1; i <= 5; i++)
            {
                PulldownButtonData ddData = new PulldownButtonData($"Dropdown{i}", $"Dropdown {i}");
                PulldownButton dd = panel.AddItem(ddData) as PulldownButton;
                for (int j = 1; j <= 10; j++)
                {
                    string className = $"RevitPlugin.Command_{i}_{j}";
                    PushButtonData pData = new PushButtonData($"{i}_{j}", $"{i}_{j}", assemblyPath, className);
                    dd.AddPushButton(pData);
                }
            }

            // Create 5 buttons
            for (int i = 6; i <= 10; i++)
            {
                string className = $"RevitPlugin.Command_{i}";
                PushButtonData pData = new PushButtonData($"Button{i}", $"Button {i}", assemblyPath, className);
                panel.AddItem(pData);
            }

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
