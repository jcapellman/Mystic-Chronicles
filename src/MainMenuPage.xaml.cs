using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Core;
using Windows.System;
using MysticChronicles.Services;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.UI;

namespace MysticChronicles
{
    public sealed partial class MainMenuPage : Page
    {
        private int menuSelection = 0;
        private const int MenuItemCount = 3;
        private bool isDialogOpen = false;

        // Animation fields
        private DispatcherTimer animationTimer;
        private float cloudOffset1 = 0;
        private float cloudOffset2 = 0;
        private float mistOffset = 0;
        private float animationTime = 0;

        public MainMenuPage()
        {
            this.InitializeComponent();

            // Start animation timer
            animationTimer = new DispatcherTimer();
            animationTimer.Interval = TimeSpan.FromMilliseconds(16); // ~60fps
            animationTimer.Tick += AnimationTimer_Tick;
            animationTimer.Start();
        }

        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            UpdateMenuCursor();
            MusicManager.PlayMusic(MusicTrack.MainMenu);
        }

        protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;
        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            args.Handled = true;

            if (isDialogOpen)
            {
                return;
            }

            if (args.VirtualKey == VirtualKey.Up || args.VirtualKey == VirtualKey.W)
            {
                menuSelection--;
                if (menuSelection < 0)
                {
                    menuSelection = MenuItemCount - 1;
                }
                UpdateMenuCursor();
            }
            else if (args.VirtualKey == VirtualKey.Down || args.VirtualKey == VirtualKey.S)
            {
                menuSelection++;
                if (menuSelection >= MenuItemCount)
                {
                    menuSelection = 0;
                }
                UpdateMenuCursor();
            }
            else if (args.VirtualKey == VirtualKey.Enter || args.VirtualKey == VirtualKey.Space)
            {
                ExecuteMenuSelection();
            }
        }

        private void UpdateMenuCursor()
        {
            cursorNewGame.Visibility = menuSelection == 0 ? Visibility.Visible : Visibility.Collapsed;
            cursorLoadGame.Visibility = menuSelection == 1 ? Visibility.Visible : Visibility.Collapsed;
            cursorExit.Visibility = menuSelection == 2 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ExecuteMenuSelection()
        {
            switch (menuSelection)
            {
                case 0:
                    BtnNewGame_Click(null, null);
                    break;
                case 1:
                    BtnLoadGame_Click(null, null);
                    break;
                case 2:
                    BtnExit_Click(null, null);
                    break;
            }
        }

        private void BtnNewGame_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CharacterCreationPage));
        }

        private async void BtnLoadGame_Click(object sender, RoutedEventArgs e)
        {
            Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;

            bool saveExists = await SaveGameManager.SaveExists();

            if (saveExists)
            {
                var saveData = await SaveGameManager.LoadGame();
                if (saveData != null)
                {
                    this.Frame.Navigate(typeof(GamePage), saveData);
                }
                else
                {
                    var dialog = new ContentDialog
                    {
                        Title = "Load Failed",
                        Content = "Failed to load saved game.",
                        CloseButtonText = "OK"
                    };
                    await dialog.ShowAsync();
                    Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
                }
            }
            else
            {
                var dialog = new ContentDialog
                {
                    Title = "No Save Data",
                    Content = "No saved game found.",
                    CloseButtonText = "OK"
                };
                await dialog.ShowAsync();
                Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void AnimationTimer_Tick(object sender, object e)
        {
            // Update animation offsets
            animationTime += 0.016f;
            cloudOffset1 += 0.3f;
            cloudOffset2 += 0.5f;
            mistOffset += 0.2f;

            // Redraw canvas
            animationCanvas?.Invalidate();
        }

        private void AnimationCanvas_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            var session = args.DrawingSession;
            float width = (float)sender.ActualWidth;
            float height = (float)sender.ActualHeight;

            // Draw cloud layers
            DrawCloudLayer(session, width, height, cloudOffset1, 0.3f, 25);
            DrawCloudLayer(session, width, height, cloudOffset2, 0.5f, 40);

            // Draw mist at bottom
            DrawMistLayer(session, width, height, mistOffset);
        }

        private void DrawCloudLayer(Microsoft.Graphics.Canvas.CanvasDrawingSession session, 
                                   float width, float height, float offset, float yPosition, float alpha)
        {
            int cloudCount = 6;
            float cloudSpacing = width / cloudCount;

            for (int i = 0; i < cloudCount; i++)
            {
                float x = (offset + i * cloudSpacing) % (width + 200) - 100;
                float y = height * yPosition;

                // Pulsating alpha for depth
                float pulseAlpha = alpha + (float)(Math.Sin(animationTime * 2 + i) * 8);

                // Draw fluffy clouds
                session.FillCircle(x, y, 50, Color.FromArgb((byte)pulseAlpha, 255, 255, 255));
                session.FillCircle(x + 40, y + 10, 45, Color.FromArgb((byte)pulseAlpha, 255, 255, 255));
                session.FillCircle(x + 80, y, 50, Color.FromArgb((byte)pulseAlpha, 255, 255, 255));
                session.FillCircle(x + 40, y - 20, 35, Color.FromArgb((byte)pulseAlpha, 255, 255, 255));
            }
        }

        private void DrawMistLayer(Microsoft.Graphics.Canvas.CanvasDrawingSession session, 
                                  float width, float height, float offset)
        {
            int mistCount = 10;
            float mistSpacing = width / mistCount;

            for (int i = 0; i < mistCount; i++)
            {
                float x = (offset * 0.4f + i * mistSpacing) % (width + 120) - 60;
                float y = height - 120;

                // Gentle wave motion
                float wave = (float)(Math.Sin(animationTime * 0.8 + i * 0.7) * 25);

                // Semi-transparent mist
                session.FillEllipse(x, y + wave, 100, 35, Color.FromArgb(50, 220, 220, 255));
                session.FillEllipse(x + 50, y + wave + 15, 80, 30, Color.FromArgb(35, 220, 220, 255));
            }
        }
    }
}
