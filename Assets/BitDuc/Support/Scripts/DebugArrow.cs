using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace BitDuc.Support
{
    public static class DebugArrow
    {
        public static void Draw(Vector3 from, Vector3 to, Color? color = null, float thickness = 0.05f, float duration = 0.032f)
        {
            var arrow = to - from;
            var arrowSize = arrow.magnitude;
            var headRatio = .1f;
            var headSize = arrowSize * headRatio;
            var headLeft = Vector3.Cross(arrow, Vector3.up).normalized * headSize;
            var headRight = -Vector3.Cross(arrow, Vector3.up).normalized * headSize;

            if (arrowSize < 0.001f)
                return;

            DebugDraw.FatLine(from, to, color, thickness, duration);
            DebugDraw.FatLine(from + arrow * .8f + headLeft, to, color, thickness, duration);
            DebugDraw.FatLine(from + arrow * .8f + headRight, to, color, thickness, duration);
        }
    }
}