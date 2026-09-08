using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BitDuc.Support.Meshes
{
    [Serializable]
    public class Circle
    {
        [Min(0)]
        public float radius = 1f;
        public float height = 0f;
        public float uvHeight = 1f;
        
        public Circle() {}

        public Circle(float radius, float height)
        {
            this.radius = radius;
            this.height = height;
        }

        public IEnumerable<Vector3> Vertices(int sides) =>
            Enumerable.Range(0, sides + 1)
                .Select(index => AngleFromIndex(sides, index))
                .Select(VertexFromAngle);

        public IEnumerable<int> Triangles(int sides, int baseIndex) =>
            Enumerable.Range(0, sides)
                .SelectMany(index => TrianglesFromIndex(sides, index))
                .Select(index => baseIndex + index);

        public IEnumerable<Vector2> UVs(int sides) =>
            Enumerable.Range(0, sides + 1)
                .Select(index => UFromIndex(index, sides + 1))
                .Select(u => new Vector2(u, uvHeight));

        public IEnumerable<Vector3> Normals(int sides) =>
            Enumerable.Range(0, sides + 1)
                .Select(index => Vector3.up);

        Vector3 VertexFromAngle(float angle) =>
            radius * new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) + Vector3.up * height;

        static float AngleFromIndex(int sides, int index) => 2 * Mathf.PI * index / sides;

        static int[] TrianglesFromIndex(int sides, int index) => new[] {
            index, index + 1, index + sides + 1,
            index + sides + 1, index + 1, index + 1 + sides + 1
        };

        float UFromIndex(int index, int count) => (float) index / (count - 1);
    }
}