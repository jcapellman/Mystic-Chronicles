using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MysticChronicles.Engine.Objects.Common;

namespace MysticChronicles.Engine.Objects.Element.Character
{
    public class PartyMember : BaseGraphicElement
    {
        private const int PartyYOffset = 175;

        private readonly int _partySpot;

        public PartyMember(ElementContainer elementContainer, string textureName, int partySpot, bool isVisible = true) : base(elementContainer, textureName, isVisible)
        {
            _partySpot = partySpot;
        }   

        public override void Render(SpriteBatch spriteBatch)
        {
            var originX = Window_Width - TextureElement.Width;
            var originY = Window_Height - (_partySpot * TextureElement.Height) - PartyYOffset;

            spriteBatch.Draw(TextureElement, new Rectangle(originX, originY, TextureElement.Width, TextureElement.Height), Color.White);
        }
    }
}