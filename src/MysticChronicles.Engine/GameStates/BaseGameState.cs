using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MysticChronicles.Engine.Objects.Common;
using MysticChronicles.Engine.Managers;

namespace MysticChronicles.Engine.GameStates
{
    public abstract class BaseGameState
    {
        protected TextureManager textureManager;
        protected int width, height;
        protected List<Texture2D> textures;

        protected BaseGameState(GameStateContainer container)
        {
            textureManager = container.TManager;
            textures = new List<Texture2D>();

            width = container.Window_Width;
            height = container.Window_Height;
        }

        public abstract void LoadContent();

        public void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var texture in textures)
            {
                spriteBatch.Draw(texture, new Rectangle(0, 0, width, height), Color.White);
            }

            spriteBatch.End();
        }
    }
}