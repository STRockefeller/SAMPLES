# SQLite Note



[Reference](https://www.sqlitetutorial.net/)

## Abstract

Linq 用多了發現SQL語法反而常常想不起來，這邊再從頭整理一次筆記，方便複習。

安裝和Command Line 指令略過不提

有些懶得詳細寫就放上linq對照

## Select

可以做簡單的四則運算

SQLite

```sqlite
Select
	1+1;
```

```
2
```



SQLite

```sqlite
Select
	4%3;
```

```
1
```



SQLite

```Sqlite
Select
	1+1=2;
```

```
1
```



SQLite

```Sqlite
Select
	1+1=3;
```

```
0
```



SQLite

```sqlite
SELECT
	10 / 5, 2 * 4 ;
```

```
2|8
```

---

一般用法

SQLite

```sqlite
SELECT
	trackid
FROM
	tracks;
```

C#

```C#
tracks.Select(t=>t.trackid);
```



SQLite

```sqlite
SELECT
	*
FROM
	tracks;
```

C#

```C#
tracks.Select(t=>t);
```



SQLite

```sqlite
SELECT
	trackid,
	name,
	composer,
	unitprice
FROM
	tracks;
```

C#

```C#
tracks.Select(t=>(t.trackid,t.name,t.composer,unitprice));
```



## Order By

ASC是升冪，DESC是降冪



SQLite

```sqlite
SELECT
	name,
	milliseconds, 
	albumid
FROM
	tracks
ORDER BY
	albumid ASC;
```

C#

```C#
tracks.Select(t=>(t.name,t.milliseconds,t.albumid)).OrderBy(t=>t.albumid);
```



SQLite

```sqlite
SELECT
	name,
	milliseconds, 
	albumid
FROM
	tracks
ORDER BY
	albumid ASC,
    milliseconds DESC;
```

C#

```C#
tracks.Select(t=>(t.name,t.milliseconds,t.albumid)).OrderBy(t=>t.albumid).ThenByDescending(t=>t.milliseconds);
```



---

選擇以第幾行為依據排列，這我不曉得怎麼用linq類比



SQLite

```sqlite
SELECT
	name,
	milliseconds, 
	albumid
FROM
	tracks
ORDER BY
	 3,2;
```

先以第三行的內容作升冪排列再以第二行的內容作升冪排列。



---

### Null數值的排序

在SQLite中Null被視為最小值，在ASC中會排在最前面，DESC則是最後方。



## Select Distinct

SQLite

```sqlite
SELECT DISTINCT
	city
FROM
	customers;
```

C#

```C#
customers.Select(c=>c.city).Distinct();
```



## Where

比較運算子

| Operator | Meaning                  |
| :------- | :----------------------- |
| =        | Equal to                 |
| <> or != | Not equal to             |
| <        | Less than                |
| >        | Greater than             |
| <=       | Less than or equal to    |
| >=       | Greater than or equal to |

邏輯運算子

| Operator                                                  | Meaning                                                      |
| :-------------------------------------------------------- | :----------------------------------------------------------- |
| ALL                                                       | returns 1 if all expressions are 1.                          |
| AND                                                       | returns 1 if both expressions are 1, and 0 if one of the expressions is 0. |
| ANY                                                       | returns 1 if any one of a set of comparisons is 1.           |
| [BETWEEN](https://www.sqlitetutorial.net/sqlite-between/) | returns 1 if a value is within a range.                      |
| [EXISTS](https://www.sqlitetutorial.net/sqlite-exists/)   | returns 1 if a subquery contains any rows.                   |
| [IN](https://www.sqlitetutorial.net/sqlite-in/)           | returns 1 if a value is in a list of values.                 |
| [LIKE](https://www.sqlitetutorial.net/sqlite-like/)       | returns 1 if a value matches a pattern                       |
| NOT                                                       | reverses the value of other operators such as NOT EXISTS, NOT IN, NOT BETWEEN, etc. |
| OR                                                        | returns true if either expression is 1                       |

---

SQLite

```sqlite
SELECT
   name,
   milliseconds,
   bytes,
   albumid
FROM
   tracks
WHERE
   albumid = 1;
```

C#

```C#
tracks.Where(t=>t.albumid==1).Select(t=>(t.name,t.millisecond,t.bytes,t.albumid));
```



SQLite

```sqlite
SELECT
	name,
	milliseconds,
	bytes,
	albumid
FROM
	tracks
WHERE
	albumid = 1
AND milliseconds > 250000;
```

C#

```C#
tracks.Where(t=>t.albumid==1&&t.milliseconds>250000).Select(t=>(t.name,t.millisecond,t.bytes,t.albumid));
```



SQLite

```sqlite
SELECT
	name,
	albumid,
	composer
FROM
	tracks
WHERE
	composer LIKE '%Smith%'
ORDER BY
	albumid;
```

這個我不會用linq類比



SQLite

```sqlite
SELECT
	name,
	albumid,
	mediatypeid
FROM
	tracks
WHERE
	mediatypeid IN (2, 3);
```

C#

```C#
tracks.Where(t=>t.mediatypeid==2||t.mediatypeid==3).Select(t=>(t.name,t.albumid,t.mediatypeid));
```



## Limit / Offset

SQLite

```sqlite
SELECT
	trackId,
	name
FROM
	tracks
LIMIT 10;
```

C#

```C#
tracks.Select(t=>(t.trackId,t.name)).Take(10);
```

---

SQLite

```sqlite
SELECT
	trackId,
	name
FROM
	tracks
LIMIT 10 OFFSET 10;
```

C#

```C#
tracks.Select(t=>(t.trackId,t.name)).Skip(10).Take(10);
```



## Between

SQLite

```sqlite
SELECT
    InvoiceId,
    BillingAddress,
    Total
FROM
    invoices
WHERE
    Total BETWEEN 14.91 and 18.86    
ORDER BY
    Total; 
```

C#

```C#
invoices.Where(i=>i.Total>=14.91&&i.Total<=18.86).Select(i=>(i.InvoiceId,i.BillingAddress,i.Total)).OrderBy(i=>i.Total);
```



SQLite

```sqlite
SELECT
    InvoiceId,
    BillingAddress,
    Total
FROM
    invoices
WHERE
    Total NOT BETWEEN 1 and 20
ORDER BY
    Total;    
```

C#

```C#
invoices.Where(i=>!(i.Total>=1&&i.Total<=20)).Select(i=>(i.InvoiceId,i.BillingAddress,i.Total)).OrderBy(i=>i.Total);
```



SQLite

```sqlite
SELECT
    *
FROM
    invoices
WHERE
    InvoiceDate BETWEEN '2010-01-01' AND '2010-01-31'
ORDER BY
    InvoiceDate;   
```

C#

```C#
invoices.Where(i=>IsInDate(i.InvoiceDate)).Select(i=>i).OrderBy(i=>i.InvoiceDate);
    
private bool IsInDate(DateTime date)=>
    return date.CompareTo(new DateTime(2010,1,1)) >= 0 && date.CompareTo(new DateTime(2010,1,31)) <= 0;
```



## In

> The SQLite `IN` operator determines whether a value matches any value in a list or a [subquery](https://www.sqlitetutorial.net/sqlite-subquery/). 

在[Where](#Where)處有一個範例有用到這個關鍵字

用法有二，其一是判斷是否在給定的集合內，另一是以query判斷。



SQLite

```sqlite
SELECT
	*
FROM
	Tracks
WHERE
	MediaTypeId IN (1, 2)
ORDER BY
	Name ASC;
```

C#

```C#
Tracks.Where(t=>t.MediaTypeId==1||t.MediaTypeId==2).Select(t=>t).OrderBy(t=>t.Name);
```



SQLite

```sqlite
SELECT
	TrackId, 
	Name, 
	AlbumId
FROM
	Tracks
WHERE
	AlbumId IN (
		SELECT
			AlbumId
		FROM
			Albums
		WHERE
			ArtistId = 12
	);
```

C#

```C#
Tracks.Where(t=>Condition(t.AlbumId)).Select(t=>(t.TrackId,t.Name,t.AlbumId));

bool Condition(int albumId)=>Albums.Where(a=>a.ArtistId==12).Contains(albumId);
```



## Like

同樣是不曉得如何用linq類比的

再次強調SQLite屬於`case-insensitive`，這個特性同樣適用於Like關鍵字



```sqlite
SELECT
	trackid,
	name	
FROM
	tracks
WHERE
	name LIKE 'Wild%';
```



```sqlite
SELECT
	trackid,
	name
FROM
	tracks
WHERE
	name LIKE '%Br_wn%';
```



如果要找'%'字元要使用反斜線 `\%`

```sqlite
SELECT c 
FROM t 
WHERE c LIKE '%10\%%' ESCAPE '\';
```



## Is Null

在SQLite中，Null=Null是不成立的。

所以以下描述

```sqlite
SELECT
	*
FROM
	tracks
Where
	Composer = null;
```

是不成立的，結果當然是甚麼都找不到



應該寫成

```sqlite
SELECT
	*
FROM
	tracks
WHERE
	Composer IS null;
```



如果要找非Null的值，同理應該寫成`IS NOT NULL`而非`!=NULL`



## Glob

與[Like](#Like)關鍵字類似，但為大小寫敏感類型

> Unlike the `LIKE` operator, the `GLOB` operator is **case sensitive** and uses the **UNIX wildcards.** 

> The following shows the wildcards used with the `GLOB` operator:
>
> - The asterisk (*) wildcard matches any number of characters.
> - The question mark (?) wildcard matches exactly one character.

SQLite

```sqlite
SELECT
	trackid,
	name
FROM
	tracks
WHERE
	name GLOB '?ere*';
```

找 `一個任意字元+"ere"+任意字串`



---

關於數字搜尋

To find the tracks whose names contain numbers, you can use the list wildcard `[0-9]` as follows:

```sqlite
SELECT
	trackid,
	name
FROM
	tracks
WHERE
	name GLOB '*[1-9]*';
```



Or to find the tracks whose name does not contain any number, you place the character `^` at the beginning of the list:

```sqlite
SELECT
	trackid,
	name
FROM
	tracks
WHERE
	name GLOB '*[^1-9]*';
```



The following statement finds the tracks whose names end with a number.

```sqlite
SELECT
	trackid,
	name
FROM
	tracks
WHERE
	name GLOB '*[1-9]';
```



## Join

一般分為`INNER JOIN`, `LEFT JOIN`,  `CROSS JOIN` ,  `RIGHT JOIN` and `FULL OUTER JOIN`

SQLite 支援前三種



範例table結構如下圖

![](https://cdn.sqlitetutorial.net/wp-content/uploads/2018/11/artists_albums.png)



### Inner Join

![](https://cdn.sqlitetutorial.net/wp-content/uploads/2015/12/SQLite-Inner-Join-Example.png)

![](https://cdn.sqlitetutorial.net/wp-content/uploads/2015/12/SQLite-inner-join-venn-diagram.png)

列出albums.Title和artists.Name (僅albums.ArtistsId和artists.ArtistsId可以對上的部分)

```sqlite
SELECT 
    Title,
    Name
FROM 
    albums
INNER JOIN artists 
    ON artists.ArtistId = albums.ArtistId;
```



也可以縮寫成

```sqlite
SELECT
   Title, 
   Name
FROM
   albums
INNER JOIN artists USING(ArtistId);
```



### Left Join

![](https://cdn.sqlitetutorial.net/wp-content/uploads/2015/12/SQLite-left-join-example.png)

![](https://cdn.sqlitetutorial.net/wp-content/uploads/2015/12/SQLite-Left-Join-Venn-Diagram.png)

和[Inner Join](#Inner Join)很像，但結果的表格以artists為主，也就是說當`artists.ArtistId = albums.ArtistId`不成立時，輸出會顯示artists的部分，另外albums的部分會以null填入。

以下述程式碼為例，若`artists.ArtistId = 3`的位置對應不到相應的`albums.ArtistId = 3`則結果會顯示一列 有Name但Title = Null的值。

```sqlite
SELECT
    Name, 
    Title
FROM
    artists
LEFT JOIN albums ON
    artists.ArtistId = albums.ArtistId;
```



> ![](https://cdn.sqlitetutorial.net/wp-content/uploads/2019/08/sqlite-join-left-join-example.png)

一樣可以用Using縮寫

```sqlite
SELECT
   Name, 
   Title
FROM
   artists
LEFT JOIN albums USING (ArtistId);
```



### Cross Join

和前兩者很不一樣，不用選擇結合條件，全都合就完事了

比如以下若products的內容是產品1、產品2、...；calendars的內容是(2020,1)、(2020,2)...

```sqlite
SELECT * 
FROM products
CROSS JOIN calendars;
```

合出來大概像

| products.Name | calendars.Year | calendars.Month |
| ------------- | -------------- | --------------- |
| 產品1         | 2020           | 1               |
| 產品1         | 2020           | 2               |
| 產品1         | 2020           | 3               |
| ...           | ...            | ...             |
| 產品2         | 2020           | 1               |
| 產品2         | 2020           | 2               |
| 產品2         | 2020           | 3               |
| 產品2         | 2020           | 4               |
| ...           | ...            | ...             |

