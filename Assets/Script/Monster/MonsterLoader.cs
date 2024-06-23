using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;



public class MonsterLoader : MonoBehaviour
{
    private string jsonFilePath = "MonsterData"; // ��ο��� Ȯ���ڸ� ����

    public Monsters LoadMonsterData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFilePath);

        if (jsonFile != null)
        {
            string jsonData = jsonFile.text;
            return JsonUtility.FromJson<Monsters>(jsonData);
        }
        else
        {
            Debug.LogError("������ ã�� �� �����ϴ�: " + jsonFilePath);
            return null;
        }
    }
}
