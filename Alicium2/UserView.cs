using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Forms;
using System.IO;
using Twitterizer;
using System.Net;
using RcLibCs;

namespace Alicium2
{
    public partial class UserView : Form
    {
        OAuthTokens oa;
        public string scr = "hoge";
        TwitterResponse<TwitterUser> showUserResponse = new TwitterResponse<TwitterUser>();
        TwitterUser user;
        public UserView(string UserScreenName, OAuthTokens oau)
        {
            InitializeComponent();
            scr = UserScreenName;
            oa = oau;
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            try
            {
                showUserResponse = TwitterUser.Show(oa, scr);
                TwitterUser user = showUserResponse.ResponseObject;
                pictureBox1.ImageLocation = user.ProfileImageLocation;
                Text = "UserViewer - " + user.ScreenName;
                label1.Text = user.Name;
                label2.Text = "@" + user.ScreenName;
                label3.Text = user.Location;
                linkLabel1.Text = user.Website;
                if (user.IsFollowing == true)
                {
                    label4.Text = "Following";
                }
                else if (user.IsFollowing == false && user.FollowRequestSent == false)
                {
                    label4.Text = "Not Following";
                }
                else
                {
                    label4.Text = "Has Sent a Follow Request";
                }
                if (user.IsProtected == true) label5.Text = "Protected";
                button1.Text = (bool)user.IsFollowing ? "unfollow" : "follow";
                textBox1.Text = user.Description;
                
            }
            catch
            {
                MessageBox.Show("Some error happened. The user should not exists.");
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                showUserResponse = TwitterUser.Show(oa, scr);
                user = showUserResponse.ResponseObject;
                if (user.IsFollowing == true)
                {
                    TwitterResponse<TwitterUser> follow = TwitterFriendship.Delete(oa, user.ScreenName);
                    update();
                }
                else if (user.IsFollowing == false)
                {
                    TwitterResponse<TwitterUser> follow = TwitterFriendship.Create(oa, user.ScreenName);
                    update();
                }
            }
            catch
            {
                MessageBox.Show("Some error happened");
            }
        }
        public void update()
        {
            showUserResponse = TwitterUser.Show(oa, scr);
            user = showUserResponse.ResponseObject;
            if (user.IsFollowing == true)
            {
                label4.Text = "Following";
            }
            else if (user.IsFollowing == false && user.FollowRequestSent == false)
            {
                label4.Text = "Not Following";
            }
            else
            {
                label4.Text = "Has Sent a Follow Request";
            }
            button1.Text = (bool)user.IsFollowing ? "unfollow" : "follow";
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            showUserResponse = TwitterUser.Show(oa, scr);
            user = showUserResponse.ResponseObject;
            if (user.IsFollowing == true)
            {
                label4.Text = "Following";
            }
            else if (user.IsFollowing == false && user.FollowRequestSent == false)
            {
                label4.Text = "Not Following";
            }
            else
            {
                label4.Text = "Has Sent a Follow Request";
            }
            button1.Text = (bool)user.IsFollowing ? "unfollow" : "follow";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
			try{
            showUserResponse = TwitterUser.Show(oa, scr);
            user = showUserResponse.ResponseObject;
            System.Diagnostics.Process.Start(user.Website);
			}
			catch
			{
			}
        }
    }
}
