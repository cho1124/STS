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

    public int numRows = 15; // ���� ��
    public int numColumns = 7; // ���� ��
    public float spacing = 1f; // ��� ���� ����
    public GameObject LinePrefab;
    public GameObject[,] nodes; // ��带 ���� ���� �迭 ����
    private List<int> StoredColumn;
    private List<GameObject> FilledNodeList;
    private GameObject lastPrefab;
    public GameObject FinalPrefab;
    private GameObject finalNode; // finalNode ������ ������ ����
    public GameObject startNode;
    public List<GameObject> canMoveList;
    public float blinkInterval = 0.5f; // �����Ÿ��� ����
    private bool isBlinking = false;

    private GameObject currentSelection; // ���� ���õ� ���
    private List<Nodes> allNodes = new List<Nodes>();


    private int canMoveNode = 0;

    void Awake()
    {


        nodes = new GameObject[numRows, numColumns]; // ���� �迭 �ʱ�ȭ
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
            // ���콺 Ŭ�� ��ġ ���
            Vector3 mousePosition = Input.mousePosition;
            //Debug.Log("Mouse Clicked at: " + mousePosition);

            // ���� ī�޶� �ִ��� Ȯ��
            if (Camera.main == null)
            {
                Debug.LogError("Main camera is not found.");
                return;
            }

            // ���� ī�޶��� ��ũ�� ��ǥ�� ���� ��ǥ�� ��ȯ
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(mousePosition);

            // ���̸� �߻�
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            // ���̰� � ������Ʈ�� �浹�ߴ��� üũ
            if (hit.collider != null)
            {
                // �浹�� ������Ʈ�� canMoveList�� ���ԵǾ� �ִ��� Ȯ��
                if (canMoveList.Contains(hit.collider.gameObject))
                {
                    // ���ԵǾ� �ִٸ� �ش� ������Ʈ�� ������ �α׷� ���
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
            // ũ�⸦ Ű���
            yield return StartCoroutine(ChangeScale(Vector3.one * 2f, blinkInterval));

            // ���� ũ��� �ǵ�����
            yield return StartCoroutine(ChangeScale(Vector3.one, blinkInterval));
        }
    }

    private IEnumerator ChangeScale(Vector3 targetScale, float duration)
    {
        float time = 0;
        List<Vector3> initialScales = new List<Vector3>();

        // �� ���� ������Ʈ�� �ʱ� ũ�⸦ ����
        foreach (var obj in canMoveList)
        {
            initialScales.Add(obj.transform.localScale);
        }

        // ũ�⸦ ������ ����
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

        // ���� ũ�� ����
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
        int numTilesToGenerate = Random.Range(3, 5); // �ּ� 3������ �ִ� 4������ ����


        for (int i = 0; i < numTilesToGenerate; i++)
        {
            GenerateTile(); // GenerateTile �޼ҵ� ȣ��
        }
    }
    public void GenerateFinalFloor()
    {
        finalNode = Instantiate(FinalPrefab, new Vector3(0, 25, 0), Quaternion.identity);
        finalNode.transform.SetParent(transform);
    }

    public void GenerateTile()
    {
        // ù ��° row ó��
        int randColumn;

        while (true)
        {
            randColumn = Random.Range(0, numColumns); // 0���� numColumns �̸��� ������ �� ����
            bool found = false;

            // ����� column �ε������� Ȯ���Ͽ� �ߺ� üũ
            foreach (var stored in StoredColumn)
            {
                if (stored == randColumn)
                {
                    found = true;
                    break; // �ߺ��� ���� ������ �ݺ��� ����
                }
            }

            if (!found)
            {
                StoredColumn.Add(randColumn); // ���� ������ randColumn�� ����Ʈ�� �߰�
                break; // �ߺ��� ���� ������ while ���� ����
            }
        }

        // �θ� ��ü�� ���� ��ǥ ��������
        Vector3 parentWorldPosition = transform.position;

        // ù ��° ���� ���ο� ��� ��ġ ���
        Vector3 position = parentWorldPosition + new Vector3(randColumn * spacing, 0, 0);

        GameObject newNodeFirstRow = Instantiate(randomPrefab(0), position, Quaternion.identity); // ù ��° ���� ���ο� ��� ����
        FilledNodeList.Add(newNodeFirstRow);
        newNodeFirstRow.transform.SetParent(transform); // �θ� ����
        nodes[0, randColumn] = newNodeFirstRow; // nodes �迭�� ���ο� ��� �Ҵ�

        // ������ row ó��
        for (int i = 1; i < numRows; i++)
        {
            int prevColumn = randColumn; // ���� �� ��ġ ����
            int offset = Random.Range(-1, 2); // -1, 0, 1 �� �ϳ��� ������ ����
            randColumn += offset;
            randColumn = Mathf.Clamp(randColumn, 0, numColumns - 1); // �� ���� ����

            // ���� ���� ���ο� ��� ��ġ ���
            position = parentWorldPosition + new Vector3(randColumn * spacing, i * spacing, 0);

            GameObject newNodeCurrentRow;
            if (nodes[i, randColumn] == null)
            {
                newNodeCurrentRow = Instantiate(randomPrefab(i), position, Quaternion.identity); // ���� ���� ���ο� ��� ����
            }
            else
            {
                newNodeCurrentRow = nodes[i, randColumn]; // �̹� �ִ� ��带 �����ͼ� �Ҵ�
                newNodeCurrentRow.SetActive(true); // �ʿ�� Ȱ��ȭ
                newNodeCurrentRow.transform.position = position; // ��ġ ������Ʈ
            }

            FilledNodeList.Add(newNodeCurrentRow);
            newNodeCurrentRow.transform.SetParent(transform); // �θ� ����
            nodes[i, randColumn] = newNodeCurrentRow; // nodes �迭�� ���ο� ��� �Ҵ�

            
        }
        
    }


    public void ConnectNodes(GameObject node1, GameObject node2, int numberOfConnectors)
    {
        Vector3 direction = (node2.transform.position - node1.transform.position).normalized;
        float distance = Vector3.Distance(node1.transform.position, node2.transform.position);
        float stepSize = distance / (numberOfConnectors + 1); // ������ ������ ���� ���

        Vector3 startPosition = node1.transform.position + direction * stepSize; // �������� ���1���� �� ���ݸ�ŭ ������ �������� ����

        // ������ ���� �θ� GameObject ����
        GameObject linesParent = new GameObject("Lines");
        linesParent.transform.parent = transform; // ���� ��ũ��Ʈ�� ��� �ִ� ������Ʈ�� �ڽ����� ����

        for (int i = 0; i < numberOfConnectors; i++)
        {
            Vector3 position = startPosition + direction * stepSize * i; // �� �������� ��ġ ���
            GameObject connector = Instantiate(LinePrefab, position, Quaternion.identity);

            // connector�� ���� ����
            Vector3 connectorDirection = (node2.transform.position - position).normalized;
            connector.transform.right = connectorDirection; // ��������Ʈ�� ���� ����
            connector.transform.rotation *= Quaternion.Euler(0f, 0f, -90f); // -90�� ȸ��

            // finalNode�� ����� ���� ���� ���� �Ʒ������� ����
            if (node2 == finalNode)
            {
                connector.transform.position -= new Vector3(0, 0.5f, 0); // y������ 0.5��ŭ �Ʒ��� �̵� (���� ���� ����)
            }

            connector.transform.parent = linesParent.transform; // linesParent�� �ڽ����� ����
        }
    }


    public void DeleteWhite()
    {
        // �ڽ� ������Ʈ���� ��ȸ
        foreach (Transform child in transform)
        {
            // FilledNodeList�� ���� ��带 ����
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
        return GetNodeType(nodeTypes[thresholds.Length]); // ������ Ÿ�� ��ȯ
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
                        // �ε����� ��ȿ���� Ȯ��
                        if (neighborColumnIndex >= 0 && neighborColumnIndex < numColumns && nodes[i + 1, neighborColumnIndex] != null)
                        {

                            // Debug.Log("i :" + i + " j : " + j + " vs i + 1 : " + (i + 1) + "neightbor : " + neighborColumnIndex);
                            // nodes[i, j]�� nodes[i + 1, neighborColumnIndex]�� ����

                            
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
        // currentNode�� �ش��ϴ� Nodes ��ü�� ã��
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