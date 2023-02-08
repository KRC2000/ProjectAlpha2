using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;


namespace ProjectAlpha2
{
    public class Location: Marker, Hoverable
    {
        public string Name { get; set; }
        private Vector2 hitboxBounds = new Point2(50, 60);
        // Location hitbox will be always above the location and centered horisontaly
        public Storage Inventory { get; private set; } = new Storage(true);

        public bool Hovered { get; set; } = false;

        public void LoadResources()
        {
            MainImage = ResourceManager.GetTextureBinding("Vladimir").Item2;
            Overlay = ResourceManager.GetTextureBinding("location_frame").Item2;
        }

        public bool DetectHit(Vector2 hitLocation)
        {           
            return RectangleF.Contains(new RectangleF(ScreenPosition.X - (hitboxBounds.X/2), ScreenPosition.Y - hitboxBounds.Y, hitboxBounds.X, hitboxBounds.Y), hitLocation.ToPoint());
        }

        public override void Draw(SpriteBatch batch)
        {
            ScreenPosition = Game1.MainCamera.WorldToScreen(WorldPosition);

            batch.Draw(MainImage, new Vector2(ScreenPosition.X - MainImage.Width/2, ScreenPosition.Y - MainImage.Height), Color.White);
            batch.Draw(Overlay, new Vector2(ScreenPosition.X - Overlay.Width/2, ScreenPosition.Y - Overlay.Height), Color.White);
            

            if (Hovered) batch.DrawRectangle(new RectangleF(ScreenPosition.X - hitboxBounds.X/2, ScreenPosition.Y - hitboxBounds.Y, hitboxBounds.X, hitboxBounds.Y), Color.Red);
        }
    }
}