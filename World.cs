using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;
using MonoGame.Extended.Input;


namespace ProjectAlpha2
{
    public static class World
    {
        public static bool IsPaused { get; set; } = false;



        public static List<Location> Locations { get; private set; } = new List<Location>();
        public static List<Traveller> Travellers { get; private set; } = new List<Traveller>();

        public static List<Hoverable> Hoverables { get; private set; } = new List<Hoverable>();

        public static void Generate()
        {
            Locations.Add(new Location(){WorldPosition = new Vector2(30, 60)});
            Locations.Add(new Location(){WorldPosition = new Vector2(140, 80)});
            Locations.Add(new Location(){WorldPosition = new Vector2(150, 90)});

            Traveller t1 = new Traveller(){WorldPosition = new Vector2(200, 100), PossessedByPlayer = true, Name = "Nikolai"};
            Traveller t2 = new Traveller(){WorldPosition = new Vector2(200, 400), PossessedByPlayer = true, Name = "Konstantin"};

            t1.SetAvatar(TextureId.AvatarCat);
            t2.SetAvatar(TextureId.AvatarElephant);

            t1.Inventory.AddItem(new Item(ItemId.JarOfPickles));
            t1.Inventory.AddItem(new Item(ItemId.JarOfPickles));
            t1.Inventory.AddItem(new Item(ItemId.Brick));
            t1.Inventory.AddItem(new Item(ItemId.Stick));
            t1.Inventory.AddItem(new Item(ItemId.JarOfPickles));
            t1.Inventory.AddItem(new Item(ItemId.JarOfPickles));
            t1.Inventory.AddItem(new Item(ItemId.JarOfPickles));

            t2.Inventory.AddItem(new Item(ItemId.Stick));
            t2.Inventory.AddItem(new Item(ItemId.Stick));
            t2.Inventory.AddItem(new Item(ItemId.Stick));
            
            t1.TravelTo(new Vector2(500, 500));
            Travellers.Add(t1);
            Travellers.Add(t2);

            Hoverables.AddRange(Locations);
            Hoverables.AddRange(Travellers);
        }

        public static void LoadCotent()
        {
            foreach (var location in Locations)
            {
                location.LoadResources();
            }

            foreach (var traveller in Travellers)
            {
                traveller.LoadResources();
            }
            
        }

        

        public static void Update(GameTime delta)
        {
            PlayerInputProcessing();

            foreach (var traveller in Travellers)
            {
                traveller.Update(delta, IsPaused);
            }
        }

        // Get all players that are controlled by player and can receive orders
        public static List<Traveller> GetPlayerPossessedTravellers()
        {
            List<Traveller> possessedTravelers = new List<Traveller>();
            foreach (var traveller in Travellers)
            {
                if (traveller.PossessedByPlayer) possessedTravelers.Add(traveller);
            }

            return possessedTravelers;
        }

        public static List<Traveller> GetSelectedTravellers()
        {
            List<Traveller> selectedTravelers = new List<Traveller>();
            foreach (var traveller in Travellers)
            {
                if (traveller.Selected) selectedTravelers.Add(traveller);
            }

            return selectedTravelers;
        }

        public static void UnselectAll()
        {
            foreach (var traveller in Travellers)
            {
                traveller.Selected = false;
            }
        }

        public static void Draw(SpriteBatch batch)
        {
            batch.Begin(SpriteSortMode.Deferred, null, SamplerState.AnisotropicClamp, null, null, null, Game1.MainCamera.GetViewMatrix());
            batch.Draw(ResourceManager.GetTextureBinding(TextureId.Terrain).Item2, new Vector2(), Color.White);
            batch.End();

            batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            //Game1.MainCamera.

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

                    // Make all selected travellers travel to cursor on click
                    foreach (var traveller in GetSelectedTravellers())
                    {
                        if (travelTargetLocation != null) traveller.TravelToLocation(travelTargetLocation);
                        else traveller.TravelTo(travelTarget);
                    }
                }

                foreach (var hoverable in Hoverables)
                {
                    if (hoverable.DetectHit(mouseScreenPos)) hoverable.Hovered = true;
                    else hoverable.Hovered = false;
                }


                if (InputManager.GetMouseState().WasButtonJustUp(MouseButton.Left))
                {
                    foreach (var traveller in Travellers)
                    {
                        if (traveller.DetectHit(mouseScreenPos))
                        {
                            if (InputManager.GetKeyboardState().IsKeyDown(Keys.LeftControl))
                            {
                                traveller.Selected = true;
                            }
                            else
                            {
                                UnselectAll();
                                traveller.Selected = true;
                            }
                        }
                    }
                }
            }
        }
    }
}