using UnityEngine;

namespace BitDuc.Support
{
    public readonly struct FatLine
    {
        public bool IsDue => Time.time >= deadline;

        readonly Vector3 from;
        readonly Vector3 to;
        readonly Color color;
        readonly float thickness;
        readonly float deadline;

        public FatLine(Vector3 @from, Vector3 to, Color color, float thickness, float duration) {
            this.from = from;
            this.to = to;
            this.color = color;
            this.thickness = thickness;
            deadline = Time.time + duration;
        }

        public void Draw()
        {
            var position = Vector3.Lerp(from, to, .5f);
            var rotation = Quaternion.LookRotation(to - from, Vector3.up);
            var size = new Vector3(thickness, thickness, (to - from).magnitude);

            Gizmos.color = color;
            Gizmos.matrix = Matrix4x4.Translate(position) * Matrix4x4.Rotate(rotation);
            Gizmos.DrawCube(Vector3.zero, size);
        }
    }
}
