namespace MysticChronicles.Models
{
    public class Character
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public int MaxMP { get; set; }
        public int CurrentMP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Magic { get; set; }
        public int Speed { get; set; }
        public int Experience { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public bool IsAlive => CurrentHP > 0;

        public void LevelUp()
        {
            Level++;
            MaxHP += 10;
            MaxMP += 5;
            Attack += 3;
            Defense += 2;
            Magic += 2;
            Speed += 1;
            CurrentHP = MaxHP;
            CurrentMP = MaxMP;
        }

        public int CalculateDamage(int targetDefense)
        {
            int baseDamage = Attack - (targetDefense / 2);
            return baseDamage > 0 ? baseDamage : 1;
        }
    }
}
