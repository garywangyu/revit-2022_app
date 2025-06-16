# Revit 2022 Plugin Example

此專案提供一個在 Revit 2022 上運行的範例外掛。啟動後會於 Revit 功能區新增 `MyPlugin` 頁籤，其中包含五個下拉選單與五個按鈕。每個選項或按鈕被點擊時皆會跳出顯示 `待製作中` 的視窗。

## 安裝步驟
以下流程假設您對程式開發並不熟悉，只需依序完成每個步驟即可在 Revit 中啟用此外掛。

1. **安裝 Visual Studio**  
   前往 [Visual Studio 官網](https://visualstudio.microsoft.com/) 下載並安裝 *Visual Studio 2019* 或 *Visual Studio 2022* 的 *Community* 版本，即可免費使用。
2. **開啟專案**  
   解壓縮本專案資料夾後，啟動 Visual Studio，選擇「開啟專案或方案」，瀏覽至 `src/RevitPlugin/RevitPlugin.csproj` 並開啟。
3. **設定 Revit 路徑**  
   在 Windows 搜尋列輸入「環境變數」，開啟「編輯系統環境變數」，點選「環境變數」後於 *使用者變數* 或 *系統變數* 新增：
   - 變數名稱：`REVIT_API_PATH`
   - 變數值：您的 Revit 2022 安裝路徑，例如 `C:\Program Files\Autodesk\Revit 2022`
4. **建置外掛**  
   於 Visual Studio 上方功能表選擇「建置 > 建置方案」。完成後在 `bin\Debug` 或 `bin\Release` 資料夾內會看到 `RevitPlugin.dll`。
5. **安裝至 Revit**  
   將上一步產生的 `RevitPlugin.dll`，以及 `src\RevitPlugin\RevitPlugin.addin` 檔案複製到 `C:\ProgramData\Autodesk\Revit\Addins\2022`。該資料夾預設為隱藏，可在檔案總管勾選「顯示隱藏的項目」後再開啟。
6. **啟用外掛**  
   重新啟動 Revit 2022，便會在功能區看到 `MyPlugin` 頁籤。

## 使用教學
- `MyPlugin` 頁籤內共有十個控制項：前五個為下拉式選單，各自包含十個選項；後五個為一般按鈕。
- 下拉選單中的選項名稱依序為 `1_1`～`1_10`、`2_1`～`2_10`...`5_1`～`5_10`。
- 按鈕名稱為 `Button 6` ～ `Button 10`。
- 點擊任何選項或按鈕，皆會彈出視窗顯示「X_X 待製作中」，`X_X` 代表所觸發的項目名稱。
- 之後可依需求在 `Commands.cs` 中替換各選項的實際功能，並可在建立按鈕時指定圖示。

## 專案結構
- `src/RevitPlugin/`：主要外掛程式碼與 .addin 檔案。
- `App.cs`：實作 `IExternalApplication`，負責在啟動時建立功能區與按鈕。
- `Commands.cs`：定義所有按鈕及選項對應的指令，目前皆僅顯示待製作視窗。
- `PlaceholderWindow.xaml`：彈出視窗的 WPF 介面。

