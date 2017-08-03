using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MysticChronicles.Engine.Objects.Common;

namespace MysticChronicles.Engine.Objects.Element.Tiles
{
    public class StaticTileElement : BaseGraphicElement
    {
        public StaticTileElement(ElementContainer elementContainer, string textureName, bool isVisible) : base(elementContainer, textureName, isVisible)
        {
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureElement, new Rectangle(0, 0, TextureElement.Width, TextureElement.Height), Color.White);
        }
    }
}