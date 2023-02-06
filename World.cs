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

        public static List<Patch> Patches;
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

            t1.SetAvatar(TextureId.AvatarCat);
            
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

            Patches = new List<Patch>();
            foreach (var obj in Level.Map.ObjectGroups[0].Objects)
            {
                Patch p = new Patch(new Vector2(obj.X, obj.Y), obj.Polygon.Points, device);
                p.SetTexture(ResourceManager.GetTextureBinding(TextureId.Forest).Item2);
                p.SetTexture(ResourceManager.GetTextureBinding("forest").Item2);
                Patches.Add(p);
                
                if (!string.IsNullOrEmpty(obj.Type) && AreaDefinitions.ContainsKey(obj.Type))
                    Areas.Add(new Area(AreaDefinitions[obj.Type], new Vector2(obj.X, obj.Y), obj.Polygon.Points, device));
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

        // Get all players that are controlled by player and can receive orders
        // public static List<Traveller> GetPlayerPossessedTravellers()
        // {
        //     List<Traveller> possessedTravelers = new List<Traveller>();
        //     foreach (var traveller in Travellers)
        //     {
        //         if (traveller.PossessedByPlayer) possessedTravelers.Add(traveller);
        //     }

        //     return possessedTravelers;
        // }

        // public static List<Traveller> GetSelectedTravellers()
        // {
        //     List<Traveller> selectedTravelers = new List<Traveller>();
        //     foreach (var traveller in Travellers)
        //     {
        //         if (traveller.Selected) selectedTravelers.Add(traveller);
        //     }

        //     return selectedTravelers;
        // }

        // public static void UnselectAll()
        // {
        //     foreach (var traveller in Travellers)
        //     {
        //         traveller.Selected = false;
        //     }
        // }

        public static void Draw(SpriteBatch batch, GraphicsDevice device)
        {
            


            batch.Begin(SpriteSortMode.Deferred, null, SamplerState.AnisotropicClamp, null, null, null, Game1.MainCamera.GetViewMatrix());
            //batch.Draw(ResourceManager.GetTextureBinding(TextureId.Terrain).Item2, new Vector2(), Color.White);
            batch.End();

            // foreach (var patch in Patches)
            foreach (var area in Areas)
            {
                
                Vector3 camPosition = new Vector3(Game1.MainCamera.Center.X, -Game1.MainCamera.Center.Y, 1);
                Vector3 camTarget = new Vector3(camPosition.X, camPosition.Y, camPosition.Z - 1);
                Matrix projectionMatrix = Matrix.CreateOrthographic(device.Viewport.Width / Game1.MainCamera.Zoom, device.Viewport.Height / Game1.MainCamera.Zoom, 0, 1000f);
                Matrix viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);
                Matrix worldMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up);
                //patch.Draw(device, projectionMatrix, viewMatrix, worldMatrix);
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

            



            // Vector3 camTarget = new Vector3(0f, 0f, 0f);
            // Vector3 camPosition = new Vector3(0f, 0f, 1f);
            // Matrix projectionMatrix = Matrix.CreateOrthographic(device.Viewport.Width, device.Viewport.Height, camPosition.Z, camPosition.Z + 1f);
            // Matrix viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, new Vector3(0f, 1f, 0f));
            // Matrix worldMatrix = Matrix.CreateWorld(camTarget, Vector3.Forward, Vector3.Up);
            // Patches[2].Draw(device, projectionMatrix, viewMatrix, worldMatrix);

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