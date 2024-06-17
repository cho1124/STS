using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab; // 타일 프리팹
    public int rows = 15; // 행 수
    public int columns = 7; // 열 수
    public float tileSize = 1f; // 타일 크기
    public float tileGap = 0.1f; // 타일 간격

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
                // 타일 위치 계산 (간격 추가)
                Vector3 tilePosition = new Vector3(col * (tileSize + tileGap), row * (tileSize + tileGap), 0);

                // 타일 생성
                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                tile.transform.parent = transform; // 부모 설정
            }
        }
    }
}
