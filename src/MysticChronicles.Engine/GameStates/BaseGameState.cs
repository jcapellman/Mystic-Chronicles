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
using MysticChronicles.Engine.Enums;

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
                TManager = textureManager,
                MainFont = _mainFont
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
        
        protected void AddGraphicElementRange(List<BaseGraphicElement> elements)
        {
            GraphicElements.AddRange(elements);
        }

        public void AddText(string text, Color color, int scale, TextAlignment textAlignment)
        {
            var position = Vector2.One;

            switch (textAlignment)
            {
                case TextAlignment.HORIZONTALLY_AND_VERTICALLY_CENTERED:
                    position.X = (Width - _mainFont.MeasureString(text).X) / 2;
                    position.Y = (Height - _mainFont.MeasureString(text).Y) / 2;
                    break;
            }

            AddText(text, color, position.X, position.Y, scale);
        }

        public void AddText(string text, Color color, float xPosition, float yPosition, int scale)
        {
            TextElements.Add(new StaticText(_mainFont, text, color, scale, xPosition, yPosition));
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