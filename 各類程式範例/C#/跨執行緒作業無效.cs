/*
常見情形
new thread 欲操作主執行緒(form)的控制項

建議將主執行緒執行的部分獨立出來呼叫callback 否則可能有新建完執行緒後卻一直執行主執行緒的情形發生
*/

using Chevalier.iBox;
using Chevalier.iBox.Devices;
using Chevalier.iBox.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Chevalier.iBox.Runtime;

namespace Test_WinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private delegate void AppendMessageCallback(string message);
        /// <summary>
        /// 新增訊息
        /// </summary>
        /// <param name="message">訊息</param>
        /// <remarks>不同執行緒也可以用</remarks>
        private void AppendMessage(string message)
        {
            try
            {
                if (this.InvokeRequired == true)
                {
                    AppendMessageCallback callback = new AppendMessageCallback(AppendMessage);
                    this.Invoke(callback, new object[] { message });
                }
                else
                {
                    string text = textBox_Message.Text;
                    if (text != "") { text = "\r\n" + text; }
                    textBox_Message.Text = message + text;

                    //textBox_Message.Text = message;
                }
            }
            catch { }
        }
