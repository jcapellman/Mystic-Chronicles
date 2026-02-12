namespace MysticChronicles.Models
{
    public enum TileType
    {
        Grass,
        Water,
        Mountain,
        Forest,
        Town
    }

    public class Tile
    {
        public TileType Type { get; set; }
        public bool IsWalkable { get; set; }
        public int EncounterRate { get; set; }

        public Tile(TileType type)
        {
            Type = type;

            if (type == TileType.Grass || type == TileType.Forest || type == TileType.Town)
            {
                IsWalkable = true;
            }
            else if (type == TileType.Water || type == TileType.Mountain)
            {
                IsWalkable = false;
            }
            else
            {
                IsWalkable = true;
            }

            if (type == TileType.Grass)
            {
                EncounterRate = 10;
            }
            else if (type == TileType.Forest)
            {
                EncounterRate = 15;
            }
            else
            {
                EncounterRate = 0;
            }
        }
    }
}
