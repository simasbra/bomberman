using System.Collections.Generic;

namespace BombermanMultiplayer.Flyweight
{
    public static class TileFlyweightFactory
    {
        private static readonly Dictionary<TileType, Tile> Flyweights = new Dictionary<TileType, Tile>();

        public static Tile GetTile(TileType type)
        {
            if (!Flyweights.ContainsKey(type))
            {
                Tile tile;

                switch (type)
                {
                    case TileType.Floor:
                        tile = new Tile(0, 0, 1, 48, 48, walkable: true, destroyable: false);
                        break;
                    case TileType.Wall:
                        tile = new Tile(0, 0, 1, 48, 48, walkable: false, destroyable: false);
                        break;
                    case TileType.Destructible:
                        tile = new Tile(0, 0, 1, 48, 48, walkable: false, destroyable: true);
                        break;
                    default:
                        tile = new Tile(0, 0, 1, 48, 48, walkable: true, destroyable: false);
                        break;
                }

                tile.LoadSprite(type == TileType.Wall
                    ? Properties.Resources.BlockNonDestructible
                    : type == TileType.Destructible
                        ? Properties.Resources.BlockDestructible
                        : null);

                Flyweights[type] = tile;
            }

            return Flyweights[type];
        }
    }

    public enum TileType
    {
        Floor,
        Wall,
        Destructible
    }
}