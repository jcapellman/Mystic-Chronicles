using Microsoft.Xna.Framework.Graphics;

using MysticChronicles.Engine.Managers;

namespace MysticChronicles.Engine.Objects.Common
{
    public class GameStateContainer
    {
        public TextureManager TManager { get; set; }

        public int Window_Width { get; set; }

        public int Window_Height { get; set; }

        public SpriteFont MainFont { get; set; }

        public GameContainer GContainer { get; set; }
        
        public int? GameID { get; set; }
    }
}