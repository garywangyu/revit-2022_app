using Autodesk.Revit.UI;
using System.Reflection;
using Autodesk.Revit.Attributes;
using System.IO;
using System.Windows.Media.Imaging;
using RevitPlugin.Managers;

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

            DynamoManager manager = DynamoManager.Instance;
            ButtonImageManager imgManager = ButtonImageManager.Instance;

            for (int i = 1; i <= 5; i++)
            {
                RibbonPanel panel = app.CreateRibbonPanel(tabName, $"Panel {i}");
                PulldownButtonData pulldownData = new PulldownButtonData($"Pulldown{i}", $"Pulldown{i}");
                PulldownButton pulldown = panel.AddItem(pulldownData) as PulldownButton;
                for (int j = 1; j <= 10; j++)
                {
                    string label = $"{i}_{j}";
                    if (i == 1 && j == 1)
                        label = "建築接合";
                    var data = new PushButtonData($"PB{i}_{j}", label, Assembly.GetExecutingAssembly().Location, $"RevitPlugin.Commands.Command{i}_{j}");
                    PushButton pb = pulldown.AddPushButton(data);
                    if (i == 1)
                    {
                        var path = imgManager.GetPath($"1_{j}");
                        if (!string.IsNullOrEmpty(path))
                        {
                            var img = ImageUtil.LoadImage(path);
                            if (img != null)
                                pb.LargeImage = img;
                        }
                    }
                    if (i == 2)
                    {
                        var item = manager.GetItem($"2_{j}");
                        if (!string.IsNullOrEmpty(item.ImagePath))
                        {
                            var img = ImageUtil.LoadImage(item.ImagePath);
                            if (img != null)
                                pb.LargeImage = img;
                        }
                    }
                }
                if (i == 1)
                {
                    var manageData = new PushButtonData("PB1_manage", "管理", Assembly.GetExecutingAssembly().Location, "RevitPlugin.Commands.Command1_Manage");
                    pulldown.AddPushButton(manageData);
                }
                if (i == 2)
                {
                    var manageData = new PushButtonData("PB2_manage", "管理", Assembly.GetExecutingAssembly().Location, "RevitPlugin.Commands.Command2_Manage");
                    pulldown.AddPushButton(manageData);
                }
            }

            RibbonPanel buttonPanel = app.CreateRibbonPanel(tabName, "Buttons");
            for (int i = 6; i <= 10; i++)
            {
                var data = new PushButtonData($"Button{i}", $"Button {i}", Assembly.GetExecutingAssembly().Location, $"RevitPlugin.Commands.Command{i}");
                buttonPanel.AddItem(data);
            }

            // 新增執行 Dynamo 的按鈕
            var runData = new PushButtonData("ButtonRunDynamo", "Run Dynamo", Assembly.GetExecutingAssembly().Location, "RevitPlugin.Commands.RunDynamoCommand");
            buttonPanel.AddItem(runData);

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication app)
        {
            return Result.Succeeded;
        }
    }
}
