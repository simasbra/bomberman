namespace BombermanMultiplayer
{
    partial class GameWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameWindow));
            this.pbGame = new System.Windows.Forms.PictureBox();
            this.refreshGraphics = new System.Windows.Forms.Timer(this.components);
            this.tlsMenu = new System.Windows.Forms.ToolStrip();
            this.tsDropDownFile = new System.Windows.Forms.ToolStripDropDownButton();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tlsbExit = new System.Windows.Forms.ToolStripButton();
            this.chkAutoSave = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbGame)).BeginInit();
            this.tlsMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbGame
            // 
            this.pbGame.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbGame.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pbGame.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbGame.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbGame.Location = new System.Drawing.Point(150, 150);
            this.pbGame.MinimumSize = new System.Drawing.Size(480, 480);
            this.pbGame.Name = "pbGame";
            this.pbGame.Size = new System.Drawing.Size(600, 600);
            this.pbGame.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbGame.TabIndex = 0;
            this.pbGame.TabStop = false;
			this.pbGame.Click += new System.EventHandler(this.pbGame_Click);

			// 
			// refreshGraphics
			// 
			this.refreshGraphics.Interval = 50;
            this.refreshGraphics.Tick += new System.EventHandler(this.refreshGraphics_Tick);
            // 
            // tlsMenu
            // 
            this.tlsMenu.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tlsMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.tlsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsDropDownFile,
            this.tlsbExit});
            this.tlsMenu.Location = new System.Drawing.Point(0, 0);
            this.tlsMenu.Name = "tlsMenu";
            this.tlsMenu.Size = new System.Drawing.Size(882, 53);
            this.tlsMenu.TabIndex = 1;
            this.tlsMenu.Text = "toolStrip1";
            this.tlsMenu.Visible = false;
            // 
            // tsDropDownFile
            // 
            this.tsDropDownFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsDropDownFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.loadToolStripMenuItem});
            this.tsDropDownFile.Image = ((System.Drawing.Image)(resources.GetObject("tsDropDownFile.Image")));
            this.tsDropDownFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsDropDownFile.Name = "tsDropDownFile";
            this.tsDropDownFile.Size = new System.Drawing.Size(85, 50);
            this.tsDropDownFile.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(179, 50);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(179, 50);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
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
            // chkAutoSave
            //
            this.chkAutoSave.AutoSize = true;
            this.chkAutoSave.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoSave.ForeColor = System.Drawing.Color.Black;
            this.chkAutoSave.Location = new System.Drawing.Point(12, 60);
            this.chkAutoSave.Name = "chkAutoSave";
            this.chkAutoSave.Size = new System.Drawing.Size(113, 32);
            this.chkAutoSave.TabIndex = 2;
            this.chkAutoSave.TabStop = false;
            this.chkAutoSave.Text = "Auto Save";
            this.chkAutoSave.UseVisualStyleBackColor = false;
            this.chkAutoSave.Visible = false;
            this.chkAutoSave.CheckedChanged += new System.EventHandler(this.chkAutoSave_CheckedChanged);
            this.chkAutoSave.Click += new System.EventHandler(this.chkAutoSave_Click);
            this.chkAutoSave.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chkAutoSave_KeyDown);
			// 
			// txtCommand
			// 
			this.txtCommand = new System.Windows.Forms.TextBox();
			this.txtCommand.Location = new System.Drawing.Point(12, 56);
			this.txtCommand.Name = "txtCommand";
			this.txtCommand.Size = new System.Drawing.Size(300, 22);
			this.txtCommand.TabIndex = 3;
			this.txtCommand.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCommand_KeyDown);
			this.txtCommand.TabStop = false;
			// 
			// btnExecute
			// 
			this.btnExecute = new System.Windows.Forms.Button();
			this.btnExecute.Location = new System.Drawing.Point(318, 54);
			this.btnExecute.Name = "btnExecute";
			this.btnExecute.Size = new System.Drawing.Size(75, 26);
			this.btnExecute.TabIndex = 4;
			this.btnExecute.Text = "Execute";
			this.btnExecute.UseVisualStyleBackColor = true;
			this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
			// 
			// lblResult
			// 
			this.lblResult = new System.Windows.Forms.Label();
			this.lblResult.Location = new System.Drawing.Point(399, 58);
			this.lblResult.Name = "lblResult";
			this.lblResult.Size = new System.Drawing.Size(400, 20);
			this.lblResult.TabIndex = 5;
			this.lblResult.Text = "";
			//
			// GameWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 853);
            this.Controls.Add(this.chkAutoSave);
            this.Controls.Add(this.tlsMenu);
            this.Controls.Add(this.pbGame);
			this.Controls.Add(this.lblResult);
			this.Controls.Add(this.btnExecute);
			this.Controls.Add(this.txtCommand);
			this.MinimumSize = new System.Drawing.Size(600, 600);
            this.Name = "GameWindow";
            this.KeyPreview = true;
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Game_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Game_KeyUp);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Game_PreviewKeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pbGame)).EndInit();
            this.tlsMenu.ResumeLayout(false);
            this.tlsMenu.PerformLayout();



            this.ResumeLayout(false);
            this.PerformLayout();

        }

		#endregion

		private System.Windows.Forms.TextBox txtCommand;
		private System.Windows.Forms.Button btnExecute;
		private System.Windows.Forms.Label lblResult;
		private System.Windows.Forms.PictureBox pbGame;
        private System.Windows.Forms.Timer refreshGraphics;
        private System.Windows.Forms.ToolStrip tlsMenu;
        private System.Windows.Forms.ToolStripDropDownButton tsDropDownFile;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton tlsbExit;
        private System.Windows.Forms.CheckBox chkAutoSave;
    }
}

