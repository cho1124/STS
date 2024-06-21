using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;



public class MonsterLoader : MonoBehaviour
{
    public string jsonFilePath = "Assets/MonsterData.json";

    public Monsters LoadMonsterData()
    {
       

        if (File.Exists(jsonFilePath))
        {
            string jsonData = File.ReadAllText(jsonFilePath);
            //Debug.Log("jsonData : " + jsonData);
            return JsonUtility.FromJson<Monsters>(jsonData);
        }
        else
        {
            Debug.LogError("파일을 찾을 수 없습니다: " + jsonFilePath);
            return null;
        }
    }
}
