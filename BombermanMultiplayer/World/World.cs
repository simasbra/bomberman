using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Media;
using System.Diagnostics;
using BombermanMultiplayer.Iterator;
using BombermanMultiplayer.Flyweight;

namespace BombermanMultiplayer
{
    [Serializable]
    public class World
    {
        public Tile[,] MapGrid;

        [NonSerialized]
        private Image Background_;

        private int _tileWidth;
        private int _tileHeight;

        public IIterator<Tile> GetTileIterator()
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

            for (int i = 0; i < MapGrid.GetLength(0); i++)
            {
                for (int j = 0; j < MapGrid.GetLength(1); j++)
                {
                    MapGrid[i, j].Draw(gr);
                }
            }
        }

        public void LoadBackground(Image background)
        {
            Background = background;
        }

        public void LoadSpriteTile(Image spriteDestroyableTile, Image spriteUndestroyableTile)
        {
            for (int i = 0; i < MapGrid.GetLength(0); i++)
            {
                for (int j = 0; j < MapGrid.GetLength(1); j++)
                {
                    if (MapGrid[i, j].Destroyable)
                    {
                        MapGrid[i, j].LoadSprite(spriteDestroyableTile);
                    }
                    else if (!MapGrid[i, j].Walkable && !MapGrid[i, j].Destroyable)
                    {
                        MapGrid[i, j].LoadSprite(spriteUndestroyableTile);
                    }
                }
            }
        }

        public void refreshTileSprites()
        {
            for (int i = 0; i < MapGrid.GetLength(0); i++)
            {
                for (int j = 0; j < MapGrid.GetLength(1); j++)
                {
                    if (MapGrid[i, j].Walkable && !MapGrid[i, j].Destroyable)
                    {
                        MapGrid[i, j].UnloadSprite();
                    }

                    if (MapGrid[i, j].Fire)
                    {
                        MapGrid[i, j].LoadSprite(Properties.Resources.Fire);
                    }
                    else if (MapGrid[i, j].Walkable && !MapGrid[i, j].Fire)
                    {
                        MapGrid[i, j].UnloadSprite();
                    }
                }
            }
        }

        public World(int hebergeurWidth, int hebergeurHeight, int TILE_WIDTH, int TILE_HEIGHT, int totalFrameTile)
        {
            _tileWidth = TILE_WIDTH;
            _tileHeight = TILE_HEIGHT;
            CreateWorldGrid(hebergeurWidth, hebergeurHeight, TILE_WIDTH, TILE_HEIGHT, totalFrameTile);
        }

        public World(int hebergeurWidth, int hebergeurHeight, Tile[,] map)
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
            _tileWidth = tileWidth;
            _tileHeight = tileHeight;

            TileFlyweightFactory.Instance.Initialize(tileWidth, tileHeight, totalFrameTile);

            Random r = new Random();
            int rand = 0;
            MapGrid = new Tile[hebergeurWidth / tileWidth, hebergeurHeight / tileHeight];
            int rows = MapGrid.GetLength(0);
            int cols = MapGrid.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int x = j * tileWidth;
                    int y = i * tileHeight;

                    TileType tileType;

                    if (j == 0 || j == cols - 1 || i == 0 || i == rows - 1)
                    {
                        tileType = TileType.Wall;
                    }
                    else if (i % 2 == 0 && j % 2 == 0)
                    {
                        tileType = TileType.Wall;
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
                            tileType = TileType.Floor;
                        }
                        else
                        {
                            rand = r.Next(0, 10);
                            if (rand >= 1)
                            {
                                tileType = TileType.Destructible;
                            }
                            else
                            {
                                tileType = TileType.Floor;
                            }
                        }
                    }

                    Tile sharedTile = TileFlyweightFactory.GetTile(tileType);
                    MapGrid[i, j] = new Tile(x, y, totalFrameTile, tileWidth, tileHeight, sharedTile.Walkable,
                        sharedTile.Destroyable);
                    MapGrid[i, j].CasePosition = new int[] { i, j };

                    if (tileType == TileType.Wall)
                    {
                        MapGrid[i, j].LoadSprite(Properties.Resources.BlockNonDestructible);
                    }
                    else if (tileType == TileType.Destructible)
                    {
                        MapGrid[i, j].LoadSprite(Properties.Resources.BlockDestructible);
                    }
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