# Revit 2022 Plugin Example

此專案提供一個在 Revit 2022 上運行的範例外掛。啟動後會於 Revit 功能區新增 `MyPlugin` 頁籤，其中包含五個下拉選單與五個按鈕。每個選項或按鈕被點擊時皆會跳出顯示 `待製作中` 的視窗。

## 安裝步驟

以下說明假設您已取得完成編譯的 `RevitPlugin.dll` 與 `RevitPlugin.addin` 兩個檔案（通常會以壓縮檔提供）。無需具備程式開發知識，只要依照下列步驟操作即可：

1. **解壓縮檔案**：若收到的是 `.zip` 壓縮檔，請在檔案總管上按右鍵選擇「全部解壓縮」，即可得到兩個檔案。
2. **關閉 Revit 2022**：在安裝外掛前請先將 Revit 關閉。
3. **開啟 Addins 目錄**：在檔案總管的位址列輸入 `C:\\ProgramData\\Autodesk\\Revit\\Addins\\2022` 後按 Enter，或自行瀏覽到此資料夾。
4. **複製檔案**：將 `RevitPlugin.dll` 與 `RevitPlugin.addin` 直接拖曳或複製到上述資料夾中，若系統詢問權限請點選「繼續」。
5. **啟動 Revit**：重新開啟 Revit 2022，即可在功能區看到新增的 `MyPlugin` 頁籤。

### 進階：自行編譯外掛（可選）
1. 安裝 Visual Studio 2019 或 2022。
2. 開啟 `src/RevitPlugin/RevitPlugin.csproj` 專案檔。
3. 將環境變數 `REVIT_API_PATH` 設定為本機 Revit 2022 的 API DLL 目錄，例如 `C:\Program Files\Autodesk\Revit 2022`。
4. 建置專案後，即可在輸出資料夾取得 `RevitPlugin.dll`，再依照上述方法安裝。

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

