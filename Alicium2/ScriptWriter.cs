using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RcLibCs;
using Twitterizer;
using System.IO;

namespace Alicium2
{
    public partial class ScriptWriter : Form
    {
        Main m;
        string a = null;
        string EditingPath
        {
            get
            {
                return a;
            }
            set
            {
                Text = "ScriptWriter - " + value;
                a = value;
            }
        }
        public ScriptWriter(Main _m)
        {
            InitializeComponent();
            m = _m;
            openFileDialog1.InitialDirectory = saveFileDialog1.InitialDirectory = Application.StartupPath + "Scripts";
        }

        private void openOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox1.Text = Rc.loadData(openFileDialog1.FileName);
                EditingPath = openFileDialog1.FileName;
            }
        }

        private void saveSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Rc.saveData(this.textBox1.Text,saveFileDialog1.FileName);
                EditingPath = saveFileDialog1.FileName;
            }
        }

        private void exitEToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        
        private void debugDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TwitterStatus[][] t = new TwitterStatus[Main.Columns.Count][];
            for (int i = 0; i < Main.Columns.Count; i++)
            {
                t[i] = Main.Columns[i].timeline.ToArray();
            }
            OAuthTokens[] o = new OAuthTokens[ExtendedOAuthTokens.Tokens.Count];
            for (int j = 0; j < ExtendedOAuthTokens.Tokens.Count; j++)
            {
                o[j] = ExtendedOAuthTokens.Tokens[j].OAuthTokens;
            }
            if (EditingPath != null)
            {
                string copy = "Scripts/" + Rc.CutString("\\", EditingPath)[Rc.CutString("\\", EditingPath).Length - 1];
                try { File.Copy(EditingPath, copy); }
                catch { }
                Script.Run(Rc.CutString(".", Rc.CutString("\\", EditingPath)[Rc.CutString("\\", EditingPath).Length - 1])[0], this.textBox1.Text ,new Action<object>(Get),new Arg().SetArgValue(t, "Tweets") , new Arg().SetArgValue(o, "Accounts"));
            }
        }
        private void Get(object o)
        {
            MessageBox.Show(o.ToString());
        }
        private void referencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"(Please refer to references of Twitterizer 2. http://www.twitterizer.net/)

References:
bool Script.Args.Return<T>(T arg) ... Return an argument to Alicium 2.x.
TwitterStatus[][] Script.Args.Tweets ... Get tweets in your columns.
                                         ([Column Number][Tweet Number])
OAuthTokens[] Script.Args.Accounts ... Get your accounts.
");
        }
        private void optionOTuoolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ScriptWriter_Load(object sender, EventArgs e)
        {

        }
    }
}
