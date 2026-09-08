using System;
using BitDuc.Support.Meshes;
using UnityEngine;

namespace BitDuc.Support.VectorGizmos
{
    [Serializable]
    public class DirectedRingGizmo
    {
        const int VertexCount = 28;
        const int PeakIndex = 36;

        public Color color = new(.25f, .5f, 1f);
        public float radius = 0.4f;
        public float thickness = 0.08f;
        public float peak = 0.2f;
        public float height = 0.01f;

        Quaternion lastValidOrientation = Quaternion.identity;
        Mesh mesh;

        public DirectedRingGizmo(
            Color? color = null,
            float? radius = null,
            float? thickness = null,
            float? peak = null,
            float? height = null
        ) {
            this.color = color ?? this.color;
            this.radius = radius ?? this.radius;
            this.thickness = thickness ?? this.thickness;
            this.peak = peak ?? this.peak;
            this.height = height ?? this.height;
        }

        public void Rebuild()
        {
            var inner = new Circle(radius - thickness, height);
            var outer = new Circle(radius + thickness, height);
            mesh = RingMesh.Build(VertexCount, new[] { inner, outer });
            var vertices = mesh.vertices;
            vertices[PeakIndex] += new Vector3(0, 0, peak);
            mesh.vertices = vertices;
            mesh.MarkDynamic();
        }

        public void Draw(Vector3 center, Vector3 direction)//TODO make mandatory
        {
            lastValidOrientation = direction.sqrMagnitude < 0.00001f ?
                lastValidOrientation :
                Quaternion.LookRotation(direction);

            var backup = Gizmos.color;
            Gizmos.color = color;
            Gizmos.DrawMesh(mesh, center, lastValidOrientation);
            Gizmos.color = backup;
        }
    }
}