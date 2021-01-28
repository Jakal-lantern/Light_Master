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
        // Draw 2D polyline in screen space
        public static void DrawPolyLine(this Device device, Vector3[] vertices, Color color)
        {
            if (vertices.Length < 2 || vertices.Any(v => !v.IsValidScreen()))
            {
                return;
            }

            var vertexStreamZeroData = vertices.Select(v => new CustomVertex.TransformedColored(v.X, v.Y, v.Z, 0, color.ToArgb())).ToArray();
            device.VertexFormat = VertexFormats.Diffuse | VertexFormats.Transformed;
            device.DrawUserPrimitives(PrimitiveType.LineStrip, vertexStreamZeroData.Length - 1, vertexStreamZeroData);
        }
    }
}
