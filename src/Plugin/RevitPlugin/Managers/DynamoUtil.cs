using System.Diagnostics;
using Autodesk.Revit.UI;

namespace RevitPlugin.Managers
{
    internal static class DynamoUtil
    {
        public static void OpenDynamo(UIApplication app, string filePath)
        {
            var cmdId = RevitCommandId.LookupPostableCommandId(PostableCommand.Dynamo);
            app.PostCommand(cmdId);

            if (!string.IsNullOrEmpty(filePath) && System.IO.File.Exists(filePath))
            {
                var info = new ProcessStartInfo(filePath)
                {
                    UseShellExecute = true
                };
                Process.Start(info);
            }
        }
    }
}
