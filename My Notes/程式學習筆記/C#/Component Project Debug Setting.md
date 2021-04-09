# 類別庫DEBUG設定

單純的情況不討論，這邊紀錄`匯出的dll須由其他的執行檔執行同時進行DEBUG的情況`。

## 建置位置設定

`專案-->屬性-->建置-->輸出路徑` 或

`偵錯-->偵錯屬性-->建置-->輸出路徑`

![](https://i.imgur.com/7sNd5mB.png)



設定完後建置可能會在該路徑下生成資料夾，我使用的是.net standard專案，建置出的資料夾名為`netstandard2.0`，輸出的內容則在該資料夾底下。若不希望多這個資料夾，可以透過編輯專案檔，在`PropertyGroup` tag 底下加入`<AppendTargetFrameworkToOutputPath>false</AppendTargetFrameworkToOutputPath>`



完整如下

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework>
  </PropertyGroup>

  <PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Debug|AnyCPU'">
    <OutputPath>C:\Users\admin\Desktop\HC_10C_K_W32_10.118.24J\DiskC\OpenCnc Shared\OCRes\Common\Plugin\DLL\</OutputPath>
    <AppendTargetFrameworkToOutputPath>false</AppendTargetFrameworkToOutputPath>
    <WarningLevel>1</WarningLevel>
  </PropertyGroup>

  <ItemGroup>
    <Reference Include="MMICommon32">
      <HintPath>..\..\..\DiskC\OpenCNC\Bin\MMICommon32.dll</HintPath>
    </Reference>
    <Reference Include="Syntec.OpenCNC">
      <HintPath>..\..\..\DiskC\OpenCNC\Bin\Syntec.OpenCNC.dll</HintPath>
    </Reference>
    <Reference Include="System.Windows.Forms">
      <HintPath>..\..\..\..\..\..\..\Windows\Microsoft.NET\Framework\v2.0.50727\System.Windows.Forms.dll</HintPath>
    </Reference>
  </ItemGroup>

</Project>
```



## Debug 執行檔設定

專案屬性中設定

* 啟動:可執行檔
* 可執行檔路徑
* 工作目錄(可執行檔位置)



![](https://i.imgur.com/1hb7Kwi.png)