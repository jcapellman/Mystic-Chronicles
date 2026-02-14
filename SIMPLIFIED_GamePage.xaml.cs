// Simplified Mystic Chronicles GamePage - Just sprite rendering!
// All RPG logic is handled by GORE.Core.Pages.BaseGamePage

using System;
using Windows.UI;
using Windows.UI.Xaml;
using Microsoft.Graphics.Canvas;
using GORE.Core.Pages;
using GORE.Core.Models;

namespace MysticChronicles
{
    /// <summary>
    /// Mystic Chronicles game page.
    /// Inherits all RPG functionality from GORE.Core.Pages.BaseGamePage.
    /// Only implements game-specific sprite rendering.
    /// </summary>
    public sealed partial class GamePage : BaseGamePage
    {
        public GamePage()
        {
            this.InitializeComponent();
        }

        // Implement exploration rendering (map + hero)
        protected override void DrawExplorationMode(CanvasDrawingSession session)
        {
            int tileSize = 32;
            int offsetX = 50;
            int offsetY = 50;

            // Draw tilemap
            for (int y = 0; y < currentMap.Height; y++)
            {
                for (int x = 0; x < currentMap.Width; x++)
                {
                    var tile = currentMap.GetTile(x, y);
                    Color tileColor = tile.Type switch
                    {
                        TileType.Grass => Colors.Green,
                        TileType.Water => Colors.Blue,
                        TileType.Mountain => Colors.Gray,
                        TileType.Forest => Colors.DarkGreen,
                        _ => Colors.Black
                    };

                    session.FillRectangle(
                        offsetX + x * tileSize,
                        offsetY + y * tileSize,
                        tileSize - 2,
                        tileSize - 2,
                        tileColor);
                }
            }

            // Draw hero
            session.FillCircle(
                offsetX + hero.X * tileSize + tileSize / 2,
                offsetY + hero.Y * tileSize + tileSize / 2,
                tileSize / 3,
                Colors.Yellow);

            session.DrawText("H",
                offsetX + hero.X * tileSize + tileSize / 2 - 8,
                offsetY + hero.Y * tileSize + tileSize / 2 - 10,
                Colors.Black);
        }

        // Implement battle rendering (background + sprites)
        protected override void DrawBattleMode(CanvasDrawingSession session)
        {
            float width = (float)canvas.ActualWidth;
            float height = (float)canvas.ActualHeight;

            // Draw background (gradient fallback if no image)
            DrawBattleBackground(session, width, height);

            // Hero on RIGHT (FF6 style)
            float heroX = width - 200;
            float heroY = height / 2 + 50;
            session.FillEllipse(heroX, heroY + 60, 35, 12, Color.FromArgb(100, 0, 0, 0));
            DrawHeroSprite(session, heroX, heroY);

            // Enemy on LEFT
            if (battleSystem?.CurrentEnemy != null)
            {
                float enemyX = 250;
                float enemyY = height / 2 - 50;
                session.FillEllipse(enemyX, enemyY + 70, 45, 15, Color.FromArgb(100, 0, 0, 0));
                DrawEnemySprite(session, enemyX, enemyY, battleSystem.CurrentEnemy.Name);

                // Enemy HP bar
                DrawEnemyHPBar(session, enemyX, enemyY);
            }
        }

        // Mystic Chronicles specific: FF6-style warrior sprite
        protected override void DrawHeroSprite(CanvasDrawingSession session, float x, float y)
        {
            // Legs
            session.FillRectangle(x - 12, y + 35, 10, 18, Color.FromArgb(255, 80, 80, 120));
            session.FillRectangle(x + 2, y + 35, 10, 18, Color.FromArgb(255, 80, 80, 120));

            // Body/Armor
            session.FillRectangle(x - 18, y + 10, 36, 28, Color.FromArgb(255, 120, 150, 200));
            session.FillRectangle(x - 15, y + 12, 30, 22, Color.FromArgb(255, 90, 120, 180));
            session.FillRectangle(x - 18, y + 30, 36, 5, Color.FromArgb(255, 100, 70, 40)); // Belt

            // Arms
            session.FillRectangle(x - 22, y + 15, 8, 18, Color.FromArgb(255, 100, 130, 180));
            session.FillRectangle(x + 14, y + 15, 8, 18, Color.FromArgb(255, 100, 130, 180));

            // Shoulders
            session.FillEllipse(x - 18, y + 15, 6, 6, Color.FromArgb(255, 140, 160, 200));
            session.FillEllipse(x + 18, y + 15, 6, 6, Color.FromArgb(255, 140, 160, 200));

            // Head
            session.FillEllipse(x, y - 5, 14, 16, Color.FromArgb(255, 255, 220, 180));
            session.FillRectangle(x - 14, y - 18, 28, 14, Color.FromArgb(255, 80, 60, 40)); // Hair
            session.FillRectangle(x - 10, y - 20, 20, 4, Color.FromArgb(255, 80, 60, 40));

            // Eyes
            session.FillRectangle(x - 7, y - 5, 3, 3, Color.FromArgb(255, 50, 50, 50));
            session.FillRectangle(x + 4, y - 5, 3, 3, Color.FromArgb(255, 50, 50, 50));

            // Sword
            session.FillRectangle(x + 20, y + 15, 3, 35, Color.FromArgb(255, 180, 180, 200));
            session.FillRectangle(x + 18, y + 48, 7, 4, Color.FromArgb(255, 120, 100, 60));
            session.FillRectangle(x + 16, y + 45, 11, 3, Color.FromArgb(255, 200, 180, 100));

            // Shield
            session.FillEllipse(x - 26, y + 20, 8, 10, Color.FromArgb(255, 150, 150, 180));
            session.FillEllipse(x - 26, y + 20, 5, 7, Color.FromArgb(255, 100, 100, 140));

            // Name
            session.DrawText(hero.Name, x - 30, y + 60, Colors.White,
                new Microsoft.Graphics.Canvas.Text.CanvasTextFormat 
                { 
                    FontSize = 16, 
                    FontWeight = Windows.UI.Text.FontWeights.Bold 
                });
        }

        // Mystic Chronicles specific: Monster sprite
        protected override void DrawEnemySprite(CanvasDrawingSession session, float x, float y, string enemyName)
        {
            // Monster body
            session.FillEllipse(x, y + 10, 50, 55, Color.FromArgb(255, 140, 50, 50));
            session.FillEllipse(x, y, 40, 45, Color.FromArgb(255, 180, 70, 70));

            // Horns
            DrawHorn(session, x - 25, y - 20, x - 15, y - 45, x - 10, y - 20);
            DrawHorn(session, x + 10, y - 20, x + 15, y - 45, x + 25, y - 20);

            // Eyes (glowing)
            session.FillEllipse(x - 18, y - 10, 12, 14, Color.FromArgb(255, 255, 50, 50));
            session.FillEllipse(x - 18, y - 10, 8, 10, Color.FromArgb(255, 255, 200, 100));
            session.FillEllipse(x + 18, y - 10, 12, 14, Color.FromArgb(255, 255, 50, 50));
            session.FillEllipse(x + 18, y - 10, 8, 10, Color.FromArgb(255, 255, 200, 100));

            // Mouth
            session.FillRectangle(x - 15, y + 15, 30, 8, Color.FromArgb(255, 50, 20, 20));

            // Fangs
            DrawFang(session, x - 10, y + 15, x - 7, y + 25, x - 4, y + 15);
            DrawFang(session, x + 4, y + 15, x + 7, y + 25, x + 10, y + 15);

            // Arms
            session.FillEllipse(x - 45, y + 20, 15, 25, Color.FromArgb(255, 120, 60, 60));
            DrawClaw(session, x - 50, y + 35, x - 55, y + 45, x - 45, y + 40);
            
            session.FillEllipse(x + 45, y + 20, 15, 25, Color.FromArgb(255, 120, 60, 60));
            DrawClaw(session, x + 50, y + 35, x + 55, y + 45, x + 45, y + 40);

            // Scales
            for (int i = 0; i < 5; i++)
            {
                session.DrawEllipse(x - 15 + (i * 8), y + 5 + (i % 2) * 8, 4, 4,
                    Color.FromArgb(150, 100, 30, 30), 2);
            }

            // Name
            session.DrawText(enemyName, x - 40, y - 90, Colors.Yellow,
                new Microsoft.Graphics.Canvas.Text.CanvasTextFormat
                {
                    FontSize = 18,
                    FontWeight = Windows.UI.Text.FontWeights.Bold
                });
        }

        // Helper methods for drawing geometry
        private void DrawHorn(CanvasDrawingSession session, float x1, float y1, float x2, float y2, float x3, float y3)
        {
            var horn = new Microsoft.Graphics.Canvas.Geometry.CanvasPathBuilder(session);
            horn.BeginFigure(x1, y1);
            horn.AddLine(x2, y2);
            horn.AddLine(x3, y3);
            horn.EndFigure(Microsoft.Graphics.Canvas.Geometry.CanvasFigureLoop.Closed);
            using (var geometry = Microsoft.Graphics.Canvas.Geometry.CanvasGeometry.CreatePath(horn))
            {
                session.FillGeometry(geometry, Color.FromArgb(255, 100, 40, 40));
            }
        }

        private void DrawFang(CanvasDrawingSession session, float x1, float y1, float x2, float y2, float x3, float y3)
        {
            var fang = new Microsoft.Graphics.Canvas.Geometry.CanvasPathBuilder(session);
            fang.BeginFigure(x1, y1);
            fang.AddLine(x2, y2);
            fang.AddLine(x3, y3);
            fang.EndFigure(Microsoft.Graphics.Canvas.Geometry.CanvasFigureLoop.Closed);
            using (var geometry = Microsoft.Graphics.Canvas.Geometry.CanvasGeometry.CreatePath(fang))
            {
                session.FillGeometry(geometry, Colors.White);
            }
        }

        private void DrawClaw(CanvasDrawingSession session, float x1, float y1, float x2, float y2, float x3, float y3)
        {
            var claw = new Microsoft.Graphics.Canvas.Geometry.CanvasPathBuilder(session);
            claw.BeginFigure(x1, y1);
            claw.AddLine(x2, y2);
            claw.AddLine(x3, y3);
            claw.EndFigure(Microsoft.Graphics.Canvas.Geometry.CanvasFigureLoop.Closed);
            using (var geometry = Microsoft.Graphics.Canvas.Geometry.CanvasGeometry.CreatePath(claw))
            {
                session.FillGeometry(geometry, Color.FromArgb(255, 200, 200, 200));
            }
        }

        private void DrawBattleBackground(CanvasDrawingSession session, float width, float height)
        {
            // Simple gradient background (can be enhanced with config)
            for (int y = 0; y < height; y++)
            {
                float ratio = y / height;
                byte r = (byte)(20 + ratio * 40);
                byte g = (byte)(10 + ratio * 30);
                byte b = (byte)(60 + ratio * 80);
                session.DrawLine(0, y, width, y, Color.FromArgb(255, r, g, b));
            }
        }

        private void DrawEnemyHPBar(CanvasDrawingSession session, float x, float y)
        {
            float barWidth = 100;
            float barHeight = 8;
            float barX = x - barWidth / 2;
            float barY = y + 75;

            session.FillRectangle(barX, barY, barWidth, barHeight, Color.FromArgb(255, 50, 50, 50));

            float hpPercent = (float)battleSystem.CurrentEnemy.CurrentHP / battleSystem.CurrentEnemy.MaxHP;
            Color hpColor = hpPercent > 0.5f ? Colors.Green : (hpPercent > 0.25f ? Colors.Yellow : Colors.Red);
            session.FillRectangle(barX, barY, barWidth * hpPercent, barHeight, hpColor);

            session.DrawRectangle(barX, barY, barWidth, barHeight, Colors.White, 1);
        }

        // UI update hooks (connect to XAML controls)
        protected override void UpdateUI()
        {
            txtHeroName.Text = hero.Name;
            txtHeroLevel.Text = hero.Level.ToString();
            txtHeroHP.Text = $"{hero.CurrentHP}/{hero.MaxHP}";
            txtHeroMP.Text = $"{hero.CurrentMP}/{hero.MaxMP}";

            double hpPercent = (double)hero.CurrentHP / hero.MaxHP;
            hpBar.Width = 250 * hpPercent;
            hpBar.Background = new Windows.UI.Xaml.Media.SolidColorBrush(
                hpPercent > 0.5 ? Colors.Green : (hpPercent > 0.25 ? Colors.Yellow : Colors.Red));

            mpBar.Width = 250 * ((double)hero.CurrentMP / hero.MaxMP);

            messageBox.Visibility = gameState == GameState.Battle ? Visibility.Visible : Visibility.Collapsed;
        }

        protected override void OnBattleMenuCursorChanged()
        {
            cursorAttack.Visibility = battleMenuSelection == 0 ? Visibility.Visible : Visibility.Collapsed;
            cursorMagic.Visibility = battleMenuSelection == 1 ? Visibility.Visible : Visibility.Collapsed;
            cursorItem.Visibility = battleMenuSelection == 2 ? Visibility.Visible : Visibility.Collapsed;
            cursorDefend.Visibility = battleMenuSelection == 3 ? Visibility.Visible : Visibility.Collapsed;
        }

        protected override void OnInGameMenuCursorChanged()
        {
            cursorResume.Visibility = inGameMenuSelection == 0 ? Visibility.Visible : Visibility.Collapsed;
            cursorSave.Visibility = inGameMenuSelection == 1 ? Visibility.Visible : Visibility.Collapsed;
            cursorMainMenu.Visibility = inGameMenuSelection == 2 ? Visibility.Visible : Visibility.Collapsed;
        }

        protected override void OnBattleStarted()
        {
            battleMenu.Visibility = Visibility.Visible;
        }

        protected override void OnBattleEnded()
        {
            battleMenu.Visibility = Visibility.Collapsed;
        }

        protected override void OnBattleMessage(string message)
        {
            txtBattleMessage.Text = message;
        }

        protected override void OnMenuToggled(bool isOpen)
        {
            inGameMenu.Visibility = isOpen ? Visibility.Visible : Visibility.Collapsed;
        }

        protected override void OnResumeGame()
        {
            ToggleMenu();
        }

        protected override async void OnSaveGame()
        {
            if (isDialogOpen) return;

            isMenuOpen = false;
            inGameMenu.Visibility = Visibility.Collapsed;
            isDialogOpen = true;

            bool success = await GORE.Core.Services.SaveGameManager.SaveGame(hero);

            var dialog = new ContentDialog
            {
                Title = success ? "Game Saved" : "Save Failed",
                Content = success ? "Your progress has been saved." : "Failed to save game.",
                CloseButtonText = "OK"
            };

            await dialog.ShowAsync();
            isDialogOpen = false;
        }

        protected override async void OnReturnToMainMenu()
        {
            if (isDialogOpen) return;

            isMenuOpen = false;
            inGameMenu.Visibility = Visibility.Collapsed;
            isDialogOpen = true;

            var dialog = new ContentDialog
            {
                Title = "Return to Main Menu?",
                Content = "Any unsaved progress will be lost.",
                PrimaryButtonText = "Yes",
                CloseButtonText = "No"
            };

            var result = await dialog.ShowAsync();
            isDialogOpen = false;

            if (result == ContentDialogResult.Primary)
            {
                this.Frame.Navigate(typeof(MainMenuPage));
            }
        }

        // Canvas event handlers
        private void Canvas_Draw(CanvasControl sender, Microsoft.Graphics.Canvas.UI.CanvasDrawEventArgs args)
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

        private void GameTimer_Tick(object sender, object e)
        {
            OnGameTimerTick(sender, e);
            canvas.Invalidate();
        }
    }
}
