# 使用教學

載入插件後，Revit 2022 將在功能區顯示 **立立製作**。
此頁籤包含兩種元件：

- 前五個下拉式選單，每個下拉式選單包含 10 個選項
- 後五個一般按鈕

目前每個選項與按鈕尚未指定功能，點擊後會顯示對話框，訊息為 `X_X 待製作中`，其中 `X` 為按鈕與選項編號。
未來可於 `src/Plugin/RevitPlugin/Commands` 目錄新增或修改對應指令實作，並可在 `Application.cs` 中調整版面配置或更新圖示。

## 執行 Dynamo 指令
執行 `2_1`~`2_10` 選項可指定 `.dyn` 檔案路徑。讀取完成後再次執行指令時，系統會自動尋找安裝於 `C:\Program Files\Dynamo` 內的 `DynamoCLI.exe`（或 `DynamoSandbox.exe`）並啟動 **Dynamo** 來載入腳本，腳本會立刻開始執行，無需透過 Dynamo 播放器。

## 直接載入 Dynamo 檔案
按下 `Run Dynamo` 按鈕可手動選取 `.dyn` 檔案，外掛會透過 Dynamo API 在背景載入並執行該腳本，不會顯示 Dynamo 介面。
