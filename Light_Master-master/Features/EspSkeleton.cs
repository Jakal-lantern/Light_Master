using System.Drawing;
using LightCheat.Data.Internal;
using LightCheat.Data.Raw;
using LightCheat.Gfx;
using Graphics = LightCheat.Gfx.Graphics;

namespace LightCheat.Features
{
    /*
     * Esp Skeleton
     */
    public static class EspSkeleton
    {
        // Draw skeleton
        public static void Draw(Graphics graphics)
        {
            foreach (var entity in graphics.GameData.Entities)
            {
                // Validate
                if (!entity.IsAlive() || entity.AddressBase == graphics.GameData.Player.AddressBase)
                {
                    continue;
                }

                // Draw
                var color = entity.Team == Team.Terrorists ? Color.Gold : Color.DodgerBlue;
                Draw(graphics, entity, color);
            }
        }

        // Draw skeleton of given entity
        public static void Draw(Graphics graphics, Entity entity, Color color)
        {
            for (var i = 0; i < entity.SkeletonCount; i++)
            {
                var (from, to) = entity.Skeleton[i];

                // Validate
                if (from == to || from < 0 || to < 0 || from >= Offsets.MAXSTUDIOBONES || to >= Offsets.MAXSTUDIOBONES)
                {
                    continue;
                }

                // Draw
                graphics.DrawPolyLineWorld(color, entity.BonesPos[from], entity.BonesPos[to]);
            }
        }

    }
}
