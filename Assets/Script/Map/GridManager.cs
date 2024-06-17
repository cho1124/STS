using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab; // Ÿ�� ������
    public int rows = 15; // �� ��
    public int columns = 7; // �� ��
    public float tileSize = 1f; // Ÿ�� ũ��
    public float tileGap = 0.1f; // Ÿ�� ����

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Ÿ�� ��ġ ��� (���� �߰�)
                Vector3 tilePosition = new Vector3(col * (tileSize + tileGap), row * (tileSize + tileGap), 0);

                // Ÿ�� ����
                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                tile.transform.parent = transform; // �θ� ����
            }
        }
    }
}
