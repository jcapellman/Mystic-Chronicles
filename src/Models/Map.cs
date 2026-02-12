using System;

namespace MysticChronicles.Models
{
    public class Map
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        private Tile[,] tiles;

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            tiles = new Tile[width, height];
        }

        public void GenerateMap()
        {
            Random random = new Random();

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int rand = random.Next(100);
                    TileType type;

                    if (rand < 60)
                        type = TileType.Grass;
                    else if (rand < 75)
                        type = TileType.Forest;
                    else if (rand < 85)
                        type = TileType.Water;
                    else
                        type = TileType.Mountain;

                    tiles[x, y] = new Tile(type);
                }
            }

            tiles[5, 5] = new Tile(TileType.Grass);
        }

        public Tile GetTile(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return null;

            return tiles[x, y];
        }

        public bool IsWalkable(int x, int y)
        {
            var tile = GetTile(x, y);
            return tile != null && tile.IsWalkable;
        }
    }
}
