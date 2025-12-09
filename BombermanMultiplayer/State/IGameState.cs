using System.Windows.Forms;

namespace BombermanMultiplayer.State
{
    public interface IGameState
    {
        void Enter(Game game);
        void HandleInput(Keys key, Game game);
        void HandleKeyUp(Keys key, Game game);
        void Update(Game game);
    }
}
