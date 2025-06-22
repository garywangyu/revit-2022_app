# 安裝教學

以下步驟針對沒有程式經驗的使用者撰寫，依序操作即可完成安裝。

1. **準備軟體**
   - 安裝好 Autodesk Revit 2022。
   - 前往 [Visual Studio 官方網站](https://visualstudio.microsoft.com/) 下載並安裝 Visual Studio 2022 Community 版，安裝過程中請勾選「.NET 桌面開發」工作負載。

2. **取得程式碼**
   - 在 GitHub 頁面點選 `Code -> Download ZIP` 下載專案，並將壓縮檔解開。
   - 解壓後會得到一個資料夾，內含 `src/Plugin/RevitPlugin` 目錄及其他檔案。
   - 您可以將此資料夾放在任意位置（例如 `C:\RevitPlugin`），稍後的步驟會以此路徑為例。

3. **設定 API 路徑**
   - 於 Windows 搜尋列輸入「環境變數」，開啟「編輯系統環境變數」。
   - 點選「環境變數」，在「使用者變數」區塊按「新增」。
   - 「變數名稱」填入 `REVIT_2022_API_PATH`，
     「變數值」填入 `C:\Program Files\Autodesk\Revit 2022`，再按下確定儲存。

4. **編譯專案**
   - 開啟 Visual Studio，選擇「開啟專案」，並瀏覽到 `RevitPlugin.csproj` 所在位置後開啟。
   - 在 Visual Studio 上方選單點擊「建置 > 建置方案」（或直接按 **F6**）。
     完成後在 `bin\Debug` 目錄會產生 `RevitPlugin.dll`。

5. **安裝外掛**
   - 編譯完成後，打開專案資料夾中的 `src\Plugin\RevitPlugin\bin\Debug`，
     在此可找到 `RevitPlugin.dll`。
   - `RevitPlugin.addin` 位於 `src\Plugin\RevitPlugin\AddIn` 目錄。
   - 複製以上兩個檔案到 `C:\ProgramData\Autodesk\Revit\Addins\2022`。
    若此目錄不存在可自行建立。

## 檔案放置對照表
| 檔案 | 產生位置 | 要複製到 |
| --- | --- | --- |
| `RevitPlugin.dll` | `src\\Plugin\\RevitPlugin\\bin\\Debug` | `C:\\ProgramData\\Autodesk\\Revit\\Addins\\2022` |
| `RevitPlugin.addin` | `src\\Plugin\\RevitPlugin\\AddIn` | `C:\\ProgramData\\Autodesk\\Revit\\Addins\\2022` |

6. **驗證安裝**
   - 啟動 Revit 2022，應可在功能區看到 **MyPluginTab**。點擊任一按鈕出現「待製作中」對話框，即代表安裝成功。

## 批次檔快速操作
若已安裝好相依軟體，也可使用 `scripts` 目錄下的批次檔簡化流程：

1. 先執行 `update.bat`，自動編譯外掛並將檔案複製到 Addins 位置。
2. 如需將個人修改回傳到 Git，可再執行 `upload.bat`，依指示輸入提交訊息後便會推送到遠端。
