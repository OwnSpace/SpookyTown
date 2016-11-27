using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Transform player;

    public LayerMask unwalkableMask;

    public float nodeRadius;

    private float nodeDiameter;

    private int gridSizeX;

    private int gridSizeY;

    public Vector2 gridWorldSize;

    public int numOfRows;

    public int numOfColumns;

    public float gridCellSize;

    public float gridCellWidth;

    public float gridCellHeight;

    public bool showGrid = true;

    public bool showNodes = true;

    public bool showLabels = true;

    public bool showObstacleBlocks = true;

    public bool nodeLabelsReady;

    // public GameObject GridLabels;

    public Vector3 origin;

    private GameObject[] obstacles;

    public Node[,] Nodes { get; set; }

    private bool nodesReady;

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

    private void OnApplicationQuit()
    {
        instance = null;
    }

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    //public void UpdateObstacles()
    //{
    //    //obstacles are not removed after building distruction; called by saveloadmap after building load
    //    obstacleArray = GameObject.FindGameObjectsWithTag("Obstacle");
    //    DetectObstacles();
    //}

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

        nodesReady = true;
    }

    //private void ResetNodeGrid()
    //{
    //    for (var i = 0; i < Nodes.GetLength(0); i++)
    //    {
    //        for (var j = 0; j < Nodes.GetLength(1); j++)
    //        {
    //            Nodes[i, j].MarkAsFree();
    //        }
    //    }
    //}

    //private void DetectObstacles()
    //{
    //    ShowHideLabels();

    //    ResetNodeGrid();

    //    if (obstacles != null && obstacles.Length > 0)
    //    {
    //        foreach (var data in obstacles)
    //        {
    //            var indexCell = GetGridIndex(data.transform.position);
    //            var col = GetColumn(indexCell);
    //            var row = GetRow(indexCell);
    //            Nodes[row, col].MarkAsObstacle();
    //        }
    //    }
    //}

    //private void ShowHideLabels()
    //{
    //    if (!showLabels && nodeLabelsReady)
    //    {
    //        GridLabels.SetActive(false);
    //    }

    //    if (showLabels)
    //    {
    //        GridLabels.SetActive(true);
    //    }
    //}

    public Vector3 GetGridCellCenter(int index)
    {
        var cellPosition = GetGridCellPosition(index);

        cellPosition.x += gridCellHeight / 2.0f;
        cellPosition.y += gridCellWidth / 2.0f;

        return cellPosition;
    }

    public Vector3 GetGridCellPosition(int index)
    {
        var row = GetRow(index);
        var col = GetColumn(index);

        var xPosInGrid = col * gridCellWidth / 2 - row * gridCellWidth / 2;
        var yPosInGrid = row * gridCellHeight / 2 + col * gridCellHeight / 2;

        return origin + new Vector3(xPosInGrid, yPosInGrid, 0.0f);
    }

    public int GetGridIndex(Vector3 pos)
    {
        var gridPos = GetGridLocation(pos);
        return (int)gridPos.x * numOfColumns + (int)gridPos.y;
    }

    private Vector2 GetGridLocation(Vector3 pos)
    {
        var row = 0;
        var col = 0;
        for (var i = 0; i < Nodes.GetLength(0); i++)
        {
            for (var j = 0; j < Nodes.GetLength(1); j++)
            {
                if (Vector3.Distance(Nodes[i, j].position, pos) < gridCellSize)
                {
                    col = i;
                    row = j;
                    break;
                }
            }
        }

        return new Vector2(col, row);
    }

    public int GetRow(int index)
    {
        var row = index / numOfColumns;
        return row;
    }

    public int GetColumn(int index)
    {
        var col = index % numOfColumns;
        return col;
    }

    //public void GetNeighbours(Node node, ICollection<Node> neighbors)
    //{
    //    var neighborPos = node.position;
    //    var neighborIndex = GetGridIndex(neighborPos);

    //    var row = GetRow(neighborIndex);
    //    var column = GetColumn(neighborIndex);

    //    // SE
    //    var leftNodeRow = row - 1;
    //    var leftNodeColumn = column;
    //    AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);

    //    // NE
    //    leftNodeRow = row;
    //    leftNodeColumn = column + 1;
    //    AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);

    //    // NW
    //    leftNodeRow = row + 1;
    //    leftNodeColumn = column;
    //    AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);

    //    // SW
    //    leftNodeRow = row;
    //    leftNodeColumn = column - 1;
    //    AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);


    //    // S
    //    leftNodeRow = row - 1;
    //    leftNodeColumn = column - 1;
    //    AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);

    //    // E
    //    leftNodeRow = row - 1;
    //    leftNodeColumn = column + 1;
    //    AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);

    //    // N
    //    leftNodeRow = row + 1;
    //    leftNodeColumn = column + 1;
    //    AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);

    //    // W
    //    leftNodeRow = row + 1;
    //    leftNodeColumn = column - 1;
    //    AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);
    //}

    //private void AssignNeighbour(int row, int column, ICollection<Node> neighbors)
    //{
    //    if (row != -1 && column != -1 &&
    //        row < numOfRows &&
    //        column < numOfColumns)
    //    {
    //        var nodeToAdd = Nodes[row, column];
    //        if (!nodeToAdd.isObstacle)
    //        {
    //            neighbors.Add(nodeToAdd);
    //        }
    //    }
    //}

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if (showGrid)
        {
            DebugDrawGrid(transform.position, numOfRows, numOfColumns, gridCellWidth, gridCellHeight, Color.blue);
        }

        // Gizmos.DrawSphere(transform.position, gridRadius);

        //if (showObstacleBlocks)
        //{
        //    var cellSize = new Vector3(gridCellSize, gridCellSize, 1.0f);
        //    if (obstacles != null && obstacles.Length > 0)
        //    {
        //        foreach (var data in obstacles)
        //        {
        //            Gizmos.DrawCube(GetGridCellCenter(GetGridIndex(data.transform.position)), cellSize);
        //        }
        //    }
        //}
    }

    private Node NodeFromPosition(Vector3 position)
    {
        var percentX = Mathf.Clamp01((position.x + gridWorldSize.x / 2) / gridWorldSize.x);
        var percentY = Mathf.Clamp01((position.z + gridWorldSize.y / 2) / gridWorldSize.y);
        var x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        var y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return Nodes[x, y];
    }

    private void DrawGrid()
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
                Gizmos.color = Color.cyan;
            }
            else
            {
                Gizmos.color = node.walkable ? Color.green : Color.red;
            }

            Gizmos.DrawCube(node.position, Vector3.one * (nodeDiameter - 0.2f));
        }
    }

    private void DebugDrawGrid(Vector3 originPos, int numRows, int numCols, float cellWidth, float cellHeight, Color color)
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

        //var width = numCols * cellWidth;
        //var height = numRows * cellHeight;

        //float correctionX = 0;
        //float correctionY = 0;

        //for (var i = 0; i < numRows + 1; i++)
        //{
        //    var startPos = originPos + new Vector3(-correctionX, -correctionY, 0.0f) + i * cellHeight * new Vector3(0.0f, 1.0f, 0.0f);
        //    var endPos = startPos + height * new Vector3(0.7074f, 0.5f, 0.0f);
        //    Debug.DrawLine(startPos, endPos, color);

        //    correctionX += cellWidth / 2;
        //    correctionY += cellHeight / 2;
        //}

        //correctionX = 0;
        //correctionY = 0;

        //for (var i = 0; i < numCols + 1; i++)
        //{
        //    var startPos = originPos + new Vector3(-correctionX, correctionY, 0.0f) + i * cellWidth * new Vector3(1.0f, 0.0f, 0.0f);
        //    var endPos = startPos + width * new Vector3(-0.499849f, 0.3535f, 0.0f);

        //    Debug.DrawLine(startPos, endPos, color);

        //}
    }
}
