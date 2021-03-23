# Process

[Reference:MSDN](https://docs.microsoft.com/zh-tw/dotnet/api/system.diagnostics.process?view=net-5.0)

```C#
public class Process : System.ComponentModel.Component, IDisposable
```

提供對本機和遠端處理序的存取，能夠啟動和停止本機系統處理序。



原本是要查call commandline的方法意外的發現Process這東西能用的地方很多，這邊筆記一下。

MSDN Sample

```C#
using System;
using System.Diagnostics;
using System.ComponentModel;

namespace MyProcessSample
{
    class MyProcess
    {
        public static void Main()
        {
            try
            {
                using (Process myProcess = new Process())
                {
                    myProcess.StartInfo.UseShellExecute = false;
                    // You can start any process, HelloWorld is a do-nothing example.
                    myProcess.StartInfo.FileName = "C:\\HelloWorld.exe";
                    myProcess.StartInfo.CreateNoWindow = true;
                    myProcess.Start();
                    // This code assumes the process you are starting will terminate itself.
                    // Given that is is started without a window so you cannot terminate it
                    // on the desktop, it must terminate itself or you can do it programmatically
                    // from this application using the Kill method.
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
```

MSDN Sample

```C#
using System;
using System.Diagnostics;
using System.ComponentModel;

namespace MyProcessSample
{
    class MyProcess
    {
        // Opens the Internet Explorer application.
        void OpenApplication(string myFavoritesPath)
        {
            // Start Internet Explorer. Defaults to the home page.
            Process.Start("IExplore.exe");

            // Display the contents of the favorites folder in the browser.
            Process.Start(myFavoritesPath);
        }

        // Opens urls and .html documents using Internet Explorer.
        void OpenWithArguments()
        {
            // url's are not considered documents. They can only be opened
            // by passing them as arguments.
            Process.Start("IExplore.exe", "www.northwindtraders.com");

            // Start a Web page using a browser associated with .html and .asp files.
            Process.Start("IExplore.exe", "C:\\myPath\\myFile.htm");
            Process.Start("IExplore.exe", "C:\\myPath\\myFile.asp");
        }

        // Uses the ProcessStartInfo class to start new processes,
        // both in a minimized mode.
        void OpenWithStartInfo()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("IExplore.exe");
            startInfo.WindowStyle = ProcessWindowStyle.Minimized;

            Process.Start(startInfo);

            startInfo.Arguments = "www.northwindtraders.com";

            Process.Start(startInfo);
        }

        static void Main()
        {
            // Get the path that stores favorite links.
            string myFavoritesPath =
                Environment.GetFolderPath(Environment.SpecialFolder.Favorites);

            MyProcess myProcess = new MyProcess();

            myProcess.OpenApplication(myFavoritesPath);
            myProcess.OpenWithArguments();
            myProcess.OpenWithStartInfo();
        }
    }
}
```



## Call CMD 範例

```C#
namespace cmdTest
{
public class Program
{
static void Main(string[] args)
{
Process p = new Process();
String str=null;

p.StartInfo.FileName = "cmd.exe";

p.StartInfo.UseShellExecute = false;
p.StartInfo.RedirectStandardInput = true;
p.StartInfo.RedirectStandardOutput = true;
p.StartInfo.RedirectStandardError = true;
//p.StartInfo.CreateNoWindow = true; //不跳出cmd視窗

p.Start();
p.StandardInput.WriteLine("ipconfig");
p.StandardInput.WriteLine("exit");

str = p.StandardOutput.ReadToEnd();
p.WaitForExit();
p.Close();


Console.WriteLine(str);
Console.ReadKey();
}
}
}
```

