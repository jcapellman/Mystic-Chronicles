using System;
using System.Collections.Generic;

using MysticChronicles.Engine.Objects.Common;
using MysticChronicles.Engine.GameObjects.Characters;
using MysticChronicles.Engine.Objects.Element;
using MysticChronicles.Engine.Objects.Element.Static;
using MysticChronicles.Engine.Objects.Element.Character;

namespace MysticChronicles.Library.Game.GameObjects.Characters
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
        
        public override Tuple<List<BaseGraphicElement>, List<StaticText>> LoadContent()
        {
            var graphicElements = new List<BaseGraphicElement>
            {
                new PartyMember(_container, $"characters/{_className}", _partyIndex)
            };

            var textElements = new List<StaticText>
            {
                AddText(_container.Font, Name, Microsoft.Xna.Framework.Color.White, 525, 460 + (_partyIndex * 50), 1),
                AddText(_container.Font, $"HP {_currentHP}/{_maxHP}", Microsoft.Xna.Framework.Color.White, 1000, 460 + (_partyIndex * 50), 1)
            };

            return new Tuple<List<BaseGraphicElement>, List<StaticText>>(graphicElements, textElements);
        }
    }
}