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
            {
                ItemLists[item.Id].Remove(item);
                if (ItemLists[item.Id].Count <= 0)
                    ItemLists.Remove(item.Id);
            }
        }

        public void RemoveItemSet(ItemId id)
        {
            if (ItemLists.ContainsKey(id))
            {
                ItemLists.Remove(id);
            }
        }

        public static void MoveItem(Item item, Storage moveFrom, Storage moveTo)
        {
            moveTo.AddItem(item);
            moveFrom.RemoveItem(item);
        }

        public static void MoveItemSet(ItemId id, Storage moveFrom, Storage moveTo)
        {
            for (int i = moveFrom.ItemLists[id].Count - 1; i >= 0; i--)
            {
                MoveItem(moveFrom.ItemLists[id][i], moveFrom, moveTo);
            }
        }

        public static void MoveAllItems(Storage From, Storage To)
        {
            foreach (var itemSet in From.ItemLists)
            {
                foreach (var item in itemSet.Value)
                {
                    MoveItem(item, From, To);
                }
            }
        }
    }
}