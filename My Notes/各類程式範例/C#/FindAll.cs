List<string> sl1 = new List<string>();
sl1.Add("2");
sl1.Add("3");
sl1.Add("2");
sl1.Add("5");

List<string> sl1find = new List<string>();
sl1find = sl1.FindAll(delegate (string s) { return s =="2"; });

//sl1 == {"2","3","2","5"}
//sl1find == {"2","2"}