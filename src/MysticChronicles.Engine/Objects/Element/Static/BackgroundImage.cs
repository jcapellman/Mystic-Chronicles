using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MysticChronicles.Engine.Objects.Common;

namespace MysticChronicles.Engine.Objects.Element.Static
{
    public class BackgroundImage : BaseGraphicElement
    {
        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureElement, new Rectangle(0, 0, Window_Width, Window_Height), Color.White);
        }

        public BackgroundImage(ElementContainer elementContainer, string textureName) : base(elementContainer, textureName)
        {
        }
    }
}