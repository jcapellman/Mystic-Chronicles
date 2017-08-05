using System.Collections.Generic;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using MysticChronicles.Engine.GameStates;
using MysticChronicles.Engine.Objects.Common;
using MysticChronicles.Engine.Objects.Element.Static;
using MysticChronicles.Library.Game.GameObjects.Characters;

namespace MysticChronicles.Library.Game.GameStates
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
            AddGraphicElement(new BackgroundImage(EContainer, $"BattleBackgrounds/{gContainer.GetGameVariable("BattleSprite")}"));
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