using System;
using System.Numerics;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using MysticChronicles.Models;
using MysticChronicles.GameEngine;

namespace MysticChronicles
{
    public sealed partial class GamePage : Page
    {
        private GameState gameState;
        private BattleSystem battleSystem;
        private InputManager inputManager;
        private Map currentMap;
        private Character hero;
        private DispatcherTimer gameTimer;

        public GamePage()
        {
            this.InitializeComponent();
            InitializeGame();
            
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(16);
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();

            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
        }

        private void InitializeGame()
        {
            hero = new Character
            {
                Name = "Hero",
                Level = 1,
                MaxHP = 100,
                CurrentHP = 100,
                MaxMP = 50,
                CurrentMP = 50,
                Attack = 20,
                Defense = 15,
                Magic = 18,
                Speed = 12,
                X = 5,
                Y = 5
            };

            currentMap = new Map(20, 15);
            currentMap.GenerateMap();

            gameState = GameState.Exploration;
            battleSystem = new BattleSystem();
            inputManager = new InputManager();

            UpdateUI();
        }

        private void Canvas_CreateResources(CanvasControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
        }

        private void Canvas_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            var session = args.DrawingSession;

            if (gameState == GameState.Exploration)
            {
                DrawExplorationMode(session);
            }
            else if (gameState == GameState.Battle)
            {
                DrawBattleMode(session);
            }
        }

        private void DrawExplorationMode(CanvasDrawingSession session)
        {
            int tileSize = 32;
            int offsetX = 50;
            int offsetY = 50;

            for (int y = 0; y < currentMap.Height; y++)
            {
                for (int x = 0; x < currentMap.Width; x++)
                {
                    var tile = currentMap.GetTile(x, y);
                    Color tileColor;

                    if (tile.Type == TileType.Grass)
                    {
                        tileColor = Colors.Green;
                    }
                    else if (tile.Type == TileType.Water)
                    {
                        tileColor = Colors.Blue;
                    }
                    else if (tile.Type == TileType.Mountain)
                    {
                        tileColor = Colors.Gray;
                    }
                    else if (tile.Type == TileType.Forest)
                    {
                        tileColor = Colors.DarkGreen;
                    }
                    else
                    {
                        tileColor = Colors.Black;
                    }

                    session.FillRectangle(
                        offsetX + x * tileSize,
                        offsetY + y * tileSize,
                        tileSize - 2,
                        tileSize - 2,
                        tileColor);
                }
            }

            session.FillCircle(
                offsetX + hero.X * tileSize + tileSize / 2,
                offsetY + hero.Y * tileSize + tileSize / 2,
                tileSize / 3,
                Colors.Yellow);

            session.DrawText(
                "H",
                offsetX + hero.X * tileSize + tileSize / 2 - 8,
                offsetY + hero.Y * tileSize + tileSize / 2 - 10,
                Colors.Black);
        }

        private void DrawBattleMode(CanvasDrawingSession session)
        {
            session.FillRectangle(0, 0, (float)canvas.ActualWidth, (float)canvas.ActualHeight, Color.FromArgb(255, 20, 20, 40));

            float heroX = 150;
            float heroY = (float)canvas.ActualHeight / 2;
            session.FillCircle(heroX, heroY, 30, Colors.LightBlue);
            session.DrawText("HERO", heroX - 20, heroY + 40, Colors.White);

            if (battleSystem.CurrentEnemy != null)
            {
                float enemyX = (float)canvas.ActualWidth - 150;
                float enemyY = (float)canvas.ActualHeight / 2;
                session.FillCircle(enemyX, enemyY, 35, Colors.Red);
                session.DrawText(battleSystem.CurrentEnemy.Name, enemyX - 30, enemyY + 45, Colors.White);
            }
        }

        private void GameTimer_Tick(object sender, object e)
        {
            canvas.Invalidate();
        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (gameState == GameState.Exploration)
            {
                HandleExplorationInput(args.VirtualKey);
            }
        }

        private void HandleExplorationInput(Windows.System.VirtualKey key)
        {
            int newX = hero.X;
            int newY = hero.Y;

            switch (key)
            {
                case Windows.System.VirtualKey.Up:
                case Windows.System.VirtualKey.W:
                    newY--;
                    break;
                case Windows.System.VirtualKey.Down:
                case Windows.System.VirtualKey.S:
                    newY++;
                    break;
                case Windows.System.VirtualKey.Left:
                case Windows.System.VirtualKey.A:
                    newX--;
                    break;
                case Windows.System.VirtualKey.Right:
                case Windows.System.VirtualKey.D:
                    newX++;
                    break;
            }

            if (currentMap.IsWalkable(newX, newY))
            {
                hero.X = newX;
                hero.Y = newY;

                Random random = new Random();
                if (random.Next(100) < 10)
                {
                    StartBattle();
                }
            }

            UpdateUI();
        }

        private void StartBattle()
        {
            gameState = GameState.Battle;
            battleSystem.StartBattle(hero);
            battleMenu.Visibility = Visibility.Visible;
            UpdateUI();
        }

        private void BtnAttack_Click(object sender, RoutedEventArgs e)
        {
            if (battleSystem.CurrentEnemy == null) return;

            string result = battleSystem.ExecutePlayerAttack();
            txtMessage.Text = result;

            if (battleSystem.CurrentEnemy.CurrentHP <= 0)
            {
                EndBattle(true);
            }
            else
            {
                string enemyAction = battleSystem.ExecuteEnemyTurn();
                txtMessage.Text += "\n" + enemyAction;

                if (hero.CurrentHP <= 0)
                {
                    EndBattle(false);
                }
            }

            UpdateUI();
        }

        private void BtnSkill_Click(object sender, RoutedEventArgs e)
        {
            if (hero.CurrentMP >= 10)
            {
                hero.CurrentMP -= 10;
                int damage = (int)(hero.Magic * 1.5);
                battleSystem.CurrentEnemy.CurrentHP -= damage;
                txtMessage.Text = $"{hero.Name} casts Fire! Deals {damage} damage!";

                if (battleSystem.CurrentEnemy.CurrentHP <= 0)
                {
                    EndBattle(true);
                }
                else
                {
                    string enemyAction = battleSystem.ExecuteEnemyTurn();
                    txtMessage.Text += "\n" + enemyAction;

                    if (hero.CurrentHP <= 0)
                    {
                        EndBattle(false);
                    }
                }
            }
            else
            {
                txtMessage.Text = "Not enough MP!";
            }

            UpdateUI();
        }

        private void BtnItem_Click(object sender, RoutedEventArgs e)
        {
            hero.CurrentHP = Math.Min(hero.CurrentHP + 30, hero.MaxHP);
            txtMessage.Text = $"{hero.Name} uses Potion! Restored 30 HP!";
            
            string enemyAction = battleSystem.ExecuteEnemyTurn();
            txtMessage.Text += "\n" + enemyAction;

            if (hero.CurrentHP <= 0)
            {
                EndBattle(false);
            }

            UpdateUI();
        }

        private void BtnDefend_Click(object sender, RoutedEventArgs e)
        {
            int originalDefense = hero.Defense;
            hero.Defense = (int)(hero.Defense * 1.5);
            
            string enemyAction = battleSystem.ExecuteEnemyTurn();
            txtMessage.Text = $"{hero.Name} defends!\n" + enemyAction;
            
            hero.Defense = originalDefense;

            if (hero.CurrentHP <= 0)
            {
                EndBattle(false);
            }

            UpdateUI();
        }

        private void EndBattle(bool victory)
        {
            gameState = GameState.Exploration;
            battleMenu.Visibility = Visibility.Collapsed;

            if (victory)
            {
                int expGained = 50;
                txtMessage.Text = $"Victory! Gained {expGained} EXP!";
                hero.CurrentHP = Math.Min(hero.CurrentHP + 20, hero.MaxHP);
                hero.CurrentMP = Math.Min(hero.CurrentMP + 10, hero.MaxMP);
            }
            else
            {
                txtMessage.Text = "Defeated! Game Over!";
                hero.CurrentHP = hero.MaxHP;
                hero.CurrentMP = hero.MaxMP;
            }

            UpdateUI();
        }

        private void UpdateUI()
        {
            txtGameState.Text = $"Mode: {gameState}";
            txtHeroStats.Text = $"{hero.Name} - Lv.{hero.Level} | HP: {hero.CurrentHP}/{hero.MaxHP} | MP: {hero.CurrentMP}/{hero.MaxMP}";

            if (gameState == GameState.Battle && battleSystem.CurrentEnemy != null)
            {
                txtMessage.Text = $"Enemy: {battleSystem.CurrentEnemy.Name} | HP: {battleSystem.CurrentEnemy.CurrentHP}/{battleSystem.CurrentEnemy.MaxHP}";
            }
        }
    }
}
