using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

using MysticChronicles.Engine;
using MysticChronicles.Library.Game.GameStates;

namespace MysticChronicles
{
    [Activity(Label = "Mystic Chronicles"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.FullUser
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]
    public class MainActivity : Microsoft.Xna.Framework.AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var g = new MainGame(MysticChronicles.Library.Game.Common.Constants.GAME_NAME, typeof(MainMenuState));
            SetContentView((View)g.Services.GetService(typeof(View)));
            g.Run();
        }
    }
}