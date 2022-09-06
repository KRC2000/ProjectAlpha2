using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;


namespace ProjectAlpha2
{
    public class Item
    {
        public ItemId Id { get; private set; }

        public Item(ItemId id)
        {
            Id = id;
        }

        public string GetName()
        {
            switch (Id)
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
        Unknown, Brick, Stick, JarOfPickles
    }
}