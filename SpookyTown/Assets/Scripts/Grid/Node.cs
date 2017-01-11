using System;

using UnityEngine;

namespace Assets.Scripts.Grid
{
    public class Node : IComparable
    {
        public float estimatedCost = 0f;

        public Vector3 position;

        public bool walkable;

        public Node(Vector3 position, bool walkable) : this(walkable)
        {
            this.position = position;
        }

        public Node(bool walkable)
        {
            this.walkable = walkable;
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
}
