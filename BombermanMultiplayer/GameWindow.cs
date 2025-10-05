using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BombermanMultiplayer
{
    public partial class GameWindow : Form
    {
        Game game;

        private BufferedGraphics bufferG = null;
        private Graphics gr;
        private Rectangle[] BonusSlot;
        public GameWindow()
        {
            InitializeComponent();
            this.pbGame.ClientSize = new Size(528, 528);

            game = new Game(this.pbGame.Width, this.pbGame.Height);

            BonusSlot = new Rectangle[4];

            for (int i = 0; i < 4; i++)
            {
                BonusSlot[i] = new Rectangle(i * pbGame.Width / 18, pbGame.Height / 25, pbGame.Width / 20, pbGame.Height / 20);
                if (i > 1)
                {
                    BonusSlot[i] = new Rectangle(3 * pbGame.Width / 4 + i * pbGame.Width / 18, pbGame.Height / 25, pbGame.Width / 20, pbGame.Height / 20);
                }
            }

            game.world.loadBackground(Properties.Resources.World);
            game.world.loadSpriteTile(Properties.Resources.BlockDestructible, Properties.Resources.BlockNonDestructible);

            // Inicializuojam visų žaidėjų pradines sprites
            if (game.players.Length > 0) game.players[0].LoadSprite(Properties.Resources.AT_DOWN);
            if (game.players.Length > 1) game.players[1].LoadSprite(Properties.Resources.T_UP);
            if (game.players.Length > 2) game.players[2].LoadSprite(Properties.Resources.AT_DOWN);
            if (game.players.Length > 3) game.players[3].LoadSprite(Properties.Resources.T_UP);

            bufferG = BufferedGraphicsManager.Current.Allocate(pbGame.CreateGraphics(), pbGame.DisplayRectangle);
            gr = bufferG.Graphics;

            this.game.LogicTimer.Start();
            this.refreshGraphics.Start();
        }


        public void Draw()
        {
            gr.Clear(pbGame.BackColor);
            game.world.refreshTileSprites();

            game.world.Draw(gr);

            // Piešiame visus žaidėjus
            for (int i = 0; i < game.players.Length; i++)
            {
                game.players[i].Draw(gr);
                game.players[i].DrawPosition(gr);
            }

            try
            {
                foreach (Bomb bomb in game.BombsOnTheMap)
                {
                    bomb.Draw(gr);
                }
            }
            catch (Exception) {}

            DrawInterface();

            bufferG.Render();
        }

        public void DrawInterface()
        {

            if (game.Paused && !game.Over)
            {
                tlsMenu.Visible = true;
                gr.DrawString("PAUSED", new System.Drawing.Font("Arial", 30), Brushes.White, pbGame.Width / 2, pbGame.Height / 2);
            }
            else if (!game.Paused && !game.Over)
            {
                tlsMenu.Visible = false;
            }
            else
            {
                tlsMenu.Visible = true;
            }

            if (game.Over)
            {
                gr.DrawString("GAME OVER", new Font("Stencil", (float)(this.pbGame.Height / 10), System.Drawing.FontStyle.Bold),
                    new SolidBrush(Color.WhiteSmoke), 0, this.pbGame.Height / 2 - this.pbGame.Height / 8);

                if (game.Winner > 0 && game.Winner <= game.players.Length)
                {
                    gr.DrawString("Player " + game.Winner + " wins", new Font("Stencil", (float)(this.pbGame.Height / 10), FontStyle.Bold),
                        new SolidBrush(Color.WhiteSmoke), 0, this.pbGame.Height / 2 - this.pbGame.Height / 8 + this.pbGame.Height / 9);
                }
                else
                {
                    gr.DrawString("Draw", new Font("Stencil", (float)(this.pbGame.Height / 10), FontStyle.Bold),
                        new SolidBrush(Color.WhiteSmoke), 0, this.pbGame.Height / 2 - this.pbGame.Height / 8 + this.pbGame.Height / 9);
                }
            }

            // Bonusų atvaizdavimas visiems žaidėjams
            for (int p = 0; p < game.players.Length; p++)
            {
                gr.DrawString("Player " + (p + 1) + " : ", new Font("Arial", 10), Brushes.White, BonusSlot[p].X, BonusSlot[p].Y - BonusSlot[p].Width / 2);
                for (int i = 0; i < game.players[p].BonusSlot.Length; i++)
                {
                    switch (game.players[p].BonusSlot[i])
                    {
                        case Objects.BonusType.PowerBomb:
                            gr.DrawImage(Properties.Resources.SuperBomb, BonusSlot[p]);
                            gr.DrawString((game.players[p].BonusTimer[i] / 1000).ToString() + "s", new Font("Arial", 10), Brushes.White, BonusSlot[p].X, BonusSlot[p].Y + BonusSlot[p].Height);
                            break;
                        case Objects.BonusType.SpeedBoost:
                            gr.DrawImage(Properties.Resources.SpeedUp, BonusSlot[p]);
                            gr.DrawString((game.players[p].BonusTimer[i] / 1000).ToString() + "s", new Font("Arial", 10), Brushes.White, BonusSlot[p].X, BonusSlot[p].Y + BonusSlot[p].Height);
                            break;
                        case Objects.BonusType.Desamorce:
                            gr.DrawImage(Properties.Resources.Deactivate, BonusSlot[p]);
                            gr.DrawString((game.players[p].BonusTimer[i] / 1000).ToString() + "s", new Font("Arial", 10), Brushes.White, BonusSlot[p].X, BonusSlot[p].Y + BonusSlot[p].Height);
                            break;
                        case Objects.BonusType.Armor:
                            gr.DrawImage(Properties.Resources.Armor, BonusSlot[p]);
                            gr.DrawString((game.players[p].BonusTimer[i] / 1000).ToString() + "s", new Font("Arial", 10), Brushes.White, BonusSlot[p].X, BonusSlot[p].Y + BonusSlot[p].Height);
                            break;
                        case Objects.BonusType.None:
                            break;
                        default:
                            break;
                    }
                }
            }
        }


        private void Game_KeyDown(object sender, KeyEventArgs e)
        {
            game.Game_KeyDown(e.KeyCode);

           
        }

        

        private void Game_KeyUp(object sender, KeyEventArgs e)
        {
            game.Game_KeyUp(e.KeyCode);
        }


        private void Game_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
        }

        private void refreshGraphics_Tick(object sender, EventArgs e)
        {

                Draw();

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (game.Over)
            {
                MessageBox.Show("You can't save the game now, the game is over !");
                return;
            }

            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Filter = "Bomberman savegame | *.bmb";
                dlg.AddExtension = true;
                dlg.FileName = "save.bmb";
                dlg.DefaultExt = ".bmb";

                if (dlg.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                else
                {
                    try
                    {
                        game.SaveGame(dlg.FileName);
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("An error has occured : " + ex.Message + " \n please try again");
                    }

                }
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Bomberman savegame | *.bmb";
                dlg.AddExtension = true;
                dlg.FileName = "save.bmb";
                dlg.DefaultExt = ".bmb";

                if (dlg.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                else
                {
                    try
                    {
                        game.LoadGame(dlg.FileName);
                        game.world.loadBackground(Properties.Resources.World);
                        game.world.loadSpriteTile(Properties.Resources.BlockDestructible, Properties.Resources.BlockNonDestructible);

                        // Inicializuojam visų žaidėjų sprites po užkrovimo
                        if (game.players.Length > 0) game.players[0].LoadSprite(Properties.Resources.AT_DOWN);
                        if (game.players.Length > 1) game.players[1].LoadSprite(Properties.Resources.T_UP);
                        if (game.players.Length > 2) game.players[2].LoadSprite(Properties.Resources.AT_DOWN);
                        if (game.players.Length > 3) game.players[3].LoadSprite(Properties.Resources.T_UP);

                        Draw();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error has occured : " + ex.Message + " \n please try again");
                    }
                }
            }
        }

        private void tlsbExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
