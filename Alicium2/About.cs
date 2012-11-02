using System;
using System.Drawing;
using System.Windows.Forms;

namespace Alicium2
{
	public partial class About : Form
	{
		public About()
		{
			InitializeComponent();
			
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			this.Close();
        }
	}
}
