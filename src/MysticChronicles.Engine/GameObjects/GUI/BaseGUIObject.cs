using System;
using System.Collections.Generic;

using MysticChronicles.Engine.Objects.Common;
using MysticChronicles.Engine.Objects.Element;
using MysticChronicles.Engine.Objects.Element.Static;

namespace MysticChronicles.Engine.GameObjects.GUI
{
    public class BaseGUIObject : BaseGameObject
    {
        public BaseGUIObject(ElementContainer container) : base(container)
        {
        }

        public override Tuple<List<BaseGraphicElement>, List<StaticText>> LoadContent()
        {
            return new Tuple<List<BaseGraphicElement>, List<StaticText>>(null, null);
        }
    }
}