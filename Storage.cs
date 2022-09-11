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

        public Dictionary<ItemId, List<Item>> ItemLists { get; private set; } =
                                                new Dictionary<ItemId, List<Item>>();

        public Storage(){}
        public Storage(bool populateRandom)
        {
            Random rand = new Random();
            int count = rand.Next(1,10);
            

            for (int i = 0; i < count; i++)
            {
                int id = rand.Next(0,(int)ItemId.Amount);
                AddItem(new Item((ItemId)id));
            }
        }

        public void AddItem(Item item)
        {
            if (!ItemLists.ContainsKey(item.Id))
                ItemLists.Add(item.Id, new List<Item>());
                
            ItemLists[item.Id].Add(item);
        }

        public void RemoveItem(Item item)
        {
            if (ItemLists.ContainsKey(item.Id))
                ItemLists[item.Id].Remove(item);
        }
    }
}