using System;
using System.Numerics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ImGuiNET;
using MonoGame.Extended;

using Vector2 = System.Numerics.Vector2;
using Vector4 = System.Numerics.Vector4;


namespace ProjectAlpha2
{
    public static class UI
    {

        public static void LoadContent()
        {
        }


        public static void Draw()
        {
            DrawOverviewWindow();
            DrawPosessedTravellersList();
            DrawSelectedTravellersOverlay();
            DrawVisitedLocationOverview();
        }

        // private static void DrawInventoryItemSet(KeyValuePair<ItemId, List<Item>> itemListKeypair)
        // {
            
        // }

        private static unsafe void DrawVisitedLocationOverview()
        {
            List<Traveller> selectedTravellers = World.GetSelectedTravellers();
            List<Traveller> travellersWhoAreInside = new List<Traveller>();
            bool someoneInside = false;

            // are there any travellers that are inside a location? Populates "someoneInside"
            // also populates "travellersWhoAreInside"
            if (selectedTravellers.Count > 0)
            {
                foreach (Traveller t in selectedTravellers)
                {
                    if (t.CurrentLocation != null)
                    {
                      someoneInside = true;
                      travellersWhoAreInside.Add(t);
                    }
                }
            }

            if (someoneInside)
            {
                ImGui.Begin("Location");
                ImGui.BeginTabBar("loc_select", ImGuiTabBarFlags.None);
                foreach (Traveller t in travellersWhoAreInside)
                {
                    if (ImGui.BeginTabItem(t.CurrentLocation.Name + $"({t.Name})"))
                    {

                        ImGui.BeginChild("child1", new Vector2(), false, ImGuiWindowFlags.None);

                        foreach (KeyValuePair<ItemId, List<Item>> itemListKeypair in t.CurrentLocation.Inventory.ItemLists)
                        {
                            if (itemListKeypair.Value.Count > 1)
                            {
                                bool treeOpened = ImGui.TreeNodeEx($"    \n\n\n##{itemListKeypair.Key}", ImGuiTreeNodeFlags.SpanAvailWidth);
                                if (ImGui.BeginPopupContextItem($"##{itemListKeypair.Key}"))
                                {
                                    if (ImGui.Button("Pick all"))
                                    {
                                        Storage.MoveItemSet(itemListKeypair.Key, t.CurrentLocation.Inventory, t.Inventory);
                                    }
                                    ImGui.EndPopup();
                                }
                                ImGui.SameLine();
                                ImGui.SetCursorPosX(ImGui.GetCursorPosX() - 35.0f);
                                ImGui.Image(ResourceManager.GetTextureBinding(TextureId.Unknown).Item1, new Vector2(38, 38));
                                ImGui.SameLine();
                                ImGui.Text($"{Item.GetNameById(itemListKeypair.Key)}\n{itemListKeypair.Value.Count}");
                                
                                if (treeOpened)
                                {
                                    foreach (Item item in itemListKeypair.Value)
                                    {
                                        ImGui.Selectable($"{item.GetName()}");

                                        if (ImGui.BeginPopupContextItem($"{item.GetHashCode()}"))
                                        {
                                            if (ImGui.Button("Pick"))
                                            {
                                                Storage.MoveItem(item, t.CurrentLocation.Inventory, t.Inventory);
                                                break;
                                            }

                                            ImGui.EndPopup();
                                        }

                                    }
                                    ImGui.TreePop();
                                }
                            }
                            else
                            {
                                ImGui.Selectable($"\n\n\n##{itemListKeypair.Key}");
                                ImGui.SameLine();
                                ImGui.BeginGroup();
                                ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 14.0f);
                                ImGui.Image(ResourceManager.GetTextureBinding(TextureId.Unknown).Item1, new Vector2(38, 38));
                                ImGui.SameLine();
                                ImGui.Text($"{Item.GetNameById(itemListKeypair.Key)}\n{itemListKeypair.Value.Count}");
                                ImGui.SetCursorPosX(ImGui.GetWindowWidth());
                                ImGui.EndGroup();


                                if (ImGui.BeginPopupContextItem($"##{itemListKeypair.Key}"))
                                {
                                    if (ImGui.Button("Pick"))
                                    {
                                        Storage.MoveItemSet(itemListKeypair.Key, t.CurrentLocation.Inventory, t.Inventory);
                                    }

                                    ImGui.EndPopup();
                                }

                            }

                            ImGui.Separator();
                        }

                        ImGui.EndChild();

                        ImGui.EndTabItem();
                    }
                }
                ImGui.EndTabBar();
                ImGui.End();
            }

        }

        private static unsafe void DrawSelectedTravellersOverlay()
        {
            if (World.GetSelectedTravellers().Count > 0)
            {
                float padding = 10;

                ImGuiWindowFlags window_flags = ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.AlwaysAutoResize |
                                                ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoFocusOnAppearing |
                                                ImGuiWindowFlags.NoNav;

                ImGui.SetNextWindowPos(new Vector2(padding, padding), ImGuiCond.Always, new Vector2());
                ImGui.SetNextWindowBgAlpha(0.35f);

                ImGui.Begin("Selection", window_flags);
                foreach (var selTrav in World.GetSelectedTravellers())
                {
                    ImGui.Text(selTrav.Name);
                }

                ImGui.End();
            }
        }

        private static unsafe void DrawPosessedTravellersList()
        {
            if (World.GetPlayerPossessedTravellers().Count > 0)
            {
                float padding = 10;

                ImGuiWindowFlags window_flags = ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.AlwaysAutoResize |
                                                ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoFocusOnAppearing |
                                                ImGuiWindowFlags.NoNav;

                ImGuiViewportPtr viewport = ImGui.GetMainViewport();
                Vector2 work_size = viewport.WorkSize;

                ImGui.SetNextWindowPos(new Vector2(padding, work_size.Y - padding), ImGuiCond.Always, new Vector2(0f, 1f));
                ImGui.SetNextWindowBgAlpha(0.35f);

                ImGui.Begin("Posessed travellers panel", window_flags);
                foreach (var posTrav in World.GetPlayerPossessedTravellers())
                {
                    ImGui.BeginGroup();

                    if (ImGui.ImageButton(ResourceManager.GetTextureBinding(posTrav.AvatarTextureId).Item1, (posTrav.Selected)? new Vector2(55, 55) : new Vector2(50, 50), 
                                    new Vector2(), new Vector2(1, 1), 0, 
                                    (posTrav.Selected)? new Vector4(0.2f, 0.2f, 0.8f, 0.5f) : Vector4.Zero, // button color
                                    Vector4.One)) // foreground image color
                    {
                        
                        if (!InputManager.GetKeyboardState().IsControlDown())
                        {
                            World.UnselectAll();
                            posTrav.Selected = true;
                        }
                        else
                            posTrav.Selected = true;

                    }
                    ImGui.Text(posTrav.Name);
                    ImGui.EndGroup();

                    ImGui.SameLine();
                }

                ImGui.End();
            }
        }

        private static unsafe void DrawOverviewWindow()
        {
            ImGui.Begin("Travellers");

            ImGui.BeginTabBar("trav_select", ImGuiTabBarFlags.None);
            foreach (var traveller in World.GetSelectedTravellers())
            {
                if (ImGui.BeginTabItem(traveller.Name))
                {

                    ImGui.EndTabItem();
                    ImGui.BeginTabBar("personal", ImGuiTabBarFlags.None);
                    if (ImGui.BeginTabItem("Inventory"))
                    {
                        ImGui.BeginChild("child1", new Vector2(), false, ImGuiWindowFlags.None);

                        foreach (KeyValuePair<ItemId, List<Item>> itemListKeypair in traveller.Inventory.ItemLists)
                        {

                            if (itemListKeypair.Value.Count > 1)
                            {
                                bool treeOpened = ImGui.TreeNodeEx($"    \n\n\n##{itemListKeypair.Key}", ImGuiTreeNodeFlags.SpanAvailWidth);
                                if (ImGui.BeginPopupContextItem($"##{itemListKeypair.Key}"))
                                {
                                    ImGui.Button("Trash all");
                                    if (traveller.CurrentLocation != null) ImGui.Button("Drop all");
                                    ImGui.EndPopup();
                                }
                                ImGui.SameLine();
                                ImGui.SetCursorPosX(ImGui.GetCursorPosX() - 35.0f);
                                ImGui.Image(ResourceManager.GetTextureBinding(TextureId.Unknown).Item1, new Vector2(38, 38));
                                ImGui.SameLine();
                                ImGui.Text($"{Item.GetNameById(itemListKeypair.Key)}\n{itemListKeypair.Value.Count}");
                                
                                if (treeOpened)
                                {
                                    foreach (Item item in itemListKeypair.Value)
                                    {
                                        ImGui.Selectable($"{item.GetName()}");


                                        if (ImGui.BeginPopupContextItem($"{item.GetHashCode()}"))
                                        {
                                            
                                            ImGui.Button("Trash");
                                            if (traveller.CurrentLocation != null)
                                            {
                                                ImGui.Button("drop");
                                            }

                                            ImGui.EndPopup();
                                        }

                                    }
                                    ImGui.TreePop();
                                }
                            }
                            else
                            {
                                ImGui.Selectable($"\n\n\n##{itemListKeypair.Key}");
                                ImGui.SameLine();
                                ImGui.BeginGroup();
                                ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 14.0f);
                                ImGui.Image(ResourceManager.GetTextureBinding(TextureId.Unknown).Item1, new Vector2(38, 38));
                                ImGui.SameLine();
                                ImGui.Text($"{Item.GetNameById(itemListKeypair.Key)}\n{itemListKeypair.Value.Count}");
                                ImGui.SetCursorPosX(ImGui.GetWindowWidth());
                                ImGui.EndGroup();


                                if (ImGui.BeginPopupContextItem($"##{itemListKeypair.Key}"))
                                {
                                    ImGui.Button("Trash");
                                    if (traveller.CurrentLocation != null)
                                    {
                                        ImGui.Button("drop");
                                    }

                                    ImGui.EndPopup();
                                }

                            }

                            ImGui.Separator();

                        }


                        ImGui.EndChild();

                        ImGui.EndTabItem();
                    }
                    if (ImGui.BeginTabItem("Stats"))
                    {

                        ImGui.EndTabItem();
                    }
                    if (ImGui.BeginTabItem("Skills"))
                    {

                        ImGui.EndTabItem();
                    }
                    ImGui.EndTabBar();
                }
            }
            ImGui.End();
        }

    }
}