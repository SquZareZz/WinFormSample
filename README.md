
# WinFormSample 

## 概要

寫給 User 用的單據比對 WinForm 程式，仿造 MVC 結構設置，具備 View(Form etc.)、Controller(MainFormHandler etc.)、Model(COA_DB_Model etc.)

## 目錄

### View
#### Form1
- [InitialValues](#initialvalues)
- [DelegateInitialize](#delegateinitialize)
- [Form1-Method](#form1-method)
- [ShippingOrderNoTextBox_TextChanged](#shippingordernotextbox_textchanged)
- [ScanShipoutRecipt_Click](#scanshipoutrecipt_click)
- [OpenDirectory_Click](#opendirectory_click)
- [COA_Preview_Click](#coa_preview_click)
- [ShippingOrderPreview_Click](#shippingorderpreview_click)
- [Filter_Click](#filter_click)
- [Check_COA_with_Order_Click](#check_coa_with_order_click)
- [Output_COA_Click](#output_coa_click)
- [SendMail_Click](#sendmail_click)
- [Clear_Click](#clear_click)
- [ClearAll](#clearall)
- [ClearPartialShippingOrder](#clearpartialshippingorder)
- [WriteLog_LocalFile](#writelog_localfile)
- [listView1_ItemActivate](#listview1_itemactivate)
#### SubForm
- [SubForm-Method](#subform-method)
- [Close_Click](#close_click)
- [DisplayFileList](#displayfilelist)
#### SubForm2
- [SubForm2-Method](#subform2-method)
- [Close_Click](#close_click)
- [Delete_Click](#delete_click)

### Controller
- [ScanShipoutRecipt](#scanshipoutrecipt)
- [UpdateSliceNo](#updatesliceno)
- [GetCOA_FileInfo](#getcoa_fileinfo)
- [GetCOA_From_DB](#getcoa_from_db)
- [CompareShippingOrderListWithLocal_COA_File](#compareshippingorderlistwithlocal_coa_file)
- [Compare_COA_with_Order](#compare_coa_with_order)
- [MoveFilesToOrderedDirectory](#movefilestoordereddirectory)
- [SendMailEvent](#sendmailevent)

## 功能介紹

### View

### InitialValues
初始化各項數值 ToolTip、FormModel、Controller(LogicAgent)、logger 還有各項實例化委派。
ToolTip：提示物件
FormModel：牽涉到所有模型共通項參數都裝在這裡（e.g. 出貨單、COA 資料）
LogicAgent：負責處理 Form1 上所有邏輯控制，避免 Form1 架構過長，且維持 MVC 模式。Controller 內的方法都是用 LogicAgent.方法() 方式呼叫
logger：log 檔案用

### DelegateInitialize
宣告委派物件、設定委派方法

### Form1-Method
建構 Form1 時，啟動初始化物件（InitializeComponent、InitializeDelegate）


### ShippingOrderNoTextBox_TextChanged
輸入出貨單號後按 Enter 可以觸發和按下「搜尋」鈕一樣找出貨單功能，詳見 [ScanShipoutRecipt](#scanshipoutrecipt)

### ScanShipoutRecipt_Click
「搜尋出貨單」按鈕按下後引發的事件，找尋 MS SQL 上的出貨單紀錄，儲存進參數 ShippingOrderList

### OpenDirectory_Click
選擇資料夾的按鈕按下引發的事件，選完資料夾會取得本地端檔案資訊，再拿檔案資訊去資料庫反查，詳見 [GetCOA_FileInfo](#getcoa_fileinfo)、[GetCOA_From_DB](#getcoa_from_db)

### COA_Preview_Click
開啟 COA 資料預覽畫面，來自 DB（[GetCOA_From_DB](#getcoa_from_db)）撈到的 COA 資料

### ShippingOrderPreview_Click
開啟出貨單資料預覽畫面，此處可以檢視所有不重複出貨單，且對單一項目刪除

### Filter_Click
篩選按鈕被按下以後，依據出貨單批號檢查資料夾內檔名是否相同

### Check_COA_with_Order_Click
核對按鈕被按下以後，核對出貨單與 COA 資訊

### Output_COA_Click
轉出按鈕被按下以後，把篩完的 COA 檔案搬到指定資料夾

### SendMail_Click
發送郵件按鈕被按下以後，寄送出貨單資訊上的核對結果給使用者

### Clear_Click
Reset 按鈕被按下以後，重置所有資訊，包含 FormModel（呼叫 [ClearAll](#clearall)）

### ClearAll
重置所有資訊，包含 FormModel

### ClearPartialShippingOrder
清除出貨單部分資料

### WriteLog_LocalFile
寫程式運行的 log

### SubForm-Method
初始化 SubForm 元件

### Close_Click
關閉視窗事件

### DisplayFileList
顯示 COA 清單

### SubForm2-Method
初始化 SubForm2 元件

### Close_Click
關閉視窗事件

### Delete_Click
刪除按鈕連動主畫面的事件

### Controller

### ScanShipoutRecipt
搜尋出貨單上的資訊，儲存進 FormModel，並把結果呈現在 listView1

### UpdateSliceNo
「搜尋出貨單」按鈕按下後，如果有重複批號，合併它的片號資料（e.g. 1-5、6-14 =>1-14）

### GetCOA_FileInfo
取得 COA 檔名的資料，利用檔名資料，去資料庫回查完整 COA 資料

### GetCOA_From_DB
取得來自 DB 的 COA 資料

### CompareShippingOrderListWithLocal_COA_File
依據出貨單內容篩選資料夾內檔名相同的項目

### Compare_COA_with_Order
核對出貨單與 COA 資訊，包含 Lot 名稱、片號等資訊

### MoveFilesToOrderedDirectory
<font color=red>**程式最終目標，搬移指定檔案到指定目錄底下**

### SendMailEvent
<font color=red>**程式最終目標，寄送結果信件給使用者**