using System.Linq;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using MysticChronicles.Engine.Managers;
using MysticChronicles.Engine.Objects.Common;

namespace MysticChronicles.Engine.GameStates
{
    public class LoadGameState : BaseGameState
    {
        public LoadGameState(GameStateContainer container) : base(container)
        {
        }

        public override void HandleInput(GamePadState gamePadState, KeyboardState keyboardState, TouchCollection touchCollection)
        {
            if (!keyboardState.GetPressedKeys().Any() && touchCollection.All(a => a.State != TouchLocationState.Pressed))
            {
                return;
            }

            RequestStateChange(new InBattleState(GsContainer));
        }

        public override async void LoadContent()
        {
            gContainer = await GameManager.LoadGame(1);
        }
    }
}