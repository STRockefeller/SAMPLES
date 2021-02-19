 

using Newtonsoft.Json;

#region jsonSerialization
        private void jsonSerialize()
        {
            Serializable serializable = new Serializable();
            string json = JsonConvert.SerializeObject(serializable);
            tbxLog.Text += "\r\nJson serializing...";
            tbxLog.Text += $"\r\n{json}";
        }

        private void deJsonSerialize()
        {
            string json = @"{'intNum':'789','strDemo':'Rockefeller','lngNum':'128956'}";
            Serializable serializable = JsonConvert.DeserializeObject<Serializable>(json);

            tbxLog.Text += "\r\nJson Deserializing..";
            tbxLog.Text += $"\r\nintNum:{serializable.intNum.ToString()}\r\nstrDemo:{serializable.strDemo}\r\nlngNum:{serializable.lngNum.ToString()}";
        }

        #endregion