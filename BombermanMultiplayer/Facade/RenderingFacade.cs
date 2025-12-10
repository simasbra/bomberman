using System.Drawing;
using System.Windows.Forms;

namespace BombermanMultiplayer.Facade
{
    /// <summary>
    /// Provides a unified interface for rendering the game scene and user interface elements.
    /// </summary>
    public class RenderingFacade
    {
        private readonly WorldRenderer _worldRenderer;
        private readonly PlayerRenderer _playerRenderer;
        private readonly ExplosiveRenderer _explosiveRenderer;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderingFacade"/> class.
        /// </summary>
        public RenderingFacade()
        {
            _worldRenderer = new WorldRenderer();
            _playerRenderer = new PlayerRenderer();
            _explosiveRenderer = new ExplosiveRenderer();
        }

        /// <summary>
        /// Draws the entire game scene, including the world, players, and explosives.
        /// </summary>
        /// <param name="gr"></param>
        /// <param name="game"></param>
        /// <param name="showPlayerPositions"></param>
        public void DrawGameScene(Graphics gr, Game game, bool showPlayerPositions = false)
        {
            if (gr == null || game == null || game.world == null) return;

            _worldRenderer.Draw(gr, game.world);

            _explosiveRenderer.Draw(gr, game.BombsOnTheMap, game.MinesOnTheMap, game.GrenadesOnTheMap);

            _playerRenderer.Draw(gr, game.players, showPlayerPositions);
        }

        /// <summary>
        /// Draws the game interface, including pause and game over messages, as well as player bonuses.
        /// </summary>
        /// <param name="gr"></param>
        /// <param name="game"></param>
        /// <param name="bonusSlots"></param>
        /// <param name="canvasSize"></param>
        public void DrawInterface(Graphics gr, Game game, Rectangle[] bonusSlots, Size canvasSize)
        {
            if (gr == null || game == null || game.world == null || bonusSlots == null) return;

            if (game.Paused && !game.Over)
            {
                gr.DrawString("PAUSED", new Font("Arial", 30), Brushes.White, canvasSize.Width / 2, canvasSize.Height / 2);
            }

            if (game.Over)
            {
                var bigFont = new Font("Stencil", (float)(canvasSize.Height / 10), FontStyle.Bold);
                gr.DrawString("GAME OVER", bigFont, Brushes.WhiteSmoke, 0, canvasSize.Height / 2 - canvasSize.Height / 8);

                string winnerText;
                if (game.Winner > 0 && game.Winner <= game.players.Length)
                    winnerText = $"Player {game.Winner} wins";
                else
                    winnerText = "Draw";

                gr.DrawString(winnerText, bigFont, Brushes.WhiteSmoke, 0, canvasSize.Height / 2 - canvasSize.Height / 8 + canvasSize.Height / 9);
            }

            // Bonuses for players
            for (int p = 0; p < game.players.Length && p < bonusSlots.Length; p++)
            {
                var player = game.players[p];
                if (player == null) continue;

                gr.DrawString("Player " + (p + 1) + " : ", new Font("Arial", 10), Brushes.White, bonusSlots[p].X, bonusSlots[p].Y - bonusSlots[p].Width / 2);

                for (int i = 0; i < player.BonusSlot.Length; i++)
                {
                    var bonusType = player.BonusSlot[i];
                    var timer = player.BonusTimer[i];

                    switch (bonusType)
                    {
                        case Objects.BonusType.PowerBomb:
                            gr.DrawImage(Properties.Resources.SuperBomb, bonusSlots[p]);
                            gr.DrawString((timer / 1000).ToString() + "s", new Font("Arial", 10), Brushes.White, bonusSlots[p].X, bonusSlots[p].Y + bonusSlots[p].Height);
                            break;
                        case Objects.BonusType.SpeedBoost:
                            gr.DrawImage(Properties.Resources.SpeedUp, bonusSlots[p]);
                            gr.DrawString((timer / 1000).ToString() + "s", new Font("Arial", 10), Brushes.White, bonusSlots[p].X, bonusSlots[p].Y + bonusSlots[p].Height);
                            break;
                        case Objects.BonusType.Desamorce:
                            gr.DrawImage(Properties.Resources.Deactivate, bonusSlots[p]);
                            gr.DrawString((timer / 1000).ToString() + "s", new Font("Arial", 10), Brushes.White, bonusSlots[p].X, bonusSlots[p].Y + bonusSlots[p].Height);
                            break;
                        case Objects.BonusType.Armor:
                            gr.DrawImage(Properties.Resources.Armor, bonusSlots[p]);
                            gr.DrawString((timer / 1000).ToString() + "s", new Font("Arial", 10), Brushes.White, bonusSlots[p].X, bonusSlots[p].Y + bonusSlots[p].Height);
                            break;
                        case Objects.BonusType.None:
                        default:
                            break;
                    }
                }
            }
        }
    }
}