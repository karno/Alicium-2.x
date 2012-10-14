namespace Alicium2
{
    partial class Main
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
        	this.components = new System.ComponentModel.Container();
        	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
        	this.statusStrip = new System.Windows.Forms.StatusStrip();
        	this.Status = new System.Windows.Forms.ToolStripStatusLabel();
        	this.menuStrip1 = new System.Windows.Forms.MenuStrip();
        	this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.exitWithoutSavingColumnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.accountsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.columnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.timer1 = new System.Windows.Forms.Timer(this.components);
        	this.panel1 = new System.Windows.Forms.Panel();
        	this.RTButton = new System.Windows.Forms.Button();
        	this.FavButton = new System.Windows.Forms.Button();
        	this.ActiveStatusView = new System.Windows.Forms.ListView();
        	this.User = new System.Windows.Forms.ColumnHeader();
        	this.Tweet = new System.Windows.Forms.ColumnHeader();
        	this.PostCount = new System.Windows.Forms.Label();
        	this.PostButton = new System.Windows.Forms.Button();
        	this.PostText = new System.Windows.Forms.TextBox();
        	this.AccountsList = new System.Windows.Forms.ListBox();
        	this.label1 = new System.Windows.Forms.Label();
        	this.statusStrip.SuspendLayout();
        	this.menuStrip1.SuspendLayout();
        	this.panel1.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// statusStrip
        	// 
        	this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.Status});
        	this.statusStrip.Location = new System.Drawing.Point(0, 424);
        	this.statusStrip.Name = "statusStrip";
        	this.statusStrip.Size = new System.Drawing.Size(640, 22);
        	this.statusStrip.TabIndex = 2;
        	this.statusStrip.Text = "StatusStrip";
        	// 
        	// Status
        	// 
        	this.Status.Name = "Status";
        	this.Status.Size = new System.Drawing.Size(37, 17);
        	this.Status.Text = "Ready";
        	this.Status.Click += new System.EventHandler(this.Status_Click);
        	// 
        	// menuStrip1
        	// 
        	this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.menuToolStripMenuItem,
        	        	        	this.accountsToolStripMenuItem,
        	        	        	this.columnsToolStripMenuItem,
        	        	        	this.helpToolStripMenuItem});
        	this.menuStrip1.Location = new System.Drawing.Point(0, 0);
        	this.menuStrip1.Name = "menuStrip1";
        	this.menuStrip1.Size = new System.Drawing.Size(640, 24);
        	this.menuStrip1.TabIndex = 4;
        	this.menuStrip1.Text = "menuStrip1";
        	// 
        	// menuToolStripMenuItem
        	// 
        	this.menuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.exitToolStripMenuItem,
        	        	        	this.exitWithoutSavingColumnsToolStripMenuItem});
        	this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
        	this.menuToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
        	this.menuToolStripMenuItem.Text = "Menu";
        	// 
        	// exitToolStripMenuItem
        	// 
        	this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
        	this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
        	this.exitToolStripMenuItem.Size = new System.Drawing.Size(280, 22);
        	this.exitToolStripMenuItem.Text = "Exit";
        	this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
        	// 
        	// exitWithoutSavingColumnsToolStripMenuItem
        	// 
        	this.exitWithoutSavingColumnsToolStripMenuItem.Name = "exitWithoutSavingColumnsToolStripMenuItem";
        	this.exitWithoutSavingColumnsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
        	        	        	| System.Windows.Forms.Keys.F4)));
        	this.exitWithoutSavingColumnsToolStripMenuItem.Size = new System.Drawing.Size(280, 22);
        	this.exitWithoutSavingColumnsToolStripMenuItem.Text = "Exit without saving Columns";
        	this.exitWithoutSavingColumnsToolStripMenuItem.Click += new System.EventHandler(this.exitWithoutSavingColumnsToolStripMenuItem_Click);
        	// 
        	// accountsToolStripMenuItem
        	// 
        	this.accountsToolStripMenuItem.Name = "accountsToolStripMenuItem";
        	this.accountsToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
        	this.accountsToolStripMenuItem.Text = "Accounts";
        	this.accountsToolStripMenuItem.Click += new System.EventHandler(this.accountsToolStripMenuItem_Click);
        	// 
        	// columnsToolStripMenuItem
        	// 
        	this.columnsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.addToolStripMenuItem});
        	this.columnsToolStripMenuItem.Name = "columnsToolStripMenuItem";
        	this.columnsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
        	this.columnsToolStripMenuItem.Text = "Columns";
        	// 
        	// addToolStripMenuItem
        	// 
        	this.addToolStripMenuItem.Name = "addToolStripMenuItem";
        	this.addToolStripMenuItem.Size = new System.Drawing.Size(90, 22);
        	this.addToolStripMenuItem.Text = "Add";
        	this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
        	// 
        	// helpToolStripMenuItem
        	// 
        	this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.aboutToolStripMenuItem});
        	this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
        	this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
        	this.helpToolStripMenuItem.Text = "Help";
        	// 
        	// aboutToolStripMenuItem
        	// 
        	this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
        	this.aboutToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
        	this.aboutToolStripMenuItem.Text = "About";
        	this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItemClick);
        	// 
        	// timer1
        	// 
        	this.timer1.Enabled = true;
        	this.timer1.Interval = 10;
        	this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
        	// 
        	// panel1
        	// 
        	this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        	this.panel1.Controls.Add(this.RTButton);
        	this.panel1.Controls.Add(this.FavButton);
        	this.panel1.Controls.Add(this.ActiveStatusView);
        	this.panel1.Controls.Add(this.PostCount);
        	this.panel1.Controls.Add(this.PostButton);
        	this.panel1.Controls.Add(this.PostText);
        	this.panel1.Controls.Add(this.AccountsList);
        	this.panel1.Location = new System.Drawing.Point(0, 337);
        	this.panel1.Name = "panel1";
        	this.panel1.Size = new System.Drawing.Size(640, 94);
        	this.panel1.TabIndex = 5;
        	this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel1Paint);
        	// 
        	// RTButton
        	// 
        	this.RTButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("RTButton.BackgroundImage")));
        	this.RTButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        	this.RTButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonFace;
        	this.RTButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        	this.RTButton.Location = new System.Drawing.Point(607, 20);
        	this.RTButton.Name = "RTButton";
        	this.RTButton.Size = new System.Drawing.Size(28, 24);
        	this.RTButton.TabIndex = 6;
        	this.RTButton.UseVisualStyleBackColor = true;
        	this.RTButton.Click += new System.EventHandler(this.RTButton_Click);
        	// 
        	// FavButton
        	// 
        	this.FavButton.BackColor = System.Drawing.Color.Transparent;
        	this.FavButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("FavButton.BackgroundImage")));
        	this.FavButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        	this.FavButton.Cursor = System.Windows.Forms.Cursors.Default;
        	this.FavButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonFace;
        	this.FavButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        	this.FavButton.ForeColor = System.Drawing.SystemColors.ControlText;
        	this.FavButton.Location = new System.Drawing.Point(607, -1);
        	this.FavButton.Name = "FavButton";
        	this.FavButton.RightToLeft = System.Windows.Forms.RightToLeft.No;
        	this.FavButton.Size = new System.Drawing.Size(32, 25);
        	this.FavButton.TabIndex = 5;
        	this.FavButton.UseVisualStyleBackColor = false;
        	this.FavButton.Click += new System.EventHandler(this.FavButton_Click);
        	// 
        	// ActiveStatusView
        	// 
        	this.ActiveStatusView.BackgroundImageTiled = true;
        	this.ActiveStatusView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
        	        	        	this.User,
        	        	        	this.Tweet});
        	this.ActiveStatusView.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
        	this.ActiveStatusView.Location = new System.Drawing.Point(3, 3);
        	this.ActiveStatusView.Name = "ActiveStatusView";
        	this.ActiveStatusView.Scrollable = false;
        	this.ActiveStatusView.Size = new System.Drawing.Size(598, 41);
        	this.ActiveStatusView.TabIndex = 4;
        	this.ActiveStatusView.UseCompatibleStateImageBehavior = false;
        	this.ActiveStatusView.View = System.Windows.Forms.View.Details;
        	// 
        	// User
        	// 
        	this.User.Text = "User";
        	this.User.Width = 74;
        	// 
        	// Tweet
        	// 
        	this.Tweet.Text = "Tweet";
        	this.Tweet.Width = 519;
        	// 
        	// PostCount
        	// 
        	this.PostCount.AutoSize = true;
        	this.PostCount.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
        	this.PostCount.Location = new System.Drawing.Point(500, 60);
        	this.PostCount.Name = "PostCount";
        	this.PostCount.Size = new System.Drawing.Size(43, 21);
        	this.PostCount.TabIndex = 3;
        	this.PostCount.Text = "140";
        	// 
        	// PostButton
        	// 
        	this.PostButton.Location = new System.Drawing.Point(549, 50);
        	this.PostButton.Name = "PostButton";
        	this.PostButton.Size = new System.Drawing.Size(86, 38);
        	this.PostButton.TabIndex = 2;
        	this.PostButton.TabStop = false;
        	this.PostButton.Text = "Post";
        	this.PostButton.UseVisualStyleBackColor = true;
        	// 
        	// PostText
        	// 
        	this.PostText.Location = new System.Drawing.Point(174, 50);
        	this.PostText.Multiline = true;
        	this.PostText.Name = "PostText";
        	this.PostText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
        	this.PostText.Size = new System.Drawing.Size(320, 38);
        	this.PostText.TabIndex = 1;
        	this.PostText.TabStop = false;
        	this.PostText.TextChanged += new System.EventHandler(this.PostText_TextChanged);
        	this.PostText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PostText_KeyDown);
        	this.PostText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PostText_KeyPress);
        	this.PostText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PostText_KeyUp);
        	// 
        	// AccountsList
        	// 
        	this.AccountsList.FormattingEnabled = true;
        	this.AccountsList.ItemHeight = 12;
        	this.AccountsList.Location = new System.Drawing.Point(3, 48);
        	this.AccountsList.Name = "AccountsList";
        	this.AccountsList.ScrollAlwaysVisible = true;
        	this.AccountsList.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
        	this.AccountsList.Size = new System.Drawing.Size(165, 40);
        	this.AccountsList.TabIndex = 0;
        	this.AccountsList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AccountsList_KeyDown);
        	// 
        	// label1
        	// 
        	this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        	this.label1.AutoSize = true;
        	this.label1.Location = new System.Drawing.Point(552, 9);
        	this.label1.Name = "label1";
        	this.label1.Size = new System.Drawing.Size(76, 12);
        	this.label1.TabIndex = 7;
        	this.label1.Text = "version 1.0.0.0";
        	// 
        	// Main
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(640, 446);
        	this.Controls.Add(this.label1);
        	this.Controls.Add(this.panel1);
        	this.Controls.Add(this.statusStrip);
        	this.Controls.Add(this.menuStrip1);
        	this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        	this.IsMdiContainer = true;
        	this.MainMenuStrip = this.menuStrip1;
        	this.MinimumSize = new System.Drawing.Size(640, 480);
        	this.Name = "Main";
        	this.Text = "Alicium 2.0";
        	this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
        	this.Load += new System.EventHandler(this.Main_Load);
        	this.statusStrip.ResumeLayout(false);
        	this.statusStrip.PerformLayout();
        	this.menuStrip1.ResumeLayout(false);
        	this.menuStrip1.PerformLayout();
        	this.panel1.ResumeLayout(false);
        	this.panel1.PerformLayout();
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }
        private System.Windows.Forms.ColumnHeader Tweet;
        private System.Windows.Forms.ColumnHeader User;
        private System.Windows.Forms.ListView ActiveStatusView;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel Status;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem accountsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem columnsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label PostCount;
        private System.Windows.Forms.Button PostButton;
        private System.Windows.Forms.TextBox PostText;
        private System.Windows.Forms.ListBox AccountsList;
        private System.Windows.Forms.ToolStripMenuItem exitWithoutSavingColumnsToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button RTButton;
        private System.Windows.Forms.Button FavButton;
    }
}



