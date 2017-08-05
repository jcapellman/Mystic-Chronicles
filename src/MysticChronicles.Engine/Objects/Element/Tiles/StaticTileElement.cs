using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MysticChronicles.Engine.Objects.Common;

namespace MysticChronicles.Engine.Objects.Element.Tiles
{
    public class StaticTileElement : BaseGraphicElement
    {
        private readonly int _positionX;
        private readonly int _positionY;

        public StaticTileElement(ElementContainer elementContainer, string textureName, bool isVisible, int positionX, int positionY) : base(elementContainer, textureName, isVisible)
        {
            _positionX = positionX;
            _positionY = positionY;
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureElement, new Rectangle(_positionX, _positionY, TextureElement.Width, TextureElement.Height), Color.White);
        }
    }
}