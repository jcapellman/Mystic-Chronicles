using System.Collections.Generic;

using MysticChronicles.Engine.Objects.Common;
using MysticChronicles.Engine.Objects.Element;
using MysticChronicles.Engine.Objects.Element.Static;
using MysticChronicles.Engine.Objects.Element.Character;

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
            
            var textElements = new List<StaticText>();
            
            return (graphicElements, textElements);
        }
    }
}