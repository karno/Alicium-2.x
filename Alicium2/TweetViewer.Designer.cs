/*
 * Created by SharpDevelop.
 * User: Canno
 * Date: 2012/11/16
 * Time: 0:21
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Alicium2
{
	partial class TweetViewer
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.UserIcon = new System.Windows.Forms.PictureBox();
			this.UserName = new System.Windows.Forms.Label();
			this.ScreenName = new System.Windows.Forms.Label();
			this.TweetText = new System.Windows.Forms.TextBox();
			this.DateTime = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.UserIcon)).BeginInit();
			this.SuspendLayout();
			// 
			// UserIcon
			// 
			this.UserIcon.Location = new System.Drawing.Point(3, 2);
			this.UserIcon.Name = "UserIcon";
			this.UserIcon.Size = new System.Drawing.Size(42, 44);
			this.UserIcon.TabIndex = 0;
			this.UserIcon.TabStop = false;
			// 
			// UserName
			// 
			this.UserName.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.UserName.Location = new System.Drawing.Point(51, 9);
			this.UserName.Name = "UserName";
			this.UserName.Size = new System.Drawing.Size(238, 21);
			this.UserName.TabIndex = 1;
			this.UserName.Text = "ほげ氏";
			// 
			// ScreenName
			// 
			this.ScreenName.Location = new System.Drawing.Point(51, 30);
			this.ScreenName.Name = "ScreenName";
			this.ScreenName.Size = new System.Drawing.Size(100, 23);
			this.ScreenName.TabIndex = 2;
			this.ScreenName.Text = "label2";
			// 
			// TweetText
			// 
			this.TweetText.Location = new System.Drawing.Point(3, 52);
			this.TweetText.Multiline = true;
			this.TweetText.Name = "TweetText";
			this.TweetText.ReadOnly = true;
			this.TweetText.Size = new System.Drawing.Size(286, 84);
			this.TweetText.TabIndex = 3;
			// 
			// DateTime
			// 
			this.DateTime.Location = new System.Drawing.Point(3, 139);
			this.DateTime.Name = "DateTime";
			this.DateTime.Size = new System.Drawing.Size(100, 18);
			this.DateTime.TabIndex = 6;
			this.DateTime.Text = "label1";
			// 
			// TweetViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 158);
			this.Controls.Add(this.DateTime);
			this.Controls.Add(this.TweetText);
			this.Controls.Add(this.ScreenName);
			this.Controls.Add(this.UserName);
			this.Controls.Add(this.UserIcon);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TweetViewer";
			this.Text = "TweetViewer";
			this.Load += new System.EventHandler(this.TweetViewerLoad);
			((System.ComponentModel.ISupportInitialize)(this.UserIcon)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Label DateTime;
		private System.Windows.Forms.TextBox TweetText;
		private System.Windows.Forms.Label ScreenName;
		private System.Windows.Forms.Label UserName;
		private System.Windows.Forms.PictureBox UserIcon;
	}
}
