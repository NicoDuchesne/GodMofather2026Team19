using System;
using BitDuc.Support.Meshes;
using UnityEngine;

namespace BitDuc.Support.VectorGizmos
{
    [Serializable]
    public class ArrowGizmo
    {
        public Color color = new(.25f, .5f, 1f);
        public float baseLength = 1f;

        [Min(0)]
        public float thickness = .08f;

        public float height = 0.01f;

        const float BaseThickness = 0.08f;
        const float HeadLenght = .25f;
        const float HeadCarve = .03f;

        Quaternion lastValidOrientation = Quaternion.identity;
        Mesh head;
        Mesh body;

        public ArrowGizmo(
            Color? color = null,
            float? thickness = null
        ) {
            this.color = color ?? this.color;
            this.thickness = thickness ?? this.thickness;
        }

        public void Rebuild()
        {
            var headLength = ComputeHeadLength();
            var neckPosition = -headLength;
            var flapsWidth = thickness * 2f;
            var neck = new Segment(neckPosition, 0f, thickness);
            var flaps = new Segment(neckPosition, 0f, thickness);
            var tip = new Segment(0f, -HeadCarve - headLength, flapsWidth);

            head = StripeMesh.Build(new[] { neck, flaps, tip });
            var vertices = head.vertices;
            head.vertices = vertices;
            body = QuadMesh.Build(1f, 1f);
        }

        public void Draw(Vector3 origin, Vector3 vector)
        {
            vector = !Application.isPlaying && vector == Vector3.zero? Vector3.forward : vector;
            lastValidOrientation = vector.sqrMagnitude < 0.00001f ?
                lastValidOrientation :
                Quaternion.LookRotation(vector);

            var colorBackup = Gizmos.color;
            Gizmos.color = color;

            var position = origin + Vector3.up * height;
            Gizmos.matrix = Matrix4x4.Translate(position + vector);
            Gizmos.matrix *= Matrix4x4.Rotate(lastValidOrientation);
            Gizmos.DrawMesh(head, Vector3.zero, Quaternion.identity);

            var bodyLength = vector.magnitude - ComputeHeadLength();
            var halfBodyLenght = Vector3.forward * (bodyLength * 0.5f);
            var scale = new Vector3(thickness, 1, bodyLength);
            var matrixBackup = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.Translate(position);
            Gizmos.matrix *= Matrix4x4.Rotate(lastValidOrientation);
            Gizmos.DrawMesh(body, halfBodyLenght, Quaternion.identity, scale);
            Gizmos.matrix = matrixBackup;

            Gizmos.color = colorBackup;
        }

        float ComputeHeadLength()
        {
            var extraThickness = thickness - BaseThickness;
            var headLength = HeadLenght + extraThickness;
            return headLength;
        }
    }
}
