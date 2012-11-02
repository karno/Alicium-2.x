using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Twitterizer;
using Twitterizer.Streaming;

namespace Alicium2
{
	public partial class ColumnEditer : Form
	{
		StreamOptions opt;
		Dictionary<string, ExtendedOAuthTokens> o;
		public ColumnEditer(Dictionary<string, ExtendedOAuthTokens> oaus)
		{
			o = oaus;
			InitializeComponent();
			opt = new StreamOptions();
		}
		int type = 0;
		private void ColumnEditer_Load(object sender, EventArgs e)
		{
			foreach(string s in o.Keys)
			{
				AccountList.Items.Add(s);
				AccountList.SelectedIndex = 0;
			}
		}
		public bool IsUserStream = true;

		private void button2_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void radioButton1_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButton1.Checked)
			{
				groupBox1.Enabled = false;
				type = 0;
			}
		}

		private void radioButton2_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButton2.Checked)
			{
				groupBox1.Enabled = false;
				type = 1;
			}
		}

		private void radioButton3_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButton3.Checked)
			{
				groupBox1.Enabled = true;
				type = 2;
			}
		}
		public Column ResultColumn;
		private void button1_Click(object sender, EventArgs e)
		{
			if (textBox1.Text != "" && AccountList.SelectedItem != null)
			{
                if (type == 2 && textBox3.Text != "")
				{
					opt.Follow.AddRange(textBox2.Lines);
					opt.Track.AddRange(textBox3.Lines);
                    Column c;
                    if (b != null)
                    {
                        c = new Column(new TwitterStream(o[AccountList.SelectedItem.ToString()].OAuthTokens, "Alicium2", opt), Column.StartType.FilterStream, textBox1.Text, b);
                    }
                    else
                    {
                        c = new Column(new TwitterStream(o[AccountList.SelectedItem.ToString()].OAuthTokens, "Alicium2", opt), Column.StartType.FilterStream, textBox1.Text);
                    }
					ResultColumn = c;
				}
				else if (type == 0)
				{
                    Column c;
                    if (b != null)
                    {
                        c = new Column(new TwitterStream(o[AccountList.SelectedItem.ToString()].OAuthTokens, "Alicium2", opt), Column.StartType.UserStream, textBox1.Text, b);
                    }
                    else
                    {
                        c = new Column(new TwitterStream(o[AccountList.SelectedItem.ToString()].OAuthTokens, "Alicium2", opt), Column.StartType.UserStream, textBox1.Text);
                    }
					ResultColumn = c;
				}
				else if (type == 1)
				{
                    Column c;
                    if (b != null)
                    {
                        c = new Column(new TwitterStream(o[AccountList.SelectedItem.ToString()].OAuthTokens, "Alicium2", opt), Column.StartType.Mentions, textBox1.Text, b);
                    }
                    else
                    {
                        c = new Column(new TwitterStream(o[AccountList.SelectedItem.ToString()].OAuthTokens, "Alicium2", opt), Column.StartType.Mentions, textBox1.Text);
                    }
					ResultColumn = c;
				}
				this.Close();
			}
			else
			{
				MessageBox.Show("Empty.");
			}
		}
		Image b;
		void ImageOpenFileOk(object sender, CancelEventArgs e)
		{
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			if(ImageOpen.ShowDialog() == DialogResult.OK)
			{
				b = Image.FromFile(ImageOpen.FileName);
				listView1.BackgroundImage = b;
			}
		}

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.BackgroundImage = null;
            b = null;
        }
	}
}
