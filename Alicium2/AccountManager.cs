using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Twitterizer;

namespace Alicium2
{
    public partial class AccountManager : Form
    {
        public Dictionary<string, ExtendedOAuthTokens> Accounts = new Dictionary<string, ExtendedOAuthTokens>();
        public AccountManager(Dictionary<string,ExtendedOAuthTokens> oauths)
        {
            InitializeComponent();
            Accounts = oauths;
            AccountsList.Items.AddRange(Accounts.Values.Select(new Func<ExtendedOAuthTokens,string>((x) => {return x.UserName;})).ToArray());
        }

        private void AccountManager_Load(object sender, EventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            Accounts.Remove((string)AccountsList.SelectedItem);
            AccountsList.Items.Clear();
            AccountsList.Items.AddRange(Accounts.Values.Select(new Func<ExtendedOAuthTokens, string>((x) => { return x.UserName; })).ToArray());
        }
        public bool Change = false;
        private void button1_Click(object sender, EventArgs e)
        {
            Change = true;
            AccountReader.Save(Accounts, "Settings/Accounts.dat");
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            OAuth o = new OAuth(false);
            o.ShowDialog();
            if (!o.Canceled)
            {
                try
                {
                    Accounts.Add(o.result.UserName, o.result);
                    AccountsList.Items.Clear();
                    AccountsList.Items.AddRange(Accounts.Values.Select(new Func<ExtendedOAuthTokens, string>((x) => { return x.UserName; })).ToArray());
                }
                catch
                {
                    MessageBox.Show("This name is already used.Please input different name.");
                }
            }
        }
    }
    public static class Data
    {
        public static Dictionary<string, ExtendedOAuthTokens> Accounts
        {
            get;
            set;
        }
    }
}
