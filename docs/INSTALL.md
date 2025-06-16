# 安裝教學

1. 下載並安裝 **Autodesk Revit 2022**。
2. 將 `src/Plugin/RevitPlugin/RevitPlugin.csproj` 使用 Visual Studio 2019 (或以上) 開啟。
3. 將環境變數 `REVIT_2022_API_PATH` 指向 Revit 2022 API 程式庫所在資料夾，通常為 `C:\Program Files\Autodesk\Revit 2022`。
4. 編譯專案後會產生 `RevitPlugin.dll`。
5. 將 `RevitPlugin.dll` 複製到 `%AppData%\Autodesk\REVIT\Addins\2022` 資料夾下。
6. 同資料夾放置 `RevitPlugin.addin` 檔案（位於 `src/Plugin/RevitPlugin/AddIn`）。
7. 重新啟動 Revit 2022 即可於功能區看到新頁籤 **MyPluginTab**。
