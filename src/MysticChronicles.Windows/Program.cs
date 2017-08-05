using System;

using MysticChronicles.Engine;
using MysticChronicles.Library.Game.Common;
using MysticChronicles.Library.Game.GameStates;

namespace MysticChronicles.Windows
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new MainGame(Constants.GAME_NAME, typeof(MainMenuState)))
                game.Run();
        }
    }
}