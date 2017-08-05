using System;
using System.Collections.Generic;
using System.Linq;

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
        protected GameContainer gContainer;
        protected TextureManager textureManager;
        protected int Width, Height;
        protected List<BaseGraphicElement> GraphicElements;
        protected List<StaticText> TextElements;
        protected int? gameID;

        private readonly SpriteFont _mainFont;

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
                TManager = textureManager,
                MainFont = _mainFont,
                GContainer = gContainer,
                GameID = gameID
        };

        protected BaseGameState(GameStateContainer container)
        {
            textureManager = container.TManager;

            GraphicElements = new List<BaseGraphicElement>();
            TextElements = new List<StaticText>();

            _mainFont = container.MainFont;

            Width = container.Window_Width;
            Height = container.Window_Height;

            gameID = container.GameID;
        }

        public ElementContainer EContainer => new ElementContainer
        {
            Window_Width = Width,
            Window_Height = Height,
            TextureManager = textureManager,
            Font = _mainFont,
            GContainer = gContainer
        };
        
        protected void AddGraphicElement(BaseGraphicElement element)
        {
            GraphicElements.Add(element);
        }
        
        protected void AddGraphicElementRange(List<BaseGraphicElement> elements)
        {
            GraphicElements.AddRange(elements);
        }

        protected void AddTextElement(StaticText textElement)
        {
            TextElements.Add(textElement);
        }

        protected void AddTextElementRange(List<StaticText> textElements)
        {
            TextElements.AddRange(textElements);
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

            foreach (var textElement in TextElements.Where(a => a.IsVisible))
            {
                textElement.Render(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}