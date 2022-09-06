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
                        ImGuiTableFlags flags = ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.NoHostExtendX |
                                                ImGuiTableFlags.Resizable | ImGuiTableFlags.BordersOuterV |
                                                ImGuiTableFlags.BordersOuter | ImGuiTableFlags.BordersInner |
                                                ImGuiTableFlags.ScrollY;

                        ImGui.BeginChild("child1", new Vector2(), false, ImGuiWindowFlags.None);
                        if (ImGui.BeginTable("split", 4, flags))
                        {
                            ImGui.TableSetupScrollFreeze(0, 1);
                            ImGui.TableSetupColumn("Image");
                            ImGui.TableSetupColumn("Item name");
                            ImGui.TableSetupColumn("Amount");
                            ImGui.TableSetupColumn("Action");
                            ImGui.TableHeadersRow();

                            ImGuiListClipper clipper = new ImGuiListClipper();
                            ImGuiListClipperPtr clipperPtr = new ImGuiListClipperPtr(&clipper);

                            foreach (var item in traveller.Inventory.Items)
                            {
                                ImGui.TableNextRow();
                                ImGui.TableSetColumnIndex(0);


                                ImGui.Image(ResourceManager.GetTextureBinding(TextureId.Unknown).Item1, new Vector2(60, 60));
                                ImGui.TableNextColumn();
                                ImGui.Text(item.GetName());
                                ImGui.TableNextColumn();
                                ImGui.Text("1234");
                                ImGui.TableNextColumn();
                                if (traveller.CurrentLocation != null)
                                {
                                    ImGui.Button("Drop all");
                                    ImGui.Button("Drop..");
                                }

                            }

                            ImGui.EndTable();
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