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

namespace BombermanMultiplayer
{
    [Serializable]
    public class World
    {
        public Tile[,] MapGrid;

        [NonSerialized]
        private Image Background_;

        /// <summary>
        /// Returns an iterator for traversing all tiles in the map grid
        /// </summary>
        /// <returns>Tile iterator over MapGrid</returns>
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
            for (int i = 0; i < MapGrid.GetLength(0); i++) //Ligne
            {
                for (int j = 0; j < MapGrid.GetLength(1); j++) //Colonne
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
            for (int i = 0; i < MapGrid.GetLength(0); i++) //Ligne
            {
                for (int j = 0; j < MapGrid.GetLength(1); j++) //Colonne
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
            int rand = 0;
            MapGrid = new Tile[hebergeurWidth / tileWidth, hebergeurHeight / tileHeight];

            int rows = MapGrid.GetLength(0);
            int cols = MapGrid.GetLength(1);

            for (int i = 0; i < rows; i++) // Row
            {
                for (int j = 0; j < cols; j++) // Column
                {
                    rand = r.Next(0, 10);

                    if (j == 0 || j == cols - 1 || i == 0 || i == rows - 1)
                    {
                        MapGrid[i, j] = new Tile(j * tileWidth, i * tileHeight, totalFrameTile, tileWidth, tileHeight, false, false);
                    }
                    else
                    {
                        if (i % 2 == 0 && j % 2 == 0)
                        {
                            MapGrid[i, j] = new Tile(j * tileWidth, i * tileHeight, totalFrameTile, tileWidth, tileHeight, false, false);
                        }
                        else
                        {
                            if ((i == 1 && (j == 1 || j == 2))
                                || (i == 2 && j == 1)
                                || (i == rows - 3 && j == cols - 2)
                                || (i == rows - 2 && (j == cols - 2 || j == cols - 3)))
                            {
                                MapGrid[i, j] = new Tile(j * tileWidth, i * tileHeight, totalFrameTile, tileWidth, tileHeight, true, false);
                            }
                            else if (rand >= 1)
                            {
                                MapGrid[i, j] = new Tile(j * tileWidth, i * tileHeight, totalFrameTile, tileWidth, tileHeight, false, true);
                            }
                            else
                            {
                                MapGrid[i, j] = new Tile(j * tileWidth, i * tileHeight, totalFrameTile, tileWidth, tileHeight, true, false);
                            }
                        }
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

        /// <summary>
        /// Regenerate the map with new random destructible blocks
        /// Note: Sprites must be reloaded after this by calling loadSpriteTile()
        /// </summary>
        public void RegenerateMap()
        {
            if (MapGrid == null || MapGrid.Length == 0)
                return;

            int rows = MapGrid.GetLength(0);
            int cols = MapGrid.GetLength(1);
            int tileWidth = MapGrid[0, 0].Source.Width;
            int tileHeight = MapGrid[0, 0].Source.Height;

            // Regenerate the grid with new random destructible blocks
            CreateWorldGrid(cols * tileWidth, rows * tileHeight, tileWidth, tileHeight, 1);
        }




    }
}
