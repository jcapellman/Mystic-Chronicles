using System;

namespace MysticChronicles.Models
{
    public class Enemy
    {
        public string Name { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int ExpReward { get; set; }
        public int GoldReward { get; set; }

        public bool IsAlive => CurrentHP > 0;

        public static Enemy CreateRandomEnemy(int playerLevel)
        {
            Random random = new Random();
            string[] enemyTypes = { "Goblin", "Slime", "Bat", "Wolf", "Skeleton" };
            string enemyName = enemyTypes[random.Next(enemyTypes.Length)];

            int levelVariance = random.Next(-1, 2);
            int enemyLevel = Math.Max(1, playerLevel + levelVariance);

            return new Enemy
            {
                Name = enemyName,
                MaxHP = 30 + (enemyLevel * 10),
                CurrentHP = 30 + (enemyLevel * 10),
                Attack = 10 + (enemyLevel * 2),
                Defense = 5 + enemyLevel,
                ExpReward = 20 + (enemyLevel * 5),
                GoldReward = 10 + (enemyLevel * 3)
            };
        }

        public int CalculateDamage(int targetDefense)
        {
            int baseDamage = Attack - (targetDefense / 2);
            return baseDamage > 0 ? baseDamage : 1;
        }
    }
}
