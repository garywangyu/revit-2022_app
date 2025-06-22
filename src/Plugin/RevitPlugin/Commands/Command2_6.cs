using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using RevitPlugin.Managers;

namespace RevitPlugin.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Command2_6 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            string key = "2_6";
            var item = DynamoManager.Instance.GetItem(key);
            string msg = string.IsNullOrEmpty(item.DynamoPath) ? "尚未指定 Dynamo 檔案" : $"已指定 Dynamo: {item.DynamoPath}";
            MessageBox.Show(msg, key);
            return Result.Succeeded;
        }
    }
}
