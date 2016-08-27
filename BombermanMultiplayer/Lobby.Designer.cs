namespace BombermanMultiplayer
{
    partial class Lobby
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                if (cts != null)
                {
                    cts.Dispose();

                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Lobby));
            this.lbConnected = new System.Windows.Forms.ListBox();
            this.btnLaunchServer = new System.Windows.Forms.Button();
            this.btnClient = new System.Windows.Forms.Button();
            this.tslMenu = new System.Windows.Forms.ToolStrip();
            this.tsDropDownFile = new System.Windows.Forms.ToolStripDropDownButton();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lbLstPlayers = new System.Windows.Forms.Label();
            this.PanelConnections = new System.Windows.Forms.Panel();
            this.lbNamePlayer2 = new System.Windows.Forms.Label();
            this.panelServer = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tbGameToLoad = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.lbPortServer = new System.Windows.Forms.Label();
            this.tbPortServer = new System.Windows.Forms.TextBox();
            this.panelClient = new System.Windows.Forms.Panel();
            this.lbAdressConnect = new System.Windows.Forms.Label();
            this.tbAddressConnect = new System.Windows.Forms.TextBox();
            this.lbPortConnect = new System.Windows.Forms.Label();
            this.tbPortConnect = new System.Windows.Forms.TextBox();
            this.tbNamePlayer = new System.Windows.Forms.TextBox();
            this.lbServerOnline = new System.Windows.Forms.Label();
            this.ConnectionTimer = new System.Windows.Forms.Timer(this.components);
            this.pbGame = new System.Windows.Forms.PictureBox();
            this.refreshGraphics = new System.Windows.Forms.Timer(this.components);
            this.AllPanels = new System.Windows.Forms.SplitContainer();
            this.OptionsPanel = new System.Windows.Forms.Panel();
            this.tlsbExit = new System.Windows.Forms.ToolStripButton();
            this.tslMenu.SuspendLayout();
            this.PanelConnections.SuspendLayout();
            this.panelServer.SuspendLayout();
            this.panelClient.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AllPanels)).BeginInit();
            this.AllPanels.Panel1.SuspendLayout();
            this.AllPanels.Panel2.SuspendLayout();
            this.AllPanels.SuspendLayout();
            this.OptionsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbConnected
            // 
            this.lbConnected.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbConnected.FormattingEnabled = true;
            this.lbConnected.ItemHeight = 16;
            this.lbConnected.Location = new System.Drawing.Point(0, 17);
            this.lbConnected.Name = "lbConnected";
            this.lbConnected.Size = new System.Drawing.Size(949, 132);
            this.lbConnected.TabIndex = 0;
            // 
            // btnLaunchServer
            // 
            this.btnLaunchServer.Location = new System.Drawing.Point(6, 97);
            this.btnLaunchServer.Name = "btnLaunchServer";
            this.btnLaunchServer.Size = new System.Drawing.Size(137, 46);
            this.btnLaunchServer.TabIndex = 1;
            this.btnLaunchServer.Text = "Launch ";
            this.btnLaunchServer.UseVisualStyleBackColor = true;
            this.btnLaunchServer.Click += new System.EventHandler(this.btnServer_Click);
            // 
            // btnClient
            // 
            this.btnClient.Location = new System.Drawing.Point(165, 45);
            this.btnClient.Name = "btnClient";
            this.btnClient.Size = new System.Drawing.Size(219, 57);
            this.btnClient.TabIndex = 2;
            this.btnClient.Text = "Connect";
            this.btnClient.UseVisualStyleBackColor = true;
            this.btnClient.Click += new System.EventHandler(this.btnClient_Click);
            // 
            // tslMenu
            // 
            this.tslMenu.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.tslMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tslMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.tslMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsDropDownFile,
            this.tlsbExit});
            this.tslMenu.Location = new System.Drawing.Point(0, 0);
            this.tslMenu.Name = "tslMenu";
            this.tslMenu.Size = new System.Drawing.Size(949, 53);
            this.tslMenu.Stretch = true;
            this.tslMenu.TabIndex = 2;
            this.tslMenu.Text = "toolStrip1";
            this.tslMenu.Visible = false;
            // 
            // tsDropDownFile
            // 
            this.tsDropDownFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsDropDownFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem});
            this.tsDropDownFile.Image = ((System.Drawing.Image)(resources.GetObject("tsDropDownFile.Image")));
            this.tsDropDownFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsDropDownFile.Name = "tsDropDownFile";
            this.tsDropDownFile.Size = new System.Drawing.Size(85, 50);
            this.tsDropDownFile.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(175, 50);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // lbLstPlayers
            // 
            this.lbLstPlayers.AutoSize = true;
            this.lbLstPlayers.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbLstPlayers.Location = new System.Drawing.Point(0, 0);
            this.lbLstPlayers.Name = "lbLstPlayers";
            this.lbLstPlayers.Size = new System.Drawing.Size(81, 17);
            this.lbLstPlayers.TabIndex = 1;
            this.lbLstPlayers.Text = "Players List";
            // 
            // PanelConnections
            // 
            this.PanelConnections.Controls.Add(this.lbNamePlayer2);
            this.PanelConnections.Controls.Add(this.panelServer);
            this.PanelConnections.Controls.Add(this.panelClient);
            this.PanelConnections.Controls.Add(this.tbNamePlayer);
            this.PanelConnections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelConnections.Location = new System.Drawing.Point(0, 0);
            this.PanelConnections.Name = "PanelConnections";
            this.PanelConnections.Size = new System.Drawing.Size(949, 181);
            this.PanelConnections.TabIndex = 4;
            // 
            // lbNamePlayer2
            // 
            this.lbNamePlayer2.AutoSize = true;
            this.lbNamePlayer2.Location = new System.Drawing.Point(367, 16);
            this.lbNamePlayer2.Name = "lbNamePlayer2";
            this.lbNamePlayer2.Size = new System.Drawing.Size(97, 17);
            this.lbNamePlayer2.TabIndex = 5;
            this.lbNamePlayer2.Text = "Player Name :";
            // 
            // panelServer
            // 
            this.panelServer.Controls.Add(this.label1);
            this.panelServer.Controls.Add(this.tbGameToLoad);
            this.panelServer.Controls.Add(this.button1);
            this.panelServer.Controls.Add(this.btnLaunchServer);
            this.panelServer.Controls.Add(this.lbPortServer);
            this.panelServer.Controls.Add(this.tbPortServer);
            this.panelServer.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelServer.Location = new System.Drawing.Point(0, 0);
            this.panelServer.Name = "panelServer";
            this.panelServer.Size = new System.Drawing.Size(337, 181);
            this.panelServer.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(199, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Game to load :";
            // 
            // tbGameToLoad
            // 
            this.tbGameToLoad.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbGameToLoad.Enabled = false;
            this.tbGameToLoad.Location = new System.Drawing.Point(202, 45);
            this.tbGameToLoad.Name = "tbGameToLoad";
            this.tbGameToLoad.ReadOnly = true;
            this.tbGameToLoad.Size = new System.Drawing.Size(100, 22);
            this.tbGameToLoad.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(202, 97);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 46);
            this.button1.TabIndex = 4;
            this.button1.Text = "Load Game";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbPortServer
            // 
            this.lbPortServer.AutoSize = true;
            this.lbPortServer.Location = new System.Drawing.Point(12, 16);
            this.lbPortServer.Name = "lbPortServer";
            this.lbPortServer.Size = new System.Drawing.Size(34, 17);
            this.lbPortServer.TabIndex = 1;
            this.lbPortServer.Text = "Port";
            // 
            // tbPortServer
            // 
            this.tbPortServer.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbPortServer.Location = new System.Drawing.Point(15, 45);
            this.tbPortServer.Name = "tbPortServer";
            this.tbPortServer.Size = new System.Drawing.Size(100, 22);
            this.tbPortServer.TabIndex = 0;
            this.tbPortServer.Text = "3000";
            // 
            // panelClient
            // 
            this.panelClient.Controls.Add(this.lbAdressConnect);
            this.panelClient.Controls.Add(this.tbAddressConnect);
            this.panelClient.Controls.Add(this.btnClient);
            this.panelClient.Controls.Add(this.lbPortConnect);
            this.panelClient.Controls.Add(this.tbPortConnect);
            this.panelClient.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelClient.Location = new System.Drawing.Point(495, 0);
            this.panelClient.Name = "panelClient";
            this.panelClient.Size = new System.Drawing.Size(454, 181);
            this.panelClient.TabIndex = 1;
            // 
            // lbAdressConnect
            // 
            this.lbAdressConnect.AutoSize = true;
            this.lbAdressConnect.Location = new System.Drawing.Point(3, 95);
            this.lbAdressConnect.Name = "lbAdressConnect";
            this.lbAdressConnect.Size = new System.Drawing.Size(20, 17);
            this.lbAdressConnect.TabIndex = 5;
            this.lbAdressConnect.Text = "IP";
            // 
            // tbAddressConnect
            // 
            this.tbAddressConnect.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbAddressConnect.Location = new System.Drawing.Point(3, 121);
            this.tbAddressConnect.Name = "tbAddressConnect";
            this.tbAddressConnect.Size = new System.Drawing.Size(100, 22);
            this.tbAddressConnect.TabIndex = 4;
            this.tbAddressConnect.Text = "127.0.0.1";
            // 
            // lbPortConnect
            // 
            this.lbPortConnect.AutoSize = true;
            this.lbPortConnect.Location = new System.Drawing.Point(3, 50);
            this.lbPortConnect.Name = "lbPortConnect";
            this.lbPortConnect.Size = new System.Drawing.Size(34, 17);
            this.lbPortConnect.TabIndex = 3;
            this.lbPortConnect.Text = "Port";
            // 
            // tbPortConnect
            // 
            this.tbPortConnect.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbPortConnect.Location = new System.Drawing.Point(3, 70);
            this.tbPortConnect.Name = "tbPortConnect";
            this.tbPortConnect.Size = new System.Drawing.Size(100, 22);
            this.tbPortConnect.TabIndex = 2;
            this.tbPortConnect.Text = "3000";
            // 
            // tbNamePlayer
            // 
            this.tbNamePlayer.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbNamePlayer.Location = new System.Drawing.Point(367, 36);
            this.tbNamePlayer.MaxLength = 10;
            this.tbNamePlayer.Name = "tbNamePlayer";
            this.tbNamePlayer.Size = new System.Drawing.Size(100, 22);
            this.tbNamePlayer.TabIndex = 4;
            this.tbNamePlayer.Text = "Player";
            // 
            // lbServerOnline
            // 
            this.lbServerOnline.AutoSize = true;
            this.lbServerOnline.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbServerOnline.ForeColor = System.Drawing.Color.Red;
            this.lbServerOnline.Location = new System.Drawing.Point(0, 457);
            this.lbServerOnline.Name = "lbServerOnline";
            this.lbServerOnline.Size = new System.Drawing.Size(92, 17);
            this.lbServerOnline.TabIndex = 5;
            this.lbServerOnline.Text = "Server online";
            this.lbServerOnline.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbServerOnline.Visible = false;
            // 
            // ConnectionTimer
            // 
            this.ConnectionTimer.Interval = 150;
            this.ConnectionTimer.Tick += new System.EventHandler(this.ConnectionTimer_Tick);
            // 
            // pbGame
            // 
            this.pbGame.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbGame.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pbGame.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbGame.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbGame.Enabled = false;
            this.pbGame.Location = new System.Drawing.Point(467, 439);
            this.pbGame.Margin = new System.Windows.Forms.Padding(60);
            this.pbGame.Name = "pbGame";
            this.pbGame.Size = new System.Drawing.Size(528, 528);
            this.pbGame.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbGame.TabIndex = 6;
            this.pbGame.TabStop = false;
            this.pbGame.Visible = false;
            // 
            // refreshGraphics
            // 
            this.refreshGraphics.Interval = 80;
            this.refreshGraphics.Tick += new System.EventHandler(this.refreshGraphics_Tick);
            // 
            // AllPanels
            // 
            this.AllPanels.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.AllPanels.IsSplitterFixed = true;
            this.AllPanels.Location = new System.Drawing.Point(0, 125);
            this.AllPanels.Name = "AllPanels";
            this.AllPanels.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // AllPanels.Panel1
            // 
            this.AllPanels.Panel1.Controls.Add(this.OptionsPanel);
            // 
            // AllPanels.Panel2
            // 
            this.AllPanels.Panel2.Controls.Add(this.PanelConnections);
            this.AllPanels.Size = new System.Drawing.Size(949, 332);
            this.AllPanels.SplitterDistance = 147;
            this.AllPanels.TabIndex = 6;
            // 
            // OptionsPanel
            // 
            this.OptionsPanel.Controls.Add(this.lbConnected);
            this.OptionsPanel.Controls.Add(this.lbLstPlayers);
            this.OptionsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.OptionsPanel.Enabled = false;
            this.OptionsPanel.Location = new System.Drawing.Point(0, 0);
            this.OptionsPanel.Name = "OptionsPanel";
            this.OptionsPanel.Size = new System.Drawing.Size(949, 147);
            this.OptionsPanel.TabIndex = 3;
            // 
            // tlsbExit
            // 
            this.tlsbExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tlsbExit.Image = ((System.Drawing.Image)(resources.GetObject("tlsbExit.Image")));
            this.tlsbExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlsbExit.Name = "tlsbExit";
            this.tlsbExit.Size = new System.Drawing.Size(77, 50);
            this.tlsbExit.Text = "Exit";
            this.tlsbExit.Click += new System.EventHandler(this.tlsbExit_Click);
            // 
            // Lobby
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(949, 474);
            this.Controls.Add(this.pbGame);
            this.Controls.Add(this.AllPanels);
            this.Controls.Add(this.tslMenu);
            this.Controls.Add(this.lbServerOnline);
            this.HelpButton = true;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Lobby";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Lobby_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Lobby_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Lobby_KeyUp);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Lobby_PreviewKeyDown);
            this.tslMenu.ResumeLayout(false);
            this.tslMenu.PerformLayout();
            this.PanelConnections.ResumeLayout(false);
            this.PanelConnections.PerformLayout();
            this.panelServer.ResumeLayout(false);
            this.panelServer.PerformLayout();
            this.panelClient.ResumeLayout(false);
            this.panelClient.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGame)).EndInit();
            this.AllPanels.Panel1.ResumeLayout(false);
            this.AllPanels.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AllPanels)).EndInit();
            this.AllPanels.ResumeLayout(false);
            this.OptionsPanel.ResumeLayout(false);
            this.OptionsPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbConnected;
        private System.Windows.Forms.Button btnLaunchServer;
        private System.Windows.Forms.Button btnClient;
        private System.Windows.Forms.Panel PanelConnections;
        private System.Windows.Forms.Panel panelServer;
        private System.Windows.Forms.Label lbPortServer;
        private System.Windows.Forms.TextBox tbPortServer;
        private System.Windows.Forms.Panel panelClient;
        private System.Windows.Forms.Label lbAdressConnect;
        private System.Windows.Forms.TextBox tbAddressConnect;
        private System.Windows.Forms.Label lbPortConnect;
        private System.Windows.Forms.TextBox tbPortConnect;
        private System.Windows.Forms.Label lbServerOnline;
        private System.Windows.Forms.Label lbLstPlayers;
        private System.Windows.Forms.Timer ConnectionTimer;
        private System.Windows.Forms.PictureBox pbGame;
        private System.Windows.Forms.Timer refreshGraphics;
        private System.Windows.Forms.ToolStrip tslMenu;
        private System.Windows.Forms.ToolStripDropDownButton tsDropDownFile;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.SplitContainer AllPanels;
        private System.Windows.Forms.Panel OptionsPanel;
        private System.Windows.Forms.Label lbNamePlayer2;
        private System.Windows.Forms.TextBox tbNamePlayer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbGameToLoad;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripButton tlsbExit;
    }
}