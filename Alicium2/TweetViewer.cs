using System;
using System.Drawing;
using System.Windows.Forms;
using Twitterizer;

namespace Alicium2
{

	public partial class TweetViewer : Form
	{
		TwitterStatus ViewTweet;
		public TweetViewer(TwitterStatus v)
		{

			InitializeComponent();
			ViewTweet = v;
		}
		
		void TweetViewerLoad(object sender, EventArgs e)
		{
			this.UserIcon.BackgroundImage = TwitterDo.GetImageFromUri(ViewTweet.User.ProfileImageLocation);
			this.UserName.Text = ViewTweet.User.Name;
			this.ScreenName.Text = "@" + ViewTweet.User.ScreenName;
			this.TweetText.Text = ViewTweet.Text;
			this.DateTime.Text = ViewTweet.CreatedDate.ToLocalTime().ToLongTimeString();
		}
		
		void FromClick(object sender, EventArgs e)
		{
			
		}
	}
}
