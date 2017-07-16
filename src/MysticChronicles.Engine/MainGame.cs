using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using MysticChronicles.Engine.Common;
using MysticChronicles.Engine.Objects.Common;
using MysticChronicles.Engine.GameStates;
using MysticChronicles.Engine.Managers;

namespace MysticChronicles.Engine
{
    public class MainGame : Game
    {
        private SpriteBatch _spriteBatch;
        
        private BaseGameState _currentGameState;

        private GameStateContainer _gsContainer;

        public MainGame()
        {
            var graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

#if DEBUG
            graphics.IsFullScreen = false;
#else
            graphics.IsFullScreen = true;
#endif

            graphics.PreferredBackBufferWidth = Constants.RESOLUTION_WIDTH;
            graphics.PreferredBackBufferHeight = Constants.RESOLUTION_HEIGHT;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        }

        protected override void Initialize()
        {
            Window.Title = Common.Constants.GAME_NAME;

            _gsContainer = new GameStateContainer
            {
                Window_Height = Window.ClientBounds.Height,
                Window_Width = Window.ClientBounds.Width,
                TManager = new TextureManager(Content),
                MainFont = Content.Load<SpriteFont>("Main")
            };

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            _currentGameState = new MainMenuState(_gsContainer);

            _currentGameState.OnRequestStateChange += CurrentGameState_OnRequestStateChange;
            _currentGameState.LoadContent();
        }

        private void CurrentGameState_OnRequestStateChange(object sender, BaseGameState e)
        {
            _currentGameState = e;

            _currentGameState.LoadContent();
        }
        
        protected override void Update(GameTime gameTime)
        {
            _currentGameState.HandleInput(GamePad.GetState(PlayerIndex.One), Keyboard.GetState(), TouchPanel.GetState());
            
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            _currentGameState.Render(_spriteBatch);

            base.Draw(gameTime);
        }
    }
}