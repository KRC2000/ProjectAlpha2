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

        public Storage(){}
        public Storage(bool populateRandom)
        {
            int count = Random.Shared.Next(1, 10);

            for (int i = 0; i < count; i++)
            {
                int id = Random.Shared.Next(0, (int)ItemId.Amount);
                Items.Add(new Item((ItemId)id));
            }
        }

        public void AddItem(Item item)
        {
            Items.Add(item);
        }

    }
}