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
using System.Threading.Tasks;


namespace Alicium2
{
    public static class Script
    {
        public static void Run(string name,string code,params Arg[] args)
        {
            string ScriptName = name;
            Worker.CreateScript(ScriptName, code, args);
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
        public static void Run(string name, string code, Action<object> Return,params Arg[] args)
        {
            string ScriptName = name;
            bool issuccess = Worker.CreateScript(ScriptName, code, args);
            try
            {
                if (issuccess)
                {
                    var p = Process.Start(AppDomain.CurrentDomain.BaseDirectory + "Scripts\\" + ScriptName + ".exe");
                    new Task(() =>
                    {
                        while (!p.HasExited) ;
                        Return(File.Exists("Scripts/" + ScriptName) ? Rc.LoadFromXML<object>("Scripts/" + ScriptName) : null);
                        p.Dispose();
                    }).Start();
                }
            }
            catch(Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Error: " + e.Message);
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
        public Arg SetArgValue<T>(T arg,string Name)
        {
            try
            {
                XML = Rc.SaveToXMLString<T>(arg);
                type = arg.GetType().Name;
                ArgName = Name;
                return this;
            }
            catch
            {
                return null;
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
        public static void SaveToXML<T>(T obj, string path)
        {
            using (var fs = System.IO.File.OpenWrite(path))
            {
                var bf = new System.Xml.Serialization.XmlSerializer(typeof(T));
                bf.Serialize(fs, obj);
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
        public static bool CreateScript(string Name,string Code,params Arg[] Args)
        {
            var FinalCode = Code + @"
namespace Script
{
    public static class Args
    {
        public static void Return<T>(T Arg)
        {
            SaveToXML(((object)Arg)," + Rc.loadData("ScriptData/DoubleQuortation") + "Scripts/" + Name + Rc.loadData("ScriptData/DoubleQuortation") + @");
        }
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
            if (Rc.loadData("Scripts/" + Name + ".cs") != FinalCode)
            {
                Rc.saveData(FinalCode, "Scripts/" + Name + ".cs");
                return Compile("Scripts/" + Name);
            }
            else return true;
            
        }
        static bool Compile(string path)
        {
            using (var q = new Process())
            {
                q.StartInfo.FileName = "C:/WINDOWS/Microsoft.NET/Framework/v4.0.30319/csc.exe";
                q.StartInfo.Arguments = "/out:" + AppDomain.CurrentDomain.BaseDirectory + path.Replace("/", "\\") + ".exe" + " /target:exe /r:" + AppDomain.CurrentDomain.BaseDirectory + "Twitterizer2.dll /r:" + AppDomain.CurrentDomain.BaseDirectory + "Newtonsoft.Json.dll " + AppDomain.CurrentDomain.BaseDirectory + path.Replace("/", "\\") + ".cs";
                q.StartInfo.RedirectStandardOutput = true;
                q.StartInfo.CreateNoWindow = true;
                q.StartInfo.UseShellExecute = false;
                bool b = q.Start();
                if (b)
                {
                    while (!q.HasExited) ;
                    string s = q.StandardOutput.ReadToEnd();
                    if (s != @"Microsoft (R) Visual C# 2010 Compiler version 4.0.30319.1
Copyright (C) Microsoft Corporation. All rights reserved.

")
                    {
                        System.Windows.Forms.MessageBox.Show(s);
                        return false;
                    }
                    return true;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("CSC couldn't started.");
                    return false;
                }
            }
        }
    }
}
