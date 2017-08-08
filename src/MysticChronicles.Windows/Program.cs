using System;

using MODEXngine.Library.Engine.Common;

using MysticChronicles.Library.Game.GameStates;

namespace MysticChronicles.Windows
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new MODEXngine.Library.Engine.MainGame(Library.Game.Common.Constants.GAME_NAME, typeof(MainMenuState)))
            {
                game.Run();
            }
        }
    }
}