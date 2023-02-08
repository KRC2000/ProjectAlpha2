using System.Collections.Generic;
using System;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ProjectAlpha2
{
    public static class ResourceManager
    {
        private static Dictionary<string, Tuple<IntPtr, Texture2D>> textureBinding_ = new Dictionary<string, Tuple<IntPtr, Texture2D>>();

        public static void AddTextureBinding(ContentManager content, string textureName)
        {
            if (textureBinding_.ContainsKey(textureName)) throw new Exception($"Texture binding with the name {textureName} already exists");
            
            Texture2D t = content.Load<Texture2D>(textureName);
            IntPtr t_ptr = Game1.imGuiRenderer.BindTexture(t);

            textureBinding_.Add(textureName, new Tuple<IntPtr, Texture2D>(t_ptr, t));
        }

        public static void LoadTextures(ContentManager content)
        {
            Console.WriteLine(content.RootDirectory);
        }

        public static Tuple<IntPtr, Texture2D> GetTextureBinding(string textureName)
        {
            Tuple<IntPtr, Texture2D> t;
            if (textureBinding_.TryGetValue(textureName, out t)) return t;
            else throw new Exception($"Failed to get texture binding, there is no registered texture binding with the name \"{textureName}\"");
        }
    }
}