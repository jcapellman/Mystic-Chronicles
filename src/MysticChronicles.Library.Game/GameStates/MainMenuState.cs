using System.Linq;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using MODEXngine.Library.Engine.GameStates;
using MODEXngine.Library.Engine.Objects.Common;

using MysticChronicles.Library.Game.GameObjects.GUI;

namespace MysticChronicles.Library.Game.GameStates
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

            gameID = 1; // TODO Rework to actually select a game

            RequestStateChange(new LoadGameState(GsContainer));
        }

        public override void LoadContent()
        {
            var mObject = new MenuObject(EContainer);

            var content = mObject.LoadContent();

            AddGraphicElementRange(content.Item1);
            AddTextElementRange(content.Item2);
        }
    }
}