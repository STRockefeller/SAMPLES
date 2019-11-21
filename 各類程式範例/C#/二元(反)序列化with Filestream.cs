using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.IO;

namespace _20191120serialize
{
   ¡@[Serializable]
    public class Serializable
    {
        public int intNum = 123;
        public string strDemo = "LaDiDa";
        public long lngNum = 123456;

        public Serializable()
        {
            MessageBox.Show("¤w«Øºc");
        }
        /*
        [Serializable]
        private class ClsSerializable
        {
            public int intNum = 123;
            public string strDemo = "LaDiDa";
            public long lngNum = 123456;
        }
        */
    }
    public partial class frmBase:Form
    {
        private void serializeBinary(Serializable o)
        {
            FileStream file = new FileStream(@"binarySerialize.txt", FileMode.Create);
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            tbxLog.Text += "\r\nBinary serialization start.";
            binaryFormatter.Serialize(file,o);
            tbxLog.Text += "\r\nBinary serialization completion.";
            file.Close();
        }
        private void deserializeBinary()
        {
            Serializable o;
            //StreamReader streamReader = new StreamReader(@"binarySerialize.txt");
            FileStream fileStream = new FileStream(@"binarySerialize.txt",FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            tbxLog.Text += "\r\nBinary deserialize start.";
            //o = binaryFormatter.Deserialize(streamReader);
            o = (Serializable)binaryFormatter.Deserialize(fileStream);
            //streamReader.Close();
			fileStream.Close();
            tbxLog.Text += "\r\nBinary deserialization completion.";
            tbxLog.Text += "\r\nThe attributes of the object will show below.";
            tbxLog.Text += $"\r\n{o.intNum.ToString()}\r\n{o.strDemo}\r\n{o.lngNum.ToString()}";
        }
    }
}
