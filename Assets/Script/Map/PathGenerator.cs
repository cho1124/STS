using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase pathTile;
    public TileBase obstacleTile;

    public int width = 7;
    public int height = 15;
    public int seed = 42;
    public int numRepetitions = 6;
    public int minStartNodes = 2;
    public int maxStartNodes = 4;

    void Start()
    {
        System.Random rng = new System.Random(seed);

        for (int i = 0; i < numRepetitions; i++)
        {
            List<Vector3Int> startNodes = GenerateStartNodes(rng, width);
            foreach (Vector3Int startNode in startNodes)
            {
                GeneratePath(rng, startNode, height, width);
            }
        }
    }

    List<Vector3Int> GenerateStartNodes(System.Random rng, int width)
    {
        int numStartNodes = rng.Next(minStartNodes, maxStartNodes + 1);
        HashSet<int> startXSet = new HashSet<int>();

        while (startXSet.Count < numStartNodes)
        {
            int startX = rng.Next(0, width);
            startXSet.Add(startX);
        }

        List<Vector3Int> startNodes = new List<Vector3Int>();
        foreach (int startX in startXSet)
        {
            startNodes.Add(new Vector3Int(startX, 0, 0));
        }

        return startNodes;
    }

    void GeneratePath(System.Random rng, Vector3Int startNode, int height, int width)
    {
        Vector3Int currentPosition = startNode;
        tilemap.SetTile(currentPosition, pathTile);

        for (int y = 1; y < height; y++)
        {
            int direction = rng.Next(0, 2) == 0 ? -1 : 1;
            currentPosition.x += direction;

            if (currentPosition.x < 0)
                currentPosition.x = 0;
            else if (currentPosition.x >= width)
                currentPosition.x = width - 1;

            currentPosition.y = y;
            tilemap.SetTile(currentPosition, pathTile);
        }
    }

    void FillObstacles(int width, int height)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                if (tilemap.GetTile(position) != pathTile)
                {
                    tilemap.SetTile(position, obstacleTile);
                }
            }
        }
    }
}
