namespace MysticChronicles.Engine.DB.Tables
{
    public class PartyMembers
    {
        public int GameID { get; set; }

        public int Level { get; set; }

        public string Name { get; set; }

        public int CurrentHP { get; set; }

        public int MaxHP { get; set; }

        public int CurrentMP { get; set; }

        public int MaxMP { get; set; }

        public int Experience { get; set; }

        public int ATK { get; set; }
        
        public int DEF { get; set; }

        public int ACC { get; set; }

        public int STR { get; set; }
    }
}