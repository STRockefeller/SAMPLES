以Winform為例

參考
Opc.Ua.Client.dll
Opc.Ua.Configuration.dll
Opc.Ua.Core.dll

program.cs-->main

	    ApplicationInstance application = new ApplicationInstance();
            application.ApplicationType = ApplicationType.Client;
            application.ConfigSectionName = "OPCUAClient";//名稱隨意


form的建構子加入引數

public frmBase(ApplicationConfiguration configuration)

program.cs-->main
Application.Run(new frmBase(application.ApplicationConfiguration));