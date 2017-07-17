using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MysticChronicles.Engine.Enums;

using MysticChronicles.Engine.Objects.Common;
using MysticChronicles.Engine.Objects.Element;
using MysticChronicles.Engine.Objects.Element.Static;

namespace MysticChronicles.Engine.GameObjects
{
    public abstract class BaseGameObject
    {
        protected ElementContainer _container;

        protected BaseGameObject(ElementContainer container)
        {
            _container = container;
        }

        public abstract (List<BaseGraphicElement> graphicElements, List<StaticText> textElements) LoadContent();

        public StaticText AddText(SpriteFont font, string text, Color color, int scale, TextAlignment textAlignment)
        {
            var position = Vector2.One;

            switch (textAlignment)
            {
                case TextAlignment.HORIZONTALLY_AND_VERTICALLY_CENTERED:
                    position.X = (_container.Window_Width - font.MeasureString(text).X) / 2;
                    position.Y = (_container.Window_Height - font.MeasureString(text).Y) / 2;
                    break;
            }

            return AddText(font, text, color, position.X, position.Y, scale);
        }

        public StaticText AddText(SpriteFont font, string text, Color color, float xPosition, float yPosition, int scale)
        {
            return new StaticText(font, text, color, scale, xPosition, yPosition);
        }
    }
}