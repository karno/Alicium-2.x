using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Twitterizer;
using Twitterizer.Streaming;
using System.Threading;
using System.Web;
using System.Net;

namespace Alicium2
{
	public partial class Column : Form
	{
		bool active;
		List<TwitterStatus> timeline = new List<TwitterStatus>();
		TwitterStream ts;
		StartType st;
		public static List<Column> Columns = new List<Column>();
		public bool Fresh = true;
		public enum StartType
		{
			UserStream,FilterStream,Mentions
		}
		public Column(TwitterStream stream, StartType s, string title)
		{
			InitializeComponent();
			ts = stream;
			if (s == StartType.UserStream)
			{
				ts.StartUserStream(null, new StreamStoppedCallback((x) => { MessageBox.Show("Stopped."); }), new StatusCreatedCallback(x => { Add(x); }), null, null, null, new EventCallback(x => { Event(x); }), null);
				var tt = TwitterTimeline.HomeTimeline(stream.Tokens);
				try
				{
					foreach (var tss in tt.ResponseObject)
					{
						textBox1.Text += tss.User.ScreenName + ": " + tss.Text + @"

";
						timeline.Add(tss);
					}
				}
				catch
				{
					MessageBox.Show(tt.ErrorMessage);
				}
			}
			else if (s == StartType.FilterStream)
			{
				string query = "";
                if (ts.StreamOptions.Track.Count != 0)
                {
                    foreach (string ss in ts.StreamOptions.Track)
                    {
                        query += ss + "+AND+";
                    }
                    query = query.Remove(query.Length - 5, 5);
                }
				ts.StartPublicStream(new StreamStoppedCallback((x) => { MessageBox.Show("Stopped."); }), new StatusCreatedCallback(x => { Add(x); }), null, new EventCallback(x => { Event(x); }));
				var tt = TwitterSearch.Search(ts.Tokens, query, new SearchOptions() { ResultType = SearchOptionsResultType.Popular });
				try
				{
					foreach (var tss in tt.ResponseObject)
					{
						textBox1.Text += tss.FromUserScreenName + ": " + tss.Text + @"

";
						timeline.Add(new TwitterStatus() { Text = tss.Text, User = new TwitterUser() { ScreenName = tss.FromUserScreenName } });
					}
				}
				catch
				{
					MessageBox.Show(tt.ErrorMessage);
				}
			}
			else if (s == StartType.Mentions)
			{
				ts.StreamOptions.Track.Add("@" + ExtendedOAuthTokens.Tokens.First<ExtendedOAuthTokens>((x) => { return x.OAuthTokens == ts.Tokens; }).UserName);
				ts.StartPublicStream(new StreamStoppedCallback((x) => { MessageBox.Show("Stopped."); }), new StatusCreatedCallback(x => { Add(x); }), null, new EventCallback(x => { Event(x); }));
				var tt = TwitterTimeline.Mentions(stream.Tokens);
				try
				{
					foreach (var tss in tt.ResponseObject)
					{
						textBox1.Text += tss.User.ScreenName + ": " + tss.Text + @"

";
						timeline.Add(tss);
					}
				}
				catch
				{
					MessageBox.Show(tt.ErrorMessage);
				}
			}
			st = s;
			this.Text = title;
			Columns.Add(this);
		}
		~Column()
		{
		}
		public TwitterStatus this[int _index]
		{
			get
			{
				return timeline[_index];
			}
		}
		public bool Active
		{
			get
			{
				return active;
			}
			set
			{
				active = value;
				if (!active)
				{
					textBox1.BackColor = Color.WhiteSmoke;
				}
				else
				{
					textBox1.BackColor = Color.Azure;
				}
			}
		}
		private void Column_Load(object sender, EventArgs e)
		{
			
		}
		void Add(TwitterStatus t)
		{
			try
			{
				if (timeline.Count > 0)
				{
					TwitterStatus[] array = this.timeline.ToArray();
					Array.Reverse(array);
					Queue<TwitterStatus> queue = new Queue<TwitterStatus>();
					TwitterStatus[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						TwitterStatus item = array2[i];
						queue.Enqueue(item);
					}
					queue.Dequeue();
					queue.Enqueue(t);
					TwitterStatus[] array3 = queue.ToArray();
					Array.Reverse(array3);
					this.timeline.Clear();
					array2 = array3;
					for (int i = 0; i < array2.Length; i++)
					{
						this.timeline.Add(array2[i]);
					}
				}
				else
				{
					timeline.Add(t);
				}
				ShowF();
			}
			catch{}
		}
		void Event(TwitterStreamEvent e)
		{
			if(this.st == StartType.UserStream)
			{
				switch(e.EventType.ToString())
				{
					case "Favorite":
						if(ExtendedOAuthTokens.Tokens.Any((x) => {return x.UserName == e.Target.ScreenName;}))
						{
							MConsole.WriteLine(e.Source.ScreenName + " Favorited your tweet [" + ((TwitterStatus)e.TargetObject).User.ScreenName + ": " + ((TwitterStatus)e.TargetObject).Text + "]");
						}
						break;
					case "Retweet":
						if(ExtendedOAuthTokens.Tokens.Any((x) => {return x.UserName == e.Target.ScreenName;}))
						{
							MConsole.WriteLine(e.Source.ScreenName + " Retweeted your tweet [" + ((TwitterStatus)e.TargetObject).User.ScreenName + ": " + ((TwitterStatus)e.TargetObject).Text + "]");
						}
						break;
					case "Follow":
						if(ExtendedOAuthTokens.Tokens.Any((x) => {return x.UserName == e.Target.ScreenName;}))
						{
							MConsole.WriteLine(e.Source.ScreenName + " Followed you [" + e.Target.ScreenName + "]");
						}
						break;
				}
			}
		}
		string ShowData = "";
		bool RenewedFlag = false;
		void ShowF()
		{
			ShowData = "";
			var thread = new Thread(new ThreadStart(() =>
			                                        {
			                                        	foreach (TwitterStatus t in timeline)
			                                        	{
			                                        		ShowData += t.User.ScreenName + ":" + t.Text + @"

";
			                                        	}
			                                        	RenewedFlag = true;
			                                        }));
			thread.Start();
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			if (RenewedFlag)
			{
				textBox1.Text = ShowData;
				RenewedFlag = false;
			}
		}
		public ColumnData ToColumnData()
		{
			string _Track = "",_Follow = "";
			ts.StreamOptions.Track.ForEach((s) => {_Track += s + ",";});
			ts.StreamOptions.Follow.ForEach((p) => {_Follow += p + ",";});
			return new ColumnData() { AccountName = TwitterAccount.VerifyCredentials(ts.Tokens).ResponseObject.ScreenName, Tille = this.Text, Track = _Track, Follow = _Follow, ColumnType = st };
		}

		private void Column_FormClosed(object sender, FormClosedEventArgs e)
		{
			Columns.Remove(this);
		}
	}
}
