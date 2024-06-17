using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int id;
    public Vector2 position;
    public List<Node> connections;

    public Node(int id, Vector2 position)
    {
        this.id = id;
        this.position = position;
        connections = new List<Node>();
    }

    public void Connect(Node other)
    {
        connections.Add(other);
        other.connections.Add(this);
    }
}
