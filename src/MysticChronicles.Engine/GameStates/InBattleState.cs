using System.Collections.Generic;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using MysticChronicles.Engine.GameObjects.Characters;
using MysticChronicles.Engine.Objects.Common;
using MysticChronicles.Engine.Objects.Element.Static;

namespace MysticChronicles.Engine.GameStates
{
    public class InBattleState : BaseGameState
    {
        private readonly List<PartyMemberObject> _partyMembers;
        
        public InBattleState(GameStateContainer container) : base(container)
        {
            _partyMembers = new List<PartyMemberObject>();

            var dbPartyMembers = gContainer.GetPartyMembers();

            for (var x = 0; x < dbPartyMembers.Count; x++)
            {
                var item = dbPartyMembers[x];

                _partyMembers.Add(new PartyMemberObject(item.Name, item.SpriteName, x, EContainer));
            }
        }

        public override void HandleInput(GamePadState gamePadState, KeyboardState keyboardState, TouchCollection touchCollection)
        {
            // TODO: Handle Input
        }

        public override void LoadContent()
        {
            AddGraphicElement(new BackgroundImage(EContainer, "BattleBackgrounds/Desert"));
            AddGraphicElement(new BackgroundImage(EContainer, "UI/BattleOverlay"));

            foreach (var partyMember in _partyMembers)
            {
                var partyMemberContent = partyMember.LoadContent();

                AddGraphicElementRange(partyMemberContent.Item1);

                AddTextElementRange(partyMemberContent.Item2);
            }   
        }
    }
}