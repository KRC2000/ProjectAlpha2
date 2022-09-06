using Microsoft.Xna.Framework;

namespace ProjectAlpha2
{
    public interface Hoverable
    {
        public bool Hovered { get; set; }

        public bool DetectHit(Vector2 hitLocation);
    }
}