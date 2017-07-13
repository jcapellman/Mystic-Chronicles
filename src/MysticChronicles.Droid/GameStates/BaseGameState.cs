using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MysticChronicles.Managers;

namespace MysticChronicles.GameStates
{
    public abstract class BaseGameState
    {
        protected TextureManager textureManager;
        protected int width, height;
        protected List<Texture2D> textures;

        protected BaseGameState(TextureManager argTextureManager, int argWidth, int argHeight)
        {
            textureManager = argTextureManager;
            textures = new List<Texture2D>();

            width = argWidth;
            height = argHeight;
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