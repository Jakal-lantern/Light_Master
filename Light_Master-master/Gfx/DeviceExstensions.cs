using System.Drawing;
using System.Linq;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using LightCheat.Utils;

namespace LightCheat.Gfx
{
    /*
     * Graphics device drawing extensions
     */
    public static class DeviceExstensions
    {
        // Draw polyline in world space
        public static void DrawPolyLineWorld(this Graphics graphics, Color color, params Vector3[] verticesWorld)
        {
            var verticesScreen = verticesWorld.Select(v => graphics.GameData.Player.MatrixViewProjectionViewport.Transform(v)).ToArray();
            graphics.DrawPolyLineScreen(verticesScreen, color);
        }


        // Draw 2D polyline in screen space
        public static void DrawPolyLineScreen(this Graphics graphics, Vector3[] vertices, Color color)
        {
            if (vertices.Length < 2 || vertices.Any(v => !v.IsValidScreen()))
            {
                return;
            }

            var vertexStreamZeroData = vertices.Select(v => new CustomVertex.TransformedColored(v.X, v.Y, v.Z, 0, color.ToArgb())).ToArray();
            graphics.Device.VertexFormat = VertexFormats.Diffuse | VertexFormats.Transformed;
            graphics.Device.DrawUserPrimitives(PrimitiveType.LineStrip, vertexStreamZeroData.Length - 1, vertexStreamZeroData);
        }
    }
}
