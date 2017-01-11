using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Grid
{
    public class GridManager : MonoBehaviour
    {
        // ReSharper disable UnassignedField.Global
        // ReSharper disable MemberCanBePrivate.Global
        public Transform player;

        public LayerMask unwalkableMask;

        public float nodeRadius;

        public Vector2 gridWorldSize;

        public bool showGrid = true;
        // ReSharper restore UnassignedField.Global
        // ReSharper restore MemberCanBePrivate.Global

        private float nodeDiameter;

        private int gridSizeX;

        private int gridSizeY;

        public Node[,] Nodes { get; set; }

        private static GridManager instance;

        public static GridManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<GridManager>();
                    if (instance == null)
                    {
                        Debug.Log("GridManager object not found. \nA single GridManager should be in the scene.");
                    }
                }

                return instance;
            }
        }

        [UsedImplicitly]
        private void OnApplicationQuit()
        {
            instance = null;
        }

        [UsedImplicitly]
        private void Start()
        {
            nodeDiameter = nodeRadius * 2;
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
            CreateGrid();
        }

        [UsedImplicitly]
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
            if (showGrid)
            {
                DebugDrawGrid(Color.blue);
            }
        }

        private void CreateGrid()
        {
            Nodes = new Node[gridSizeX, gridSizeY];
            var worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

            for (var i = 0; i < gridSizeX; i++)
            {
                for (var j = 0; j < gridSizeY; j++)
                {
                    var worldPoint = worldBottomLeft + Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.forward * (j * nodeDiameter + nodeRadius);
                    var walkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask);
                    var node = new Node(worldPoint, walkable);
                    Nodes[i, j] = node;
                }
            }
        }

        private Node NodeFromPosition(Vector3 position)
        {
            var percentX = Mathf.Clamp01((position.x + gridWorldSize.x / 2) / gridWorldSize.x);
            var percentY = Mathf.Clamp01((position.z + gridWorldSize.y / 2) / gridWorldSize.y);
            var x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
            var y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

            return Nodes[x, y];
        }

        private void DebugDrawGrid(Color color)
        {
            if (Nodes == null)
            {
                return;
            }

            var playerNode = NodeFromPosition(player.position);
            foreach (var node in Nodes)
            {
                if (playerNode == node)
                {
                    Gizmos.color = color;
                }
                else
                {
                    var c = node.walkable ? Color.green : Color.red;
                    c.a = 0.5f;
                    Gizmos.color = c;
                }

                Gizmos.DrawCube(node.position, new Vector3(nodeDiameter - 0.2f, 0.2f, nodeDiameter - 0.2f));
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(node.position, 0.08f);
            }
        }
    }
}
