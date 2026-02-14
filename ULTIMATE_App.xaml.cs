// Mystic Chronicles - The world's simplest RPG implementation!
// This entire game is driven by Assets/game.json and the GORE Engine

using System;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GORE.Engine;

namespace MysticChronicles
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            // Initialize GORE Engine - handles everything!
            await GOREEngine.StartAsync();

            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                rootFrame.Navigate(typeof(MainMenuPage), e.Arguments);
            }

            Window.Current.Activate();
        }
    }
}
