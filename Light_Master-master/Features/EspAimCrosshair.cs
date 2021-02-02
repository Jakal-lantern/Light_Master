using Microsoft.DirectX;
using LightCheat.Gfx;
using LightCheat.Utils;

namespace LightCheat.Features
{
    /*
     * ESP Aim Crosshair
     */
    class EspAimCrosshair
    {
        // Get aim corssahir in screen space
        public static Vector3 GetPositionScreen(Graphics graphics)
        {
            var screenSize = graphics.GameProcess.WindowRectangleClient.Size;
            var aspectRatio = (double)screenSize.Width / screenSize.Height;
            var player = graphics.GameData.Player;
            var fovY = ((double)player.Fov).DegreeToRadian();
            var fovX = fovY * aspectRatio;
            var punchX = ((double)player.AimPunchAngle.X * Offsets.weapon_recoil_scale).DegreeToRadian();
            var punchY = ((double)player.AimPunchAngle.Y * Offsets.weapon_recoil_scale).DegreeToRadian();
            var pointClip = new Vector3
            (
                (float)(-punchY / fovX),
                (float)(-punchX / fovY), 
                0
            );
            return player.MatrixViewport.Transform(pointClip);
        }

        // Draw aim crosshair
        public static void Draw(Graphics graphics)
        {
            Draw(graphics, GetPositionScreen(graphics));
        }

        private static void Draw(Graphics graphics, Vector3 pointScreen)
        {
            const int radius = 3;
            var color = System.Drawing.Color.LawnGreen;
            graphics.DrawPolyLineScreen(new[] { pointScreen - new Vector3(radius, 0, 0), pointScreen + new Vector3(radius + 1, 0, 0), }, color);
            graphics.DrawPolyLineScreen(new[] { pointScreen - new Vector3(0, radius, 0), pointScreen + new Vector3(0, radius + 1, 0), }, color);
        }
    }
}
