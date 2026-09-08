using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BitDuc.Support.Meshes
{
    public static class StripeMesh
    {
        public static Mesh Build(Segment[] segments)
        {
            var mesh = CreateMesh(segments);
            mesh.RecalculateNormals();
            return mesh;
        }

        public static Vector3[] Vertices(Segment[] segments) =>
            segments.SelectMany(segment => segment.Vertices).ToArray();

        static Mesh CreateMesh(Segment[] segments) => new() {
            name = "StripeMesh",
            vertices = Vertices(segments),
            triangles = Triangles(segments).ToArray(),
            normals = Normals(segments).ToArray(),
            uv = UVs(segments).ToArray()
        };

        static IEnumerable<int> Triangles(Segment[] segments) =>
            segments.Take(segments.Length - 1)
                .SelectMany((segment, index) => segment.Triangles(index));

        static IEnumerable<Vector3> Normals(Segment[] segments) =>
            segments.SelectMany(segment => segment.Normals);

        static IEnumerable<Vector2> UVs(Segment[] segments) =>
            segments.SelectMany(segment => segment.UVs);
    }
}
