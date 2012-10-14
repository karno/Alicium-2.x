using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Alicium2
{
    public partial class hoge : Form
    {
        public hoge(Image i)
        {
            InitializeComponent();
            this.pictureBox1.Image = i;
        }

        private void hoge_Load(object sender, EventArgs e)
        {

        }
        public static void ShowF(Image i)
        {
            new hoge(i).ShowDialog();
        }
    }
}
