using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase pathTile;
    public TileBase obstacleTile;

    public int width = 10;
    public int height = 10;
    public int pathDensity = 5;
    public bool ascensionZero = true;

    private System.Random rng;

    void Start()
    {
        GenerateMaps(System.DateTime.Now.Ticks, height, width, pathDensity, ascensionZero);
    }

    void GenerateMaps(long seed, int mapHeight, int mapWidth, int pathDensity, bool ascensionZero)
    {
        rng = new System.Random((int)seed);

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                TileBase tile = GenerateTile(pathDensity, ascensionZero);
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }

    TileBase GenerateTile(int pathDensity, bool ascensionZero)
    {
        int randomValue = rng.Next(0, 10);
        if (randomValue < pathDensity)
        {
            return pathTile;
        }
        else
        {
            return obstacleTile;
        }
    }
}
