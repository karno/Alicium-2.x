using System;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Collections.Generic;

//Alicium2.x背景画像問題用Offline test moduleだよ

namespace Alicium_Tests
{
    //XmlSerializerでごにょごにょするクラス
    public static class XMLIO
    {
        public static T Load<T>(string path)
        {
            using (var fs = File.OpenRead(path))
            {
                var f = new System.Xml.Serialization.XmlSerializer(typeof(T));
                T obj = (T)f.Deserialize(fs);
                return obj;
            }
        }
        public static void Save<T>(T obj, string path)
        {
            using (var fs = File.OpenWrite(path))
            {
                var bf = new System.Xml.Serialization.XmlSerializer(typeof(T));
                bf.Serialize(fs, obj);
            }
        }
    }

    //Alicium2.Columnもどき
    public class Body
    {
        public Image i;
        public int num;
        public Body(Image _i, int _num)
        {
            i = new Bitmap(_i);
            num = _num;
        }
        public void Show()
        {
            ImageView.ShowF(i);
        }
        public Data ToData()
        {
            var s = "Settings/" + num + ".bmp";
            using (var str = new FileStream(s, FileMode.OpenOrCreate, FileAccess.Write))
            {
                i.Save(str, System.Drawing.Imaging.ImageFormat.Png);
            }
            return new Data().Set(s, num); ;
        }
        ~Body()
        {
            i.Dispose();
        }
    }

    //Alicium2.ColumnDataもどき
    public class Data
    {
        public Data Set(string _path, int _num)
        {
            path = _path;
            num = _num;
            return this;
        }
        public int num;
        public string path;
        public Body ToBody()
        {
            using (var st = File.OpenRead(path))
            {
                var i = Bitmap.FromStream(st);
                return new Body(i, num);
            }
        }
    }
    static class Run
    {
        static string Open()
        {
            var o = new System.Windows.Forms.OpenFileDialog();
            while (o.ShowDialog() != System.Windows.Forms.DialogResult.OK) { }
            return o.FileName;
        }
        [STAThread]
        static void Main()
        {
            while (true)
            {

                var l = new List<Body>();
                var m = new List<Data>();

                //Body表示,Image保存,Body2Data,Serializeテスト

                for (int i = 0; i < 3; i++)
                {
                    l.Add(new Data().Set(Open(),i).ToBody());
                    l[i].Show();
                }
                Directory.SetCurrentDirectory(System.Windows.Forms.Application.StartupPath);
                foreach (Body b in l)
                {
                    m.Add(b.ToData());
                }

                for (int k = 0; k < m.Count; k++)
                {
                    XMLIO.Save(m[k], "Settings/" + k + ".xml");
                }

                l.Clear();
                m.Clear();
                GC.Collect();

                //Deserialize,Image読込,Data2Body,Body表示テスト

                int count = 0;
                while (File.Exists("Settings/" + count + ".xml"))
                {
                    m.Add(XMLIO.Load<Data>("Settings/" + count + ".xml"));
                    count++;
                }

                foreach (Data d in m)
                {
                    l.Add(d.ToBody());
                    d.ToBody().Show();
                }

                l.Clear();
                m.Clear();
                GC.Collect();
            }
            //whileループでData,画像上書きテスト
        }

    }

}