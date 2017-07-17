using System.Collections.Generic;

using Microsoft.Xna.Framework;

using MysticChronicles.Engine.Objects.Common;
using MysticChronicles.Engine.Objects.Element;
using MysticChronicles.Engine.Objects.Element.Static;

namespace MysticChronicles.Engine.GameObjects.GUI
{
    public class MenuObject : BaseGameObject
    {
        public override (List<BaseGraphicElement> graphicElements, List<StaticText> textElements) LoadContent()
        {
            var graphicElements = new List<BaseGraphicElement> { new BackgroundImage(_container, "UI/MainMenu") };

            var textElements = new List<StaticText>
            {
                AddText(_container.Font, "Press any key to continue", Color.White, 1, Enums.TextAlignment.HORIZONTALLY_AND_VERTICALLY_CENTERED)
            };

            return (graphicElements, textElements);
        }

        public MenuObject(ElementContainer container) : base(container)
        {
        }
    }
}