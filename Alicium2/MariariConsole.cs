using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Alicium2
{
    public partial class MariariConsole : Form
    {
        List<string> write;
        bool flag = false;
        public MariariConsole(string[] text)
        {
            write = new List<string>();
            InitializeComponent();
            write.AddRange(text);
        }
        private void MariariConsole_Load(object sender, EventArgs e)
        {
            textBox1.Lines = write.ToArray();
            textBox1.Select(textBox1.Text.Length, 0);
            textBox1.ScrollToCaret();
        }
        public void Add(string text)
        {
            write.Add(text);
            flag = true;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (flag)
            {
                textBox1.Lines = write.ToArray();
                textBox1.Select(textBox1.Text.Length, 0);
                textBox1.ScrollToCaret();
                flag = false;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
    public static class MConsole
    {
        public static string nowtext = "";
        public static string now = "";
        public static List<string> line = new List<string>();
        public static int index = 0;
        static MariariConsole mc = new MariariConsole(new string[0]);
        public static void WriteLine(string text)
        {
            nowtext = index + ": " + text + @"
";
            now = text;
            line.Add(text);
            mc.Add(nowtext);
            index++;
        }
        public static void Write(string text)
        {
            nowtext = index + ": " + text;
            mc.Add(nowtext);
            index++;
        }
        public static void Show()
        {
            mc.Show();
            mc.Disposed += new EventHandler(mc_Disposed);
        }

        static void mc_Disposed(object sender, EventArgs e)
        {
            mc = new MariariConsole(line.ToArray());
        }
    }
}
