using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

            string? exe = FindDynamoExecutable();
            if (exe == null)
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
            try
            {
                Process.Start(start);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"無法啟動 Dynamo: {ex.Message}", "錯誤");
            }
        }

        private static string? FindDynamoExecutable()
        {
            string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            var searchRoots = new[]
            {
                Path.Combine(programFiles, "Dynamo", "Dynamo Revit"),
                Path.Combine(programFiles, "Dynamo", "Dynamo Core")
            };
            foreach (var root in searchRoots)
            {
                if (!Directory.Exists(root))
                    continue;
                var cli = Directory.EnumerateFiles(root, "DynamoCLI.exe", SearchOption.AllDirectories).FirstOrDefault();
                if (cli != null)
                    return cli;
                var sandbox = Directory.EnumerateFiles(root, "DynamoSandbox.exe", SearchOption.AllDirectories).FirstOrDefault();
                if (sandbox != null)
                    return sandbox;
            }
            return null;
        }
    }
}
