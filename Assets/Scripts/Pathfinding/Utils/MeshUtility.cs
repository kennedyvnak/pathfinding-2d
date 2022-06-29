using UnityEngine;

namespace Kennedy.UnityUtility.Pathfinding
{
    public static class MeshUtility
    {
        // vertices rotations
        private static readonly Quaternion k_vr0 = Quaternion.Euler(0, 0, -270);
        private static readonly Quaternion k_vr1 = Quaternion.Euler(0, 0, -180);
        private static readonly Quaternion k_vr2 = Quaternion.Euler(0, 0, -90);
        private static readonly Quaternion k_vr3 = Quaternion.Euler(0, 0, 0);

        public static void CreateEmptyMeshData(int quadCount, out Vector3[] vertices, out Vector2[] uv, out int[] tris)
        {
            vertices = new Vector3[4 * quadCount];
            uv = new Vector2[4 * quadCount];
            tris = new int[6 * quadCount];
        }

        public static void AddQuadToMeshData(Vector3[] vertices, Vector2[] uv, int[] tris, int index, Vector3 pos, float size, Vector2 uv0, Vector2 uv1, Vector2 uv2, Vector2 uv3)
        {
            // relocate vertices
            int vIndex = index * 4;
            int vIndex0 = vIndex;
            int vIndex1 = vIndex + 1;
            int vIndex2 = vIndex + 2;
            int vIndex3 = vIndex + 3;

            var baseSize = new Vector2(size, size) * 0.5f;

            vertices[vIndex0] = pos + k_vr0 * baseSize;
            vertices[vIndex1] = pos + k_vr1 * baseSize;
            vertices[vIndex2] = pos + k_vr2 * baseSize;
            vertices[vIndex3] = pos + k_vr3 * baseSize;

            // relocate UVs
            uv[vIndex0] = uv0;
            uv[vIndex1] = uv1;
            uv[vIndex2] = uv2;
            uv[vIndex3] = uv3;

            // create triangles
            int tIndex = index * 6;

            tris[tIndex + 0] = vIndex0;
            tris[tIndex + 1] = vIndex3;
            tris[tIndex + 2] = vIndex1;

            tris[tIndex + 3] = vIndex1;
            tris[tIndex + 4] = vIndex3;
            tris[tIndex + 5] = vIndex2;
        }
    }
}