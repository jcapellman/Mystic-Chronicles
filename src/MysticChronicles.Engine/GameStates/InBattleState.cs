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
            _partyMembers = new List<PartyMemberObject>
            {
                new PartyMemberObject("Solozar", "Soldier", 1, EContainer),
                new PartyMemberObject("Tainer", "Tank", 2, EContainer),
                new PartyMemberObject("Katarn", "Sniper", 3, EContainer)
            };
        }

        public override void HandleInput(GamePadState gamePadState, KeyboardState keyboardState, TouchCollection touchCollection)
        {
            // TODO: Handle Input
        }

        public override void LoadContent()
        {
            foreach (var partyMember in _partyMembers)
            {
                var partyMemberContent = partyMember.LoadContent();

                AddGraphicElementRange(partyMemberContent.Item1);

                AddTextElementRange(partyMemberContent.Item2);
            }
            
            AddGraphicElement(new BackgroundImage(EContainer, "BattleBackgrounds/Desert"));
            AddGraphicElement(new BackgroundImage(EContainer, "UI/BattleOverlay"));
        }
    }
}