using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectAlpha2
{
    public abstract class Marker
    {
        public Vector2 ScreenPosition { get; protected set; }
        public Vector2 WorldPosition { get; set; } 

        public Texture2D MainImage { get; protected set; }
        public Texture2D Overlay { get; protected set; }

        public abstract void Draw(SpriteBatch batch);
    }
}