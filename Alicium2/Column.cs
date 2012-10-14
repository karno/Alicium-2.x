using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.IO;
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
        public Image back;
        public enum StartType
        {
            UserStream, FilterStream, Mentions
        }
        public static string Ids = "0123456789abdefghikmnopqrstuvwxyz";
        public Column(TwitterStream stream, StartType s, string title)
        {
            if (back != null)
            {
                listView1.BackgroundImage = back;
            }
            InitializeComponent();
            listView1.SmallImageList = new ImageList();
            ts = stream;
            if (s == StartType.UserStream)
            {
                ts.StartUserStream(null, new StreamStoppedCallback((x) => { toolStripStatusLabel1.Text = "Stopped."; }), new StatusCreatedCallback(x => { Add(x); }), null, null, null, new EventCallback(x => { Event(x); }), null);
                var tt = TwitterTimeline.HomeTimeline(stream.Tokens);
                try
                {
                    foreach (var tss in tt.ResponseObject)
                    {
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
                ts.StartPublicStream(new StreamStoppedCallback((x) => { toolStripStatusLabel1.Text = "Stopped."; }), new StatusCreatedCallback(x => { Add(x); }), null, new EventCallback(x => { Event(x); }));
                var tt = TwitterSearch.Search(ts.Tokens, query, new SearchOptions() { ResultType = SearchOptionsResultType.Popular });
                try
                {
                    foreach (var tss in tt.ResponseObject)
                    {
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
                ts.StartPublicStream(new StreamStoppedCallback((x) => { toolStripStatusLabel1.Text = "Stopped."; }), new StatusCreatedCallback(x => { Add(x); }), null, new EventCallback(x => { Event(x); }));
                var tt = TwitterTimeline.Mentions(stream.Tokens);
                try
                {
                    foreach (var tss in tt.ResponseObject)
                    {
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
            ShowF();
        }
        public Column(TwitterStream stream, StartType s, string title, Image b)
        {
            InitializeComponent();
            back = new Bitmap(b);
            listView1.BackgroundImage = back;
            listView1.SmallImageList = new ImageList();
            ts = stream;
            if (s == StartType.UserStream)
            {
                ts.StartUserStream(null, new StreamStoppedCallback((x) => { toolStripStatusLabel1.Text = "Stopped."; }), new StatusCreatedCallback(x => { Add(x); }), null, null, null, new EventCallback(x => { Event(x); }), null);
                var tt = TwitterTimeline.HomeTimeline(stream.Tokens);
                try
                {
                    foreach (var tss in tt.ResponseObject)
                    {
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
                ts.StartPublicStream(new StreamStoppedCallback((x) => { toolStripStatusLabel1.Text = "Stopped."; }), new StatusCreatedCallback(x => { Add(x); }), null, new EventCallback(x => { Event(x); }));
                var tt = TwitterSearch.Search(ts.Tokens, query, new SearchOptions() { ResultType = SearchOptionsResultType.Popular });
                try
                {
                    foreach (var tss in tt.ResponseObject)
                    {
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
                ts.StartPublicStream(new StreamStoppedCallback((x) => { toolStripStatusLabel1.Text = "Stopped."; }), new StatusCreatedCallback(x => { Add(x); }), null, new EventCallback(x => { Event(x); }));
                var tt = TwitterTimeline.Mentions(stream.Tokens);
                try
                {
                    foreach (var tss in tt.ResponseObject)
                    {
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
            ShowF();
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
                    listView1.BackColor = Color.WhiteSmoke;
                }
                else
                {
                    listView1.BackColor = Color.Azure;
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
                    var rev = timeline.ToArray().Reverse().ToList();
                    rev.Add(t);
                    rev.Reverse();
                    timeline = rev;
                }
                else
                {
                    timeline.Add(t);
                }
                ShowF();
            }
            catch { }
        }
        public void Renew()
        {
            if (st == StartType.UserStream)
            {
                timeline.Clear();
                var tt = TwitterTimeline.HomeTimeline(ts.Tokens);
                try
                {
                    foreach (var tss in tt.ResponseObject)
                    {
                        timeline.Add(tss);
                    }
                }
                catch
                {
                    MessageBox.Show(tt.ErrorMessage);
                }
            }
            else if (st == StartType.FilterStream)
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
                timeline.Clear();
                var tt = TwitterSearch.Search(ts.Tokens, query, new SearchOptions() { ResultType = SearchOptionsResultType.Popular });
                try
                {
                    foreach (var tss in tt.ResponseObject)
                    {
                        timeline.Add(new TwitterStatus() { Text = tss.Text, User = new TwitterUser() { ScreenName = tss.FromUserScreenName } });
                    }
                }
                catch
                {
                    MessageBox.Show(tt.ErrorMessage);
                }
            }
            else if (st == StartType.Mentions)
            {
                timeline.Clear();
                var tt = TwitterTimeline.Mentions(ts.Tokens);
                try
                {
                    foreach (var tss in tt.ResponseObject)
                    {
                        timeline.Add(tss);
                    }
                }
                catch
                {
                    MessageBox.Show(tt.ErrorMessage);
                }
            }
            ShowF();
        }
        void Event(TwitterStreamEvent e)
        {
            if (this.st == StartType.UserStream)
            {
                switch (e.EventType.ToString())
                {
                    case "Favorite":
                        if (ExtendedOAuthTokens.Tokens.Any((x) => { return x.UserName == e.Target.ScreenName; }))
                        {
                            MConsole.WriteLine(e.Source.ScreenName + " Favorited your tweet.");
                        }
                        break;
                    case "Retweet":
                        if (ExtendedOAuthTokens.Tokens.Any((x) => { return x.UserName == e.Target.ScreenName; }))
                        {
                            MConsole.WriteLine(e.Source.ScreenName + " Retweeted your tweet.");
                        }
                        break;
                    case "Follow":
                        if (ExtendedOAuthTokens.Tokens.Any((x) => { return x.UserName == e.Target.ScreenName; }))
                        {
                            MConsole.WriteLine(e.Source.ScreenName + " Followed you.");
                        }
                        break;
                }
            }
        }
        public List<ListViewItem> ShowData = new List<ListViewItem>();
        bool RenewedFlag = false;
        void ShowF()
        {
            var th = new Thread(new ThreadStart(() =>
                                                {
                                                    ShowData.Clear();
                                                    for (int i = 0; i < timeline.Count; i++)
                                                    {
                                                        if (listView1.SmallImageList != null && !listView1.SmallImageList.Images.ContainsKey(timeline[i].User.ScreenName))
                                                        {
                                                            string uri = timeline[i].User.ProfileImageLocation;
                                                            WebClient wc = new WebClient();
                                                            Stream stream = wc.OpenRead(uri);
                                                            Bitmap bitmap = new Bitmap(stream);
                                                            stream.Close();
                                                            try
                                                            {
                                                                Invoke(new Action(() =>
                                                                                  listView1.SmallImageList.Images.Add(timeline[i].User.ScreenName, bitmap)));
                                                            }
                                                            catch { }
                                                        }
                                                        ShowData.Add(new ListViewItem(new string[] { timeline[i].User.ScreenName, timeline[i].Text }, timeline[i].User.ScreenName));
                                                    }
                                                    RenewedFlag = true;
                                                }));
            th.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (RenewedFlag)
            {
                listView1.Items.Clear();
                listView1.Items.AddRange(ShowData.ToArray());
                RenewedFlag = false;
            }
        }
        public ColumnData ToColumnData()
        {
            var _Track = String.Join(",", ts.StreamOptions.Track);
            var _Follow = String.Join(",", ts.StreamOptions.Follow);
            if (back == null)
            {
                return new ColumnData() { AccountName = TwitterAccount.VerifyCredentials(ts.Tokens).ResponseObject.ScreenName, Tille = this.Text, Track = _Track, Follow = _Follow, ColumnType = st, Image = "null" };
            }
            else
            {
                var save = "Settings/" + this.Text[0] + ".bmp";
                try
                {
                    using (var str = new FileStream(save, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        back.Save(str, ImageFormat.Png);
                    }
                }
                catch { } // 握りつぶす
                return new ColumnData() { AccountName = TwitterAccount.VerifyCredentials(ts.Tokens).ResponseObject.ScreenName, Tille = this.Text, Track = _Track, Follow = _Follow, ColumnType = st, Image = "Settings/" + this.Text[0] + ".bmp" };
            }
        }

        private void Column_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (back != null)
            {
                File.Delete("Settings/" + this.Text.ToCharArray()[0] + ".bmp");
                back.Dispose();
            }
            Columns.Remove(this);
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            Renew();
        }

        private void toolStripDropDownButton2_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Restarting...";
            ts.EndStream();
            timeline.Clear();
            if (st == StartType.UserStream)
            {
                ts.StartUserStream(null, new StreamStoppedCallback((x) => { toolStripStatusLabel1.Text = "Stopped."; }), new StatusCreatedCallback(x => { Add(x); }), null, null, null, new EventCallback(x => { Event(x); }), null);
                var tt = TwitterTimeline.HomeTimeline(ts.Tokens);
                try
                {
                    foreach (var tss in tt.ResponseObject)
                    {
                        timeline.Add(tss);
                    }
                    toolStripStatusLabel1.Text = "Restarted.";
                }
                catch
                {
                    MessageBox.Show(tt.ErrorMessage);
                }
            }
            else if (st == StartType.FilterStream)
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
                ts.StartPublicStream(new StreamStoppedCallback((x) => { toolStripStatusLabel1.Text = "Stopped."; }), new StatusCreatedCallback(x => { Add(x); }), null, new EventCallback(x => { Event(x); }));
                var tt = TwitterSearch.Search(ts.Tokens, query, new SearchOptions() { ResultType = SearchOptionsResultType.Popular });
                try
                {
                    foreach (var tss in tt.ResponseObject)
                    {
                        timeline.Add(new TwitterStatus() { Text = tss.Text, User = new TwitterUser() { ScreenName = tss.FromUserScreenName } });
                    }
                    toolStripStatusLabel1.Text = "Restarted.";
                }
                catch
                {
                    MessageBox.Show(tt.ErrorMessage);
                }
            }
            else if (st == StartType.Mentions)
            {
                ts.StreamOptions.Track.Add("@" + ExtendedOAuthTokens.Tokens.First<ExtendedOAuthTokens>((x) => { return x.OAuthTokens == ts.Tokens; }).UserName);
                ts.StartPublicStream(new StreamStoppedCallback((x) => { toolStripStatusLabel1.Text = "Stopped."; }), new StatusCreatedCallback(x => { Add(x); }), null, new EventCallback(x => { Event(x); }));
                var tt = TwitterTimeline.Mentions(ts.Tokens);
                try
                {
                    foreach (var tss in tt.ResponseObject)
                    {
                        timeline.Add(tss);
                    }
                    toolStripStatusLabel1.Text = "Restarted.";
                }
                catch
                {
                    MessageBox.Show(tt.ErrorMessage);
                }
            }
            ShowF();
        }


        void StatusStrip1ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        void ToolStripDropDownButton3Click(object sender, EventArgs e)
        {
            Main.ActivateColumn(Main.Columns.IndexOf(this));
        }

        void ListView1SelectedIndexChanged(object sender, EventArgs e)
        {
            Main.ActivateColumn(Main.Columns.IndexOf(this));
            try
            {
                Main.Selected = false;
                Main.ActivateStatus(timeline.IndexOf(timeline.Where((x) => { return x == timeline[listView1.SelectedIndices[0]]; }).ToArray()[0]));
            }
            catch
            {
            }
        }
    }
}