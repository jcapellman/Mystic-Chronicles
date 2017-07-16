using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MysticChronicles.Engine.Objects.Element.Static
{
    public class StaticText : BaseElement
    {
        private readonly int _xPosition;
        private readonly int _yPosition;

        private readonly string _text;

        private readonly SpriteFont _font;

        private readonly Color _color;

        public StaticText(SpriteFont font, string text, Color color, int xPosition, int yPosition, bool isVisible = true)
        {
            _font = font;
            _text = text;
            _color = color;
            _xPosition = xPosition;
            _yPosition = yPosition;

            IsVisible = isVisible;
        }

        public void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, _text, new Vector2(_xPosition, _yPosition), _color);
        }
    }
}