using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectAlpha2
{
    public class Patch
    {
        public List<VertexPositionColorTexture> Vertices;

        public Vector2 TextureScaling = new Vector2(50, 50);
        private VertexBuffer vertexBuffer;
        private BasicEffect basicEffect;

        /// <summary>
        /// Points must be described in the clockwise order!
        /// </summary>
        /// <param name="points"></param>
        /// <param name="device"></param>
        public Patch(Vector2 pos, List<Vector2> points, GraphicsDevice device)
        {
            if (points.Count < 3) throw new System.Exception("Can't create patch from the shape with less then 3 points");

            GenTriangulatedMesh(pos, points);

            vertexBuffer = new VertexBuffer(device, typeof(VertexPositionColorTexture), Vertices.Count, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColorTexture>(Vertices.ToArray());

            basicEffect = new BasicEffect(device);
            basicEffect.Alpha = 1f;
            basicEffect.LightingEnabled = false;
            basicEffect.VertexColorEnabled = true;
            basicEffect.TextureEnabled = true;


        }

        public void SetTexture(Texture2D texture)
        {
            basicEffect.Texture = texture;
        }

        // Only works for convex shapes
        private void GenTriangulatedMesh(Vector2 pos, List<Vector2> points)
        {
            Vertices = new List<VertexPositionColorTexture>();

            for (int i = 1; i < points.Count - 1; i++)
            {
                Vertices.Add(new VertexPositionColorTexture(new Vector3(points[0].X + pos.X, -points[0].Y - pos.Y, 0), Color.White,
                                new Vector2((points[0].X + pos.X) / TextureScaling.X, (points[0].Y + pos.Y) / TextureScaling.Y)));
                Vertices.Add(new VertexPositionColorTexture(new Vector3(points[i].X + pos.X, -points[i].Y - pos.Y, 0), Color.White,
                                new Vector2((points[i].X + pos.X) / TextureScaling.X, (points[i].Y + pos.Y) / TextureScaling.Y)));
                Vertices.Add(new VertexPositionColorTexture(new Vector3(points[i + 1].X + pos.X, -points[i + 1].Y - pos.Y, 0), Color.White,
                                new Vector2((points[i + 1].X + pos.X) / TextureScaling.X, (points[i + 1].Y + pos.Y) / TextureScaling.Y)));

            }
        }

        public void Draw(GraphicsDevice device, Matrix projectionMatrix, Matrix viewMatrix, Matrix worldMatrix)
        {
            basicEffect.Projection = projectionMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.World = worldMatrix;
            basicEffect.TextureEnabled = (basicEffect.Texture == null) ? false : true;

            device.BlendState = BlendState.Opaque;
            device.DepthStencilState = DepthStencilState.DepthRead;
            device.SamplerStates[0] = SamplerState.PointWrap;

            device.SetVertexBuffer(vertexBuffer);

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawPrimitives(PrimitiveType.TriangleList, 0, Vertices.Count / 3);
            }

        }
    }
}