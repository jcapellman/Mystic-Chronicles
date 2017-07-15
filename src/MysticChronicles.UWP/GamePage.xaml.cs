using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using MysticChronicles.Engine;

namespace MysticChronicles.UWP
{
    public sealed partial class GamePage : Page
    {
		readonly MainGame _game;

		public GamePage()
        {
            this.InitializeComponent();
            
			var launchArguments = string.Empty;
            _game = MonoGame.Framework.XamlGame<MainGame>.Create(launchArguments, Window.Current.CoreWindow, swapChainPanel);
        }
    }
}