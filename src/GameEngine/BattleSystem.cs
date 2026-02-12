using System;
using MysticChronicles.Models;

namespace MysticChronicles.GameEngine
{
    public class BattleSystem
    {
        public Enemy CurrentEnemy { get; private set; }
        private Character player;
        private Random random;

        public BattleSystem()
        {
            random = new Random();
        }

        public void StartBattle(Character hero)
        {
            player = hero;
            CurrentEnemy = Enemy.CreateRandomEnemy(hero.Level);
        }

        public string ExecutePlayerAttack()
        {
            if (CurrentEnemy == null || !CurrentEnemy.IsAlive)
                return "No enemy to attack!";

            int damage = player.CalculateDamage(CurrentEnemy.Defense);
            int variance = random.Next(-2, 3);
            damage = Math.Max(1, damage + variance);

            CurrentEnemy.CurrentHP -= damage;

            return $"{player.Name} attacks {CurrentEnemy.Name} for {damage} damage!";
        }

        public string ExecuteEnemyTurn()
        {
            if (CurrentEnemy == null || !CurrentEnemy.IsAlive)
                return "";

            int damage = CurrentEnemy.CalculateDamage(player.Defense);
            int variance = random.Next(-2, 3);
            damage = Math.Max(1, damage + variance);

            player.CurrentHP -= damage;

            return $"{CurrentEnemy.Name} attacks {player.Name} for {damage} damage!";
        }

        public bool IsBattleOver()
        {
            return !player.IsAlive || !CurrentEnemy.IsAlive;
        }

        public bool IsPlayerVictorious()
        {
            return player.IsAlive && !CurrentEnemy.IsAlive;
        }
    }
}
