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

            string exe = FindDynamoExe();
            if (string.IsNullOrEmpty(exe))
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

        private static string FindDynamoExe()
        {
            string[] roots =
            {
                @"C:\\Program Files\\Dynamo\\Dynamo Revit",
                @"C:\\Program Files\\Dynamo\\Dynamo Core"
            };
            foreach (var root in roots)
            {
                if (!Directory.Exists(root))
                    continue;
                var dirs = Directory.GetDirectories(root);
                System.Array.Sort(dirs);
                System.Array.Reverse(dirs);
                foreach (var dir in dirs)
                {
                    string cli = Path.Combine(dir, "DynamoCLI.exe");
                    if (File.Exists(cli))
                        return cli;
                    string sandbox = Path.Combine(dir, "DynamoSandbox.exe");
                    if (File.Exists(sandbox))
                        return sandbox;
                }
            }
            return string.Empty;
        }
    }
}
