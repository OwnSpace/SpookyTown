using System;

using UnityEngine;

public class Node : IComparable
{
    public float nodeTotalCost = 1;

    public float estimatedCost = 0.0f;

    public bool isObstacle;

    public Node parent = null;

    public Vector3 position;

    public bool walkable;

    public Node(Vector3 position, bool walkable)
    {
        this.position = position;
        this.walkable = walkable;
    }

    public void MarkAsObstacle()
    {
        isObstacle = true;
    }

    public void MarkAsFree()
    {
        isObstacle = false;
    }

    public int CompareTo(object obj)
    {
        var node = obj as Node;
        if (node == null)
        {
            return -1;
        }

        if (estimatedCost < node.estimatedCost)
        {
            return -1;
        }

        if (estimatedCost > node.estimatedCost)
        {
            return 1;
        }

        return 0;
    }
}
