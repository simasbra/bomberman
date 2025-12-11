using System.Drawing;
using BombermanMultiplayer.Objects;

namespace BombermanMultiplayer.Composite
{
    /// <summary>
    /// Adapter that makes Bomb objects work with IExplosive interface
    /// </summary>
    [Serializable]
    public class BombExplosiveAdapter : IExplosive
    {
        private Bomb bomb;
        private Tile[,] mapGrid;
        private Player[] players;

        public BombExplosiveAdapter(Bomb bomb, Tile[,] mapGrid, Player[] players)
        {
            this.bomb = bomb;
            this.mapGrid = mapGrid;
            this.players = players;
        }

        public Bomb Bomb => bomb;

        public void Update(int elapsedTime)
        {
            if (bomb != null)
            {
                bomb.UpdateFrame(elapsedTime);
                bomb.TimingExplosion(elapsedTime);
            }
        }

        public void Explode()
        {
            if (bomb != null && bomb.Exploding && mapGrid != null && players != null)
            {
                bomb.Explosion(mapGrid, players);
            }
        }

        public bool IsExploding()
        {
            return bomb != null && bomb.Exploding;
        }

        public Point GetPosition()
        {
            if (bomb != null && bomb.CasePosition != null && bomb.CasePosition.Length >= 2)
            {
                return new Point(bomb.CasePosition[1], bomb.CasePosition[0]);
            }
            return new Point(0, 0);
        }
    }

    /// <summary>
    /// Adapter that makes Mine objects work with IExplosive interface
    /// </summary>
    [Serializable]
    public class MineExplosiveAdapter : IExplosive
    {
        private Mine mine;
        private Tile[,] mapGrid;
        private Player[] players;

        public MineExplosiveAdapter(Mine mine, Tile[,] mapGrid, Player[] players)
        {
            this.mine = mine;
            this.mapGrid = mapGrid;
            this.players = players;
        }

        public Mine Mine => mine;

        public void Update(int elapsedTime)
        {
            if (mine != null)
            {
                mine.CheckProximity(players);
                mine.UpdateFrame(elapsedTime);
                mine.TimingExplosion(elapsedTime);
            }
        }

        public void Explode()
        {
            if (mine != null && mine.Exploding && mapGrid != null && players != null)
            {
                mine.Explosion(mapGrid, players);
            }
        }

        public bool IsExploding()
        {
            return mine != null && mine.Exploding;
        }

        public Point GetPosition()
        {
            if (mine != null && mine.CasePosition != null && mine.CasePosition.Length >= 2)
            {
                return new Point(mine.CasePosition[1], mine.CasePosition[0]);
            }
            return new Point(0, 0);
        }
    }

    /// <summary>
    /// Adapter that makes Grenade objects work with IExplosive interface
    /// </summary>
    [Serializable]
    public class GrenadeExplosiveAdapter : IExplosive
    {
        private Grenade grenade;
        private Tile[,] mapGrid;
        private Player[] players;

        public GrenadeExplosiveAdapter(Grenade grenade, Tile[,] mapGrid, Player[] players)
        {
            this.grenade = grenade;
            this.mapGrid = mapGrid;
            this.players = players;
        }

        public Grenade Grenade => grenade;

        public void Update(int elapsedTime)
        {
            if (grenade != null)
            {
                grenade.MoveGrenade(mapGrid);
                grenade.UpdateFrame(elapsedTime);
                grenade.TimingExplosion(elapsedTime);
            }
        }

        public void Explode()
        {
            if (grenade != null && grenade.Exploding && mapGrid != null && players != null)
            {
                grenade.Explosion(mapGrid, players);
            }
        }

        public bool IsExploding()
        {
            return grenade != null && grenade.Exploding;
        }

        public Point GetPosition()
        {
            if (grenade != null && grenade.CasePosition != null && grenade.CasePosition.Length >= 2)
            {
                return new Point(grenade.CasePosition[1], grenade.CasePosition[0]);
            }
            return new Point(0, 0);
        }
    }
}
