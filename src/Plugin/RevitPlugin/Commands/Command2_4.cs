using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using RevitPlugin.Managers;

namespace RevitPlugin.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Command2_4 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            string key = "2_4";
            var item = DynamoManager.Instance.GetItem(key);
            if (string.IsNullOrEmpty(item.DynamoPath) || !System.IO.File.Exists(item.DynamoPath))
            {
                MessageBox.Show("尚未指定 Dynamo 檔案", key);
                return Result.Succeeded;
            }

            if (!string.IsNullOrEmpty(item.Instruction))
                MessageBox.Show(item.Instruction, "操作說明");

            DynamoPlayerUtil.OpenPlayer(commandData.Application);
            return Result.Succeeded;
        }
    }
}
