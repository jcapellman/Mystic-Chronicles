using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using MysticChronicles.Engine.Objects.Common;
using MysticChronicles.Engine.Managers;
using MysticChronicles.Engine.Objects.Element;
using MysticChronicles.Engine.Objects.Element.Static;

namespace MysticChronicles.Engine.GameStates
{
    public abstract class BaseGameState
    {
        protected TextureManager textureManager;
        protected int Width, Height;
        protected List<BaseGraphicElement> GraphicElements;
        protected List<StaticText> TextElements;

        private SpriteFont _mainFont;

        #region State Change Event
        public event EventHandler<BaseGameState> OnRequestStateChange;

        public void RequestStateChange(BaseGameState gameState)
        {
            var handler = OnRequestStateChange;

            handler?.Invoke(null, gameState);
        }
        #endregion

        protected GameStateContainer GsContainer => new GameStateContainer
        {
                Window_Height = Height,
                Window_Width = Width,
                TManager = textureManager
        };

        protected BaseGameState(GameStateContainer container)
        {
            textureManager = container.TManager;

            GraphicElements = new List<BaseGraphicElement>();
            TextElements = new List<StaticText>();

            _mainFont = container.MainFont;

            Width = container.Window_Width;
            Height = container.Window_Height;
        }

        public ElementContainer EContainer => new ElementContainer
        {
            Window_Width = Width,
            Window_Height = Height,
            TextureManager = textureManager
        };
        
        protected void AddGraphicElement(BaseGraphicElement element)
        {
            GraphicElements.Add(element);
        }

        public void AddText(string text, Color color, int xPosition, int yPosition)
        {
            TextElements.Add(new StaticText(_mainFont, text, color, xPosition, yPosition));
        }

        public abstract void HandleInput(GamePadState gamePadState, KeyboardState keyboardState, TouchCollection touchCollection);
        
        public abstract void LoadContent();

        public void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var element in GraphicElements.Where(a => a.IsVisible))
            {
                element.Render(spriteBatch);
            }

            foreach (var textElement in TextElements)
            {
                textElement.Render(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}