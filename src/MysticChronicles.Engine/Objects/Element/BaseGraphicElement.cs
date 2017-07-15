using Microsoft.Xna.Framework.Graphics;

using MysticChronicles.Engine.Objects.Common;

namespace MysticChronicles.Engine.Objects.Element
{
    public abstract class BaseGraphicElement : BaseElement
    {
        protected Texture2D TextureElement;
        protected int Window_Width;
        protected int Window_Height;

        protected BaseGraphicElement(ElementContainer elementContainer, string textureName)
        {
            TextureElement = elementContainer.TextureManager.LoadTexture(textureName);

            Window_Width = elementContainer.Window_Width;
            Window_Height = elementContainer.Window_Height;
        }

        public abstract void Render(SpriteBatch spriteBatch);
    }
}