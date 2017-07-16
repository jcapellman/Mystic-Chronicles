using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using MysticChronicles.Engine.Objects.Common;
using MysticChronicles.Engine.Objects.Element.Character;
using MysticChronicles.Engine.Objects.Element.Static;

namespace MysticChronicles.Engine.GameStates
{
    public class InBattleState : BaseGameState
    {
        public InBattleState(GameStateContainer container) : base(container) { }

        public override void HandleInput(GamePadState gamePadState, KeyboardState keyboardState, TouchCollection touchCollection)
        {
            // TODO: Handle Input
        }

        public override void LoadContent()
        {
            AddGraphicElement(new BackgroundImage(EContainer, "BattleBackgrounds/Forest"));
            AddGraphicElement(new BackgroundImage(EContainer, "UI/BattleOverlay"));
            AddGraphicElement(new PartyMember(EContainer, "characters/knight"));
        }
    }
}