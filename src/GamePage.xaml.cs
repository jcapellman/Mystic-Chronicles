using System;
using System.Numerics;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using MysticChronicles.Models;
using MysticChronicles.GameEngine;
using MysticChronicles.Services;

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
        private bool isMenuOpen = false;
        private int battleMenuSelection = 0;
        private const int BattleMenuItemCount = 4;
        private int inGameMenuSelection = 0;
        private const int InGameMenuItemCount = 3;
        private bool isDialogOpen = false;
        private Microsoft.Graphics.Canvas.CanvasBitmap battleBackgroundBitmap;

        // Cloud/Mist animation
        private float cloudOffset1 = 0;
        private float cloudOffset2 = 0;
        private float mistOffset = 0;
        private float animationTime = 0;

        // Full screen mode
        private bool isFullScreen = false;

        public GamePage()
        {
            this.InitializeComponent();

            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(16);
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
        }

        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;

            // Enter full screen mode and hide cursor for immersive gameplay
            EnterFullScreenMode();
            HideCursor();

            if (e.Parameter is string heroName)
            {
                InitializeNewGame(heroName);
            }
            else if (e.Parameter is SaveData saveData)
            {
                LoadGame(saveData);
            }
            else
            {
                InitializeNewGame("Hero");
            }
        }

        protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;
            gameTimer.Stop();

            // Restore cursor and exit full screen when leaving game
            ShowCursor();
            ExitFullScreenMode();
        }

        private void InitializeNewGame(string heroName)
        {
            hero = new Character
            {
                Name = heroName,
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

            MusicManager.PlayMusic(MusicTrack.Exploration);
            UpdateUI();
        }

        private void LoadGame(SaveData saveData)
        {
            hero = saveData.ToCharacter();

            currentMap = new Map(20, 15);
            currentMap.GenerateMap();

            gameState = GameState.Exploration;
            battleSystem = new BattleSystem();
            inputManager = new InputManager();

            MusicManager.PlayMusic(MusicTrack.Exploration);
            UpdateUI();
        }

        private async void Canvas_CreateResources(CanvasControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(LoadBattleBackgroundAsync(sender).AsAsyncAction());
        }

        private async System.Threading.Tasks.Task LoadBattleBackgroundAsync(CanvasControl sender)
        {
            try
            {
                var uri = new Uri("ms-appx:///Assets/BattleBackgrounds/City.png");
                battleBackgroundBitmap = await Microsoft.Graphics.Canvas.CanvasBitmap.LoadAsync(sender, uri);
            }
            catch (Exception)
            {
                // If image fails to load, battleBackgroundBitmap will remain null
                // and we'll fall back to gradient backgrounds
            }
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
            DrawBattleBackground(session);

            float width = (float)canvas.ActualWidth;
            float height = (float)canvas.ActualHeight;

            // FF6 Style: Hero on the RIGHT side
            float heroX = width - 200;
            float heroY = height / 2 + 50;

            // Draw hero platform/shadow
            session.FillEllipse(heroX, heroY + 60, 35, 12, Color.FromArgb(100, 0, 0, 0));

            // Draw FF6-style hero sprite
            DrawHeroSprite(session, heroX, heroY);

            // FF6 Style: Enemy on the LEFT side
            if (battleSystem.CurrentEnemy != null)
            {
                float enemyX = 250;
                float enemyY = height / 2 - 50;

                // Draw enemy platform/shadow
                session.FillEllipse(enemyX, enemyY + 70, 45, 15, Color.FromArgb(100, 0, 0, 0));

                // Draw FF6-style enemy sprite
                DrawEnemySprite(session, enemyX, enemyY, battleSystem.CurrentEnemy.Name);

                // Enemy name above
                session.DrawText(battleSystem.CurrentEnemy.Name, enemyX - 40, enemyY - 90, Colors.Yellow,
                    new Microsoft.Graphics.Canvas.Text.CanvasTextFormat { FontSize = 18, FontWeight = Windows.UI.Text.FontWeights.Bold });

                // Enemy HP bar (FF6 style - above enemy)
                float barWidth = 100;
                float barHeight = 8;
                float barX = enemyX - barWidth / 2;
                float barY = enemyY + 75;

                // HP bar background
                session.FillRectangle(barX, barY, barWidth, barHeight, Color.FromArgb(255, 50, 50, 50));

                // HP bar fill
                float hpPercent = (float)battleSystem.CurrentEnemy.CurrentHP / battleSystem.CurrentEnemy.MaxHP;
                Color hpColor = hpPercent > 0.5f ? Colors.Green : (hpPercent > 0.25f ? Colors.Yellow : Colors.Red);
                session.FillRectangle(barX, barY, barWidth * hpPercent, barHeight, hpColor);

                // HP bar border
                session.DrawRectangle(barX, barY, barWidth, barHeight, Colors.White, 1);
            }
        }

        private void DrawHeroSprite(CanvasDrawingSession session, float x, float y)
        {
            // FF6-style warrior sprite (pixel art style)

            // Legs
            session.FillRectangle(x - 12, y + 35, 10, 18, Color.FromArgb(255, 80, 80, 120)); // Left leg
            session.FillRectangle(x + 2, y + 35, 10, 18, Color.FromArgb(255, 80, 80, 120)); // Right leg

            // Body/Armor
            session.FillRectangle(x - 18, y + 10, 36, 28, Color.FromArgb(255, 120, 150, 200)); // Torso
            session.FillRectangle(x - 15, y + 12, 30, 22, Color.FromArgb(255, 90, 120, 180)); // Armor detail

            // Belt
            session.FillRectangle(x - 18, y + 30, 36, 5, Color.FromArgb(255, 100, 70, 40));

            // Arms
            session.FillRectangle(x - 22, y + 15, 8, 18, Color.FromArgb(255, 100, 130, 180)); // Left arm
            session.FillRectangle(x + 14, y + 15, 8, 18, Color.FromArgb(255, 100, 130, 180)); // Right arm

            // Shoulders
            session.FillEllipse(x - 18, y + 15, 6, 6, Color.FromArgb(255, 140, 160, 200));
            session.FillEllipse(x + 18, y + 15, 6, 6, Color.FromArgb(255, 140, 160, 200));

            // Head
            session.FillEllipse(x, y - 5, 14, 16, Color.FromArgb(255, 255, 220, 180)); // Face

            // Hair
            session.FillRectangle(x - 14, y - 18, 28, 14, Color.FromArgb(255, 80, 60, 40)); // Hair
            session.FillRectangle(x - 10, y - 20, 20, 4, Color.FromArgb(255, 80, 60, 40)); // Hair top

            // Eyes
            session.FillRectangle(x - 7, y - 5, 3, 3, Color.FromArgb(255, 50, 50, 50));
            session.FillRectangle(x + 4, y - 5, 3, 3, Color.FromArgb(255, 50, 50, 50));

            // Sword (held in right arm)
            session.FillRectangle(x + 20, y + 15, 3, 35, Color.FromArgb(255, 180, 180, 200)); // Blade
            session.FillRectangle(x + 18, y + 48, 7, 4, Color.FromArgb(255, 120, 100, 60)); // Handle
            session.FillRectangle(x + 16, y + 45, 11, 3, Color.FromArgb(255, 200, 180, 100)); // Guard

            // Shield (on left arm)
            session.FillEllipse(x - 26, y + 20, 8, 10, Color.FromArgb(255, 150, 150, 180));
            session.FillEllipse(x - 26, y + 20, 5, 7, Color.FromArgb(255, 100, 100, 140));

            // Name below
            session.DrawText(hero.Name, x - 30, y + 60, Colors.White,
                new Microsoft.Graphics.Canvas.Text.CanvasTextFormat { FontSize = 16, FontWeight = Windows.UI.Text.FontWeights.Bold });
        }

        private void DrawEnemySprite(CanvasDrawingSession session, float x, float y, string enemyName)
        {
            // FF6-style monster sprite (menacing creature)

            // Monster body (larger, menacing)
            session.FillEllipse(x, y + 10, 50, 55, Color.FromArgb(255, 140, 50, 50)); // Main body
            session.FillEllipse(x, y, 40, 45, Color.FromArgb(255, 180, 70, 70)); // Upper body

            // Spikes/horns on top (left horn)
            var leftHorn = new Microsoft.Graphics.Canvas.Geometry.CanvasPathBuilder(session);
            leftHorn.BeginFigure(x - 25, y - 20);
            leftHorn.AddLine(x - 15, y - 45);
            leftHorn.AddLine(x - 10, y - 20);
            leftHorn.EndFigure(Microsoft.Graphics.Canvas.Geometry.CanvasFigureLoop.Closed);
            using (var geometry = Microsoft.Graphics.Canvas.Geometry.CanvasGeometry.CreatePath(leftHorn))
            {
                session.FillGeometry(geometry, Color.FromArgb(255, 100, 40, 40));
            }

            // Right horn
            var rightHorn = new Microsoft.Graphics.Canvas.Geometry.CanvasPathBuilder(session);
            rightHorn.BeginFigure(x + 10, y - 20);
            rightHorn.AddLine(x + 15, y - 45);
            rightHorn.AddLine(x + 25, y - 20);
            rightHorn.EndFigure(Microsoft.Graphics.Canvas.Geometry.CanvasFigureLoop.Closed);
            using (var geometry = Microsoft.Graphics.Canvas.Geometry.CanvasGeometry.CreatePath(rightHorn))
            {
                session.FillGeometry(geometry, Color.FromArgb(255, 100, 40, 40));
            }

            // Eyes (glowing)
            session.FillEllipse(x - 18, y - 10, 12, 14, Color.FromArgb(255, 255, 50, 50)); // Left eye
            session.FillEllipse(x - 18, y - 10, 8, 10, Color.FromArgb(255, 255, 200, 100)); // Eye glow

            session.FillEllipse(x + 18, y - 10, 12, 14, Color.FromArgb(255, 255, 50, 50)); // Right eye
            session.FillEllipse(x + 18, y - 10, 8, 10, Color.FromArgb(255, 255, 200, 100)); // Eye glow

            // Mouth/fangs
            session.FillRectangle(x - 15, y + 15, 30, 8, Color.FromArgb(255, 50, 20, 20)); // Mouth

            // Left fang
            var leftFang = new Microsoft.Graphics.Canvas.Geometry.CanvasPathBuilder(session);
            leftFang.BeginFigure(x - 10, y + 15);
            leftFang.AddLine(x - 7, y + 25);
            leftFang.AddLine(x - 4, y + 15);
            leftFang.EndFigure(Microsoft.Graphics.Canvas.Geometry.CanvasFigureLoop.Closed);
            using (var geometry = Microsoft.Graphics.Canvas.Geometry.CanvasGeometry.CreatePath(leftFang))
            {
                session.FillGeometry(geometry, Colors.White);
            }

            // Right fang
            var rightFang = new Microsoft.Graphics.Canvas.Geometry.CanvasPathBuilder(session);
            rightFang.BeginFigure(x + 4, y + 15);
            rightFang.AddLine(x + 7, y + 25);
            rightFang.AddLine(x + 10, y + 15);
            rightFang.EndFigure(Microsoft.Graphics.Canvas.Geometry.CanvasFigureLoop.Closed);
            using (var geometry = Microsoft.Graphics.Canvas.Geometry.CanvasGeometry.CreatePath(rightFang))
            {
                session.FillGeometry(geometry, Colors.White);
            }

            // Arms/claws
            session.FillEllipse(x - 45, y + 20, 15, 25, Color.FromArgb(255, 120, 60, 60)); // Left arm

            // Left claw
            var leftClaw = new Microsoft.Graphics.Canvas.Geometry.CanvasPathBuilder(session);
            leftClaw.BeginFigure(x - 50, y + 35);
            leftClaw.AddLine(x - 55, y + 45);
            leftClaw.AddLine(x - 45, y + 40);
            leftClaw.EndFigure(Microsoft.Graphics.Canvas.Geometry.CanvasFigureLoop.Closed);
            using (var geometry = Microsoft.Graphics.Canvas.Geometry.CanvasGeometry.CreatePath(leftClaw))
            {
                session.FillGeometry(geometry, Color.FromArgb(255, 200, 200, 200));
            }

            session.FillEllipse(x + 45, y + 20, 15, 25, Color.FromArgb(255, 120, 60, 60)); // Right arm

            // Right claw
            var rightClaw = new Microsoft.Graphics.Canvas.Geometry.CanvasPathBuilder(session);
            rightClaw.BeginFigure(x + 50, y + 35);
            rightClaw.AddLine(x + 55, y + 45);
            rightClaw.AddLine(x + 45, y + 40);
            rightClaw.EndFigure(Microsoft.Graphics.Canvas.Geometry.CanvasFigureLoop.Closed);
            using (var geometry = Microsoft.Graphics.Canvas.Geometry.CanvasGeometry.CreatePath(rightClaw))
            {
                session.FillGeometry(geometry, Color.FromArgb(255, 200, 200, 200));
            }

            // Body details/scales
            for (int i = 0; i < 5; i++)
            {
                session.DrawEllipse(x - 15 + (i * 8), y + 5 + (i % 2) * 8, 4, 4, 
                    Color.FromArgb(150, 100, 30, 30), 2);
            }
        }

        private void DrawBattleBackground(CanvasDrawingSession session)
        {
            float width = (float)canvas.ActualWidth;
            float height = (float)canvas.ActualHeight;

            // If we have a battle background image loaded, use it (Final Fantasy style)
            if (battleBackgroundBitmap != null)
            {
                // Calculate scaling to fill the screen while maintaining aspect ratio
                float scaleX = width / (float)battleBackgroundBitmap.Size.Width;
                float scaleY = height / (float)battleBackgroundBitmap.Size.Height;
                float scale = Math.Max(scaleX, scaleY); // Use max to ensure full coverage

                float scaledWidth = (float)battleBackgroundBitmap.Size.Width * scale;
                float scaledHeight = (float)battleBackgroundBitmap.Size.Height * scale;

                // Center the image
                float x = (width - scaledWidth) / 2;
                float y = (height - scaledHeight) / 2;

                session.DrawImage(battleBackgroundBitmap, 
                    new Windows.Foundation.Rect(x, y, scaledWidth, scaledHeight));

                return;
            }

            // Fallback to gradient backgrounds if image didn't load
            int seed = 0;
            if (battleSystem != null && battleSystem.CurrentEnemy != null)
            {
                seed = battleSystem.CurrentEnemy.Name.GetHashCode();
            }

            Random rand = new Random(seed);
            int backgroundType = rand.Next(5);

            if (backgroundType == 0)
            {
                for (int y = 0; y < height; y++)
                {
                    float ratio = y / height;
                    byte r = (byte)(20 + ratio * 40);
                    byte g = (byte)(10 + ratio * 30);
                    byte b = (byte)(60 + ratio * 80);
                    session.DrawLine(0, y, width, y, Color.FromArgb(255, r, g, b));
                }
            }
            else if (backgroundType == 1)
            {
                for (int y = 0; y < height; y++)
                {
                    float ratio = y / height;
                    byte r = (byte)(40 + ratio * 60);
                    byte g = (byte)(20 + ratio * 40);
                    byte b = (byte)(20 + ratio * 30);
                    session.DrawLine(0, y, width, y, Color.FromArgb(255, r, g, b));
                }
            }
            else if (backgroundType == 2)
            {
                for (int y = 0; y < height; y++)
                {
                    float ratio = y / height;
                    byte r = (byte)(10 + ratio * 30);
                    byte g = (byte)(40 + ratio * 70);
                    byte b = (byte)(30 + ratio * 50);
                    session.DrawLine(0, y, width, y, Color.FromArgb(255, r, g, b));
                }
            }
            else if (backgroundType == 3)
            {
                for (int y = 0; y < height; y++)
                {
                    float ratio = y / height;
                    byte r = (byte)(60 + ratio * 80);
                    byte g = (byte)(40 + ratio * 60);
                    byte b = (byte)(10 + ratio * 20);
                    session.DrawLine(0, y, width, y, Color.FromArgb(255, r, g, b));
                }
            }
            else
            {
                for (int y = 0; y < height; y++)
                {
                    float ratio = y / height;
                    byte r = (byte)(30 + ratio * 50);
                    byte g = (byte)(20 + ratio * 40);
                    byte b = (byte)(50 + ratio * 90);
                    session.DrawLine(0, y, width, y, Color.FromArgb(255, r, g, b));
                }
            }

            for (int i = 0; i < 50; i++)
            {
                float x = rand.Next((int)width);
                float y = rand.Next((int)height);
                session.FillCircle(x, y, 2, Color.FromArgb(100, 255, 255, 255));
            }
        }

        private void GameTimer_Tick(object sender, object e)
        {
            // Animate clouds/mist (only in battle)
            if (gameState == GameState.Battle)
            {
                animationTime += 0.016f; // ~60fps
                cloudOffset1 += 0.5f;
                cloudOffset2 += 0.3f;
                mistOffset += 0.2f;
            }

            canvas.Invalidate();
        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            args.Handled = true;

            if (isDialogOpen)
            {
                return;
            }

            if (args.VirtualKey == Windows.System.VirtualKey.Escape)
            {
                ToggleMenu();
                return;
            }

            if (isMenuOpen)
            {
                HandleInGameMenuInput(args.VirtualKey);
                return;
            }

            if (gameState == GameState.Battle)
            {
                HandleBattleInput(args.VirtualKey);
            }
            else if (gameState == GameState.Exploration)
            {
                HandleExplorationInput(args.VirtualKey);
            }
        }

        private void HandleInGameMenuInput(Windows.System.VirtualKey key)
        {
            if (key == Windows.System.VirtualKey.Up || key == Windows.System.VirtualKey.W)
            {
                inGameMenuSelection--;
                if (inGameMenuSelection < 0)
                {
                    inGameMenuSelection = InGameMenuItemCount - 1;
                }
                UpdateInGameMenuCursor();
            }
            else if (key == Windows.System.VirtualKey.Down || key == Windows.System.VirtualKey.S)
            {
                inGameMenuSelection++;
                if (inGameMenuSelection >= InGameMenuItemCount)
                {
                    inGameMenuSelection = 0;
                }
                UpdateInGameMenuCursor();
            }
            else if (key == Windows.System.VirtualKey.Enter || key == Windows.System.VirtualKey.Space)
            {
                ExecuteInGameMenuSelection();
            }
        }

        private void UpdateInGameMenuCursor()
        {
            cursorResume.Visibility = inGameMenuSelection == 0 ? Visibility.Visible : Visibility.Collapsed;
            cursorSave.Visibility = inGameMenuSelection == 1 ? Visibility.Visible : Visibility.Collapsed;
            cursorMainMenu.Visibility = inGameMenuSelection == 2 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ExecuteInGameMenuSelection()
        {
            switch (inGameMenuSelection)
            {
                case 0:
                    BtnResume_Click(null, null);
                    break;
                case 1:
                    if (gameState == GameState.Exploration)
                    {
                        BtnSave_Click(null, null);
                    }
                    break;
                case 2:
                    BtnMainMenu_Click(null, null);
                    break;
            }
        }

        private void HandleBattleInput(Windows.System.VirtualKey key)
        {
            if (battleSystem.CurrentEnemy == null)
            {
                return;
            }

            if (key == Windows.System.VirtualKey.Up || key == Windows.System.VirtualKey.W)
            {
                battleMenuSelection--;
                if (battleMenuSelection < 0)
                {
                    battleMenuSelection = BattleMenuItemCount - 1;
                }
                UpdateBattleMenuHighlight();
            }
            else if (key == Windows.System.VirtualKey.Down || key == Windows.System.VirtualKey.S)
            {
                battleMenuSelection++;
                if (battleMenuSelection >= BattleMenuItemCount)
                {
                    battleMenuSelection = 0;
                }
                UpdateBattleMenuHighlight();
            }
            else if (key == Windows.System.VirtualKey.Enter || key == Windows.System.VirtualKey.Space)
            {
                ExecuteBattleCommand(battleMenuSelection);
            }
        }

        private void UpdateBattleMenuHighlight()
        {
            cursorAttack.Visibility = battleMenuSelection == 0 ? Visibility.Visible : Visibility.Collapsed;
            cursorMagic.Visibility = battleMenuSelection == 1 ? Visibility.Visible : Visibility.Collapsed;
            cursorItem.Visibility = battleMenuSelection == 2 ? Visibility.Visible : Visibility.Collapsed;
            cursorDefend.Visibility = battleMenuSelection == 3 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ExecuteBattleCommand(int commandIndex)
        {
            switch (commandIndex)
            {
                case 0:
                    ExecuteAttackCommand();
                    break;
                case 1:
                    ExecuteMagicCommand();
                    break;
                case 2:
                    ExecuteItemCommand();
                    break;
                case 3:
                    ExecuteDefendCommand();
                    break;
            }
        }

        private void ToggleMenu()
        {
            if (gameState == GameState.Battle)
            {
                return;
            }

            isMenuOpen = !isMenuOpen;
            inGameMenu.Visibility = isMenuOpen ? Visibility.Visible : Visibility.Collapsed;

            if (isMenuOpen)
            {
                inGameMenuSelection = 0;
                UpdateInGameMenuCursor();
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
            battleMenuSelection = 0;
            UpdateBattleMenuHighlight();
            MusicManager.PlayMusic(MusicTrack.Battle);
            UpdateUI();
        }

        private void ExecuteAttackCommand()
        {
            if (battleSystem.CurrentEnemy == null) return;

            string result = battleSystem.ExecutePlayerAttack();
            txtBattleMessage.Text = result;

            if (battleSystem.CurrentEnemy.CurrentHP <= 0)
            {
                EndBattle(true);
            }
            else
            {
                string enemyAction = battleSystem.ExecuteEnemyTurn();
                txtBattleMessage.Text += "\n" + enemyAction;

                if (hero.CurrentHP <= 0)
                {
                    EndBattle(false);
                }
            }

            UpdateUI();
        }

        private void ExecuteMagicCommand()
        {
            if (hero.CurrentMP >= 10)
            {
                hero.CurrentMP -= 10;
                int damage = (int)(hero.Magic * 1.5);
                battleSystem.CurrentEnemy.CurrentHP -= damage;
                txtBattleMessage.Text = $"{hero.Name} casts Fire! Deals {damage} damage!";

                if (battleSystem.CurrentEnemy.CurrentHP <= 0)
                {
                    EndBattle(true);
                }
                else
                {
                    string enemyAction = battleSystem.ExecuteEnemyTurn();
                    txtBattleMessage.Text += "\n" + enemyAction;

                    if (hero.CurrentHP <= 0)
                    {
                        EndBattle(false);
                    }
                }
            }
            else
            {
                txtBattleMessage.Text = "Not enough MP!";
            }

            UpdateUI();
        }

        private void ExecuteItemCommand()
        {
            hero.CurrentHP = Math.Min(hero.CurrentHP + 30, hero.MaxHP);
            txtBattleMessage.Text = $"{hero.Name} uses Potion! Restored 30 HP!";

            string enemyAction = battleSystem.ExecuteEnemyTurn();
            txtBattleMessage.Text += "\n" + enemyAction;

            if (hero.CurrentHP <= 0)
            {
                EndBattle(false);
            }

            UpdateUI();
        }

        private void ExecuteDefendCommand()
        {
            int originalDefense = hero.Defense;
            hero.Defense = (int)(hero.Defense * 1.5);

            string enemyAction = battleSystem.ExecuteEnemyTurn();
            txtBattleMessage.Text = $"{hero.Name} defends!\n" + enemyAction;

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
                txtBattleMessage.Text = $"Victory! Gained {expGained} EXP!";
                hero.CurrentHP = Math.Min(hero.CurrentHP + 20, hero.MaxHP);
                hero.CurrentMP = Math.Min(hero.CurrentMP + 10, hero.MaxMP);
                MusicManager.PlayMusic(MusicTrack.Victory);

                // Keep message box visible for victory message
                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
                timer.Tick += (s, e) =>
                {
                    messageBox.Visibility = Visibility.Collapsed;
                    ((DispatcherTimer)s).Stop();
                };
                timer.Start();

                // Return to exploration music after 5 seconds
                var musicTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
                musicTimer.Tick += (s, e) =>
                {
                    MusicManager.PlayMusic(MusicTrack.Exploration);
                    ((DispatcherTimer)s).Stop();
                };
                musicTimer.Start();
            }
            else
            {
                txtBattleMessage.Text = "Defeated! Game Over!";
                hero.CurrentHP = hero.MaxHP;
                hero.CurrentMP = hero.MaxMP;
                MusicManager.PlayMusic(MusicTrack.GameOver);
            }

            UpdateUI();
        }

        private void UpdateUI()
        {
            // Update character info
            txtHeroName.Text = hero.Name;
            txtHeroLevel.Text = hero.Level.ToString();
            txtHeroHP.Text = $"{hero.CurrentHP}/{hero.MaxHP}";
            txtHeroMP.Text = $"{hero.CurrentMP}/{hero.MaxMP}";

            // Update HP bar width (FF6 style)
            double hpPercent = (double)hero.CurrentHP / hero.MaxHP;
            hpBar.Width = 250 * hpPercent;

            // Change HP bar color based on health
            if (hpPercent > 0.5)
                hpBar.Background = new Windows.UI.Xaml.Media.SolidColorBrush(Colors.Green);
            else if (hpPercent > 0.25)
                hpBar.Background = new Windows.UI.Xaml.Media.SolidColorBrush(Colors.Yellow);
            else
                hpBar.Background = new Windows.UI.Xaml.Media.SolidColorBrush(Colors.Red);

            // Update MP bar width
            double mpPercent = (double)hero.CurrentMP / hero.MaxMP;
            mpBar.Width = 250 * mpPercent;

            // Show message box during battle
            if (gameState == GameState.Battle)
            {
                messageBox.Visibility = Visibility.Visible;
                if (battleSystem != null && battleSystem.CurrentEnemy != null)
                {
                    txtBattleMessage.Text = $"Enemy: {battleSystem.CurrentEnemy.Name} | HP: {battleSystem.CurrentEnemy.CurrentHP}/{battleSystem.CurrentEnemy.MaxHP}";
                }
            }
            else
            {
                messageBox.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnResume_Click(object sender, RoutedEventArgs e)
        {
            ToggleMenu();
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (isDialogOpen) return;

            isMenuOpen = false;
            inGameMenu.Visibility = Visibility.Collapsed;
            isDialogOpen = true;

            bool success = await SaveGameManager.SaveGame(hero);

            var dialog = new ContentDialog
            {
                Title = success ? "Game Saved" : "Save Failed",
                Content = success ? "Your progress has been saved." : "Failed to save game.",
                CloseButtonText = "OK"
            };

            await dialog.ShowAsync();
            isDialogOpen = false;
        }

        private async void BtnMainMenu_Click(object sender, RoutedEventArgs e)
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

        // Full Screen and Cursor Management
        private void EnterFullScreenMode()
        {
            var view = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
            if (view.TryEnterFullScreenMode())
            {
                isFullScreen = true;
            }
        }

        private void ExitFullScreenMode()
        {
            var view = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
            view.ExitFullScreenMode();
            isFullScreen = false;
        }

        private void ToggleFullScreen()
        {
            if (isFullScreen)
            {
                ExitFullScreenMode();
            }
            else
            {
                EnterFullScreenMode();
            }
        }

        private void HideCursor()
        {
            Window.Current.CoreWindow.PointerCursor = null;
        }

        private void ShowCursor()
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(
                Windows.UI.Core.CoreCursorType.Arrow, 0);
        }
    }
}
