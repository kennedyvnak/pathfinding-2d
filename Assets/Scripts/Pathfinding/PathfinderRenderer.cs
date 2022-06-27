using UnityEngine;

namespace Kennedy.UnityUtility.Pathfinding
{
    // TODO: Improve render mode
    [RequireComponent(typeof(Pathfinder))]
    [ExecuteInEditMode]
    public class PathfinderRenderer : MonoBehaviour
    {
        private Pathfinder _pathfinder;

        private void Awake()
        {
            _pathfinder = GetComponent<Pathfinder>();
        }

        private void Start()
        {
        }

        private void OnDrawGizmos()
        {
            float minX = _pathfinder.GraphOffset.x, maxX = _pathfinder.GraphWidth * _pathfinder.GraphCellSize + _pathfinder.GraphOffset.x;
            float minY = _pathfinder.GraphOffset.y, maxY = _pathfinder.GraphHeight * _pathfinder.GraphCellSize + _pathfinder.GraphOffset.y;

            Gizmos.color = new Color(1, 0.92f, 0.016f, .5f);
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

            if (_pathfinder.graph == null) return;

            Vector2 size = new Vector2(.5f, .5f) * _pathfinder.graph.cellSize;
            foreach (PathNode node in _pathfinder.graph.nodes)
            {
                Gizmos.color = node.walkable ? Color.cyan : Color.red;
                Gizmos.DrawWireCube(node.worldPosition, size);
            }
        }
    }
}