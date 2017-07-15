using MysticChronicles.Engine.Objects.Common;

namespace MysticChronicles.Engine.GameStates
{
    public class MainMenuState : BaseGameState
    {
        public MainMenuState(GameStateContainer container) : base(container)
        {
        }

        public override void LoadContent()
        {
            textures.Add(textureManager.LoadTexture("UI/MainMenu"));
        }
    }
}