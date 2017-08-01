using System;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using MysticChronicles.Engine.Objects.Common;

namespace MysticChronicles.Engine.GameStates
{
    public class WorldMapGameState : BaseGameState
    {
        public WorldMapGameState(GameStateContainer container) : base(container)
        {
        }

        public override void HandleInput(GamePadState gamePadState, KeyboardState keyboardState, TouchCollection touchCollection)
        {
            throw new NotImplementedException();
        }

        public override void LoadContent()
        {
            throw new NotImplementedException();
        }
    }
}