using System.Linq;

using Microsoft.Xna.Framework.Input;

using MysticChronicles.Engine.Objects.Common;
using MysticChronicles.Engine.Objects.Element.Static;

namespace MysticChronicles.Engine.GameStates
{
    public class MainMenuState : BaseGameState
    {
        public MainMenuState(GameStateContainer container) : base(container) { }

        public override void HandleInput(GamePadState gamePadState, KeyboardState keyboardState)
        {
            if (!keyboardState.GetPressedKeys().Any())
            {
                return;
            }
            
            RequestStateChange(new InBattleState(GSContainer));
        }

        public override void LoadContent()
        {
            AddGraphicElement(new BackgroundImage(EContainer, "UI/MainMenu"));
        }
    }
}