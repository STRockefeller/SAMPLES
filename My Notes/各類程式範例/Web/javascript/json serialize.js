//serialize

回傳字串=JSON.stringify(欲序列化的物件);

//deserialize

物件=JSON.parse(json字串);
//另有eval()方法但不推薦 (且於ts中無法執行)