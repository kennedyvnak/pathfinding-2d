using UnityEngine;

namespace Kennedy.UnityUtility.Pathfinding
{
    // TODO: Add lines in mesh
    [RequireComponent(typeof(Pathfinder))]
    public class PathfinderRenderer : MonoBehaviour
    {
        [SerializeField] private MeshFilter m_Filter;

        private Pathfinder _pathfinder;
        private Mesh _mesh;
        private Vector3[] _vertices;
        private Vector2[] _uv;
        private int[] _tris;

        public Pathfinder pathfinder
        {
            get
            {
                if (!_pathfinder)
                    _pathfinder = GetComponent<Pathfinder>();
                return _pathfinder;
            }
        }

        public Mesh GeneratedMesh
        {
            get
            {
                if (!_mesh)
                    GenerateMesh();
                return _mesh;
            }
        }

        private void Start()
        {
            GenerateMesh();
            if (pathfinder.graph != null)
                pathfinder.graph.NodeChanged += UpdateNode;
        }

        private void OnDestroy()
        {
            if (_mesh)
                DestroyImmediate(_mesh);
        }

        public void GenerateMesh()
        {
            if (!_mesh)
            {
                _mesh = new Mesh();
                _mesh.name = "Pathfinder-graph";
            }

            int width = pathfinder.GraphWidth;
            int height = pathfinder.GraphHeight;

            MeshUtility.CreateEmptyMeshData(width * height, out _vertices, out _uv, out _tris);

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    UpdateNode(x, y);

            RebuildMesh();

            if (m_Filter && m_Filter.sharedMesh != _mesh)
                m_Filter.sharedMesh = _mesh;
        }

        private void RebuildMesh()
        {
            _mesh.Clear(false);
            _mesh.vertices = _vertices;
            _mesh.uv = _uv;
            _mesh.triangles = _tris;
        }

        private void UpdateNode(PathNode node)
        {
            UpdateNode(node.position.x, node.position.y);
            RebuildMesh();
        }

        private void UpdateNode(int x, int y)
        {
            CellPosition cell = new CellPosition(x, y);
            bool walkable = pathfinder.graph != null && pathfinder.graph.GetNode(cell).walkable;

            Vector2 uv0 = new Vector2(walkable ? 0.0f : 0.5f, 1f);
            Vector2 uv1 = new Vector2(uv0.x + 0.5f, 1f);
            Vector2 uv2 = new Vector2(uv1.x, 1f);
            Vector2 uv3 = new Vector2(uv0.x, 0f);

            Vector2 position = pathfinder.GetCellWorldPosition(cell);
            position += new Vector2(.5f, .5f) * pathfinder.GraphCellSize;
            int index = x + y * pathfinder.GraphWidth;

            MeshUtility.AddQuadToMeshData(_vertices, _uv, _tris, index, position, pathfinder.GraphCellSize, uv0, uv1, uv2, uv3);
        }
    }
}