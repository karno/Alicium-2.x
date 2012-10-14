using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Twitterizer;
using System.IO;
using RcLibCs;

namespace Alicium2
{
	public class TwitterDo
	{
		private readonly Main m;
		public TwitterDo(Main _m)
		{
			m = _m;
		}
		public void Post(string Tweet,ExtendedOAuthTokens[] NowTokens)
		{
            new Task(() =>
            {
                foreach (ExtendedOAuthTokens o in m.NowTokens)
                {
                    var t = TwitterStatus.Update(o.OAuthTokens, Tweet);
                    if (t.Result != RequestResult.Success)
                    {
                        if (t.Result == RequestResult.RateLimited)
                        {
                            MConsole.WriteLine(o.UserName + " has been RateLimited. ResetDate must be " + t.RateLimiting.ResetDate.ToString() + ".");
                        }
                        else
                        {
                            MConsole.WriteLine("Some error happened : " + t.ErrorMessage);
                        }
                    }
                    else MConsole.WriteLine("Success to tweet with " + o.UserName + ": " + Tweet);
                }
            }).Start();
		}
        public void Post(string Tweet, TwitterStatus ReplyTo, ExtendedOAuthTokens[] NowToken)
		{
            new Task(() =>
            {
                foreach (ExtendedOAuthTokens o in m.NowTokens)
                {
                    var t = TwitterStatus.Update(o.OAuthTokens, Tweet, new StatusUpdateOptions() { InReplyToStatusId = ReplyTo.Id });
                    if (t.Result != RequestResult.Success)
                    {
                        if (t.Result == RequestResult.RateLimited)
                        {
                            MConsole.WriteLine(o.UserName + " has been RateLimited. ResetDate must be " + t.RateLimiting.ResetDate.ToString() + ".");
                        }
                        else
                        {
                            MConsole.WriteLine("Some error happened : " + t.ErrorMessage);
                        }
                    }
                    else MConsole.WriteLine("Success to tweet as " + o.UserName + " " + Tweet);
                }
            }).Start();
		}
        public void Favorite(TwitterStatus t, ExtendedOAuthTokens[] NowToken)
		{
			new Task(() =>
	        {
                foreach (ExtendedOAuthTokens o in m.NowTokens)
                {
                    var f = TwitterFavorite.Create(o.OAuthTokens, t.Id);

                    if (f.Result != RequestResult.Success)
                    {
                        if (f.Result == RequestResult.RateLimited)
                        {
                            MConsole.WriteLine(o.UserName + " has been RateLimited. ResetDate must be " + f.RateLimiting.ResetDate.ToString() + ".");
                        }
                        else
                        {
                            MConsole.WriteLine("Some error happened : " + f.ErrorMessage);
                        }
                    }
                    else MConsole.WriteLine("Success to favorite [" + t.User.ScreenName + ": " + t.Text + "] as " + o.UserName);
                };
	        }).Start();
		}
        public void Retweet(TwitterStatus t, ExtendedOAuthTokens[] NowToken)
        {
			new Task(() =>
            {
                foreach (ExtendedOAuthTokens o in m.NowTokens)
                {
                    var f = TwitterStatus.Retweet(o.OAuthTokens, t.Id);
                    if (f.Result != RequestResult.Success)
                    {
                        if (f.Result == RequestResult.RateLimited)
                        {
                            MConsole.WriteLine(o.UserName + " has been RateLimited. ResetDate must be " + f.RateLimiting.ResetDate.ToString() + ".");
                        }
                        else
                        {
                            MConsole.WriteLine("Some error happened : " + f.ErrorMessage);
                        }
                    }
                    else MConsole.WriteLine("Success to retweet [" + t.User.ScreenName + ": " + t.Text + "] as " + o.UserName);
                }
			}).Start();
		}
	}
	public class ExtendedOAuthTokens
	{
		public ExtendedOAuthTokens()
		{
			Tokens.Add(this);
		}
		OAuthTokens o;

		public static string Consumer_key = "RMQYKqZrL8JgpwH4h0Ypw";
		public static string Consumer_secret = "2yM7fktBX6SdeGdvedJ9Vj6Sti1tcRCC4sUSQXCxS4";
		public string AccessToken = "", AccessTokenSecret = "";
		public string UserName = "";
		public static List<ExtendedOAuthTokens> Tokens = new List<ExtendedOAuthTokens>();
		public ExtendedOAuthTokens Create(string _AccessToken, string _AccessSecret , string _UserName)
		{
			o = new OAuthTokens();
			o.AccessToken = AccessToken = _AccessToken;
			o.AccessTokenSecret = AccessTokenSecret = _AccessSecret;
			o.ConsumerKey = Consumer_key;
			o.ConsumerSecret = Consumer_secret;
			UserName = _UserName;
			return this;
		}
		public OAuthTokens OAuthTokens
		{
			get
			{
				return o;
			}
		}
	}

	public static class AccountReader
	{

        public static Dictionary<string, ExtendedOAuthTokens> Read(string path)
        {
            if (File.Exists(path))
            {
                var d = new Dictionary<string, ExtendedOAuthTokens>();
                var read = Rc.CutString(";", Rc.loadData(path));
                foreach (var s in read)
                {
                    var add = new ExtendedOAuthTokens().Create(Rc.CutString(",", s)[1], Rc.CutString(",", s)[2],
                                                               Rc.CutString(",", s)[0]);
                    d[Rc.CutString(",", s)[0]] = add;
                }
                return d;
            }
            else
            {
                return new Dictionary<string, ExtendedOAuthTokens>();
            }
        }

	    public static bool Save(Dictionary<string, ExtendedOAuthTokens> d, string path)
		{
			try
			{
				string save = "";
				foreach (ExtendedOAuthTokens o in d.Values)
				{
					save += o.UserName + "," + o.AccessToken + "," + o.AccessTokenSecret + ";";
				}
				Rc.saveData(save, path);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
	public static class ColumnReader
	{
		public static Column[] Load(Main main)
		{
			int load = 0;
			var l = new List<Column>();
			while (File.Exists("Settings/Columns_" + load + ".dat"))
			{
				var data = (ColumnData)Rc.LoadFromXML<ColumnData>("Settings/Columns_" + load + ".dat");
				var add = data.ToColumn(main);
				if(add != null)
				{
					l.Add(add);
				}
				load++;
			}
			return l.ToArray();
		}
		public static bool Save(Column[] f)
		{
			//try
			{
				int load = 0;
				while (File.Exists("Settings/Columns_" + load + ".dat"))
				{
					File.Delete("Settings/Columns_" + load + ".dat");
					load++;
				}
				for (int i = 0; i < f.Length; i++)
				{
					Rc.SaveToXML<ColumnData>(f[i].ToColumnData(), "Settings/Columns_" + i + ".dat");
				}
                MessageBox.Show("Success to save.");
				return true;
			}
			/*catch
            {
                MessageBox.Show("Failed to save.");
                return false;
            }*/
		}
	}
	public class ColumnData
	{
		public string Track;
		public string Follow;
		public string AccountName;
		public string Tille;
        public string Image;
		public Column.StartType ColumnType;
		public ColumnData Set(string _AccountName, string _Title, string _Track, string _Follow, Column.StartType Start)
		{
			return new ColumnData() { AccountName = _AccountName, Tille = _Title, Track = _Track, Follow = _Follow, ColumnType = Start,Image = "null"};
		}
		public ColumnData Set(string _AccountName, string _Title, string _Track, string _Follow, Column.StartType Start,string b)
		{
			return new ColumnData() { AccountName = _AccountName, Tille = _Title, Track = _Track, Follow = _Follow, ColumnType = Start,Image = b};
		}
		public Column ToColumn(Main m)
		{
			var v = new Twitterizer.Streaming.StreamOptions();
			v.Track.AddRange(Rc.CutString(",", Track));
			v.Follow.AddRange(Rc.CutString(",", Follow));
			try
			{
                if (Image == "null")
                    return new Column(new Twitterizer.Streaming.TwitterStream(m.Accounts[AccountName].OAuthTokens, "Alicium", v), ColumnType, Tille);
                else
                {
                    using (var f = File.Open(Image, FileMode.Open, FileAccess.Read))
                    {
                        var b = Bitmap.FromStream(f);
                        return new Column(new Twitterizer.Streaming.TwitterStream(m.Accounts[AccountName].OAuthTokens, "Alicium", v), ColumnType, Tille, (Image)b.Clone());
                    }
                }
			}
			catch
			{
				MessageBox.Show("Can't find "+AccountName+" from accounts data.Please authenticate again.","Not Found",MessageBoxButtons.OK,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button1,MessageBoxOptions.DefaultDesktopOnly,false);
				return null;
			}
		}
	}
}