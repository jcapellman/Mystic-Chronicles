using MysticChronicles.Droid.Objects.Common;

namespace MysticChronicles.GameStates
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