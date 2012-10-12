using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Twitterizer;
using Twitterizer.Streaming;
using RcLibCs;

namespace Alicium2
{
	public partial class Main : Form
	{
		int version = 1000;
		public ExtendedOAuthTokens[] NowTokens
		{
			get
			{
				return Accounts.Values.ToList().Where((t) => AccountsList.SelectedItems.Contains(t.UserName)).ToArray();
			}
		}
		TwitterDo twitterDo;
		public static List<Column> Columns = new List<Column>();
		public static Column ActiveColumn;
		public static TwitterStatus ActiveStatus;
		public Dictionary<string, ExtendedOAuthTokens> Accounts = new Dictionary<string, ExtendedOAuthTokens>();
		int CommandCount = 0;
		string Command = "";
		bool CommandHandled = false, IsCommandMode = false , Tweeted = false;
		public Main()
		{
			InitializeComponent();
			Try(new Action(() =>
			               {
			               	if (version < CheckLatestVersion())
			               	{
			               		MessageBox.Show("There is an update.");
			               	}
			               	Accounts = AccountReader.Read("Settings/Accounts.dat");
			               	if (Accounts.Count != 0)
			               	{
			               		AccountsList.Items.AddRange(Accounts.Keys.ToArray());
			               	}
			               	else
			               	{
			               		MessageBox.Show("Accounts not found.Let's authenticate your first account.");
			               		AccountManager m = new AccountManager(Accounts);
			               		m.ShowDialog();
			               		if (m.Change)
			               		{
			               			Accounts.Clear();
			               			AccountsList.Items.Clear();
			               			Accounts = AccountReader.Read("Settings/Accounts.dat");
			               			if (Accounts.Count != 0)
			               			{
			               				AccountsList.Items.AddRange(Accounts.Keys.ToArray());
			               				AccountsList.SelectedIndex = 0;
			               			}
			               		}
			               	}
			               	var f = ColumnReader.Load(this);
			               	for (int i = 0; i < f.Length; i++)
			               	{
			               		f[i].TopLevel = false;
			               		f[i].TopMost = false;
			               		f[i].Fresh = false;
			               		f[i].Parent = this;
			               		f[i].Show();
			               		f[i].Size = new Size(240, this.Size.Height - 160);
			               		f[i].Text = f[i].Text.Remove(0,3);
			               		f[i].Text = i + ": " + f[i].Text;
			               		f[i].Location = new Point(240 * i, f[i].Fresh ? 0 : 27);
			               		f[i].Size = new Size(240, this.Size.Height - 160);
			               		Columns.Add(f[i]);
			               	}
			               	twitterDo = new TwitterDo(this);
			               }));
		}
		~Main()
		{
		}
		public static void Try(Action todo)
		{
			try
			{
				todo();
			}
			catch(Exception e)
			{
				MessageBox.Show(e.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error,MessageBoxDefaultButton.Button1,MessageBoxOptions.DefaultDesktopOnly,false);
				System.Environment.Exit(1);
			}
		}
		public int CheckLatestVersion()
		{
			WebClient webClient = new WebClient();
			Stream stream = webClient.OpenRead("http://cannotdebug.blog.fc2.com/blog-entry-9.html");
			Encoding encoding = Encoding.GetEncoding("UTF-8");
			StreamReader streamReader = new StreamReader(stream, encoding);
			string text = streamReader.ReadToEnd();
			streamReader.Close();
			stream.Close();
			int num = text.IndexOf("最新バージョンは[", 0);
			string text2 = text.Substring(num + 9, 7);
			return int.Parse(text2.Replace(".", ""));
		}
		private void Main_Load(object sender, EventArgs e)
		{
			this.TopLevel = true;
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void addToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var c = new ColumnEditer(Accounts);
			c.ShowDialog();
			if (c.ResultColumn != null)
			{
				var r = c.ResultColumn;
				
				r.MdiParent = this;
				r.Show();
				r.Size = new Size(240, this.Size.Height - 160);
				if (Columns.Count > 0)
				{
					r.Text = (int.Parse(Columns[Columns.Count - 1].Text[0].ToString()) + 1) + ": " + r.Text;
				}
				else
				{
					r.Text = 0 + ": " + r.Text;
				}
				Columns.Add(r);
			}
		}
		public static bool Selected = false;
		private void timer1_Tick(object sender, EventArgs e)
		{
			for (int i = 0; i < Columns.Count; i++)
			{
				if (!Column.Columns.Contains((Form)Columns[i]))
				{
					Columns.Remove(Columns[i]);
				}
				else
				{
					Columns[i].Location = new Point((Columns == null || Columns[0] == null ? 360 : Columns[0].Size.Width) * i, Columns[i].Fresh ? 0 : 27);
					Columns[i].Size = new Size(Columns == null || Columns[0] == null ? 360 : Columns[0].Size.Width, this.Size.Height - 160);
					if (ActiveColumn == null || ActiveColumn != Columns[i]) Columns[i].Active = false;
				}
			}
			if(!Selected)
			{
				ShowActiveStatus();
				Selected = true;
			}
			if (MConsole.nowtext != null && MConsole.nowtext != "")
			{
				Status.Text = MConsole.now;
			}
		}

		private void PostText_TextChanged(object sender, EventArgs e)
		{
			int Count = (140 - PostText.Text.Length);
			PostCount.Text = Count.ToString();
			if (Count < 0)
			{
				PostButton.Enabled = false;
			}
			else
			{
				PostButton.Enabled = true;
			}
		}

		private void Main_FormClosed(object sender, FormClosedEventArgs e)
		{
			bool b = ColumnReader.Save(Columns.ToArray());
			bool c = AccountReader.Save(Accounts, "Settings/Accounts.dat");
			if(!b||!c)
			{
				MessageBox.Show("Failed to save your data.");
			}
		}
		private void exitWithoutSavingColumnsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Environment.Exit(0);
		}
		private void PostText_KeyDown(object sender, KeyEventArgs e)
		{
			if (!IsCommandMode)
			{
				if (e.Control && e.KeyCode == Keys.Enter)
				{
					if (AccountsList.SelectedItems.Count != 0)
					{
						if (ActiveStatus == null)
						{
							twitterDo.Post(PostText.Text, NowTokens);
							PostText.Text = "";
						}
						else
						{
							twitterDo.Post(PostText.Text, ActiveStatus, NowTokens);
							PostText.Text = "";
						}
						Tweeted = true;
					}
					else
					{
						MConsole.WriteLine("You haven't selected any accounts.");
					}
				}
				else if (e.Control && e.KeyCode == Keys.F)
				{
					if (ActiveStatus != null)
					{
						twitterDo.Favorite(ActiveStatus, NowTokens);
					}
				}
				else if (e.Control && e.KeyCode == Keys.R)
				{
					if (ActiveStatus != null)
					{
						twitterDo.Retweet(ActiveStatus, NowTokens);
					}
				}
				else if (e.Control && e.KeyCode == Keys.C)
				{
					IsCommandMode = true;
					PostText.Text = @"Command Mode
";
					MConsole.WriteLine(ActiveColumn == null ? "c[Number] ... Activate the [Number]th column" : " [Number] ... Activate the [Number]th Tweet from Column that is activated");
					PostText.ReadOnly = true;
				}
				else if (e.KeyCode == Keys.Delete)
				{
					ActiveStatus = null;
					Command = "";
					PostText.Text = "";
					MConsole.WriteLine("Ready");
				}
			}
			else
			{
				if (e.Control && e.KeyCode == Keys.C)
				{
					IsCommandMode = false;
					PostText.ReadOnly = false;
					CommandCount = 0;
					Command = "";
					PostText.Text = "";
				}
			}
		}
		private void PostText_KeyPress(object sender, KeyPressEventArgs e)
		{
			int o;
			if (IsCommandMode && int.TryParse(e.KeyChar.ToString(),out o))
			{
				if (CommandCount == 0)
				{
					try
					{
						ActivateStatus(o);
						ShowActiveStatus();
						IsCommandMode = false;
						PostText.ReadOnly = false;
						CommandCount = 0;
						Command = "";
						PostText.Text = "";
						PostText.Text = "@" + ActiveStatus.User.ScreenName + " ";
						MConsole.WriteLine("Ctrl+F ... Favorite this Tweet. Ctrl+R ... Retweet this Tweet.");
						CommandHandled = true;
					}
					catch
					{
						IsCommandMode = false;
						PostText.ReadOnly = false;
						CommandCount = 0;
						MConsole.WriteLine("Ready");
						Command = "";
						PostText.Text = "";
					}
				}
				else
				{
					Command += e.KeyChar;
					MConsole.WriteLine("Command[" + Command + "]");
					CommandCount++;
				}
			}
			else if (IsCommandMode && char.IsLetter(e.KeyChar))
			{
				Command += e.KeyChar;
				MConsole.WriteLine("Command[" + Command + "]");
				CommandCount++;
			}
			if (CommandCount >= 2)
			{
				if ((Command[0] == 'c' || Command[0] == 'C') && int.TryParse(Command[1].ToString(), out o))
				{
					try
					{
						ActivateColumn(int.Parse(Command[1].ToString()));
						IsCommandMode = false;
						PostText.ReadOnly = false;
						CommandCount = 0;
						MConsole.WriteLine("Ready");
						Command = "";
						PostText.Text = "";
						CommandHandled = true;
					}
					catch
					{
						IsCommandMode = false;
						PostText.ReadOnly = false;
						CommandCount = 0;
						MConsole.WriteLine("Ready");
						Command = "";
						PostText.Text = "";
					}
				}
				else
				{
					IsCommandMode = false;
					PostText.ReadOnly = false;
					CommandCount = 0;
					MConsole.WriteLine("Ready");
					Command = "";
					PostText.Text = "";
				}
			}
		}
		public static void ActivateStatus(int index)
		{
			ActiveStatus = ActiveColumn[index];
		}
		public void ShowActiveStatus()
		{
			if (ActiveStatus != null)
			{
				PostText.Text = "@" + ActiveStatus.User.ScreenName + " ";
				PostText.Select(PostText.Text.Length - 1,0);
				var th = new System.Threading.Thread(new System.Threading.ThreadStart(() =>
				                                                                      {
				                                                                      	Invoke(new Action(() =>
				                                                                      	                  {
				                                                                      	                  	ActiveStatusView.SmallImageList = new ImageList();
				                                                                      	                  }));
				                                                                      	string uri = ActiveStatus.User.ProfileImageLocation;
				                                                                      	using (WebClient wc = new WebClient())
				                                                                      	{
				                                                                      		using (Stream stream = wc.OpenRead(uri))
				                                                                      		{
				                                                                      			Bitmap bitmap = new Bitmap(stream);
				                                                                      			Invoke(new Action(() => ActiveStatusView.SmallImageList.Images.Add(ActiveStatus.User.ScreenName, bitmap)));
				                                                                      		}
				                                                                      	}
				                                                                      	var i = new ListViewItem(new[] { ActiveStatus.User.ScreenName, ActiveStatus.Text }, ActiveStatus.User.ScreenName);
				                                                                      	Invoke(new Action(() =>
				                                                                      	                  {
				                                                                      	                  	ActiveStatusView.Items.Clear();
				                                                                      	                  	ActiveStatusView.Items.Add(i);
				                                                                      	                  }));
				                                                                      }));
				th.Start();
			}
			else
			{
				var th = new System.Threading.Thread(new System.Threading.ThreadStart(() =>
				                                                                      {
				                                                                      	Invoke(new Action(() =>
				                                                                      	                  {
				                                                                      	                  	ActiveStatusView.SmallImageList = new ImageList();
				                                                                      	                  }));
				                                                                      	var i = new ListViewItem(new[] { "null", "null" });
				                                                                      	Invoke(new Action(() =>
				                                                                      	                  {
				                                                                      	                  	ActiveStatusView.Items.Clear();
				                                                                      	                  	ActiveStatusView.Items.Add(i);
				                                                                      	                  }));
				                                                                      }));
				th.Start();
			}
		}
		public static void ActivateColumn(int index)
		{
			ActiveColumn = Columns[index];
			Columns[index].Active = true;
		}
		private void Status_Click(object sender, EventArgs e)
		{
			MConsole.Show();
		}

		private void AccountsList_KeyDown(object sender, KeyEventArgs e)
		{
			
		}

		private void PostText_KeyUp(object sender, KeyEventArgs e)
		{
			if (CommandHandled || Tweeted)
			{
				PostText.SelectionStart--;
				PostText.Text = PostText.Text.Remove(PostText.SelectionStart, 1);
				PostText.Text = PostText.Text.Replace("\n","").Replace("\r","");
				PostText.SelectionStart = PostText.Text.Length;
				PostText.Update();
				CommandHandled = Tweeted = false;
			}
		}

		private void accountsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AccountManager m = new AccountManager(Accounts);
			m.ShowDialog();
			if (m.Change)
			{
				Accounts.Clear();
				AccountsList.Items.Clear();
				Accounts = AccountReader.Read("Settings/Accounts.dat");
				if (Accounts.Count != 0)
				{
					AccountsList.Items.AddRange(Accounts.Keys.ToArray());
				}
			}
		}

		
		void AboutToolStripMenuItemClick(object sender, EventArgs e)
		{
			new About().ShowDialog();
		}

		private void RTButton_Click(object sender, EventArgs e)
		{
			if(ActiveStatus !=null)
			{
				twitterDo.Retweet(ActiveStatus,NowTokens);
			}
		}

		private void FavButton_Click(object sender, EventArgs e)
		{
			if(ActiveStatus !=null)
			{
				twitterDo.Favorite(ActiveStatus,NowTokens);
			}
		}
		
		void Panel1Paint(object sender, PaintEventArgs e)
		{
			
		}
	}
}
