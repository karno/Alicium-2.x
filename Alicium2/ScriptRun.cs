using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using RcLibCs;
using System.Diagnostics;


namespace Alicium2
{
    public static class Script
    {
        static void Run(string name,string code)
        {
            string ScriptName = name;
            Arg a = new Arg();
            {
                a.ArgName = "Columns";
                a.SetArgValue<string>("test");
            }
            Worker.CreateScript(ScriptName, code, a);
            try
            {
                Console.WriteLine("Run " + ScriptName + " now...");
                Process.Start(AppDomain.CurrentDomain.BaseDirectory + "Scripts\\" + ScriptName + ".exe");
            }
            catch
            {
                Console.WriteLine("Compile failed.");
            }
        }
    }
    public class Arg
    {
        public string ArgName
        {
            get;
            set;
        }
        internal string XML = "";
        internal string type = "";
        public bool SetArgValue<T>(T arg)
        {
            try
            {
                XML = Rc.SaveToXMLString<T>(arg);
                type = arg.GetType().Name;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
    public class Worker
    {
        static string add = @"
        static T LoadFromXMLString<T>(string XML)
        {
            using (System.IO.StringReader fs = new System.IO.StringReader(XML))
            {
                System.Xml.Serialization.XmlSerializer f = new System.Xml.Serialization.XmlSerializer(typeof(T));
                T obj = (T)f.Deserialize(fs);
                return obj;
            }
        }
        static string loadData(string name)
        {
            using (System.IO.StreamReader streamReader = new System.IO.StreamReader(name, System.Text
.Encoding.Unicode))
            {
                string result = streamReader.ReadToEnd();
                return result;
            }
        }";
        public static void CreateScript(string Name,string Code,params Arg[] Args)
        {
            var FinalCode = Code + @"
namespace Script
{
    public static class Args
    {
";
            foreach (Arg a in Args)
            {
                Rc.saveData(a.XML, "ScriptData/" + a.ArgName);
                FinalCode += @"
        public static " + a.type + " " + a.ArgName + @"
        {
        get
            {
            return LoadFromXMLString<" + a.type + ">(loadData(" + Rc.loadData("ScriptData/DoubleQuortation") + "ScriptData/" + a.ArgName + Rc.loadData("ScriptData/DoubleQuortation") + @"));
            }
        }
";
            }
            FinalCode += add + @"
    }
}";
            Console.WriteLine(@"Compile code : 
" + FinalCode);
            Rc.saveData(FinalCode, "Scripts/" + Name + ".cs");
            Compile("Scripts/" + Name);
            
        }
        static bool Compile(string path)
        {
            Process q = new Process();
            q.StartInfo.FileName = "C:/WINDOWS/Microsoft.NET/Framework/v4.0.30319/csc.exe";
            q.StartInfo.Arguments = "/out:" + AppDomain.CurrentDomain.BaseDirectory + path.Replace("/", "\\") + ".exe" + " /target:exe /r:" + AppDomain.CurrentDomain.BaseDirectory + "Twitterizer2.dll /r:" + AppDomain.CurrentDomain.BaseDirectory + "Newtonsoft.Json.dll " + AppDomain.CurrentDomain.BaseDirectory + path.Replace("/", "\\") + ".cs";
            q.StartInfo.RedirectStandardOutput = true;
            q.StartInfo.CreateNoWindow = true;
            q.StartInfo.UseShellExecute = false;
            bool b = q.Start();
            if (b)
            {
                Console.WriteLine(q.StandardOutput.ReadToEnd());
            }
            else
            {
                Console.WriteLine("CSC couldn't started.");
            }
            return true;
        }
    }
}
