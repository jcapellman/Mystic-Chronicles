using Microsoft.Xna.Framework.Content;

using MysticChronicles.Managers;

namespace MysticChronicles.GameStates
{
    public class MainMenuState : BaseGameState
    {
        public MainMenuState(TextureManager argTextureManager, int argWidth, int argHeight) : base(argTextureManager, argWidth, argHeight)
        {
        }

        public override void LoadContent()
        {
            textures.Add(textureManager.LoadTexture("UI/MainMenu"));
        }
    }
}