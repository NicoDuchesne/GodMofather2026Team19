using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BitDuc.Support.Meshes
{
    public static class QuadMesh
    {
        public static Mesh Build(float width, float length)
        {
            var mesh = CreateMesh(width, length);
            mesh.RecalculateNormals();
            return mesh;
        }

        public static Vector3[] Vertices(float width, float length)
        {
            var halfWidth = width * .5f;
            var halfLength = length * .5f;
            return new Vector3[] {
                new(-width * .5f, 0, -halfLength),
                new(halfWidth, 0, -halfLength),
                new(-width * .5f, 0, halfLength),
                new(halfWidth, 0, halfLength)
            };
        }

        static Mesh CreateMesh(float width, float length) => new() {
            name = "QuadMesh",
            vertices = Vertices(width, length),
            triangles = new[] { 0, 2, 3, 0, 3, 1 },
            normals = Normals(),
            uv = UVs()
        };

        static Vector3[] Normals() =>
            Enumerable.Range(0, 4).Select(_ => Vector3.up).ToArray();

        static Vector2[] UVs() => new Vector2[] {
            new(0, 0),
            new(1, 0),
            new(0, 1),
            new(1, 1)
        };
    }
}