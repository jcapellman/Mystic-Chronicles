using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MysticChronicles.Engine.Enums;
using MysticChronicles.Engine.Objects.Element.Static;

namespace MysticChronicles.Engine.GameObjects
{
    public class BaseGameObject
    {
        public StaticText AddText(SpriteFont font, string text, Color color, int scale, TextAlignment textAlignment, int screenWidth, int screenHeight)
        {
            var position = Vector2.One;

            switch (textAlignment)
            {
                case TextAlignment.HORIZONTALLY_AND_VERTICALLY_CENTERED:
                    position.X = (screenWidth - font.MeasureString(text).X) / 2;
                    position.Y = (screenHeight - font.MeasureString(text).Y) / 2;
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