# SQLite in C#

首先要安裝Nuget套件 System.Data.SQLite 

```C#
using System.Data.SQLite;
```

## 建立DataBase

```C#
SQLiteConnection.CreateFile(dataBasePath:string);
```

可以搭配System.IO的File.Exists(fileName):bool來使用

```C#
if(!File.Exists(dataBasePath))
    SQLiteConnection.CreateFile(dataBasePath);
```

## 連線至DB

```C#
string connectionString = "data source ="+dataBasePath;
	SQLiteConnection connection = new SQLiteConnection(connectionString);
	connection.Open();
```

## Query 指令 

參考 https://dotblogs.com.tw/chichiBlog/2017/10/16/155514

Asp.net 主要執行SQL 有三種方法，ExecuteNonQuery() 、 ExecuteScalar()、ExecuteReader()

**ExecuteNonQuery( )：**

主要用來執行INSERT、UPDATE、DELETE和其他沒有返回值得SQL命令。

**ExecuteScalar( )：**

返回結果集為：第一列的第一行。

經常用來執行SQL的COUNT、AVG、MIN、MAX 和 SUM 函數。

PS: ExecuteScalar 返回為Object類型，必須強置轉型。

EX:

```C#
object objResult = objCMD.ExecuteScalar()
```

假設想要轉型成string 

```C#
string result = cmd.ExecuteScalar().ToString();
```

**ExecuteReader( )：**

快速的對資料庫進行查詢並得到結果。

返回為DataReader物件，如果在SqlCommand物件中調用，則返回SqlDataReader。

對SqlDataReader.Read的每次調用都會從結果集中返回一行。


若我的資料不只有一行，是好多行要怎麼利用 ExecuteReader() 來全部讀出呢??

因為 ExecuteReader ( ) 它是集中返回一行，所以我們必須讓她一直重複讀取的動作，直到無不到東西為止

我們透過SqlDataReader去接executeReader() 所返回的物件，並且透過sqlDataReader .Read() 讓它一直讀

但怕讀到的為空白行，所以特別增加一個條件確定sqlDataReader  不為空白行(DBnull.Value) 

```C#
SqlConnection conn = openMSsqlConnection(); //連線
SqlCommand cmd = new SqlCommand(SQLString, conn);
SqlDataReader dr = cmd.ExecuteReader();
while (dr.Read())
{
   //防止為空白行
  if (!dr[0].Equals(DBNull.Value))
  {
     //do something               
  }
}
```

```C#
string SQLString = "SELECT USERSNAME,USERID FROM USERS WHERE USERID LIKE '06%'";

SqlConnection conn = openMSsqlConnection(); //連線
SqlCommand cmd = new SqlCommand(SQLString, conn);
SqlDataReader dr = cmd.ExecuteReader();
while (dr.Read())
{
   //防止為空白行
  if (!dr[0].Equals(DBNull.Value))
  {
      string USERSNAME = dr["USERSNAME"].ToString();
      string USERID = dr["USERID"].ToString();
      //do something  
  }
}
```



註 ：NO CURRENT ROW 錯誤訊息 需要加入 if(SqlDataReader.Read())條件進行以確保目前有資料 (即便select出來的只有一列亦是如此)



## 關於`System.Data.Sqlite`以及`Microsoft.Data.Sqlite`的區別

https://stackoverflow.com/questions/51933421/system-data-sqlite-vs-microsoft-data-sqlite

https://docs.microsoft.com/zh-tw/dotnet/standard/data/sqlite/compare

目前看來是兩邊同時有在發展，雙方有一部分的方法不互通，現階段只有Microsoft的版本對EF core和.net core支援。