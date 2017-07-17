using System.Collections.Generic;

using MysticChronicles.Engine.Objects.Common;
using MysticChronicles.Engine.Objects.Element;
using MysticChronicles.Engine.Objects.Element.Static;

namespace MysticChronicles.Engine.GameObjects.Characters
{
    public abstract class BaseCharacterObject : BaseGameObject
    {
        protected string Name { get; set; }

        public abstract (List<BaseGraphicElement> graphicElements, List<StaticText> textElements) LoadContent(ElementContainer container);
    }
}