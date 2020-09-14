//引用
using system.IO;
//引用

private void button1_Click(object sender, EventArgs e)
        {
            // 讀取TXT檔內文串
            /*
                StreamReader str = new StreamReader(@"E:\pixnet\20160614\Lab2_TXT_Read_Write\Read.TXT");
                StreamReader str = new StreamReader(讀取TXT檔路徑)    
                str.ReadLine(); (一行一行讀取)
                str.ReadToEnd();(一次讀取全部)
                str.Close(); (關閉str)
            */
            StreamReader str = new StreamReader(@"E:\pixnet\20160614\Lab2_TXT_Read_Write\Read.TXT");
            string ReadLine1, ReadLine2, ReadAll;
            ReadLine1 = str.ReadLine();
            ReadLine2 = str.ReadLine();
            ReadAll = str.ReadToEnd();
            MessageBox.Show("ReadLine1 = " + ReadLine1);
            MessageBox.Show("ReadLine2 = " + ReadLine2);
            MessageBox.Show("ReadAll = " + ReadAll);
            str.Close();
        }

        Write的按鈕事件下輸入

private void button2_Click(object sender, EventArgs e)
        {
            // 將字串寫入TXT檔
            StreamWriter str = new StreamWriter(@"E:\pixnet\20160614\Lab2_TXT_Read_Write\Write.TXT");
            string WriteWord = "aaaaa";
            str.WriteLine(WriteWord);
            str.WriteLine("bbb");
            str.Close();
        }

        protected void Button1_Click(object sender, EventArgs e)
    {
        FileStream fileStream = new FileStream(@"c:\test1.txt", FileMode.Create);

        fileStream.Close();   //切記開了要關,不然會被佔用而無法修改喔!!!

        using (StreamWriter sw = new StreamWriter(@"c:\test1.txt"))
        {
            // 欲寫入的文字資料 ~

            sw.Write("This is  ");
            sw.WriteLine("Shinyo Test lallalallaaaa.");
            sw.WriteLine("-------o   ---  V ---- o  -----");
            sw.Write("Time is : ");
            sw.WriteLine(DateTime.Now);
        }

    }
