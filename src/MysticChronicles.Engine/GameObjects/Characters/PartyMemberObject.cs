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
        private int _currentHP;
        private int _maxHP;

        public PartyMemberObject(string name, string className, int partyIndex, ElementContainer container) : base(container)
        {
            Name = name;
            _partyIndex = partyIndex;
            _className = className;
        }
        
        public override (List<BaseGraphicElement> graphicElements, List<StaticText> textElements) LoadContent()
        {
            var graphicElements = new List<BaseGraphicElement> {new PartyMember(_container, $"characters/{_className}", _partyIndex)};

            var textElements = new List<StaticText>
            {
                AddText(_container.Font, Name, Color.White, 525, 510 + (_partyIndex * 50), 1),
                AddText(_container.Font, $"HP {_currentHP}/{_maxHP}", Color.White, 1000, 510 + (_partyIndex * 50), 1)
            };

            return (graphicElements, textElements);
        }
    }
}