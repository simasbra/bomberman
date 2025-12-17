using System.Collections.Generic;

namespace BombermanMultiplayer.Flyweight
{
    public class TileFlyweightFactory
    {
        private static TileFlyweightFactory _instance;
        private readonly Dictionary<TileType, Tile> Flyweights = new Dictionary<TileType, Tile>();
        private int TileWidth;
        private int TileHeight;
        private int TotalFrames;

        private TileFlyweightFactory()
        {
        }

        public static TileFlyweightFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TileFlyweightFactory();
                }

                return _instance;
            }
        }

        public void Initialize(int tileWidth, int tileHeight, int totalFrames)
        {
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            TotalFrames = totalFrames;
            Flyweights.Clear();
        }

        public static Tile GetTile(TileType type)
        {
            return Instance.GetTileInternal(type);
        }

        private Tile GetTileInternal(TileType type)
        {
            if (!Flyweights.ContainsKey(type))
            {
                Tile tile;

                switch (type)
                {
                    case TileType.Floor:
                        tile = new Tile(0, 0, TotalFrames, TileWidth, TileHeight, walkable: true, destroyable: false);
                        break;
                    case TileType.Wall:
                        tile = new Tile(0, 0, TotalFrames, TileWidth, TileHeight, walkable: false, destroyable: false);
                        tile.LoadSprite(Properties.Resources.BlockNonDestructible);
                        break;
                    case TileType.Destructible:
                        tile = new Tile(0, 0, TotalFrames, TileWidth, TileHeight, walkable: false, destroyable: true);
                        tile.LoadSprite(Properties.Resources.BlockDestructible);
                        break;
                    default:
                        tile = new Tile(0, 0, TotalFrames, TileWidth, TileHeight, walkable: true, destroyable: false);
                        break;
                }

                Flyweights[type] = tile;
            }

            return Flyweights[type];
        }

        public int GetFlyweightCount()
        {
            return Flyweights.Count;
        }
    }

    public enum TileType
    {
        Floor,
        Wall,
        Destructible
    }
}