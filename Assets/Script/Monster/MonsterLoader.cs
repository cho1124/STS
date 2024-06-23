using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;



public class MonsterLoader : MonoBehaviour
{
    private string jsonFilePath = "MonsterData"; // 경로에서 확장자를 제거

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
            Debug.LogError("파일을 찾을 수 없습니다: " + jsonFilePath);
            return null;
        }
    }
}
