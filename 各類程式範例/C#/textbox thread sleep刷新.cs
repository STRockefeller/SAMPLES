//必須加上tbxLog.Refresh()才可以在每個迴圈都將訊息顯示在tbxLog中，否則主執行緒(Form)會停止直到迴圈跑完再一次顯示。
        private void btnCt1NT_Click(object sender, EventArgs e)
        {

            int i = 0;
            while(i<20)
            {
                tbxLog.Text += $"\r\nNThread1{i.ToString()}";
                i++;
                Thread.Sleep(1000);
                tbxLog.Refresh();
            }
        }