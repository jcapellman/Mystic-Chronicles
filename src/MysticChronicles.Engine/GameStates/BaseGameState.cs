using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;

using MysticChronicles.Engine.Objects.Common;
using MysticChronicles.Engine.Managers;
using MysticChronicles.Engine.Objects.Element;

namespace MysticChronicles.Engine.GameStates
{
    public abstract class BaseGameState
    {
        protected TextureManager textureManager;
        protected int width, height;
        protected List<BaseGraphicElement> graphicElements;

        protected BaseGameState(GameStateContainer container)
        {
            textureManager = container.TManager;
            graphicElements = new List<BaseGraphicElement>();

            width = container.Window_Width;
            height = container.Window_Height;
        }

        public ElementContainer EContainer => new ElementContainer
        {
            Window_Width = width,
            Window_Height = height,
            TextureManager = textureManager
        };
        
        protected void AddGraphicElement(BaseGraphicElement element)
        {
            graphicElements.Add(element);
        }

        public abstract void LoadContent();

        public void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var element in graphicElements)
            {
                element.Render(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}