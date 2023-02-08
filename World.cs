using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;
using MonoGame.Extended.Input;

using Framework;

namespace ProjectAlpha2
{
    public static class World
    {

        public static DateTime Time { get; private set; } = new DateTime(2000, 1, 1, 12, 0, 0);

        public static Traveller PosessedTraveller;

        public static List<Area> Areas = new List<Area>();

        public static List<Location> Locations { get; private set; } = new List<Location>();
        public static List<Traveller> Travellers { get; private set; } = new List<Traveller>();
        public static Dictionary<string, AreaDefinition> AreaDefinitions;
        public static List<Hoverable> Hoverables { get; private set; } = new List<Hoverable>();

        public static void Generate()
        {
            Locations.Add(new Location(){WorldPosition = new Vector2(30, 60), Name = "Vladivostok"});
            Locations.Add(new Location(){WorldPosition = new Vector2(140, 80), Name = "Mariupol"});
            Locations.Add(new Location(){WorldPosition = new Vector2(150, 90), Name = "Donetsk"});

            Traveller t1 = new Traveller(){WorldPosition = new Vector2(200, 100), Name = "Nikolai"};
            PosessedTraveller = t1;

            t1.SetAvatar("cat");
            
            t1.TravelTo(new Vector2(500, 500));
            Travellers.Add(t1);

            Hoverables.AddRange(Locations);
            Hoverables.AddRange(Travellers);

            using (StreamReader reader = new StreamReader(Path.Join("Content", "AreaDefinitions.json")))
            {
                AreaDefinitions = (Dictionary<string, AreaDefinition>)JsonSerializer.Deserialize
                        (reader.BaseStream, typeof(Dictionary<string, AreaDefinition>));
            }
        }

        public static void LoadCotent(GraphicsDevice device)
        {
            foreach (var location in Locations)
            {
                location.LoadResources();
            }

            foreach (var traveller in Travellers)
            {
                traveller.LoadResources();
            }

            Framework.Level Level = new Framework.Level("Content/World.tmx");

            foreach (var obj in Level.Map.ObjectGroups[0].Objects)
            {
               
                if (!string.IsNullOrEmpty(obj.Class) && AreaDefinitions.ContainsKey(obj.Class))
                    Areas.Add(new Area(AreaDefinitions[obj.Class], new Vector2(obj.X, obj.Y), obj.Polygon.Points, device));
                else
                    Areas.Add(new Area(AreaDefinitions["unknown"], new Vector2(obj.X, obj.Y), obj.Polygon.Points, device));
            }

        }

        
        public static void Update(GameTime delta)
        {
            PlayerInputProcessing();

            foreach (var traveller in Travellers)
            {
                traveller.Update(delta);
            }
        }

        public static void Draw(SpriteBatch batch, GraphicsDevice device)
        {
            //batch.Begin(SpriteSortMode.Deferred, null, SamplerState.AnisotropicClamp, null, null, null, Game1.MainCamera.GetViewMatrix());
            //batch.Draw(ResourceManager.GetTextureBinding(TextureId.Terrain).Item2, new Vector2(), Color.White);
            //batch.End();

            foreach (var area in Areas)
            {
                
                Vector3 camPosition = new Vector3(Game1.MainCamera.Center.X, -Game1.MainCamera.Center.Y, 1);
                Vector3 camTarget = new Vector3(camPosition.X, camPosition.Y, camPosition.Z - 1);
                Matrix projectionMatrix = Matrix.CreateOrthographic(device.Viewport.Width / Game1.MainCamera.Zoom, device.Viewport.Height / Game1.MainCamera.Zoom, 0, 1000f);
                Matrix viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);
                Matrix worldMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up);
                area.Draw(device, projectionMatrix, viewMatrix, worldMatrix);
            }

            batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            
            foreach (var location in Locations)
            {
                location.Draw(batch);
            }

            foreach (var traveller in Travellers)
            {
                traveller.Draw(batch);
            }

            batch.End();
        }

        public static void AddTime(TimeSpan timespan)
        {
            Time = Time + timespan;
        }

        private static void PlayerInputProcessing()
        {
            Vector2 mouseWorldPos = Vector2.Transform(Mouse.GetState().Position.ToVector2(), Game1.MainCamera.GetInverseViewMatrix());
            Vector2 mouseScreenPos = Mouse.GetState().Position.ToVector2();

            if (!Game1.IsMouseCapturedByUi)
            {
                if (InputManager.GetMouseState().WasButtonJustUp(MouseButton.Right))
                {
                    Vector2 travelTarget;
                    Location travelTargetLocation = null;

                    // Get target world position or location that was targeted
                    foreach (var location in Locations)
                    {
                        if (location.DetectHit(mouseScreenPos))
                        {
                            if (travelTargetLocation == null || travelTargetLocation.WorldPosition.Y < location.WorldPosition.Y) travelTargetLocation = location;
                        }
                    }
                    if (travelTargetLocation == null) travelTarget = mouseWorldPos;
                    else travelTarget = travelTargetLocation.WorldPosition;
                    
                    if (travelTargetLocation != null) PosessedTraveller.TravelToLocation(travelTargetLocation);
                    else PosessedTraveller.TravelTo(travelTarget);
                    
                }

                foreach (var hoverable in Hoverables)
                {
                    if (hoverable.DetectHit(mouseScreenPos)) hoverable.Hovered = true;
                    else hoverable.Hovered = false;
                }


            }
        }
    }
}