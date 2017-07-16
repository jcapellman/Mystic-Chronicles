using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MysticChronicles.Engine.Objects.Common;

namespace MysticChronicles.Engine.Objects.Element.Character
{
    public class PartyMember : BaseGraphicElement
    {    
        public PartyMember(ElementContainer elementContainer, string textureName, bool isVisible = true) : base(elementContainer, textureName, isVisible)
        {
        }   

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureElement, new Rectangle(0, 0, TextureElement.Width, TextureElement.Height), Color.White);
        }
    }
}