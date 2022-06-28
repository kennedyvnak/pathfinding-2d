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

        private void Awake()
        {
            _pathfinder = GetComponent<Pathfinder>();
        }

        private void Start()
        {
            UpdateMesh();
            if (_pathfinder.graph != null)
                _pathfinder.graph.NodeChanged += UpdateNode;
        }

        private void OnDestroy()
        {
            if (_mesh) DestroyImmediate(_mesh);
        }

        [ContextMenu("Preview Mesh")]
        public void UpdateMesh()
        {
            if (!m_Filter) return;
            if (!_mesh) _mesh = new Mesh();

            int width = _pathfinder.GraphWidth;
            int height = _pathfinder.GraphHeight;

            MeshUtility.CreateEmptyMeshArrays(width * height, out _vertices, out _uv, out _tris);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    UpdateNode(x, y);
                }
            }

            RebuildMesh();

            m_Filter.mesh = _mesh;
        }

        private void RebuildMesh()
        {
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
            bool walkable = _pathfinder.graph != null && _pathfinder.graph.GetNode(cell).walkable;
            Vector2 uvVal = new Vector2(walkable ? 0 : 1, 0);
            Vector2 position = _pathfinder.GetCellWorldPosition(cell);
            position += new Vector2(.5f, .5f) * _pathfinder.GraphCellSize;
            int index = x + y * _pathfinder.GraphWidth;

            MeshUtility.AddToMeshArrays(_vertices, _uv, _tris, index, position, 0, Vector2.one * _pathfinder.GraphCellSize, uvVal, uvVal);
        }

        private void OnDrawGizmos()
        {
            if (_pathfinder == null) _pathfinder = GetComponent<Pathfinder>();

            float minX = _pathfinder.GraphOffset.x, maxX = _pathfinder.GraphWidth * _pathfinder.GraphCellSize + _pathfinder.GraphOffset.x;
            float minY = _pathfinder.GraphOffset.y, maxY = _pathfinder.GraphHeight * _pathfinder.GraphCellSize + _pathfinder.GraphOffset.y;

            Gizmos.color = new Color(1, 0.92f, 0.016f, .25f);
            for (int x = 0; x < _pathfinder.GraphWidth; x++)
            {
                float xWorldPos = x * _pathfinder.GraphCellSize + _pathfinder.GraphOffset.x;
                Gizmos.DrawLine(new Vector2(xWorldPos, minY), new Vector2(xWorldPos, maxY));
            }
            for (int y = 0; y < _pathfinder.GraphHeight; y++)
            {
                float yWorldPos = y * _pathfinder.GraphCellSize + _pathfinder.GraphOffset.y;
                Gizmos.DrawLine(new Vector2(minX, yWorldPos), new Vector2(maxX, yWorldPos));
            }

            Gizmos.DrawLine(new Vector2(minX, maxY), new Vector2(maxX, maxY));
            Gizmos.DrawLine(new Vector2(maxX, minY), new Vector2(maxX, maxY));
        }
    }
}