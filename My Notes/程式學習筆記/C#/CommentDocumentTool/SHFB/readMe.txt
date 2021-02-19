EWSoftware 的 SHFB 註解文檔生成工具
1.首先於VS上安裝SHFB Nuget套件
2.於方案中新增專案：專案類型選擇Documentation-->Sandcastle Help File Build Project
3.對目標專案選擇：屬性-->建置-->輸出-->勾選"XML文件檔案"
4.在新增的SHFB專案中"Documentation Sources"底下加入目標專案的.csproj檔案。
5.建置。