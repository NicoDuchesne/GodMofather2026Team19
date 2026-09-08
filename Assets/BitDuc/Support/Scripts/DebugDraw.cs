using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BitDuc.Support
{
    public class DebugDraw : MonoBehaviour
    {
        static DebugDraw debugDraw;
        const HideFlags Flags = HideFlags.DontSave;
        
        List<FatLine> toDraw = new();
        static readonly Color DefaultColor = Color.blue - new Color(0, 0, 0, 0.5f);

        public static void FatLine(Vector3 from, Vector3 to, Color? color = null, float thickness = .05f, float duration = 0.016f)
        {
            Instance.UpdateQueue(new FatLine(from, to, color ?? DefaultColor, thickness, duration));
        }

        static DebugDraw Instance => debugDraw ??= Instantiate();

        static DebugDraw Instantiate() =>
            new GameObject("DebugDraw") { hideFlags = Flags }
                .AddComponent<DebugDraw>();

        void OnDrawGizmos()
        {
            foreach (var figure in toDraw)
                figure.Draw();

            toDraw = RemoveExpired().ToList();
        }

        void UpdateQueue(FatLine line)
        {
            toDraw = RemoveExpired().Append(line).ToList();
        }

        IEnumerable<FatLine> RemoveExpired() =>
            toDraw.Where(figure => !figure.IsDue);
    }
}