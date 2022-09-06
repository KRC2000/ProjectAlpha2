using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;


namespace ProjectAlpha2
{
    public class Traveller: Marker, Hoverable
    {
        public string Name { get; set; }

        public TextureId AvatarTextureId { get; private set; }
        public bool IsTraveling { get; private set; } = false;
        public float TravelSpeed { get; private set; } = 30;
        public bool PossessedByPlayer { get; set; } = false;
        public bool Selected { get; set; } = false;
        public Location CurrentLocation { get; private set; } = null;
        public Storage Inventory { get; private set; } = new Storage(true);
        public bool Hovered { get; set; } = false;

        private Vector2 travelTargetPos;
        private Location travelTargetLocation = null;
        private float travelTargetDistanceLeft;
        private Vector2 travelTargetVector;
        private Vector2 travelTargetUnitVector;
        private Vector2 hitboxBounds = new Point2(50, 60);

        public void SetAvatar(TextureId id)
        {
            AvatarTextureId = id;
            MainImage = ResourceManager.GetTextureBinding(id).Item2;
        }

        public void LoadResources()
        {
            Overlay = ResourceManager.GetTextureBinding(TextureId.MarkerCircle).Item2;
        }

        public void TravelTo(Vector2 target)
        {
            travelTargetPos = target;
            IsTraveling = true;
            travelTargetVector = travelTargetPos - WorldPosition;
            travelTargetUnitVector = Vector2.Normalize(travelTargetVector);
            travelTargetDistanceLeft = travelTargetVector.Length();
            CurrentLocation = null;
            travelTargetLocation = null;
        }

        public void TravelToLocation(Location location)
        {
            TravelTo(location.WorldPosition);
            travelTargetLocation = location;
            CurrentLocation = null;
        }

        public void StopTravel()
        {
            IsTraveling = false;
            travelTargetLocation = null;
        }

        public void Update(GameTime delta, bool isWorldPaused)
        {
            if (!isWorldPaused)
            {
                if (IsTraveling)
                {
                    if (travelTargetDistanceLeft > 0)
                    {
                        Vector2 step = travelTargetUnitVector * TravelSpeed * delta.GetElapsedSeconds();
                        WorldPosition += step;
                        travelTargetDistanceLeft -= step.Length();
                    }
                    else
                    { // Arrive to the target
                        WorldPosition = travelTargetPos;
                        CurrentLocation = travelTargetLocation;
                        IsTraveling = false;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            ScreenPosition = Game1.MainCamera.WorldToScreen(WorldPosition);
            Vector2 screenTravelTarget = Vector2.Transform(travelTargetPos, Game1.MainCamera.GetViewMatrix());

            batch.Draw((MainImage != null) ? MainImage : ResourceManager.GetTextureBinding(TextureId.Unknown).Item2, new Rectangle((int)ScreenPosition.X - (int)hitboxBounds.X/2, (int)ScreenPosition.Y - (int)hitboxBounds.Y, (int)hitboxBounds.X, (int)hitboxBounds.X),  Color.White);
            batch.Draw(Overlay, new Rectangle((int)ScreenPosition.X - 30/2, (int)ScreenPosition.Y - 20/2, 30, 20), (Selected) ? Color.Red : Color.White);

            if (IsTraveling) batch.DrawLine(ScreenPosition, screenTravelTarget, Color.Black);

            if (Hovered) batch.DrawRectangle(new RectangleF(ScreenPosition.X - hitboxBounds.X/2, ScreenPosition.Y - hitboxBounds.Y, hitboxBounds.X, hitboxBounds.Y), Color.Red);
        }

        public bool DetectHit(Vector2 hitLocation)
        {           
            return RectangleF.Contains(new RectangleF(ScreenPosition.X - (hitboxBounds.X/2), ScreenPosition.Y - hitboxBounds.Y, hitboxBounds.X, hitboxBounds.Y), hitLocation.ToPoint());
        }
    }
}