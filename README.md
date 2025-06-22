 Revit 2022 外掛程式示例

此專案提供一個在 Revit 2022 上運行的範例外掛。啟動後會於 Revit 功能區新增 `MyPluginTab` 頁籤，其中包含五個下拉選單與五個按鈕。每個選項或按鈕被點擊時皆會跳出顯示 `待製作中` 的視窗。

## 安裝步驟
下列說明以一般使用者為對象，即使沒有程式開發經驗也能完成安裝。更完整的步驟可參考 `docs/INSTALL.md`。

1. **準備工具**
   - 請先安裝好 Revit 2022。
   - 下載並安裝 Visual Studio 2022（建議 Community 版即可），安裝時記得勾選「.NET 桌面開發」工作負載。

2. **取得外掛程式碼**
   - 下載或複製本專案程式碼，解壓縮後可看到 `src/Plugin/RevitPlugin` 目錄與 `RevitPlugin.csproj` 檔案。

3. **設定 Revit API 路徑**
   - 在 Windows「開始」選單搜尋「環境變數」，開啟「編輯系統環境變數」。
   - 按下「環境變數」按鈕，在「使用者變數」區塊點選「新增」。
   - 變數名稱填入 `REVIT_2022_API_PATH`，變數值填入 `C:\Program Files\Autodesk\Revit 2022`，並按下「確定」。

4. **建置外掛**
   - 在檔案總管中雙擊 `RevitPlugin.csproj` 以 Visual Studio 開啟專案。
   - 在 Visual Studio 的上方功能表選擇「建置 > 建置方案」。完成後 `bin\Debug` 目錄會產生 `RevitPlugin.dll` 與 `RevitPlugin.addin`。

5. **安裝到 Revit**
   - 從 `src\Plugin\RevitPlugin\bin\Debug` 取得 `RevitPlugin.dll`。
   - 從 `src\Plugin\RevitPlugin\AddIn` 取得 `RevitPlugin.addin`。
   - **只需這兩個檔案**，請不要將整個專案資料夾複製到 Addins 位置。
   - 將 `RevitPlugin.dll` 與 `RevitPlugin.addin` 一併放入
     `C:\ProgramData\Autodesk\Revit\Addins\2022`，確保兩者位於同一目錄下。
     此資料夾位於 **ProgramData**，並不在 `C:\Program Files` 底下，預設也不會
     包含 `RevitPlugin` 子資料夾，可自行建立。
   - 如果 `Addins\2022` 目錄不存在，也請手動建立。
   - 重新啟動 Revit 2022，於功能區即可看到 `MyPluginTab` 頁籤。
   - 注意：專案根目錄下另有 `src/RevitPlugin/RevitPlugin.addin`，那是舊版範例檔，
     請勿複製或使用，以免 Revit 讀取到錯誤的路徑。

## 使用教學
-  `MyPluginTab` 頁籤內共有十個控制項：前五個為下拉式選單，各自包含十個選項； 後五個為一般按鈕。
- 下拉選單中的選項名稱依序為 '1_1'~'1_10'、'2_1'~'2_10'...'5_1'~'5_10'。
- '按鈕 6' ~ '按鈕 10'。
- 點擊任何選項或按鈕，皆會彈出視窗顯示「X_X 待製作中」，'X_X' 代表所觸發的項目名稱。
- 之後可依需求在 `Commands.cs` 中替換各選項的實際功能，並可在建立按鈕時指定圖示。

## 專案結構
- `src/Plugin/RevitPlugin/`：目前主要的外掛程式碼與 .addin 檔案。
- `Application.cs`：實作 `IExternalApplication`，負責在啟動時建立功能區與按鈕。
- `Commands/`：存放各按鈕及選項的指令程式碼，目前皆僅顯示待製作視窗。
- `src/RevitPlugin/`：早期的簡化範例，所有指令集中在單一檔案並示範 WPF 視窗用法，不會由批次檔編譯，可作為參考。

## 便利批次檔
對於不熟悉指令列操作的使用者，`scripts` 目錄提供兩個批次檔：

- `update.bat`：自動編譯外掛並複製更新後的檔案到 Revit `Addins` 資料夾。
- `upload.bat`：協助將變更提交並推送到遠端 Git 倉庫。

在 Windows 檔案總管中雙擊即可執行，上傳前會要求輸入提交訊息。