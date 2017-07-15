using System;

using MysticChronicles.Engine;

namespace MysticChronicles.Windows
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new MainGame())
                game.Run();
        }
    }
}