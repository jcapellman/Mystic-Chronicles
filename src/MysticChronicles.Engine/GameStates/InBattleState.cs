using Microsoft.Xna.Framework.Input;

using MysticChronicles.Engine.Objects.Common;
using MysticChronicles.Engine.Objects.Element.Static;

namespace MysticChronicles.Engine.GameStates
{
    public class InBattleState : BaseGameState
    {
        public InBattleState(GameStateContainer container) : base(container) { }

        public override void HandleInput(GamePadState gamePadState)
        {
            throw new System.NotImplementedException();
        }

        public override void LoadContent()
        {
            AddGraphicElement(new BackgroundImage(EContainer, "Battle Backgrounds/Forest"));
        }
    }
}