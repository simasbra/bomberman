using System;
using System.Drawing;
using BombermanMultiplayer.Iterator;
using BombermanMultiplayer.Flyweight;

namespace BombermanMultiplayer
{
    [Serializable]
    public class World
    {
        public TileContext[,] MapGrid;

        [NonSerialized]
        private Image Background_;

        /// <summary>
        /// Returns an iterator for traversing all tiles in the map grid
        /// </summary>
        /// <returns>Tile iterator over MapGrid</returns>
        public IIterator<TileContext> GetTileIterator()
        {
            return new TileIterator(MapGrid);
        }

        public Image Background
        {
            get { return Background_; }

            set { Background_ = value; }
        }


        public void Draw(Graphics gr)
        {
            if (Background != null)
            {
                gr.DrawImage(Background, gr.VisibleClipBounds.X, gr.VisibleClipBounds.Y, gr.VisibleClipBounds.Width,
                    gr.VisibleClipBounds.Height);
            }

            for (int i = 0; i < MapGrid.GetLength(0); i++) //Ligne
            {
                for (int j = 0; j < MapGrid.GetLength(1); j++) //Colonne
                {
                    MapGrid[i, j].Draw(gr);
                }
            }
        }

        public void loadBackground(Image background)
        {
            Background = background;
        }

        public void loadSpriteTile(Image spriteDestroyableTile, Image spriteUndestroyableTile)
        {
            // First ensure flyweights are initialized/updated
            int rows = MapGrid.GetLength(0);
            int cols = MapGrid.GetLength(1);
            int tileWidth = MapGrid[0, 0].IntrinsicTile_Source_Width;
            int tileHeight = MapGrid[0, 0].IntrinsicTile_Source_Height;

            // Re-initialize flyweights on the client to ensure sprites are loaded
            TileFlyweightFactory.GetTile(TileType.Wall, 1, tileWidth, tileHeight).LoadSprite(spriteUndestroyableTile);
            TileFlyweightFactory.GetTile(TileType.Destructible, 1, tileWidth, tileHeight).LoadSprite(spriteDestroyableTile);
            TileFlyweightFactory.GetTile(TileType.Floor, 1, tileWidth, tileHeight).UnloadSprite();

            // Re-bind IntrinsicTile to the local flyweight instances after serialization
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    TileType type = TileType.Floor;
                    if (!MapGrid[i, j].Walkable && !MapGrid[i, j].Destroyable) type = TileType.Wall;
                    else if (MapGrid[i, j].Destroyable) type = TileType.Destructible;

                    MapGrid[i, j].RebindIntrinsic(TileFlyweightFactory.GetTile(type, 1, tileWidth, tileHeight));
                }
            }
        }

        public void refreshTileSprites()
        {
            // This method originally reloaded fire sprites.
            // With TileContext.Draw handling Fire drawing, this might be less critical
            // but we still want to keep the logic for any other sprite refreshes if needed.
        }

        public World(int hebergeurWidth, int hebergeurHeight, int TILE_WIDTH, int TILE_HEIGHT, int totalFrameTile)
        {
            CreateWorldGrid(hebergeurWidth, hebergeurHeight, TILE_WIDTH, TILE_HEIGHT, totalFrameTile);
        }

        public World(int hebergeurWidth, int hebergeurHeight, TileContext[,] map)
        {
            MapGrid = map;
        }

        public World()
        {
        }

        /// <summary>
        /// Creates a grid of tiles for the world based on specified dimensions and tile properties.
        /// Uses Flyweight pattern to share identical tile objects (floor, wall, destructible) 
        /// and dramatically reduce memory usage while preserving all existing functionality.
        /// </summary>
        /// <param name="hebergeurWidth">The width of the world in pixels.</param>
        /// <param name="hebergeurHeight">The height of the world in pixels.</param>
        /// <param name="tileWidth">The width of a single tile in pixels.</param>
        /// <param name="tileHeight">The height of a single tile in pixels.</param>
        /// <param name="totalFrameTile">The total number of frames for tile animations.</param>
        public void CreateWorldGrid(
            int hebergeurWidth,
            int hebergeurHeight,
            int tileWidth,
            int tileHeight,
            int totalFrameTile)
        {
            Random r = new Random();
            MapGrid = new TileContext[hebergeurWidth / tileWidth, hebergeurHeight / tileHeight];
            int rows = MapGrid.GetLength(0);
            int cols = MapGrid.GetLength(1);

            for (int i = 0; i < rows; i++) // Row
            {
                for (int j = 0; j < cols; j++) // Column
                {
                    int x = j * tileWidth;
                    int y = i * tileHeight;

                    TileType type;

                    if (j == 0 || j == cols - 1 || i == 0 || i == rows - 1)
                    {
                        type = TileType.Wall;
                    }
                    else if (i % 2 == 0 && j % 2 == 0)
                    {
                        type = TileType.Wall;
                    }
                    else
                    {
                        bool isPlayerSpawn = 
                            (i == 1 && (j == 1 || j == 2)) ||
                            (i == 2 && j == 1) ||
                            (i == rows - 3 && j == cols - 2) ||
                            (i == rows - 2 && (j == cols - 2 || j == cols - 3));

                        if (isPlayerSpawn)
                        {
                            type = TileType.Floor;
                        }
                        else
                        {
                            int rand = r.Next(0, 10);
                            if (rand >= 1)
                            {
                                type = TileType.Destructible;
                            }
                            else
                            {
                                type = TileType.Floor;
                            }
                        }
                    }

                    Tile sharedTile = TileFlyweightFactory.GetTile(type, totalFrameTile, tileWidth, tileHeight);
                    MapGrid[i, j] = new TileContext(sharedTile, x, y);
                }
            }

            // Ensure all four corners and their adjacent tiles are walkable and not destroyable
            // Top-left
            MapGrid[1, 1].Walkable = true;
            MapGrid[1, 1].Destroyable = false;
            MapGrid[1, 2].Walkable = true;
            MapGrid[1, 2].Destroyable = false;
            MapGrid[2, 1].Walkable = true;
            MapGrid[2, 1].Destroyable = false;

            // Top-right
            MapGrid[1, cols - 2].Walkable = true;
            MapGrid[1, cols - 2].Destroyable = false;
            MapGrid[1, cols - 3].Walkable = true;
            MapGrid[1, cols - 3].Destroyable = false;
            MapGrid[2, cols - 2].Walkable = true;
            MapGrid[2, cols - 2].Destroyable = false;

            // Bottom-left
            MapGrid[rows - 2, 1].Walkable = true;
            MapGrid[rows - 2, 1].Destroyable = false;
            MapGrid[rows - 2, 2].Walkable = true;
            MapGrid[rows - 2, 2].Destroyable = false;
            MapGrid[rows - 3, 1].Walkable = true;
            MapGrid[rows - 3, 1].Destroyable = false;

            // Bottom-right
            MapGrid[rows - 2, cols - 2].Walkable = true;
            MapGrid[rows - 2, cols - 2].Destroyable = false;
            MapGrid[rows - 2, cols - 3].Walkable = true;
            MapGrid[rows - 2, cols - 3].Destroyable = false;
            MapGrid[rows - 3, cols - 2].Walkable = true;
            MapGrid[rows - 3, cols - 2].Destroyable = false;
        }
    }
}