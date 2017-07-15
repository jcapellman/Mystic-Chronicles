using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MysticChronicles.Engine.Managers
{
    public class TextureManager
    {
        private readonly ContentManager contentManager;

        public TextureManager(ContentManager argContentManager)
        {
            contentManager = argContentManager;
        }

        internal Texture2D LoadTexture(string assetName)
        {
            return contentManager.Load<Texture2D>(assetName);
        }
    }
}