using System.Collections.Generic;

using MysticChronicles.Engine.Objects.Common;
using MysticChronicles.Engine.Objects.Element;
using MysticChronicles.Engine.Objects.Element.Static;
using MysticChronicles.Engine.Objects.Element.Character;

using Microsoft.Xna.Framework;

namespace MysticChronicles.Engine.GameObjects.Characters
{
    public class PartyMemberObject : BaseCharacterObject
    {
        private readonly int _partyIndex;
        private readonly string _className;

        public PartyMemberObject(string name, string className, int partyIndex)
        {
            Name = name;
            _partyIndex = partyIndex;
            _className = className;
        }
        
        public override (List<BaseGraphicElement> graphicElements, List<StaticText> textElements) LoadContent(ElementContainer container)
        {
            var graphicElements = new List<BaseGraphicElement> {new PartyMember(container, $"characters/{_className}", _partyIndex)};

            var textElements = new List<StaticText>
            {
                AddText(container.Font, Name, Color.White, 525, 510, 1),
                AddText(container.Font, "HP 100/200", Color.White, 1000, 510, 1)
            };

            return (graphicElements, textElements);
        }
    }
}