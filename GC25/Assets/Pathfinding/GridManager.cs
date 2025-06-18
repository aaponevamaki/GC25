using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Vector2 gridWorldSize; // e.g. (width, height) in world units
    public float nodeRadius; // size of each node (half-size)
    public float obstacleAvoidanceRadius = 0.5f; // how far from walls enemy will keep
    Node[,] grid;

    int gridSizeX, gridSizeY;

    public LayerMask unwalkableMask; // Layer for walls

    void Start()
    {
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / (nodeRadius * 2));
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / (nodeRadius * 2));
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeRadius * 2 + nodeRadius) + Vector3.up * (y * nodeRadius * 2 + nodeRadius);
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius * 0.9f + obstacleAvoidanceRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = Mathf.Clamp01((worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float percentY = Mathf.Clamp01((worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue; // skip self

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    Node neighbour = grid[checkX, checkY];

                    if (x != 0 && y != 0)
                    {
                        Node nodeX = grid[node.gridX + x, node.gridY];
                        Node nodeY = grid[node.gridX, node.gridY + y];

                        if (!nodeX.walkable || !nodeY.walkable) continue;
                    }

                    neighbours.Add(neighbour);
                }
            }
        }
        return neighbours;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));

        if (grid == null)
            return;

        Color unwalkableColor = Color.red;
        Color walkableColor = Color.gray;
        unwalkableColor.a = 0.75f;
        walkableColor.a = 0.75f;

        foreach (Node node in grid)
        {
            Gizmos.color = (node.walkable) ? walkableColor : unwalkableColor;
            float gizmoSize = nodeRadius;
            Gizmos.DrawCube(node.worldPosition, Vector3.one * gizmoSize);
        }
    }
}

public class Node
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parent;

    public int fCost { get { return gCost + hCost; } }

    public Node(bool walkable, Vector3 worldPos, int x, int y)
    {
        this.walkable = walkable;
        worldPosition = worldPos;
        gridX = x;
        gridY = y;
    }
}
