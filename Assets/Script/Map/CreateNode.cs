using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Nodes
{
    public GameObject currentGameObj;
    public List<GameObject> nextGameObjs;
    public bool isCleared = false;
    public int currentFloor = 0;


    public Nodes(GameObject currentObj)
    {
        currentGameObj = currentObj;
        
        nextGameObjs = new List<GameObject>();
    }
}


public class CreateNode : MonoBehaviour
{
    
    public List<GameObject> checkPrefab;

    public List<Node> Nodetypes;

    public int numRows = 15; // 행의 수
    public int numColumns = 7; // 열의 수
    public float spacing = 1f; // 노드 간의 간격
    public GameObject LinePrefab;
    public GameObject[,] nodes; // 노드를 담을 이중 배열 선언
    private List<int> StoredColumn;
    private List<GameObject> FilledNodeList;
    private GameObject lastPrefab;
    public GameObject FinalPrefab;
    private GameObject finalNode; // finalNode 참조를 저장할 변수
    public GameObject startNode;
    public List<GameObject> canMoveList;
    public float blinkInterval = 0.5f; // 깜빡거리는 간격
    private bool isBlinking = false;

    private GameObject currentSelection; // 현재 선택된 노드
    private List<Nodes> allNodes = new List<Nodes>();


    private int canMoveNode = 0;

    void Awake()
    {


        nodes = new GameObject[numRows, numColumns]; // 이중 배열 초기화
        StoredColumn = new List<int>();
        FilledNodeList = new List<GameObject>();
        canMoveList = new List<GameObject>();


        GenerateFirstFloor();

        DeleteWhite();


        GenerateFinalFloor();
        
        ConnectLine();

       
        //for(int i = 0; i <  numRows; i++)
        //{
        //    for(int j = 0; j < numColumns; j++)
        //    {
        //        if (nodes[i, j] != null && i == canMoveNode)
        //        {
        //            canMoveList.Add(nodes[i, j]);
        //            
        //        }
        //    }
        //}



        PrintConnectedNodes(startNode);

    }


    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 클릭 위치 출력
            Vector3 mousePosition = Input.mousePosition;
            //Debug.Log("Mouse Clicked at: " + mousePosition);

            // 메인 카메라가 있는지 확인
            if (Camera.main == null)
            {
                Debug.LogError("Main camera is not found.");
                return;
            }

            // 메인 카메라의 스크린 좌표를 월드 좌표로 변환
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(mousePosition);

            // 레이를 발사
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            // 레이가 어떤 오브젝트와 충돌했는지 체크
            if (hit.collider != null)
            {
                // 충돌한 오브젝트가 canMoveList에 포함되어 있는지 확인
                if (canMoveList.Contains(hit.collider.gameObject))
                {
                    // 포함되어 있다면 해당 오브젝트의 정보를 로그로 출력
                    Debug.Log("Clicked on: " + hit.collider.gameObject.name);
                    currentSelection = hit.collider.gameObject;
                    currentSelection.SetActive(true);
                    Transform childTransform = currentSelection.transform.Find("Clear");
                    childTransform.gameObject.SetActive(true);
                    canMoveList.Clear();
                    PrintConnectedNodes(currentSelection);
                }
                else
                {
                    Debug.Log("Object hit but not in canMoveList.");
                }
            }
            else
            {
                Debug.Log("No object hit by raycast.");
            }
        }
    }


    private void generateCanMoveList()
    {
        canMoveList.Clear();

        if(canMoveNode == 0)
        {
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numColumns; j++)
                {
                    if (nodes[i, j] != null && i == canMoveNode)
                    {
                        canMoveList.Add(nodes[i, j]);

                    }
                }
            }
            
        }
        //else if(canMoveNode > )
        //{
        //
        //}

        canMoveNode++;

    }




    private IEnumerator BlinkCoroutine()
    {
        isBlinking = true;
        while (true)
        {
            // 크기를 키우기
            yield return StartCoroutine(ChangeScale(Vector3.one * 2f, blinkInterval));

            // 원래 크기로 되돌리기
            yield return StartCoroutine(ChangeScale(Vector3.one, blinkInterval));
        }
    }

    private IEnumerator ChangeScale(Vector3 targetScale, float duration)
    {
        float time = 0;
        List<Vector3> initialScales = new List<Vector3>();

        // 각 게임 오브젝트의 초기 크기를 저장
        foreach (var obj in canMoveList)
        {
            initialScales.Add(obj.transform.localScale);
        }

        // 크기를 서서히 변경
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            for (int i = 0; i < canMoveList.Count; i++)
            {
                if (i < initialScales.Count)
                {
                    canMoveList[i].transform.localScale = Vector3.Lerp(initialScales[i], targetScale, t);
                }
            }
            yield return null;
        }

        // 최종 크기 설정
        for (int i = 0; i < canMoveList.Count; i++)
        {
            if (i < initialScales.Count)
            {
                canMoveList[i].transform.localScale = targetScale;
            }
        }
    }




    public void GenerateFirstFloor()
    {
        int numTilesToGenerate = Random.Range(3, 5); // 최소 3번에서 최대 4번까지 실행


        for (int i = 0; i < numTilesToGenerate; i++)
        {
            GenerateTile(); // GenerateTile 메소드 호출
        }
    }
    public void GenerateFinalFloor()
    {
        finalNode = Instantiate(FinalPrefab, new Vector3(0, 25, 0), Quaternion.identity);
        finalNode.transform.SetParent(transform);
    }

    public void GenerateTile()
    {
        // 첫 번째 row 처리
        int randColumn;

        while (true)
        {
            randColumn = Random.Range(0, numColumns); // 0부터 numColumns 미만의 랜덤한 열 선택
            bool found = false;

            // 저장된 column 인덱스들을 확인하여 중복 체크
            foreach (var stored in StoredColumn)
            {
                if (stored == randColumn)
                {
                    found = true;
                    break; // 중복된 값이 있으면 반복문 종료
                }
            }

            if (!found)
            {
                StoredColumn.Add(randColumn); // 현재 선택한 randColumn을 리스트에 추가
                break; // 중복된 값이 없으면 while 루프 종료
            }
        }

        // 부모 객체의 월드 좌표 가져오기
        Vector3 parentWorldPosition = transform.position;

        // 첫 번째 행의 새로운 노드 위치 계산
        Vector3 position = parentWorldPosition + new Vector3(randColumn * spacing, 0, 0);

        GameObject newNodeFirstRow = Instantiate(randomPrefab(0), position, Quaternion.identity); // 첫 번째 행의 새로운 노드 생성
        FilledNodeList.Add(newNodeFirstRow);
        newNodeFirstRow.transform.SetParent(transform); // 부모 설정
        nodes[0, randColumn] = newNodeFirstRow; // nodes 배열에 새로운 노드 할당

        // 나머지 row 처리
        for (int i = 1; i < numRows; i++)
        {
            int prevColumn = randColumn; // 이전 열 위치 저장
            int offset = Random.Range(-1, 2); // -1, 0, 1 중 하나의 오프셋 선택
            randColumn += offset;
            randColumn = Mathf.Clamp(randColumn, 0, numColumns - 1); // 열 범위 제한

            // 현재 행의 새로운 노드 위치 계산
            position = parentWorldPosition + new Vector3(randColumn * spacing, i * spacing, 0);

            GameObject newNodeCurrentRow;
            if (nodes[i, randColumn] == null)
            {
                newNodeCurrentRow = Instantiate(randomPrefab(i), position, Quaternion.identity); // 현재 행의 새로운 노드 생성
            }
            else
            {
                newNodeCurrentRow = nodes[i, randColumn]; // 이미 있는 노드를 가져와서 할당
                newNodeCurrentRow.SetActive(true); // 필요시 활성화
                newNodeCurrentRow.transform.position = position; // 위치 업데이트
            }

            FilledNodeList.Add(newNodeCurrentRow);
            newNodeCurrentRow.transform.SetParent(transform); // 부모 설정
            nodes[i, randColumn] = newNodeCurrentRow; // nodes 배열에 새로운 노드 할당

            
        }
        
    }


    public void ConnectNodes(GameObject node1, GameObject node2, int numberOfConnectors)
    {
        Vector3 direction = (node2.transform.position - node1.transform.position).normalized;
        float distance = Vector3.Distance(node1.transform.position, node2.transform.position);
        float stepSize = distance / (numberOfConnectors + 1); // 연결점 사이의 간격 계산

        Vector3 startPosition = node1.transform.position + direction * stepSize; // 시작점을 노드1에서 한 간격만큼 떨어진 지점으로 설정

        // 선들을 담을 부모 GameObject 생성
        GameObject linesParent = new GameObject("Lines");
        linesParent.transform.parent = transform; // 현재 스크립트를 담고 있는 오브젝트의 자식으로 설정

        for (int i = 0; i < numberOfConnectors; i++)
        {
            Vector3 position = startPosition + direction * stepSize * i; // 각 연결점의 위치 계산
            GameObject connector = Instantiate(LinePrefab, position, Quaternion.identity);

            // connector의 방향 설정
            Vector3 connectorDirection = (node2.transform.position - position).normalized;
            connector.transform.right = connectorDirection; // 스프라이트의 방향 설정
            connector.transform.rotation *= Quaternion.Euler(0f, 0f, -90f); // -90도 회전

            // finalNode에 연결될 때만 선을 조금 아래쪽으로 조정
            if (node2 == finalNode)
            {
                connector.transform.position -= new Vector3(0, 0.5f, 0); // y축으로 0.5만큼 아래로 이동 (값은 조정 가능)
            }

            connector.transform.parent = linesParent.transform; // linesParent의 자식으로 설정
        }
    }


    public void DeleteWhite()
    {
        // 자식 오브젝트들을 순회
        foreach (Transform child in transform)
        {
            // FilledNodeList에 없는 노드를 삭제
            if (!FilledNodeList.Contains(child.gameObject))
            {
                Destroy(child.gameObject);
            }
        }
    }

    public GameObject randomPrefab(int i)
    {
        GameObject newObj = null;
        float randomValue;

        if (i <= 5)
        {
            if (i == 0)
            {
                newObj = GetNodeType(NodeType.Normal);
            }
            else
            {
                do
                {
                    randomValue = Random.Range(0f, 1f);
                    newObj = SelectNode(randomValue, new[] { 0.4f, 0.7f }, new[] { NodeType.Normal, NodeType.Event, NodeType.Shop });
                } while (newObj == GetNodeType(NodeType.Shop));
            }
        }
        else
        {
            if (i == 8)
            {
                newObj = GetNodeType(NodeType.Treasure);
            }
            else if (i == 13)
            {
                do
                {
                    randomValue = Random.Range(0f, 1f);
                    newObj = SelectNode(randomValue, new[] { 0.4f, 0.7f, 0.9f }, new[] { NodeType.Normal, NodeType.Event, NodeType.Elite, NodeType.Shop });
                } while ((newObj == lastPrefab) && (newObj == GetNodeType(NodeType.Shop)));
            }
            else if (i == 14)
            {
                newObj = GetNodeType(NodeType.Rest);
            }
            else
            {
                do
                {
                    randomValue = Random.Range(0f, 1f);
                    newObj = SelectNode(randomValue, new[] { 0.35f, 0.6f, 0.8f, 0.95f }, new[] { NodeType.Normal, NodeType.Event, NodeType.Elite, NodeType.Rest, NodeType.Shop });
                } while ((newObj == lastPrefab) && (newObj == GetNodeType(NodeType.Elite) || newObj == GetNodeType(NodeType.Rest) || newObj == GetNodeType(NodeType.Shop)));
            }
        }

        lastPrefab = newObj;
        return newObj;
    }
    

    private GameObject SelectNode(float randomValue, float[] thresholds, NodeType[] nodeTypes)
    {
        for (int j = 0; j < thresholds.Length; j++)
        {
            if (randomValue < thresholds[j])
            {
                return GetNodeType(nodeTypes[j]);
            }
        }
        return GetNodeType(nodeTypes[thresholds.Length]); // 마지막 타입 반환
    }

    public GameObject GetNodeType(NodeType nodeType)
    {
        switch (nodeType)
        {
            case NodeType.Normal:
                return checkPrefab[0];
            case NodeType.Elite:
                return checkPrefab[2];
            case NodeType.Rest:
                return checkPrefab[3];
            case NodeType.Shop:
                return checkPrefab[4];
            case NodeType.Treasure:
                return checkPrefab[5];
            case NodeType.Event:
                return checkPrefab[1];
            case NodeType.Boss:
                return FinalPrefab;
            default:
                return null;
        }
    }



    private void ConnectLine()
    {
        Nodes StartNode = new Nodes(startNode);
        for (int j = 0; j < numColumns; j++)
        {
            if(nodes[0, j] != null)
            {
                
                StartNode.nextGameObjs.Add(nodes[0, j]);
                StartNode.currentFloor = 0;
                
            }
        }
        allNodes.Add(StartNode);



        for (int i = 0; i < numRows - 1; i++)
        {
            for (int j = 0; j < numColumns; j++)
            {
                if (nodes[i, j] != null)
                {
                    Nodes newNodes = new Nodes(nodes[i, j]);

                    newNodes.currentFloor = i;


                    for (int k = -1; k <= 1; k++)
                    {
                        int neighborColumnIndex = j + k;
                        // 인덱스가 유효한지 확인
                        if (neighborColumnIndex >= 0 && neighborColumnIndex < numColumns && nodes[i + 1, neighborColumnIndex] != null)
                        {

                            // Debug.Log("i :" + i + " j : " + j + " vs i + 1 : " + (i + 1) + "neightbor : " + neighborColumnIndex);
                            // nodes[i, j]와 nodes[i + 1, neighborColumnIndex]를 연결

                            
                            //newNodes.currentGameObj = nodes[i, j];
                            newNodes.nextGameObjs.Add(nodes[i + 1, neighborColumnIndex]);
                            
                            ConnectNodes(nodes[i, j], nodes[i + 1, neighborColumnIndex], 5);
                        }


                    }
                    allNodes.Add(newNodes);
                }
            }
        }
        for (int j = 0; j < numColumns; j++)
        {
            
            if (nodes[14, j] != null)
            {
                Nodes newNodes = new Nodes(nodes[14, j]);
                ConnectNodes(nodes[14, j], finalNode, 10);
                
                newNodes.nextGameObjs.Add(finalNode);
                newNodes.currentFloor = 14;
                allNodes.Add(newNodes);
            }
                

            
        }

        Nodes finaleNode = new Nodes(finalNode);
        finaleNode.currentFloor = 15;
        allNodes.Add(finaleNode);




    }



    void PrintConnectedNodes(GameObject currentNode)
    {
        // currentNode에 해당하는 Nodes 객체를 찾음
        Nodes currentNodeObject = allNodes.Find(node => node.currentGameObj == currentNode);

        if (currentNodeObject != null)
        {
            Debug.Log("Current Node Tag :  " + currentNodeObject.currentGameObj.tag);
            Debug.Log("Current Node Floor :  " + currentNodeObject.currentFloor);


            foreach (GameObject nextNode in currentNodeObject.nextGameObjs)
            {

                Debug.Log("Connected Node: " + nextNode.name);
                canMoveList.Add(nextNode);

            }
        }
        else
        {
            Debug.Log("No connected nodes found for the current node.");
        }
        StopCoroutine("BlinkCoroutine");
        StartCoroutine("BlinkCoroutine");

    }
}