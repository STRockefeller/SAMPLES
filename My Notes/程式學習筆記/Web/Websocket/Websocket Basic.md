# Websocket 基礎

Reference:

[midium article](https://medium.com/enjoy-life-enjoy-coding/javascript-websocket-%E8%AE%93%E5%89%8D%E5%BE%8C%E7%AB%AF%E6%B2%92%E6%9C%89%E8%B7%9D%E9%9B%A2-34536c333e1b)

## Review with Questions





## Abstract

 

> `WebSocket` 是網路協定的一種， Client 可以透過此協定與 Server 做溝通，而他和一般 `http` 或 `https` 不同的是， `WebSocket` 協定只需透過一次連結便能保持連線，不必再透過一直發送 `Request` 來與 Server 互動。



## How to use

### Install `express` and `websocket`

```pow
npm install express
npm install ws
```



### Server



#### import package

```js
const express = require('express')
const SocketServer = require('ws').Server
```



#### build server

```js
//指定開啟的 port
const PORT = 3000

//創建 express 的物件，並綁定及監聽 3000 port ，且設定開啟後在 console 中提示
const server = express()
    .listen(PORT, () => console.log(`Listening on ${PORT}`))

//將 express 交給 SocketServer 開啟 WebSocket 的服務
const wss = new SocketServer({ server })
```



#### onConnect

##### open and close

```js
wss.on('connection', ws => {

    console.log('Client connected')

    ws.on('close', () => {
        console.log('Close connected')
    })
})
```



##### receive message

```js
    ws.on('message', data => {
        ws.send(data)
    })
```

這個範例會把收到的訊息直接回傳給client



##### send message

上面的範例順便演示過了就是使用`send()`方法



也可以使用timer循環發送訊息

```js
setInterval(()=>{
        ws.send(String(new Date()))
    },1000)
```





### Client



#### build a websocket object and connect to server



```js
let ws = new WebSocket('ws://localhost:3000')
```



#### onConnect

#### open and close

```js
ws.onopen = () => {
    console.log('open connection')
}

ws.onclose = () => {
    console.log('close connection')
}
```



##### receive message

```js
ws.onmessage = event => {
    console.log(event)
}
```





## 補充

Websocket Server 是可以同時與多個client 達成連線的。

### 測試方法

直接以node 指令執行 server，例如

```powershell
PS C:\Users\admin\Desktop\websocket test> node .\wsServer.js
Listening on 3000
Client connected
Client connected
Close connected
Client connected
```

