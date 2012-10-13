using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Alicium_Tests
{
    public partial class ImageView : Form
    {
        public ImageView(Image i)
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(i);
        }

        private void ImageView_Load(object sender, EventArgs e)
        {

        }
        public static void ShowF(Image i)
        {
            new ImageView(i).ShowDialog();
        }
    }
}
