using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace RevitPlugin.Managers
{
    internal static class DynamoRunner
    {
        /// <summary>
        /// 直接啟動 Dynamo 執行指定腳本。
        /// </summary>
        public static void RunScript(string dynPath)
        {
            if (!File.Exists(dynPath))
            {
                MessageBox.Show("找不到 Dynamo 檔案", "錯誤");
                return;
            }

            string exe = @"C:\\Program Files\\Dynamo\\Dynamo Revit\\2\\DynamoCLI.exe";
            if (!File.Exists(exe))
            {
                exe = @"C:\\Program Files\\Dynamo\\Dynamo Core\\2\\DynamoSandbox.exe";
            }
            if (!File.Exists(exe))
            {
                MessageBox.Show("找不到Dynamo執行檔案", "錯誤");
                return;
            }

            var start = new ProcessStartInfo
            {
                FileName = exe,
                Arguments = $"-o \"{dynPath}\" -c",
                UseShellExecute = false,
                CreateNoWindow = true
            };
            Process.Start(start);
        }
    }
}
