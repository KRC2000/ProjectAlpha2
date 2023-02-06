using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectAlpha2
{
    public class Area: Patch
    {
        public AreaDefinition Definition = null;
        public Area(AreaDefinition definition, Vector2 pos, List<Vector2> points, GraphicsDevice device):
        base(pos, points, device)
        {
            Definition = definition;
            SetTexture(ResourceManager.GetTextureBinding(Definition.Resource).Item2);
            //SetTexture()
            //Definition.

            
            // ResourceManager.
            // SetTexture()
        }
    }
}