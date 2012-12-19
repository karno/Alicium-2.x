using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Net;
using Twitterizer;
using RcLibCs;

namespace Alicium2
{
	public partial class OAuth : Form
	{
		string ConsumerKey = "RMQYKqZrL8JgpwH4h0Ypw";
		string ConsumerSecret = "2yM7fktBX6SdeGdvedJ9Vj6Sti1tcRCC4sUSQXCxS4";
		public bool Canceled = true;
		bool first = true;
		OAuthTokenResponse oatr;
		public OAuth(bool b)
		{
			InitializeComponent();
			first = b;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			string uris = "Couldn't connect to Server.Please retry again.";
			try
			{
				oatr = OAuthUtility.GetRequestToken(ConsumerKey, ConsumerSecret, "oob");
				Uri uri = Twitterizer.OAuthUtility.BuildAuthorizationUri(oatr.Token);
				uris = uri.ToString();
				webBrowser1.Navigate(uris);
			}
			catch
			{
				Interaction.InputBox("Couldn't open WebBrouser. Please copy this url and paste it to your brouser's URL box.", "Error", uris);
			}
		}
		public ExtendedOAuthTokens result;
		private void button2_Click(object sender, EventArgs e)
		{
			string pin = textBox_PIN.Text;
			OAuthTokenResponse res = OAuthUtility.GetAccessToken(
				ConsumerKey, ConsumerSecret, oatr.Token, pin);
			string AccessToken = res.Token;
			string AccessTokenSecret = res.TokenSecret;
			textBox_Output.Text += "Accesss Token: " + AccessToken + " Have gotten\r\n";
			textBox_Output.Text += "Accesss Token Secret: " + AccessTokenSecret + " Have gotten\r\n";
			result = new ExtendedOAuthTokens().Create(AccessToken, AccessTokenSecret, "");
			bool error=false;
			int trycount=0;
			do{
				try
				{
					var a = TwitterAccount.VerifyCredentials(result.OAuthTokens).ResponseObject.ScreenName;
					result.UserName = a;
					error = false;
				}
				catch{error = true;}
				finally
				{
					trycount++;
				}
			}
			while(error && trycount < 5);
			if(error)
			{
				MessageBox.Show("Failed to get your account data.please try later.");
				this.Close();
			}
			button3.Enabled = true;
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}
		public string UrlEncode(string value)
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

		private void button3_Click(object sender, EventArgs e)
		{
			Canceled = false;
			this.Close();
		}

		private void button4_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void button5_Click(object sender, EventArgs e)
		{
			if (first) System.Environment.Exit(0);
			else this.Close();
		}

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
		}


	}
}