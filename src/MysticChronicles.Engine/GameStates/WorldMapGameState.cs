using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using MysticChronicles.Engine.Objects.Common;
using MysticChronicles.Engine.Objects.Element.Tiles;

namespace MysticChronicles.Engine.GameStates
{
    public class WorldMapGameState : BaseGameState
    {
        public WorldMapGameState(GameStateContainer container) : base(container)
        {
        }

        public override void HandleInput(GamePadState gamePadState, KeyboardState keyboardState, TouchCollection touchCollection)
        {
            // TODO Handle Game Input of charcter
        }

        public override void LoadContent()
        {
            // TODO Load Map Format to read tiles
            AddGraphicElement(new StaticTileElement(EContainer, "Tiles/Terrain/Grass_TopLeft", true));
        }
    }
}