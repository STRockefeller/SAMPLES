# Redis Cheat Sheet



Copy From https://simplecheatsheet.com/tag/redis-cheat-sheet/



The **Redis cheat sheet** includes basic syntax and methods to help you using Redis. [**Redis**](https://redis.io/topics/introduction) is an open-source (BSD licensed), in-memory data structure store, used as a database, cache, and message broker. It supports data structures such as strings, hashes, lists, sets, sorted sets with range queries, bitmaps, hyperloglogs, geospatial indexes with radius queries and streams.

## [Redis: Commands Cheat Sheet](https://simplecheatsheet.com/redis-commands/)

**The basic syntax of Redis client.**

```
$redis-cli 
```

**Run Commands on the Remote Server**

You need to connect to the server by the same client **redis-cli**

```
$ redis-cli -h host -p port -a password
```

## [Redis: Keys Cheat Sheet](https://simplecheatsheet.com/redis-keys/)

**Del Command**

```
redis > DEL KEY_NAME
```

**Dump Command**

```
redis > DUMP KEY_NAME
```

**Exists Command**

```
redis > EXISTS KEY_NAME
```

**Expire Command**

```
redis > Expire KEY_NAME TIME_IN_SECONDS
```

**Expireat Command**

```
redis > Expireat KEY_NAME TIME_IN_UNIX_TIMESTAMP
```

**Pexpire Command**

```
redis > PEXPIRE KEY_NAME TIME_IN_MILLISECONDS
```

**Pexpireat Command**

```
redis > PEXPIREAT KEY_NAME TIME_IN_MILLISECONDS_IN_UNIX_TIMESTAMP
```

**Keys Command**

```
redis > KEYS PATTERN
```

**Move Command**

```
redis > MOVE KEY_NAME DESTINATION_DATABASE
```

**Persist Command**

```
redis > PERSIST KEY_NAME
```

**PTTL Command**

```
redis > SET tutorialname redis OK
```

**TTL Command**

```
redis > TTL KEY_NAME 
```

**Random key Command**

```
redis > RANDOMKEY 
```

**Rename Command**

```
redis > RENAME OLD_KEY_NAME NEW_KEY_NAME 
```

**Renamenx Command**

```
redis > RENAMENX OLD_KEY_NAME NEW_KEY_NAME
```

**Type Command**

```
redis > TYPE KEY_NAME 
```

## [Redis: Strings Cheat Sheet](https://simplecheatsheet.com/redis-strings/)

**Set Command**

```
redis > SET KEY_NAME VALUE
```

**Get Command**

```
redis > GET KEY_NAME
```

**Getrange Command**

```
redis > GETRANGE KEY_NAME start end
```

**Getset Command**

```
redis > GETSET KEY_NAME VALUE
```

**Getbit Command**

```
redis > GETBIT KEY_NAME OFFSET
```

**Mget Command**

```
redis > MGET KEY1 KEY2 .. KEYN
```

**Setbit Command**

```
redis > SETBIT KEY_NAME OFFSET
```

**Setex Command**

```
redis > SETEX KEY_NAME TIMEOUT VALUE
```

**Setnx Command**

```
redis > SETNX KEY_NAME VALUE
```

**Setrange Command**

```
redis > SETRANGE KEY_NAME OFFSET VALUE
```

**Strlen Command**

```
redis > STRLEN KEY_NAME 
```

**Mset Command**

```
redis > MSET key1 value1 key2 value2 .. keyN valueN
```

**Msetnx Command**

```
redis > MSETNX key1 value1 key2 value2 .. keyN valueN 
```

**Psetex Command**

```
redis > PSETEX key1 EXPIRY_IN_MILLISECONDS value1
```

**Incr Command**

```
redis > INCR KEY_NAME
```

**Incrby Command**

```
redis > INCRBY KEY_NAME INCR_AMOUNT
```

**Incrbyfloat Command**

```
redis > INCRBYFLOAT KEY_NAME INCR_AMOUNT 
```

**Decr Command**

```
redis > DECR KEY_NAME
```

**Decrby Command**

```
redis > DECRBY KEY_NAME DECREMENT_AMOUNT
```

**Append Command**

```
redis > APPEND KEY_NAME NEW_VALUE
```

## [Redis: Hashes Cheat Sheet](https://simplecheatsheet.com/redis-hashes/)

**Hdel Command**

```
redis > HDEL KEY_NAME FIELD1.. FIELDN
```

**Hexists Command**

```
redis > HEXISTS KEY_NAME FIELD_NAME
```

**Hget Command**

```
redis > HGET KEY_NAME FIELD_NAME 
```

**Hgetall Command**

```
redis > HGETALL KEY_NAME 
```

**Hincrby Command**

```
redis > HINCRBY KEY_NAME FIELD_NAME INCR_BY_NUMBER
```

**Hincrbyfloat Command**

```
redis > HINCRBYFLOAT KEY_NAME FIELD_NAME INCR_BY_NUMBER 
```

**Hkeys Command**

```
redis > HKEYS KEY_NAME FIELD_NAME INCR_BY_NUMBER 
```

**Hlen Command**

```
redis > HLEN KEY_NAME
```

**Hmget Command**

```
redis > HMGET KEY_NAME FIELD1...FIELDN 
```

**Hset Command**

```
redis > HSET KEY_NAME FIELD VALUE
```

**Hsetnx Command**

```
redis > HSETNX KEY_NAME FIELD VALUE
```

**Hvals Command**

```
redis > HVALS KEY_NAME FIELD VALUE
```



## [Redis: Lists Cheat Sheet](https://simplecheatsheet.com/redis-lists/)

**Blpop Command**

```
redis > BLPOP LIST1 ... LISTN TIMEOUT
```

**Brpop Command**

```
redis > BRPOP LIST1 ... LISTN TIMEOUT
```

**Brpoplpush Command**

```
redis > BRPOPLPUSH LIST1 ANOTHER_LIST TIMEOUT 
```

**Lindex Command**

```
redis > LINDEX KEY_NAME INDEX_POSITION
```

**Linsert Command**

```
redis > LINSERT KEY_NAME BEFORE EXISTING_VALUE NEW_VALUE
```

**Llen Command**

```
redis > LLEN KEY_NAME
```

**Lpop Command**

```
redis > LPOP KEY_NAME
```

**Lpush Command**

```
redis > LPUSH KEY_NAME VALUE1.. VALUEN
```

**Lpushx Command**

```
redis > LPUSHX KEY_NAME VALUE1.. VALUEN
```

**Lrange Command**

```
redis > LRANGE KEY_NAME START END
```

**Lrem Command**

```
redis > LREM KEY_NAME COUNT VALUE 
```

**Lset Command**

```
redis > LSET KEY_NAME INDEX VALUE
```

**Ltrim Command**

```
redis > LTRIM KEY_NAME START STOP
```

**Rpop Command**

```
redis > RPOP KEY_NAME
```

**Rpoplpush Command**

```
redis > RPOPLPUSH SOURCE_KEY_NAME DESTINATION_KEY_NAME
```

**Rpush Command**

```
redis > RPUSH KEY_NAME VALUE1..VALUEN 
```

**Rpushx Command**

```
redis > RPUSHX KEY_NAME VALUE1..VALUEN
```

## [Redis: Sets Cheat Sheet](https://simplecheatsheet.com/redis-sets/)

**Sadd Command**

```
redis > SADD KEY_NAME VALUE1..VALUEN
```

**Scard Command**

```
redis > SCARD KEY_NAME
```

**Sdiff Command**

```
redis > SDIFF FIRST_KEY OTHER_KEY1..OTHER_KEYN
```

**Sdiffstore Command**

```
redis > SDIFFSTORE DESTINATION_KEY KEY1..KEYN 
```

**Sinter Command**

```
redis > SINTER KEY KEY1..KEYN
```

**Sinterstore Command**

```
redis > SINTERSTORE DESTINATION_KEY KEY KEY1..KEYN
```

**Sismember Command**

```
redis > SISMEMBER KEY VALUE
```

**Smove Command**

```
redis > SMOVE SOURCE DESTINATION MEMBER
```

**Spop Command**

```
redis > SPOP KEY
```

**Srandmember Command**

```
redis > SRANDMEMBER KEY [count] 
```

**Srem Command**

```
redis > SREM KEY MEMBER1..MEMBERN
```

**Sunion Command**

```
redis > SUNION KEY KEY1..KEYN
```

**Sunionstore Command**

```
redis > SUNIONSTORE DESTINATION KEY KEY1..KEYN
```

**Sscan Command**

```
redis > SSCAN KEY [MATCH pattern] [COUNT count]
```

## [Redis: HyperLogLog Cheat Sheet](https://simplecheatsheet.com/redis-hyperloglog/)

#### Pfadd Command

```
redis > PFADD KEY_NAME ELEMENTS_TO_BE_ADDED
```

#### Pfcount Command

```
redis > PFCOUNT KEY_NAME KEY1 ... KEYN 
```

#### Pfmerge Command

```
redis > PFMERGE KEY_NAME KEY1 ... KEYN
```

## [Redis: Publish Subscribe Cheat Sheet](https://simplecheatsheet.com/redis-publish-subscribe/)

#### Psubscribe Command

```
redis > PSUBSCRIBE CHANNEL_NAME_OR_PATTERN [PATTERN...]
```

#### Pubsub Command

```
redis > PUBSUB subcommand [argument [argument ...]]
```

#### Publish Command

```
redis > PUBLISH channel message
```

#### Punsubscribe Command

```
redis > PUNSUBSCRIBE [pattern [pattern ...]] 
```

#### PubSub Subscribe Command

```
redis > SUBSCRIBE channel [channel ...] 
```

#### Unsubscribe Command

```
redis > UNSUBSCRIBE channel [channel ...]
```

## [Redis: Transactions Cheat Sheet](https://simplecheatsheet.com/redis-transactions/)

#### Discard Command

```
redis > DISCARD 
```

#### Exec Command

```
redis > EXEC
```

#### Multi Command

```
redis > MULTI
```

#### Unwatch Command

```
redis > UNWATCH
```

#### Watch Command

```
redis > WATCH key [key ...]
```

## [Redis: Scripting Cheat Sheet](https://simplecheatsheet.com/redis-scripting/)

#### **Eval Command**

```
redis > EVAL script numkeys key [key ...] arg [arg ...]
```

#### **Evalsha Command**

```
redis > EVALSHA sha1 numkeys key [key ...] arg [arg ...]
```

#### Scripting Script Exists Command

```
redis > SCRIPT EXISTS script [script ...]
```

#### Scripting Script Flush Command

```
redis > SCRIPT FLUSH 
```

#### Scripting Script Kill Command

```
redis > SCRIPT KILL
```

#### Scripting Script Load Command

```
redis > SCRIPT LOAD script
```

## [Redis: Connections Cheat Sheet](https://simplecheatsheet.com/redis-connections/)

#### Auth Command

```
redis > AUTH PASSWORD
```

#### Echo Command

```
redis > ECHO "SAMPLE_STRING"
```

#### Ping Command

```
redis > PING
```

#### Quit Command

```
redis > QUIT
```

#### Select Command

```
redis > SELECT DB_INDEX
```