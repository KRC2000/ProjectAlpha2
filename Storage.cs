using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ImGuiNET;
using MonoGame.Extended;


namespace ProjectAlpha2
{
    public class Storage
    {
        public string Name { get; private set; } = "Storage";
        public List<Item> Items { get; private set; } = new List<Item>();

        public void AddItem(Item item)
        {
            Items.Add(item);
        }

        public void DrawGui()
        {
            ImGui.Begin(Name);
            
            ImGui.End();
        }
    }
}