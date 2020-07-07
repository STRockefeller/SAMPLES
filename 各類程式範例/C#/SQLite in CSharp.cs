1.安裝Nuget套件 System.Data.SQLite 
2.using System.Data.SQLite;
3.建立DataBase
	SQLiteConnection.CreateFile(dataBasePath:string);
	可以搭配System.IO的File.Exists(fileName):bool來使用
4.連線至DB
	string connectionString = "data source ="+dataBasePath;
	SQLiteConnection connection = new SQLiteConnection(connectionString);
	connection.Open();
5.Query 指令 (以下參考 https://dotblogs.com.tw/chichiBlog/2017/10/16/155514)
Asp.net 主要執行SQL 有三種方法，ExecuteNonQuery() 、 ExecuteScalar()、ExecuteReader()

ExecuteNonQuery( )：

主要用來執行INSERT、UPDATE、DELETE和其他沒有返回值得SQL命令。

ExecuteScalar( )：

返回結果集為：第一列的第一行。

經常用來執行SQL的COUNT、AVG、MIN、MAX 和 SUM 函數。

PS: ExecuteScalar 返回為Object類型，必須強置轉型。

EX:

object objResult = objCMD.ExecuteScalar()
假設想要轉型成string 
string result = cmd.ExecuteScalar().ToString();

ExecuteReader( )：

快速的對資料庫進行查詢並得到結果。

返回為DataReader物件，如果在SqlCommand物件中調用，則返回SqlDataReader。

對SqlDataReader.Read的每次調用都會從結果集中返回一行。

Q&A：
Q1：若我的資料不只有一行，是好多行要怎麼利用 ExecuteReader() 來全部讀出呢??

A1：因為 ExecuteReader ( ) 它是集中返回一行，所以我們必須讓她一直重複讀取的動作，直到無不到東西為止

我們透過SqlDataReader去接executeReader() 所返回的物件，並且透過sqlDataReader .Read() 讓它一直讀

但怕讀到的為空白行，所以特別增加一個條件確定sqlDataReader  不為空白行(DBnull.Value) 

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
           
2018/05/25更新

果然這樣寫我還是不記得怎麼寫，在寫詳細點。這樣應該更清楚啦~~

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



6.註 NO CURRENT ROW 錯誤訊息 需要加入 if(SqlDataReader.Read())條件進行以確保目前有資料 (即便select出來的只有一列亦是如此)
