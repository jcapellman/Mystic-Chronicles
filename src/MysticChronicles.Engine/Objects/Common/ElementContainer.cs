using Microsoft.Xna.Framework.Graphics;

using MysticChronicles.Engine.Managers;

namespace MysticChronicles.Engine.Objects.Common
{
    public class ElementContainer
    {
        public int Window_Width { get; set; }

        public int Window_Height { get; set; }

        public TextureManager TextureManager { get; set; }

        public SpriteFont Font { get; set; }
    }
}