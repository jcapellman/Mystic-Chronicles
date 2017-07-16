using System.Linq;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using MysticChronicles.Engine.Objects.Common;
using MysticChronicles.Engine.Objects.Element.Static;

namespace MysticChronicles.Engine.GameStates
{
    public class MainMenuState : BaseGameState
    {
        public MainMenuState(GameStateContainer container) : base(container) { }

        public override void HandleInput(GamePadState gamePadState, KeyboardState keyboardState, TouchCollection tocuCollection)
        {
            if (!keyboardState.GetPressedKeys().Any() && tocuCollection.All(a => a.State != TouchLocationState.Pressed))
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