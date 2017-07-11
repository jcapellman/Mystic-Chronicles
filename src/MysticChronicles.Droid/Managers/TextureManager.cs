using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MysticChronicles.Managers
{
    public class TextureManager
    {
        internal Texture2D LoadTexture(ContentManager content, string assetName)
        {
            return content.Load<Texture2D>(assetName);
        }
    }
}