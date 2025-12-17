using System.Collections.Generic;

namespace BombermanMultiplayer.Flyweight
{
    public static class TileFlyweightFactory
    {
        private static readonly Dictionary<TileType, Tile> Flyweights = new Dictionary<TileType, Tile>();

        public static Tile GetTile(TileType type, int totalFrame, int tileWidth, int tileHeight)
        {
            if (!Flyweights.ContainsKey(type))
            {
                Tile tile;

                switch (type)
                {
                    case TileType.Floor:
                        tile = new Tile(0, 0, totalFrame, tileWidth, tileHeight, walkable: true, destroyable: false);
                        break;
                    case TileType.Wall:
                        tile = new Tile(0, 0, totalFrame, tileWidth, tileHeight, walkable: false, destroyable: false);
                        break;
                    case TileType.Destructible:
                        tile = new Tile(0, 0, totalFrame, tileWidth, tileHeight, walkable: false, destroyable: true);
                        break;
                    default:
                        tile = new Tile(0, 0, totalFrame, tileWidth, tileHeight, walkable: true, destroyable: false);
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