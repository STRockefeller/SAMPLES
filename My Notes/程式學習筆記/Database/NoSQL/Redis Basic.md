# Redis Basic

Reference:

https://redis.io/

https://blog.techbridge.cc/2016/06/18/redis-introduction/

http://tw.gitbook.net/redis/redis_quick_guide.html

https://marcus116.blogspot.com/2019/02/how-to-install-redis-in-windows-os.html

https://www.tutorialspoint.com/redis/redis_data_types.htm



## Review with Questions

* 嘗試說明在redis中`list` `sets` `sorted sets`三種型別的區別。 [Ans](#List)



## Abstract

[Redis](http://redis.io/) 是一個 in-memory 的 key-value database，因此常常被用在需要快取（Cache）一些資料的場合，可以減輕許多後端資料庫的壓力。

以下是Redis的一些代表特色

- Redis數據庫完全在內存中，使用磁盤僅用於持久性。
- 相比許多鍵值數據存儲，Redis擁有一套較為豐富的數據類型。
- Redis可以將數據複製到任意數量的從服務器。

**Redis 優勢**

- 異常快速：Redis的速度非常快，每秒能執行約11萬集合，每秒約81000+條記錄。
- 支持豐富的數據類型：Redis支持最大多數開發人員已經知道像列表，集合，有序集合，散列數據類型。這使得它非常容易解決各種各樣的問題，因為我們知道哪些問題是可以處理通過它的數據類型更好。
- 操作都是原子性：所有Redis操作是原子的，這保證了如果兩個客戶端同時訪問的Redis服務器將獲得更新後的值。
- 多功能實用工具：Redis是一個多實用的工具，可以在多個用例如緩存，消息，隊列使用(Redis原生支持發布/訂閱)，任何短暫的數據，應用程序，如Web應用程序會話，網頁命中計數等。



## Installation(Windows 10)

> Redis 主要是運行在 Linux 環境，因此在官網下載區是找不到 Windows 版安裝程式，需要到 [MicrosoftArchive/redis](https://github.com/MicrosoftArchive/redis) 提供的 github 頁面中點擊 release page 連結才有 Windows for Redis 安裝檔



目前看來這個項目已經五六年沒有更新了，最新的release時間在*1 Jul 2016* 分別是3.0.504 和 3.2.100(pre)

還有分安裝版和免安裝版，這次筆記過程中使用3.2.100(pre)的免安裝版本。



下載完成後打開server

```powershell
[5200] 19 Jul 13:02:41.991 # Warning: no config file specified, using the default config. In order to specify a config file use C:\Users\admin\Downloads\Redis-x64-3.2.100\redis-server.exe /path/to/redis.conf
                _._
           _.-``__ ''-._
      _.-``    `.  `_.  ''-._           Redis 3.2.100 (00000000/0) 64 bit
  .-`` .-```.  ```\/    _.,_ ''-._
 (    '      ,       .-`  | `,    )     Running in standalone mode
 |`-._`-...-` __...-.``-._|'` _.-'|     Port: 6379
 |    `-._   `._    /     _.-'    |     PID: 5200
  `-._    `-._  `-./  _.-'    _.-'
 |`-._`-._    `-.__.-'    _.-'_.-'|
 |    `-._`-._        _.-'_.-'    |           http://redis.io
  `-._    `-._`-.__.-'_.-'    _.-'
 |`-._`-._    `-.__.-'    _.-'_.-'|
 |    `-._`-._        _.-'_.-'    |
  `-._    `-._`-.__.-'_.-'    _.-'
      `-._    `-.__.-'    _.-'
          `-._        _.-'
              `-.__.-'

[5200] 19 Jul 13:02:42.036 # Server started, Redis version 3.2.100
[5200] 19 Jul 13:02:42.039 * The server is now ready to accept connections on port 6379
```

接著打開client

```powershell
127.0.0.1:6379>
```

試著輸入點東西

```powershell
127.0.0.1:6379> info
# Server
redis_version:3.2.100
redis_git_sha1:00000000
redis_git_dirty:0
redis_build_id:dd26f1f93c5130ee
redis_mode:standalone
os:Windows
arch_bits:64
multiplexing_api:WinSock_IOCP
process_id:5200
run_id:a5a80368670d1181cd377757fb031ead602b63b3
tcp_port:6379
uptime_in_seconds:204
uptime_in_days:0
hz:10
lru_clock:16058430
executable:C:\Users\admin\Downloads\Redis-x64-3.2.100\redis-server.exe
config_file:

# Clients
connected_clients:1
client_longest_output_list:0
client_biggest_input_buf:0
blocked_clients:0

# Memory
used_memory:690056
used_memory_human:673.88K
used_memory_rss:652272
used_memory_rss_human:636.98K
used_memory_peak:690056
used_memory_peak_human:673.88K
total_system_memory:0
total_system_memory_human:0B
used_memory_lua:37888
used_memory_lua_human:37.00K
maxmemory:0
maxmemory_human:0B
maxmemory_policy:noeviction
mem_fragmentation_ratio:0.95
mem_allocator:jemalloc-3.6.0

# Persistence
loading:0
rdb_changes_since_last_save:0
rdb_bgsave_in_progress:0
rdb_last_save_time:1626670962
rdb_last_bgsave_status:ok
rdb_last_bgsave_time_sec:-1
rdb_current_bgsave_time_sec:-1
aof_enabled:0
aof_rewrite_in_progress:0
aof_rewrite_scheduled:0
aof_last_rewrite_time_sec:-1
aof_current_rewrite_time_sec:-1
aof_last_bgrewrite_status:ok
aof_last_write_status:ok

# Stats
total_connections_received:1
total_commands_processed:1
instantaneous_ops_per_sec:0
total_net_input_bytes:31
total_net_output_bytes:5941289
instantaneous_input_kbps:0.00
instantaneous_output_kbps:0.00
rejected_connections:0
sync_full:0
sync_partial_ok:0
sync_partial_err:0
expired_keys:0
evicted_keys:0
keyspace_hits:0
keyspace_misses:0
pubsub_channels:0
pubsub_patterns:0
latest_fork_usec:0
migrate_cached_sockets:0

# Replication
role:master
connected_slaves:0
master_repl_offset:0
repl_backlog_active:0
repl_backlog_size:1048576
repl_backlog_first_byte_offset:0
repl_backlog_histlen:0

# CPU
used_cpu_sys:0.11
used_cpu_user:0.09
used_cpu_sys_children:0.00
used_cpu_user_children:0.00

# Cluster
cluster_enabled:0

# Keyspace
```

看起來沒問題。



## Data Type

共五種

### String

> Redis string is a sequence of bytes. Strings in Redis are binary safe, meaning they have a known length not determined by any special terminating characters. Thus, you can store anything up to 512 megabytes in one string.

```powershell
127.0.0.1:6379> set str "Hello World"
OK
127.0.0.1:6379> get str
"Hello World"
```



### Hashes

> A Redis hash is a collection of key value pairs. Redis Hashes are maps between string fields and string values. Hence, they are used to represent objects.

```powershell
redis 127.0.0.1:6379> HMSET user:1 username tutorialspoint password 
tutorialspoint points 200 
OK 
redis 127.0.0.1:6379> HGETALL user:1  
1) "username" 
2) "tutorialspoint" 
3) "password" 
4) "tutorialspoint" 
5) "points" 
6) "200"
```



### List

> Redis Lists are simply lists of strings, sorted by insertion order. You can add elements to a Redis List on the head or on the tail.

```powershell
redis 127.0.0.1:6379> lpush tutoriallist redis 
(integer) 1 
redis 127.0.0.1:6379> lpush tutoriallist mongodb 
(integer) 2 
redis 127.0.0.1:6379> lpush tutoriallist rabitmq 
(integer) 3 
redis 127.0.0.1:6379> lrange tutoriallist 0 10  

1) "rabitmq" 
2) "mongodb" 
3) "redis"
```



### Sets

> Redis Sets are an unordered collection of strings. In Redis, you can add, remove, and test for the existence of members in O(1) time complexity.



```powershell
redis 127.0.0.1:6379> sadd tutoriallist redis 
(integer) 1 
redis 127.0.0.1:6379> sadd tutoriallist mongodb 
(integer) 1 
redis 127.0.0.1:6379> sadd tutoriallist rabitmq 
(integer) 1 
redis 127.0.0.1:6379> sadd tutoriallist rabitmq 
(integer) 0 
redis 127.0.0.1:6379> smembers tutoriallist  

1) "rabitmq" 
2) "mongodb" 
3) "redis" 
```



### Sorted Sets

> Redis Sorted Sets are similar to Redis Sets, non-repeating collections of Strings. The difference is, every member of a Sorted Set is associated with a score, that is used in order to take the sorted set ordered, from the smallest to the greatest score. While members are unique, the scores may be repeated.

```powershell
redis 127.0.0.1:6379> zadd tutoriallist 0 redis 
(integer) 1 
redis 127.0.0.1:6379> zadd tutoriallist 0 mongodb 
(integer) 1 
redis 127.0.0.1:6379> zadd tutoriallist 0 rabitmq 
(integer) 1 
redis 127.0.0.1:6379> zadd tutoriallist 0 rabitmq 
(integer) 0 
redis 127.0.0.1:6379> ZRANGEBYSCORE tutoriallist 0 1000  

1) "redis" 
2) "mongodb" 
3) "rabitmq" 
```



## Basic Command

這邊只列出一些常用的基礎指令，詳細可以看[官網](https://redis.io/commands)，或cheat sheet



### `SET`/`GET`

寫法

```powershell
set [key] [value]
get [key]
```



key-value的讀寫

```powershell
127.0.0.1:6379> set hello world!
OK
127.0.0.1:6379> get hello
"world!"
```

備註:我試著重新開啟client依然可以讀取到`hello`的值，重新開啟server後就讀不到了

繼續嘗試

```powershell
127.0.0.1:6379> set int_value 100
OK
127.0.0.1:6379> get int_value
"100"
```

結合上方的結果看來set進去的value會被預設作為string看待



### `DECR`/`INCR`

寫法

```powershell
incr [key]
decr [key]
```



減少和增加

拿上面的`int_value`繼續使用



```powershell
127.0.0.1:6379> set int_value 100
OK
127.0.0.1:6379> get int_value
"100"
127.0.0.1:6379> decr int_value
(integer) 99
127.0.0.1:6379> incr int_value
(integer) 100
```

相當於`int_value--` / `int_value++`



### `DECRBY`/`INCRBY`

寫法

```powershell
incrby [key] [increment]
decrby [key] [decrement]
```

增減指定數值(整數)



```powershell
127.0.0.1:6379> incrby int_value 100
(integer) 200
127.0.0.1:6379> decrby int_value 50.1
(error) ERR value is not an integer or out of range
127.0.0.1:6379> decrby int_value 50
(integer) 150
```

中間試了一下給他非整數能不能運作，結果是不行



### `HSET`/`HGET`

寫法

```powershell
hset [key] [field] [value]
hget [key] [field]
```

hash map 的讀寫



```powershell
127.0.0.1:6379> hset sophie item1 magic_grass
(integer) 1
127.0.0.1:6379> hset sophie item2 gobalt_grass
(integer) 1
127.0.0.1:6379> hget sophie item1
"magic_grass"
127.0.0.1:6379> hget sophie item2
"gobalt_grass"
```





```powershell
redis> HSET mydata name "nick"
redis> HSET mydata nickname "nicknick"
redis> HGET mydata name
"nick"
```

可以存取一個 value 底下的 field，讓你可以更多元的使用，例如說你可以定義 key 的規則是：POST + 文章 id，裡面就可以存這篇文章的讚數、回覆數等等，就不用每一次都去 Database 裡面重新抓取。

關於[Hashes](#Hashes)

