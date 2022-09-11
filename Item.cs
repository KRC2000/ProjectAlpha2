using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;


namespace ProjectAlpha2
{
    public class Item
    {
        public string Name { get; private set; } = null;
        public ItemId Id { get; private set; }

        public Item(ItemId id)
        {
            Id = id;
        }

        public void SetCustomName(string name)
        {
            Name = name;
        }

        public string GetName()
        {
            if (Name != null) return Name;

            return GetNameById(Id);
        }

        public static string GetNameById(ItemId id)
        {
            
            switch (id)
            {
                case ItemId.Unknown:
                    return "Unknown item";
                case ItemId.Brick:
                    return "Brick";
                case ItemId.Stick:
                    return "Stick";
                case ItemId.JarOfPickles:
                    return "Jar of pickles";
                default:
                    return "Unknown item";
            }
        }
    }

   

    public enum ItemId
    {
        Unknown, Brick, Stick, JarOfPickles, 
        Amount
    }
}