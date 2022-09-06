using System.Collections.Generic;
using System;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ProjectAlpha2
{
    public static class ResourceManager
    {
        private static Dictionary<TextureId, Tuple<IntPtr, Texture2D>> textureBinding = new Dictionary<TextureId, Tuple<IntPtr, Texture2D>>();

        public static void AddTextureBinding(ContentManager content, TextureId id, string textureName)
        {
            Texture2D t = content.Load<Texture2D>(textureName);
            IntPtr t_ptr = Game1.imGuiRenderer.BindTexture(t);

            textureBinding.Add(id, new Tuple<IntPtr, Texture2D>(t_ptr, t));
        }

        public static Tuple<IntPtr, Texture2D> GetTextureBinding(TextureId id)
        {
            Tuple<IntPtr, Texture2D> t;
            if (textureBinding.TryGetValue(id, out t)) return t;
            else throw new Exception($"There is no texture binding with id {id}");
        }
    }

}