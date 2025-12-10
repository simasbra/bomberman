using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BombermanMultiplayer.Facade;

namespace BombermanMultiplayer
{
    public partial class GameWindow : Form
    {
        Game game;
        private RenderingFacade _renderingFacade;
        private GameState gameState;
        private SaveGameDataObserver saveGameObserver;

        private BufferedGraphics bufferG = null;
        private Graphics gr;
        private Rectangle[] BonusSlot;
        public GameWindow()
        {
            InitializeComponent();
            this.pbGame.ClientSize = new Size(528, 528);

            _renderingFacade = new RenderingFacade();

            game = new Game(this.pbGame.Width, this.pbGame.Height);

            // Initialize observer pattern
            gameState = new GameState();
            saveGameObserver = new SaveGameDataObserver(gameState);
            game.SetGameState(gameState);

            game.world.loadBackground(Properties.Resources.World);
            game.world.loadSpriteTile(Properties.Resources.BlockDestructible, Properties.Resources.BlockNonDestructible);

            // Inicializuojam visų žaidėjų pradines sprites
            if (game.players.Length > 0) game.players[0].LoadSprite(Properties.Resources.AT_DOWN);
            if (game.players.Length > 1) game.players[1].LoadSprite(Properties.Resources.T_UP);
            if (game.players.Length > 2) game.players[2].LoadSprite(Properties.Resources.AT_DOWN);
            if (game.players.Length > 3) game.players[3].LoadSprite(Properties.Resources.T_UP);

            bufferG = BufferedGraphicsManager.Current.Allocate(pbGame.CreateGraphics(), pbGame.DisplayRectangle);
            gr = bufferG.Graphics;

            BonusSlot = new Rectangle[4];
            for (int i = 0; i < BonusSlot.Length; i++)
            {
                BonusSlot[i] = new Rectangle(10 + i * 60, 10, 50, 50);
            }

            this.game.LogicTimer.Start();
            this.refreshGraphics.Start();
        }


        public void Draw()
        {

            _renderingFacade.DrawGameScene(gr, game);
            tlsMenu.Visible = game.Paused || game.Over;
            chkAutoSave.Visible = game.Paused && !game.Over;
            _renderingFacade.DrawInterface(gr, game, BonusSlot, pbGame.ClientSize);

            bufferG.Render();

        }


        private void Game_KeyDown(object sender, KeyEventArgs e)
        {
            // Handle Escape key explicitly to ensure it works even when controls have focus
            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                game.Game_KeyDown(e.KeyCode);
                return;
            }

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

        private void chkAutoSave_CheckedChanged(object sender, EventArgs e)
        {
            // Update observer state to match checkbox state when user changes it
            if (saveGameObserver != null && sender is CheckBox checkBox)
            {
                saveGameObserver.SetAutoSaveEnabled(checkBox.Checked);
            }
        }

        private void chkAutoSave_Click(object sender, EventArgs e)
        {
            // Return focus to form after click to prevent Space key from toggling checkbox
            this.Focus();
            if (pbGame != null && pbGame.CanFocus)
            {
                pbGame.Focus();
            }
        }

        private void chkAutoSave_KeyDown(object sender, KeyEventArgs e)
        {
            // Prevent Space key from toggling checkbox - forward to game instead
            if (e.KeyCode == Keys.Space)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                game.Game_KeyDown(Keys.Space);
                this.Focus();
                return;
            }

            // Process Escape key to ensure unpause works
            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                game.Game_KeyDown(Keys.Escape);
                this.Focus();
            }
        }
    }
}
