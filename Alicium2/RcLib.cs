using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;

namespace RcLibCs
{
	internal class Rc
	{
		public static T LoadFromXML<T>(string path)
		{
			using (var fs = File.OpenRead(path))
			{
				var f = new System.Xml.Serialization.XmlSerializer(typeof(T));
				T obj = (T)f.Deserialize(fs);
				return obj;
			}
		}
		public static void SaveToXML<T>(T obj, string path)
		{
			using (var fs = File.OpenWrite(path))
			{
				var bf = new System.Xml.Serialization.XmlSerializer(typeof(T));
				bf.Serialize(fs, obj);
			}
		}
		public static string UrlEncode(string value)
		{
			string unreserved = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
			StringBuilder result = new StringBuilder();
			byte[] data = Encoding.UTF8.GetBytes(value);
			foreach (byte b in data)
			{
				if (b < 0x80 && unreserved.IndexOf((char)b) != -1)
					result.Append((char)b);
				else
					result.Append('%' + String.Format("{0:X2}", (int)b));
			}
			return result.ToString();
		}
		public static string loadData(string name)
		{
			using(var streamReader = new StreamReader(name, Encoding.GetEncoding("Shift_JIS")))
			{
				string result = streamReader.ReadToEnd();
				return result;
			}
		}
		public static void saveData(string bunsyou, string name)
		{
			using(var streamWriter = new StreamWriter(File.Create(name), Encoding.GetEncoding("Shift_JIS")))
			{
				streamWriter.Write(bunsyou);
			}
		}
		public static Dictionary<string, string> LoadIni(string path)
		{
			string str = Rc.loadData(path);
			string[] array = Rc.CutString2("\n", str);
			var dictionary = new Dictionary<string, string>();
			bool flag = false;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string str2 = array2[i];
				if (!flag)
				{
					flag = true;
				}
				else
				{
					string key = Rc.CutString2("=", str2)[0];
					string value;
					if (Rc.CutString2("=", str2).Length != 1)
					{
						value = Rc.CutString2("=", str2)[1];
					}
					else
					{
						value = "";
					}
					dictionary[key] = value;
				}
			}
			return dictionary;
		}
		public static bool ExportIni(string path, Dictionary<string, string> Dic)
		{
			try
			{
				string export = @"[Alicium]
";
				string[] keys = new string[Dic.Count];
				string[] values = new string[Dic.Count];
				Dic.Keys.CopyTo(keys, 0);
				Dic.Values.CopyTo(values, 0);
				for (int i = 0; i < Dic.Count; i++)
				{
					if (keys[i] == "" || values[i] == "")
					{
					}
					else
					{
						export += keys[i] + "=" + values[i] + @"
";
					}
				}
				Rc.saveData(export, path);
				return true;
			}
			catch
			{
				return false;
			}
		}
		public static int SearchStringLines(string[] Lines, string Word)
		{
			bool flag = false;
			int result;
			for (int i = 0; i < Lines.Length; i++)
			{
				for (int j = 0; j < Word.Length; j++)
				{
					if (Lines[i].Length >= Word.Length)
					{
						if (Lines[i][j] == Word[j])
						{
							flag = true;
						}
					}
				}
				if (flag)
				{
					result = i;
					return result;
				}
			}
			result = -1;
			return result;
		}
		public static string[] CutString(string kugiri, string str)
		{
			string[] result;
			try
			{
				str = str.Replace("\n", "").Replace("\r", "");
				string[] separator = new string[]
				{
					kugiri
				};
				string[] array = str.Split(separator, StringSplitOptions.RemoveEmptyEntries);
				result = array;
			}
			catch
			{
				result = new string[]
				{
					"Error!!!!!!!!!!!!!!!!!!!!!!!!",
					"Error!!!!!!!!!!!!!!!!!!!!!!!!",
					"Error!!!!!!!!!!!!!!!!!!!!!!!!",
					"Error!!!!!!!!!!!!!!!!!!!!!!!!",
					"Error!!!!!!!!!!!!!!!!!!!!!!!!",
					"Error!!!!!!!!!!!!!!!!!!!!!!!!",
					"Error!!!!!!!!!!!!!!!!!!!!!!!!",
					"Error!!!!!!!!!!!!!!!!!!!!!!!!",
					"Error!!!!!!!!!!!!!!!!!!!!!!!!",
					"Error!!!!!!!!!!!!!!!!!!!!!!!!"
				};
			}
			return result;
		}
		public static string[] CutString2(string kugiri, string str)
		{
			string[] result;
			try
			{
				str = str.Replace("\r", "");
				string[] separator = new string[]
				{
					kugiri
				};
				string[] array = str.Split(separator, StringSplitOptions.RemoveEmptyEntries);
				result = array;
			}
			catch
			{
				result = new string[]
				{
					"Error!!!!!!!!!!!!!!!!!!!!!!!!",
					"Error!!!!!!!!!!!!!!!!!!!!!!!!",
					"Error!!!!!!!!!!!!!!!!!!!!!!!!",
					"Error!!!!!!!!!!!!!!!!!!!!!!!!",
					"Error!!!!!!!!!!!!!!!!!!!!!!!!",
					"Error!!!!!!!!!!!!!!!!!!!!!!!!",
					"Error!!!!!!!!!!!!!!!!!!!!!!!!",
					"Error!!!!!!!!!!!!!!!!!!!!!!!!",
					"Error!!!!!!!!!!!!!!!!!!!!!!!!",
					"Error!!!!!!!!!!!!!!!!!!!!!!!!"
				};
			}
			return result;
		}
		public static float GetRadian(float Kakudo)
		{
			return Kakudo * (float)Math.PI / 180f;
		}
		public static float Getkakudo(float Radian)
		{
			return Radian * 180f / (float)Math.PI;
		}
		public static object CreateInstance(string name, params object[] args)
		{
			return Activator.CreateInstance(Type.GetType(name), args);
		}
	}
}
