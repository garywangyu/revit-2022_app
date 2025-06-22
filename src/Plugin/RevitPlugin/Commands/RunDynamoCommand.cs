using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using Microsoft.Win32;
using Dynamo.Applications;

namespace RevitPlugin.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class RunDynamoCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // 1. open file dialog to select .dyn file
            var dlg = new OpenFileDialog { Filter = "Dynamo 文件 (*.dyn)|*.dyn" };
            if (dlg.ShowDialog() == true)
            {
                string dynPath = dlg.FileName;
                try
                {
                    var dynamoRevit = new DynamoRevit();
                    var journalData = new Dictionary<string, string>()
                    {
                        { JournalKeys.AutomationModeKey, "True" },
                        { JournalKeys.DynPathKey, dynPath },
                        { JournalKeys.DynPathExecuteKey, "True" },
                        { JournalKeys.ShowUiKey, "False" }
                    };

                    var cmdData = new DynamoRevitCommandData
                    {
                        Application = commandData.Application,
                        JournalData = journalData
                    };

                    Result dynResult = dynamoRevit.ExecuteCommand(cmdData);
                    if (dynResult != Result.Succeeded)
                    {
                        TaskDialog.Show("Dynamo 執行結果", "Dynamo 腳本執行未成功，請檢查腳本內容或輸入。");
                    }
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("執行錯誤", $"執行 Dynamo 時發生例外: {ex.Message}");
                }
            }
            return Result.Succeeded;
        }
    }
}
