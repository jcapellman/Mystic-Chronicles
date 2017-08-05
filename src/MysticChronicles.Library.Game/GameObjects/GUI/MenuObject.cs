using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using MysticChronicles.Engine.GameObjects.GUI;
using MysticChronicles.Engine.Objects.Common;
using MysticChronicles.Engine.Objects.Element;
using MysticChronicles.Engine.Objects.Element.Static;

namespace MysticChronicles.Library.Game.GameObjects.GUI
{
    public class MenuObject : BaseGUIObject
    {
        public override Tuple<List<BaseGraphicElement>, List<StaticText>> LoadContent()
        {
            var graphicElements = new List<BaseGraphicElement> { new BackgroundImage(_container, "UI/MainMenu") };

            var textElements = new List<StaticText>
            {
                AddText(_container.Font, "Press any key to continue", Color.White, 1, Engine.Enums.TextAlignment.HORIZONTALLY_AND_VERTICALLY_CENTERED)
            };

            return new Tuple<List<BaseGraphicElement>, List<StaticText>>(graphicElements, textElements);
        }

        public MenuObject(ElementContainer container) : base(container)
        {
        }
    }
}