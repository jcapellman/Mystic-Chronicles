using Microsoft.Xna.Framework;
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
            AddGraphicElement(new BackgroundImage(EContainer, "BattleBackgrounds/Desert"));
            AddGraphicElement(new BackgroundImage(EContainer, "UI/BattleOverlay"));

            AddGraphicElement(new PartyMember(EContainer, "characters/Soldier", 1));
            AddGraphicElement(new PartyMember(EContainer, "characters/Tank", 2));
            AddGraphicElement(new PartyMember(EContainer, "characters/Sniper", 3));

            AddText("Solozar", Color.White, 525, 510, 1);
            AddText("Tainer", Color.White, 525, 560, 1);
            AddText("Katarn", Color.White, 525, 610, 1);
        }
    }
}