using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BitDuc.Support.Meshes
{
    public static class RingMesh
    {
        public static Mesh Build(int sides, Circle[] rings)
        {
            var mesh = CreateMesh(sides, rings);
            mesh.RecalculateNormals();
            return mesh;
        }

        public static Vector3[] Vertices(int sides, Circle[] rings) =>
            rings.SelectMany(ring => ring.Vertices(sides)).ToArray();

        static Mesh CreateMesh(int sides, Circle[] rings) => new() {
            name = "CircularMesh",
            vertices = Vertices(sides, rings),
            triangles = Triangles(sides, rings).ToArray(),
            normals = Normals(sides, rings).ToArray(),
            uv = UVs(sides, rings).ToArray()
        };

        static IEnumerable<int> Triangles(int sides, Circle[] rings) =>
            rings.Take(rings.Length - 1)
                .SelectMany((ring, index) => ring.Triangles(sides, index * (sides + 1)));

        static IEnumerable<Vector3> Normals(int sides, Circle[] rings) =>
            rings.SelectMany(ring => ring.Normals(sides));

        static IEnumerable<Vector2> UVs(int sides, Circle[] rings) =>
            rings.SelectMany(ring => ring.UVs(sides));
    }
}