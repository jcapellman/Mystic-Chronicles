using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MysticChronicles.Engine.Objects.Common;

using MysticChronicles.Engine.GameStates;
using MysticChronicles.Engine.Managers;

namespace MysticChronicles.Engine
{
    public class MainGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        private BaseGameState currentGameState;

        private GameStateContainer gsContainer;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        }

        protected override void Initialize()
        {
            gsContainer = new GameStateContainer
            {
                Window_Height = Window.ClientBounds.Height,
                Window_Width = Window.ClientBounds.Width,
                TManager = new TextureManager(Content)
            };

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            currentGameState = new MainMenuState(gsContainer);

            currentGameState.OnRequestStateChange += CurrentGameState_OnRequestStateChange;
            currentGameState.LoadContent();
        }

        private void CurrentGameState_OnRequestStateChange(object sender, BaseGameState e)
        {
            currentGameState = e;
        }
        
        protected override void Update(GameTime gameTime)
        {
            currentGameState.HandleInput(GamePad.GetState(PlayerIndex.One), Keyboard.GetState());
            
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            currentGameState.Render(spriteBatch);

            base.Draw(gameTime);
        }
    }
}