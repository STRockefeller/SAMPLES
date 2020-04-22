//參考 > 加入參考 > 瀏覽 > C:\Windows\assembly\GAC_MSIL\Microsoft.Office.Interop.Excel > 往裡面的資料夾內找到 Microsoft.Office.Interop.Excel.dll
//引用
using System.Data.OleDb;
using Excel = Microsoft.Office.Interop.Excel;
//引用



            // 設定儲存檔名
            string pathFileName = @"C:\Users\user\Desktop\t\OutputExcel.xlsx";

            // 開啟一個新的應用程式
            Excel.Application ExcelApplication = new Excel.Application();

            // 使Excel為可見狀態
            ExcelApplication.Visible = true;

            // 新增新的活頁簿
            ExcelApplication.Workbooks.Add(Type.Missing);

            // 使用第一個活頁簿
            Excel._Workbook BookName = ExcelApplication.Workbooks[1];

            

            for (int i = 0; i <=11; i++)
            {
                string sheetName = cbxSheetName.Items[i].ToString();

                //新增工作表
                var xlSheets = ExcelApplication.Sheets as Excel.Sheets;
                var xlNewSheet = (Excel.Worksheet)xlSheets.Add(xlSheets[i+1], Type.Missing, Type.Missing, Type.Missing);
                xlNewSheet.Name = sheetName;

                try
                {
                    // 引用第一個工作表
                    Excel._Worksheet SheetName = (Excel._Worksheet)BookName.Worksheets[i+1];

                    // 設定工作表的名
                    SheetName.Name = sheetName;

                    // 將目前的工作表變成現用工作表
                    SheetName.Activate();

                    // 設定第1列資料
                    ExcelApplication.Cells[1, 1] = "No.";
                    ExcelApplication.Cells[1, 2] = "Date";
                    ExcelApplication.Cells[1, 3] = "Income/Payment";
                    ExcelApplication.Cells[1, 4] = "Amount";
                    ExcelApplication.Cells[1, 5] = "Type";
                    ExcelApplication.Cells[1, 6] = "Object";
                    ExcelApplication.Cells[1, 7] = "Detail";
                    ExcelApplication.Cells[1, 8] = "Budget Rest";
                    ExcelApplication.Cells[1, 9] = "Total Rest";

                    
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine("產出Excel出問題" + Environment.NewLine + "錯誤訊息:" + ex.Message);
                }
            }
            try
            {
                //儲存活頁簿
                BookName.SaveAs(pathFileName);
                MessageBox.Show("已儲存完畢" + Environment.NewLine + "儲存路徑為:" + pathFileName);
            }
            catch( Exception ex)
            {
                Console.WriteLine("產出Excel出問題" + Environment.NewLine + "錯誤訊息:" + ex.Message);
            }
        






