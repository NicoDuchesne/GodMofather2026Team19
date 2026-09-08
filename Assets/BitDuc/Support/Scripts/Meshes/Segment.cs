using System;
using UnityEngine;

namespace BitDuc.Support.Meshes
{
    [Serializable]
    public class Segment
    {
        public float position;
        public float sidesOffset;

        [Min(0)]
        public float width;

        public Segment() {}

        public Segment(float position, float sidesOffset, float width)
        {
            this.position = position;
            this.sidesOffset = sidesOffset;
            this.width = width;
        }

        public Vector3[] Vertices => new[]
        {
            new Vector3(-width * .5f, 0f, position + sidesOffset),
            new Vector3(0f, 0f, position),
            new Vector3(width * .5f, 0f, position + sidesOffset),
        };

        public Vector3[] Normals => new[]
        {
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 1f, 0f),
        };

        public Vector2[] UVs => new[]
        {
            new Vector2(0f, 0f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
        };

        public int[] Triangles(int index) => TriangleFromBase(index * 3);

        static int[] TriangleFromBase(int baseIndex) => new[]
        {
            baseIndex + 0, baseIndex + 3, baseIndex + 4,
            baseIndex + 0, baseIndex + 4, baseIndex + 1,
            baseIndex + 1, baseIndex + 4, baseIndex + 2,
            baseIndex + 4, baseIndex + 5, baseIndex + 2,
        };
    }
}
